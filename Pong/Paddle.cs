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
  private bool _IsFarSide;
  private float _paddleRotation;
  private short _clampMin;
  private short _clampMax;

  // Position along axis of movement
  private short _currentPos;

  // Center of the paddle
  private Point _drawPosition;
  private Vector2 _paddleOrigin;

  public Paddle(GraphicsDeviceManager graphics, PaddleMovementDirection paddleMovDir, bool IsFarSide, Keys positiveMovKey, Keys negativeMovKey, Color paddleColor)
  {
    _paddleMovDir = paddleMovDir;
    _IsFarSide = IsFarSide;
    _positiveMovKey = positiveMovKey;
    _negativeMovKey = negativeMovKey;
    _paddleColor = paddleColor;

    _currentPos = (short)(paddleMovDir == PaddleMovementDirection.Vertical ? graphics.GraphicsDevice.Viewport.Height / 2 :graphics.GraphicsDevice.Viewport.Width / 2);

    // assuming symmetrical paddle texture
    _paddleRotation = MathHelper.ToRadians(paddleMovDir == PaddleMovementDirection.Vertical ? 0.0f : 90.0f);

    // set paddle origin to the center of the texture
    _paddleOrigin.X = Globals.paddleSize.X / 2; 
    _paddleOrigin.Y = Globals.paddleSize.Y / 2;

    // calculate constant position
    if (Globals.gameType == GameType.TwoPlayer) // always only vertical paddles & full field size
    {
      if (IsFarSide)
        _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderPadding);
      else
        _constantPos = borderPadding;
    }
    else // four players
    {
      if (paddleMovDir == PaddleMovementDirection.Vertical) // vertical movement
      {
        short sideBorderSize = (short)((graphics.GraphicsDevice.Viewport.Width - graphics.GraphicsDevice.Viewport.Height) / 2);

        if (IsFarSide)
          _constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - sideBorderSize - borderPadding);
        else
          _constantPos = (short)(sideBorderSize + borderPadding);
      }
      else // horizontal movement
      {
        if (IsFarSide)
          _constantPos = (short)(graphics.GraphicsDevice.Viewport.Height - borderPadding);
        else
          _constantPos = borderPadding;
      }
    }

    // calculate movement clamps
    if (Globals.gameType == GameType.TwoPlayer)
    {
      _clampMin = (short)(Globals.paddleSize.Y / 2);
      _clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y / 2);
    }
    else // 4p
    {
      if (paddleMovDir == PaddleMovementDirection.Vertical)
      {
        _clampMin = (short)(Globals.paddleSize.Y / 2);
        _clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y / 2);
      }
      else // horizontal
      {
        short sideBorderSize = (short)((graphics.GraphicsDevice.Viewport.Width - graphics.GraphicsDevice.Viewport.Height) / 2);
        _clampMin = (short)(sideBorderSize + Globals.paddleSize.Y / 2);
        _clampMax = (short)(sideBorderSize + graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y / 2);
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
      _paddleOrigin,
      SpriteEffects.None,
      1
    );
  }
}