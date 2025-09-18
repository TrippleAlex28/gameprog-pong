using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
class Paddle
{
  public const short borderPadding = 16; 

  private Game1 _instance;

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

  public Paddle(Game1 instance, PaddleMovementDirection paddleMovDir, bool IsFarSide, Keys positiveMovKey, Keys negativeMovKey, Color paddleColor)
  {
    _instance = instance;
    this.paddleMovDir = paddleMovDir;
    this.IsFarSide = IsFarSide;
    this.positiveMovKey = positiveMovKey;
    this.negativeMovKey = negativeMovKey;
    this.paddleColor = paddleColor;
  }

  public void LoadContent()
  {
    currentPos = (short)(paddleMovDir.Equals(PaddleMovementDirection.Vertical) ? _instance.graphics.PreferredBackBufferHeight / 2 : _instance.graphics.PreferredBackBufferWidth / 2);
    // assuming symmetrical paddle texture
    paddleRotation = MathHelper.ToRadians(paddleMovDir.Equals(PaddleMovementDirection.Vertical) ? 0.0f : 90.0f);

    paddleOrigin.X = Globals.paddleTexture.Width / 2;
    paddleOrigin.Y = Globals.paddleTexture.Height / 2;

    // calculate constant position
    if (Globals.gameType.Equals(GameType.TwoPlayer)) // always only vertical paddles, full field size
    {
      if (IsFarSide)
      {
        constantPos = (short)(_instance.graphics.PreferredBackBufferWidth - borderPadding);
      }
      else
      {
        constantPos = borderPadding;
      }
    }
    else // four players
    {
      if (paddleMovDir.Equals(PaddleMovementDirection.Vertical))
      {
        short sideBorderSize = (short)((_instance.graphics.PreferredBackBufferWidth - _instance.graphics.PreferredBackBufferHeight) / 2);
        if (IsFarSide)
        {
          constantPos = (short)(_instance.graphics.PreferredBackBufferWidth - sideBorderSize - borderPadding);
        }
        else
        {
          constantPos = (short)(sideBorderSize + borderPadding);
        }
      }
      else // horizontal
      {
        if (IsFarSide)
        {
          constantPos = (short)(_instance.graphics.PreferredBackBufferHeight - borderPadding);
        }
        else
        {
          constantPos = borderPadding;
        }
      }
    }

    // calculate movement clamps
    if (Globals.gameType.Equals(GameType.TwoPlayer))
    {
      clampMin = (short)(Globals.paddleTexture.Height / 2);
      clampMax = (short)(_instance.graphics.PreferredBackBufferHeight - Globals.paddleTexture.Height / 2);
    }
    else // 4p
    {
      if (paddleMovDir.Equals(PaddleMovementDirection.Vertical))
      {
        clampMin = (short)(Globals.paddleTexture.Height / 2);
        clampMax = (short)(_instance.graphics.PreferredBackBufferHeight - Globals.paddleTexture.Height / 2);
      }
      else // horizontal
      {
        short sideBorderSize = (short)((_instance.graphics.PreferredBackBufferWidth - _instance.graphics.PreferredBackBufferHeight) / 2);
        clampMin = (short)(sideBorderSize + Globals.paddleTexture.Height / 2);
        clampMax = (short)(sideBorderSize + _instance.graphics.PreferredBackBufferHeight - Globals.paddleTexture.Height / 2);
      }
    }
  }

  public void Update(GameTime gameTime)
  {
    // don't update when not participating
    if (Globals.gameType.Equals(GameType.TwoPlayer) && paddleMovDir.Equals(PaddleMovementDirection.Horizontal)) return;

    sbyte desiredMovDir = 0;
    if (Keyboard.GetState().IsKeyDown(positiveMovKey))
    {
      desiredMovDir++;
    }
    if (Keyboard.GetState().IsKeyDown(negativeMovKey))
    {
      desiredMovDir--;
    }

    // Rounded to integever because you can't move a sprite a fraction of a pixel
    short desiredMov = (short)(desiredMovDir * Globals.paddleSpeed * gameTime.ElapsedGameTime.TotalSeconds);

    currentPos += desiredMov;
    currentPos = (short)MathHelper.Clamp(currentPos, clampMin, clampMax);

    drawPosition = new Point(
      paddleMovDir.Equals(PaddleMovementDirection.Vertical) ? constantPos : currentPos,
      paddleMovDir.Equals(PaddleMovementDirection.Vertical) ? currentPos : constantPos
    );
  }

  public void Draw(GameTime gameTime)
  {
    // don't update when not participating
    if (Globals.gameType.Equals(GameType.TwoPlayer) && paddleMovDir.Equals(PaddleMovementDirection.Horizontal)) return;
    
    _instance.spriteBatch.Draw(
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