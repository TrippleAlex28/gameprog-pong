using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
  class Globals
  {
    public static bool isFullScreen = false;
    public static Point windowSize = new Point(640, 480);

    public static SpriteBatch spriteBatch;
    public static Texture2D paddleTexture;
    public static Texture2D ballTexture;

    public static GameState gameState = GameState.MainMenu;
    public static GameType gameType;

    public static Keys continueKey = Keys.Space;
    public static Keys resetKey = Keys.R;

    public static Color backgroundColor = Color.Black;
    // public static Texture2D backgroundTexture;
    public static Color textColor = Color.White;
    public static Color[] playerColors = [
      Color.Blue,
      Color.Red,
      Color.Green,
      Color.Pink
    ];

    public static Point paddleSize = new Point(8, 48);
    public static short ballSize = 24;

    // Speed is measured in pixels per second
    public static short paddleSpeed = 256;
    public static short ballSpeedBase = 128;
    public static short ballSpeedIncrement = 32;
  }
}