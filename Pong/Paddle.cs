using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
  class Paddle
  {
    PaddleMovementDirection paddleMovDir;
    Keys positiveMovKey;
    Keys negativeMovKey;
    Color paddleColor;
    int currentPos;

    public Paddle(PaddleMovementDirection paddleMovDir, Keys positiveMovKey, Keys negativeMovKey, Color paddleColor)
    {
      this.paddleMovDir = paddleMovDir;
      this.positiveMovKey = positiveMovKey;
      this.negativeMovKey = negativeMovKey;
      this.paddleColor = paddleColor;

      currentPos = paddleMovDir.Equals(PaddleMovementDirection.Vertical) ? Globals.windowSize.Y / 2 : Globals.windowSize.X / 2;
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
      sbyte desiredMovDir = 0;
      if (keyboardState.IsKeyDown(positiveMovKey))
      {
        desiredMovDir++;
      }
      if (keyboardState.IsKeyDown(negativeMovKey))
      {
        desiredMovDir--;
      }

      // Rounded to integever because you can't move a sprite a fraction of a pixel
      int desiredMov = (int)(desiredMovDir * Globals.paddleSpeed * gameTime.ElapsedGameTime.TotalSeconds);

      currentPos += desiredMov;
      currentPos = MathHelper.Clamp(currentPos, 0, 100); // TODO: Clamp to field bounds
    }

    public void Draw(GameTime gameTime)
    {
      Globals.spriteBatch.Draw(Globals.paddleTexture, new Rectangle(new Point(0), Globals.paddleSize), paddleColor);
    }
  }
}