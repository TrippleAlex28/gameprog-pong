using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
class Paddle
{
  public const short borderPadding = 16; 

  PaddleMovementDirection paddleMovDir;
  Keys positiveMovKey;
  Keys negativeMovKey;
  Color paddleColor;

  // the constant position perpendicular to the movement axis
  short constantPos;
  bool IsFarSide;
  float paddleRotation;
  short clampMin;
  short clampMax;

  // Position along axis of movement
  short currentPos;
  Point drawPosition;
  Vector2 paddleOrigin;

  public Paddle(GraphicsDeviceManager graphics, PaddleMovementDirection paddleMovDir, bool IsFarSide, Keys positiveMovKey, Keys negativeMovKey, Color paddleColor)
  {
    this.paddleMovDir = paddleMovDir;
    this.IsFarSide = IsFarSide;
    this.positiveMovKey = positiveMovKey;
    this.negativeMovKey = negativeMovKey;
    this.paddleColor = paddleColor;

    currentPos = (short)(paddleMovDir == PaddleMovementDirection.Vertical ?graphics.PreferredBackBufferHeight / 2 :graphics.PreferredBackBufferWidth / 2);

    // assuming symmetrical paddle texture
    paddleRotation = MathHelper.ToRadians(paddleMovDir == PaddleMovementDirection.Vertical ? 0.0f : 90.0f);

    paddleOrigin.X = Globals.paddleTexture.Width / 2;
    paddleOrigin.Y = Globals.paddleTexture.Height / 2;

    // calculate constant position
    if (Globals.gameType == GameType.TwoPlayer) // always only vertical paddles, full field size
    {
      if (IsFarSide)
        constantPos = (short)(graphics.PreferredBackBufferWidth - borderPadding);
      else
        constantPos = borderPadding;
    }
    else // four players
    {
      if (paddleMovDir == PaddleMovementDirection.Vertical)
      {
        short sideBorderSize = (short)((graphics.PreferredBackBufferWidth - graphics.PreferredBackBufferHeight) / 2);

        if (IsFarSide)
          constantPos = (short)(graphics.PreferredBackBufferWidth - sideBorderSize - borderPadding);
        else
          constantPos = (short)(sideBorderSize + borderPadding);
      }
      else // horizontal
      {
        if (IsFarSide)
          constantPos = (short)(graphics.PreferredBackBufferHeight - borderPadding);
        else
          constantPos = borderPadding;
      }
    }

    // calculate movement clamps
    if (Globals.gameType == GameType.TwoPlayer)
    {
      clampMin = (short)(Globals.paddleTexture.Height / 2);
      clampMax = (short)(graphics.PreferredBackBufferHeight - Globals.paddleTexture.Height / 2);
    }
    else // 4p
    {
      if (paddleMovDir == PaddleMovementDirection.Vertical)
      {
        clampMin = (short)(Globals.paddleTexture.Height / 2);
        clampMax = (short)(graphics.PreferredBackBufferHeight - Globals.paddleTexture.Height / 2);
      }
      else // horizontal
      {
        short sideBorderSize = (short)((graphics.PreferredBackBufferWidth - graphics.PreferredBackBufferHeight) / 2);
        clampMin = (short)(sideBorderSize + Globals.paddleTexture.Height / 2);
        clampMax = (short)(sideBorderSize + graphics.PreferredBackBufferHeight - Globals.paddleTexture.Height / 2);
      }
    }
  }

  public void Update(GameTime gameTime)
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