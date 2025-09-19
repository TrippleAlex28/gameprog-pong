using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
class Globals
{
  public static Texture2D paddleTexture;
  public static Texture2D ballTexture;
  public static SpriteFont font;

  public static GameState gameState = GameState.MainMenu;
  public static GameType gameType = GameType.TwoPlayer;

  public const Keys TwoPlayersKey = Keys.F2;
  public const Keys FourPlayersKey = Keys.F4;
  public const Keys ResetKey = Keys.Space;

  public static Color backgroundColor = Color.White;
  // public static Texture2D backgroundTexture;
  public static Color textColor = Color.Black;
  public static Color[] playerColors = [
    Color.Blue,
    Color.Red,
    Color.Green,
    Color.Pink
  ];

  public static readonly Point paddleSize = new Point(8, 48);
  public const short ballSize = 24;

  // Speed is measured in pixels per second
  public static short paddleSpeed = 896;
  public static short ballSpeedBase = 128;
  public static short ballSpeedIncrement = 32;
  public const float ViewportSizeMultiplier = 640f;
}