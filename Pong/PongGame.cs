using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
public class PongGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Ball _ball;
    private Paddle[] _paddles = new Paddle[4];

    public PongGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        Window.Title = "Pong - 5463947 & 9392998";
        Window.AllowUserResizing = true;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Globals.paddleTexture = Content.Load<Texture2D>("sprites/paddle");
        Globals.ballTexture = Content.Load<Texture2D>("sprites/ball");
        Globals.font = Content.Load<SpriteFont>("default_font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (Globals.gameState)
        {
            case GameState.MainMenu:
                UpdateMainMenu(gameTime);
                break;
            case GameState.InGame:
                UpdateInGame(gameTime);
                break;
            case GameState.GameOver:
                UpdateGameOver(gameTime);
                break;
            default:
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Globals.backgroundColor);
        _spriteBatch.Begin();

        switch (Globals.gameState)
        {
            case GameState.MainMenu:
                DrawMainMenu(gameTime);
                break;
            case GameState.InGame:
                DrawInGame(gameTime);
                break;
            case GameState.GameOver:
                DrawGameOver(gameTime);
                break;
            default:
                break;
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void UpdateMainMenu(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(Globals.TwoPlayersKey))
        {
            Globals.gameState = GameState.InGame;
            Globals.gameType = GameType.TwoPlayer;
            OnGameStart();
        }
        else if (state.IsKeyDown(Globals.FourPlayersKey))
        {
            Globals.gameState = GameState.InGame;
            Globals.gameType = GameType.FourPlayer;
            OnGameStart();
        }
    }
    
    private void DrawMainMenu(GameTime gameTime)
    {
        DrawStringInCenter(0, "Welkom bij PONG!");
        DrawStringInCenter(1, "Druk op <" + Globals.TwoPlayersKey.ToString().ToUpper() + "> voor 2 player of <" + Globals.FourPlayersKey.ToString().ToUpper() + "> voor 4 player om te beginnen!");
    }

    private void OnGameStart()
    {
        Window.AllowUserResizing = false;
        
        _ball = new Ball(_graphics.GraphicsDevice.Viewport);
        
        _paddles[0] = new Paddle(_graphics, PaddleMovementDirection.Vertical, false, Keys.S, Keys.W, Globals.playerColors[0]);
        _paddles[1] = new Paddle(_graphics, PaddleMovementDirection.Vertical, true, Keys.Down, Keys.Up, Globals.playerColors[1]);

        if (Globals.gameType != GameType.FourPlayer) return;
        _paddles[2] = new Paddle(_graphics, PaddleMovementDirection.Horizontal, false, Keys.I, Keys.U, Globals.playerColors[2]);
        _paddles[3] = new Paddle(_graphics, PaddleMovementDirection.Horizontal, true, Keys.B, Keys.V, Globals.playerColors[3]);
    }
    
    private void UpdateInGame(GameTime gameTime)
    {
        if (_ball.Position.Y <= 0 ||
            _ball.Position.Y >= _graphics.GraphicsDevice.Viewport.Height - Globals.ballSize / 2f)
            _ball.MirrorAngle(false);

        _ball.Update(gameTime);
    }
    
    private void DrawInGame(GameTime gameTime)
    {
        _ball.Draw(_spriteBatch, gameTime);
    }

    private void UpdateGameOver(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Globals.ResetKey))
        {
            Window.AllowUserResizing = true;
            Globals.gameState = GameState.MainMenu;
        }
    }
    
    private void DrawGameOver(GameTime gameTime)
    {
        DrawStringInCenter(0, "Game over, press <" + Globals.ResetKey.ToString().ToUpper() + "> to exit to Main Menu");
    }
        
    private void DrawStringInCenter(short row, string text)
    {
        Vector2 stringSize = Globals.font.MeasureString(text);
        Vector2 position =
            new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f,
                _graphics.GraphicsDevice.Viewport.Height / 2f) - (stringSize / 2f) + new Vector2(0, stringSize.Y * row);
        _spriteBatch.DrawString(Globals.font, text, position, Globals.textColor);
    }
}
