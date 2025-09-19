using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
public class PongGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Rectangle _prevWindowSize;
    private float _resizeTextTimeLeft;

    private Ball _ball;
    private (Paddle paddle, ushort health) [] _paddles = new(Paddle, ushort)[4];
    
    public PongGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        Window.Title = "Pong - 5463947 & 9392998";
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += Window_ClientSizeChanged;

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

        _prevWindowSize = new Rectangle(Window.Position.X, Window.Position.Y, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Globals.paddleTexture = Content.Load<Texture2D>("sprites/paddle");
        Globals.heartTexture = Content.Load<Texture2D>("sprites/heart");
        Globals.ballTexture = Content.Load<Texture2D>("sprites/ball");
        Globals.font = Content.Load<SpriteFont>("default_font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Globals.gameState = GameState.MainMenu;

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
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
    
    #region MainMenu

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
    
    #endregion

    #region InGame
    
    private void UpdateInGame(GameTime gameTime)
    {
        // Ball collision and update
        if (_ball.Position.Y <= 0 ||
            _ball.Position.Y >= _graphics.GraphicsDevice.Viewport.Height - Globals.ballSize / 2f)
            _ball.MirrorAngle(false);

        _ball.Update(gameTime);

        // Paddle health and update
        for (int i = 0; i < _paddles.Length && i < (Globals.gameType == GameType.TwoPlayer ? 2 : 4); i++)
        {
            _paddles[i].paddle.Update(gameTime);
            if (_paddles[i].health == 0)
            {
                Globals.gameState = GameState.GameOver;
            }
        }
        
        if (_resizeTextTimeLeft > 0)
            _resizeTextTimeLeft -= gameTime.ElapsedGameTime.Milliseconds;
    }
    
    private void DrawInGame(GameTime gameTime)
    {
        _ball.Draw(_spriteBatch, gameTime);

        for (int i = 0; i < _paddles.Length && i < (Globals.gameType == GameType.TwoPlayer ? 2 : 4); i++)
            _paddles[i].paddle.Draw(_spriteBatch, gameTime);
        
        if (_resizeTextTimeLeft > 0) 
        {
            String text = "You cannot resize when game is running";
            _spriteBatch.DrawString(Globals.font, text, new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f - Globals.font.MeasureString(text).X / 2f, 0), Globals.textColor);
        }
        
        Health.DrawPaddleHearts(_spriteBatch, _graphics.GraphicsDevice.Viewport, _paddles.Select(p => p.health).ToArray());
    }
    
    #endregion

    #region GameOver
    
    private void UpdateGameOver(GameTime gameTime) { }
    
    private void DrawGameOver(GameTime gameTime)
    {
        DrawStringInCenter(0, "Game over, press <" + Globals.ResetKey.ToString().ToUpper() + "> to exit to Main Menu");
    }
    
    #endregion

    #region Events
    
    private void Window_ClientSizeChanged(object sender, EventArgs e) => OnWindowResize();
    
    private void OnGameStart()
    {
        //Window.AllowUserResizing = false;

        _ball = new Ball(_graphics.GraphicsDevice.Viewport);

        _paddles[0].paddle = new Paddle(_graphics, PaddleMovementDirection.Vertical, false, Keys.S, Keys.W, Globals.playerColors[0]);
        _paddles[0].health = Globals.playerBaseHealth;
        _paddles[1].paddle = new Paddle(_graphics, PaddleMovementDirection.Vertical, true, Keys.Down, Keys.Up, Globals.playerColors[1]);
        _paddles[1].health = Globals.playerBaseHealth;

        if (Globals.gameType != GameType.FourPlayer) return;

        _paddles[2].paddle = new Paddle(_graphics, PaddleMovementDirection.Horizontal, false, Keys.I, Keys.U, Globals.playerColors[2]);
        _paddles[2].health = Globals.playerBaseHealth;
        _paddles[3].paddle = new Paddle(_graphics, PaddleMovementDirection.Horizontal, true, Keys.B, Keys.V, Globals.playerColors[3]);
        _paddles[3].health = Globals.playerBaseHealth;
    }
    
    private void OnWindowResize()
    {
        if (Globals.gameState == GameState.InGame)
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _prevWindowSize.Width;
            _graphics.PreferredBackBufferHeight = _prevWindowSize.Height;
            _graphics.ApplyChanges();
            
            Window.Position = new Point(_prevWindowSize.X, _prevWindowSize.Y);

            _resizeTextTimeLeft = 2000;
        }
        else
        {
            _prevWindowSize = new Rectangle(Window.Position.X, Window.Position.Y, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        }
    }
    
    #endregion
        
    private void DrawStringInCenter(short row, string text)
    {
        Vector2 stringSize = Globals.font.MeasureString(text);
        Vector2 position =
            new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f,
                _graphics.GraphicsDevice.Viewport.Height / 2f) - (stringSize / 2f) + new Vector2(0, stringSize.Y * row);
        _spriteBatch.DrawString(Globals.font, text, position, Globals.textColor);
    }
}
