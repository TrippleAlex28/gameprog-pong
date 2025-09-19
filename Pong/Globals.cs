using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;

class Globals
{
    public static Texture2D paddleTexture;
    public static Texture2D heartTexture;
    public static Texture2D ballTexture;
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

    public const int playerBaseHealth = 3;

    public static readonly Point paddleSize = new Point(8, 48);
    public const short ballSize = 24;

    // Speed is measured in pixels per second
    public static short paddleSpeed = 256;
    public static short ballSpeedBase = 256;
    public static short ballSpeedIncrement = 32;
  
    public const short heartDrawSize = 32;
    public const short heartDrawGap = 8;
    public const short heartEdgePadding = 16;
  
    // Game initially was made for 720p resolution.
    // So if window is resized it is compared to initial value that was game made for and values are scaled.
    public const short ViewportSizeMultiplier = 1280;
}