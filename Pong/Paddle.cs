using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
class Paddle
{
  public const short borderPadding = 16; 

  private PaddleMovementDirection paddleMovDir;
  private Keys positiveMovKey;
  private Keys negativeMovKey;
  private Color paddleColor;

  // the constant position perpendicular to the movement axis
  private short constantPos;
  private bool IsFarSide;
  private float paddleRotation;
  private short clampMin;
  private short clampMax;

  // Position along axis of movement
  private short currentPos;
  private Point drawPosition;
  private Vector2 paddleOrigin;

  public Paddle(GraphicsDeviceManager graphics, PaddleMovementDirection paddleMovDir, bool IsFarSide, Keys positiveMovKey, Keys negativeMovKey, Color paddleColor)
  {
    this.paddleMovDir = paddleMovDir;
    this.IsFarSide = IsFarSide;
    this.positiveMovKey = positiveMovKey;
    this.negativeMovKey = negativeMovKey;
    this.paddleColor = paddleColor;

    currentPos = (short)(paddleMovDir == PaddleMovementDirection.Vertical ? graphics.GraphicsDevice.Viewport.Height / 2 :graphics.GraphicsDevice.Viewport.Width / 2);

    // assuming symmetrical paddle texture
    paddleRotation = MathHelper.ToRadians(paddleMovDir == PaddleMovementDirection.Vertical ? 0.0f : 90.0f);

    // set paddle origin to the center of the texture
    paddleOrigin.X = Globals.paddleSize.X / 2; 
    paddleOrigin.Y = Globals.paddleSize.Y / 2;

    // calculate constant position
    if (Globals.gameType == GameType.TwoPlayer) // always only vertical paddles & full field size
    {
      if (IsFarSide)
        constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - borderPadding);
      else
        constantPos = borderPadding;
    }
    else // four players
    {
      if (paddleMovDir == PaddleMovementDirection.Vertical) // vertical movement
      {
        short sideBorderSize = (short)((graphics.GraphicsDevice.Viewport.Width - graphics.GraphicsDevice.Viewport.Height) / 2);

        if (IsFarSide)
          constantPos = (short)(graphics.GraphicsDevice.Viewport.Width - sideBorderSize - borderPadding);
        else
          constantPos = (short)(sideBorderSize + borderPadding);
      }
      else // horizontal movement
      {
        if (IsFarSide)
          constantPos = (short)(graphics.GraphicsDevice.Viewport.Height - borderPadding);
        else
          constantPos = borderPadding;
      }
    }

    // calculate movement clamps
    if (Globals.gameType == GameType.TwoPlayer)
    {
      clampMin = (short)(Globals.paddleSize.Y / 2);
      clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y / 2);
    }
    else // 4p
    {
      if (paddleMovDir == PaddleMovementDirection.Vertical)
      {
        clampMin = (short)(Globals.paddleSize.Y / 2);
        clampMax = (short)(graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y / 2);
      }
      else // horizontal
      {
        short sideBorderSize = (short)((graphics.GraphicsDevice.Viewport.Width - graphics.GraphicsDevice.Viewport.Height) / 2);
        clampMin = (short)(sideBorderSize + Globals.paddleSize.Y / 2);
        clampMax = (short)(sideBorderSize + graphics.GraphicsDevice.Viewport.Height - Globals.paddleSize.Y / 2);
      }
    }
  }

  public void Update(Ball ball, GameTime gameTime)
  {
    // don't update when not participating
    if (Globals.gameType == GameType.TwoPlayer && paddleMovDir == PaddleMovementDirection.Horizontal) return;

    sbyte desiredMovDir = 0;
    if (Keyboard.GetState().IsKeyDown(positiveMovKey))
      desiredMovDir++;
    if (Keyboard.GetState().IsKeyDown(negativeMovKey))
      desiredMovDir--;

    // Rounded to integever because you can't move a sprite a fraction of a pixel
    short desiredMov = (short)(desiredMovDir * Globals.paddleSpeed * gameTime.ElapsedGameTime.TotalSeconds);

    currentPos += desiredMov;
    currentPos = (short)MathHelper.Clamp(currentPos, clampMin, clampMax);

    drawPosition = new Point(
      paddleMovDir == PaddleMovementDirection.Vertical ? constantPos : currentPos,
      paddleMovDir == PaddleMovementDirection.Vertical ? currentPos : constantPos
    );

    // handle ball collisions
    if (ball.Position.X >= drawPosition.X)
    {

    }
    
  }

  public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    // don't update when not participating
    if (Globals.gameType == GameType.TwoPlayer && paddleMovDir == PaddleMovementDirection.Horizontal) return;
    
    spriteBatch.Draw(
      Globals.paddleTexture,
      new Rectangle(drawPosition, Globals.paddleSize),
      null,
      paddleColor,
      paddleRotation,
      paddleOrigin,
      SpriteEffects.None,
      1
    );
  }
}