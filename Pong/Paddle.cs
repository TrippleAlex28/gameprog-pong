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
  private Color _paddleColor;

  // the constant position perpendicular to the movement axis
  private short _constantPos;

  private short _paddleSpeed;
  private bool _IsFarSide;
  private float _paddleRotation;
  private short _clampMin;
  private short _clampMax;

  // Position along axis of movement
  private short _currentPos;

  // Center of the paddle
  private Point _drawPosition;

  public Paddle(GraphicsDeviceManager graphics, PaddleMovementDirection paddleMovDir, bool IsFarSide, Keys positiveMovKey, Keys negativeMovKey, Color paddleColor)
  {
    _paddleMovDir = paddleMovDir;
    _IsFarSide = IsFarSide;
    _positiveMovKey = positiveMovKey;
    _negativeMovKey = negativeMovKey;
    _paddleColor = paddleColor;

    // Set initial position to center of movement axis
    _currentPos = (short)(paddleMovDir == PaddleMovementDirection.Vertical
      ? graphics.GraphicsDevice.Viewport.Height / 2 - Globals.paddleSize.Y / 2
      : graphics.GraphicsDevice.Viewport.Width / 2 - Globals.paddleSize.Y / 2
    );

    // set the paddle rotation, assuming the paddle texture is symmetrical
    _paddleRotation = MathHelper.ToRadians(paddleMovDir == PaddleMovementDirection.Vertical ? 0.0f : 90.0f);

    // calculate constant position
    if (Globals.gameType == GameType.TwoPlayer)
    {
      if (IsFarSide)
        _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderPadding);
      else
        _constantPos = borderPadding;
    }
    else
    {
      (Point borderSize, bool IsWidthBorder) = Utils.GetSideBorderSize(new(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height));

      if (paddleMovDir == PaddleMovementDirection.Vertical)
      {
        if (IsWidthBorder)
        {
          if (IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderSize.X - borderPadding);
          else
            _constantPos = (short)(borderSize.X + borderPadding);
        }
        else
        {
          if (IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderPadding);
          else
            _constantPos = borderPadding;
        }
      }
      else
      {
        if (IsWidthBorder)
        {
          if (IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Height - borderPadding);
          else
            _constantPos = borderPadding;
        }
        else
        {
          if (IsFarSide)
            _constantPos = (short)(graphics.GraphicsDevice.Viewport.Height - borderSize.Y - borderPadding);
          else
            _constantPos = (short)(borderSize.Y + borderPadding);
        }
      }
    }

    // calculate movement clamps
    if (Globals.gameType == GameType.TwoPlayer)
    {
      _clampMin = 0;
      _clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y);
    }
    else
    {
      if (paddleMovDir == PaddleMovementDirection.Vertical)
      {
        _clampMin = 0;
        _clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y);
      }
      else
      {
        short sideBorderSize = (short)((graphics.GraphicsDevice.Viewport.Width - graphics.GraphicsDevice.Viewport.Height) / 2);
        _clampMin = (short)(sideBorderSize + Globals.paddleSize.Y);
        _clampMax = (short)(sideBorderSize + graphics.GraphicsDevice.Viewport.Height);
      }
    }


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
    // if (Utils.IsColliding(new(_drawPosition)))
  }

  public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    // don't update when not participating
    if (Globals.gameType == GameType.TwoPlayer && _paddleMovDir == PaddleMovementDirection.Horizontal) return;

    spriteBatch.Draw(
      Globals.paddleTexture,
      new Rectangle(_drawPosition, Globals.paddleSize),
      null,
      _paddleColor,
      _paddleRotation,
      Vector2.Zero,
      SpriteEffects.None,
      1
    );
  }
}