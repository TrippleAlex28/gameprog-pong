using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;

class Globals
{
  public static Texture2D paddleTexture;
  public static Texture2D heartTexture;
  public static Texture2D ballTexture;

  public static Texture2D pixelTexture;

  public static SpriteFont font;

  public static GameState gameState = GameState.MainMenu;
  public static GameType gameType = GameType.TwoPlayer;

  public const Keys TwoPlayersKey = Keys.F2;
  public const Keys FourPlayersKey = Keys.F4;
  public const Keys ResetKey = Keys.Space;

  public static Color backgroundColor = Color.White;
  public static Color textColor = Color.Black;
  public static Color[] playerColors = [
    Color.Blue,
    Color.Red,
    Color.Green,
    Color.Pink
  ];

  public const short playerBaseHealth = 3;

  public static readonly Point basePaddleSize = new Point(12, 96);
  public const short baseBallSize = 32;

  // Speed is measured in pixels per second
  public const short paddleSpeed = 384;
  public const short ballSpeedBase = 256;
  public const short ballSpeedIncrement = 32;

  // Game initially was made for 720p resolution.
  // So if window is resized it is compared to initial value that was game made for and values are scaled.
  public static readonly Point ViewportSizeMultiplier = new(1280, 720);
}