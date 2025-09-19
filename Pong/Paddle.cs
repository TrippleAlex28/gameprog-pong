using System;
using System.Security.Claims;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;

class Paddle
{
  public const short borderPadding = 16;

  private PaddleMovementDirection _paddleMovDir;
  private Keys _positiveMovKey;
  private Keys _negativeMovKey;

  // the constant position perpendicular to the movement axis
  private short _constantPos;
  private bool _IsFarSide;
  private float _paddleRotation;
  private short _clampMin;
  private short _clampMax;

  // Position along axis of movement
  private short _currentPos;

  // Center of the paddle
  private Point _drawPosition;
  private Point _drawSize;

  private const float _ballRedirectMaxAngle = 75.0f;
  private const float _ballRedirectMinAngle = 5.0f;
  private const float _ballPrevAngleImpact = .25f;

  private short _playerId;

  public Paddle(GraphicsDeviceManager graphics, PaddleMovementDirection paddleMovDir, bool IsFarSide, Keys positiveMovKey, Keys negativeMovKey, short playerId)
  {
    _paddleMovDir = paddleMovDir;
    _IsFarSide = IsFarSide;
    _positiveMovKey = positiveMovKey;
    _negativeMovKey = negativeMovKey;
    _playerId = playerId;

    _drawSize = Utils.GetScaledPaddleSize(new(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height));

    // Set initial position to center of movement axis
    SetInitialPaddlePosition(graphics);

    // set the paddle rotation, assuming the paddle texture is symmetrical
    SetInitialPaddleRotation();

    // calculate constant position
    SetConstantPosition(graphics);

    // calculate movement clamps
    SetMovementClamps(graphics);
  }

  public void Update(Ball ball, GameTime gameTime)
  {
    // don't update when not participating
    if (Globals.gameType == GameType.TwoPlayer && _paddleMovDir == PaddleMovementDirection.Horizontal) return;

    sbyte desiredMovDir = 0;
    if (Keyboard.GetState().IsKeyDown(_positiveMovKey))
      desiredMovDir++;
    if (Keyboard.GetState().IsKeyDown(_negativeMovKey))
      desiredMovDir--;

    // Rounded to integever because you can't move a sprite a fraction of a pixel
    short desiredMov = (short)(desiredMovDir * Globals.paddleSpeed * gameTime.ElapsedGameTime.TotalSeconds);

    _currentPos += desiredMov;
    _currentPos = (short)MathHelper.Clamp(_currentPos, _clampMin, _clampMax);

    _drawPosition = new Point(
      _paddleMovDir == PaddleMovementDirection.Vertical ? _constantPos : _currentPos,
      _paddleMovDir == PaddleMovementDirection.Vertical ? _currentPos : _constantPos
    );

    // handle ball collisions
    Rectangle ballRect = new(new((int)ball.Position.X, (int)ball.Position.Y), ball.DrawSize);
    (bool collided, Point obj1Offset, Point obj2Offset) = Utils.IsColliding(new(_drawPosition, _drawSize), ballRect);
    if (collided)
    {
      // calculate ball redirection angle based on where it landed on the paddle
      ball.Bounce(_playerId, CalculateBallRedirectAngle(obj1Offset.Y + ball.DrawSize.Y / 2, ball.GetAngleDeg()));
    }
  }

  public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    // don't update when not participating
    if (Globals.gameType == GameType.TwoPlayer && _paddleMovDir == PaddleMovementDirection.Horizontal) return;

    spriteBatch.Draw(
      Globals.paddleTexture,
      new Rectangle(_drawPosition, _drawSize),
      null,
      Globals.playerColors[_playerId],
      _paddleRotation,
      Vector2.Zero,
      SpriteEffects.None,
      1
    );
  }



  private float CalculateBallRedirectAngle(int verticalPaddleOffset, float prevBallAngle)
  {
    // Map the ball hit paddle offset from -1 to 1
    float halfHeight = _drawSize.Y * .5f;
    float relativeY = verticalPaddleOffset / halfHeight - 1;

    // inverse relative Y so the direction of the ball inverses
    relativeY = -relativeY;

    // Clamp it just to make sure, but I'm pretty sure this should never activate
    relativeY = MathHelper.Clamp(relativeY, -1f, 1f);

    // calculate base angle based on the relative Y
    float baseAngle = !_IsFarSide
      ? relativeY * _ballRedirectMaxAngle
      : 180f - (relativeY * _ballRedirectMaxAngle);

    // Prevent the ball from going straight ahead (booooriiiing)
    if (!_IsFarSide)
    {
      baseAngle = Utils.NormalizeSignedAngleDeg(baseAngle);
      if (baseAngle > 0 && baseAngle < _ballRedirectMinAngle) baseAngle = _ballRedirectMinAngle;
      if (baseAngle < 0 && baseAngle > -_ballRedirectMinAngle) baseAngle = -_ballRedirectMinAngle;
    }
    else
    {
      baseAngle = Utils.NormalizeSignedAngleDeg(baseAngle - 180f);
      if (baseAngle > 0 && baseAngle < _ballRedirectMinAngle) baseAngle = 180f - _ballRedirectMinAngle;
      if (baseAngle < 0 && baseAngle > -_ballRedirectMinAngle) baseAngle = 180f - -_ballRedirectMinAngle;
    }


    // TODO: If you hit the ball dead center on your paddle, perfect hit mechanic 
    if (baseAngle == 0)
    {
      Console.WriteLine("PERFECT HIT");
      if (_IsFarSide) baseAngle = -180f;
    }  
      
    // Mix new angle and old angle to smoothen out the bounces
    float newAngle = MathHelper.Lerp(baseAngle, prevBallAngle, _ballPrevAngleImpact);

    // Normalize new angle (not really necessary ig)
    newAngle = Utils.NormalizeAngleDeg(newAngle);
    
    return newAngle;
  }

  private void SetInitialPaddlePosition(GraphicsDeviceManager graphics)
  {
    _currentPos = (short)(_paddleMovDir == PaddleMovementDirection.Vertical
      ? graphics.GraphicsDevice.Viewport.Height / 2 - _drawSize.Y / 2
      : graphics.GraphicsDevice.Viewport.Width / 2 - _drawSize.Y / 2
    );
  }

  private void SetInitialPaddleRotation()
  {
    _paddleRotation = MathHelper.ToRadians(_paddleMovDir == PaddleMovementDirection.Vertical ? 0.0f : 90.0f);
  }

  private void SetConstantPosition(GraphicsDeviceManager graphics)
  {
    if (Globals.gameType == GameType.TwoPlayer)
    {
      if (_IsFarSide)
        _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderPadding);
      else
        _constantPos = borderPadding;
    }
    else
    {
      (Point borderSize, bool IsWidthBorder) = Utils.GetSideBorderSize(new(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height));

      if (_paddleMovDir == PaddleMovementDirection.Vertical)
      {
        if (IsWidthBorder)
        {
          if (_IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderSize.X - borderPadding);
          else
            _constantPos = (short)(borderSize.X + borderPadding);
        }
        else
        {
          if (_IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderPadding);
          else
            _constantPos = borderPadding;
        }
      }
      else
      {
        if (IsWidthBorder)
        {
          if (_IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Height - borderPadding);
          else
            _constantPos = borderPadding;
        }
        else
        {
          if (_IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Height - borderSize.Y - borderPadding);
          else
            _constantPos = (short)(borderSize.Y + borderPadding);
        }
      }
    }
  }

  private void SetMovementClamps(GraphicsDeviceManager graphics)
  {
    if (Globals.gameType == GameType.TwoPlayer)
    {
      _clampMin = 0;
      _clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - _drawSize.Y);
    }
    else
    {
      if (_paddleMovDir == PaddleMovementDirection.Vertical)
      {
        _clampMin = 0;
        _clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - _drawSize.Y);
      }
      else
      {
        short sideBorderSize = (short)((graphics.GraphicsDevice.Viewport.Width - graphics.GraphicsDevice.Viewport.Height) / 2);
        _clampMin = (short)(sideBorderSize + _drawSize.Y);
        _clampMax = (short)(sideBorderSize + graphics.GraphicsDevice.Viewport.Height);
      }
    }
  }
}