using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
public class PongGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Ball _ball;
    private (Paddle paddle, int health) [] _paddles = new(Paddle, int)[4];
    
    public PongGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 640;
        _graphics.PreferredBackBufferHeight = 480;

        Window.Title = "Pong - 5463947 & 9392998";

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        
        _ball = new Ball(_graphics.GraphicsDevice.Viewport);
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
        if (Keyboard.GetState().IsKeyDown(Globals.continueKey))
        {
            Globals.gameState = GameState.InGame;
            OnGameStart();
        }
    }
    private void DrawMainMenu(GameTime gameTime)
    {
        DrawStringOnCenter("Welkom bij PONG! Druk op <SPACE> om te beginnen!");
    }
    private void OnGameStart()
    {
        _paddles[0].paddle = new Paddle(_graphics, PaddleMovementDirection.Vertical, false, Keys.S, Keys.W, Globals.playerColors[0]);
        _paddles[0].health = Globals.playerBaseHealth;
        _paddles[1].paddle = new Paddle(_graphics, PaddleMovementDirection.Vertical, true, Keys.Down, Keys.Up, Globals.playerColors[1]);
        _paddles[1].health = Globals.playerBaseHealth;

        if (Globals.gameType == GameType.FourPlayer)
        {
            _paddles[2].paddle = new Paddle(_graphics, PaddleMovementDirection.Horizontal, false, Keys.I, Keys.U, Globals.playerColors[2]);
            _paddles[2].health = Globals.playerBaseHealth;
            _paddles[3].paddle = new Paddle(_graphics, PaddleMovementDirection.Horizontal, true, Keys.B, Keys.V, Globals.playerColors[3]);
            _paddles[3].health = Globals.playerBaseHealth;
        }   
    }

    private void UpdateInGame(GameTime gameTime)
    {
        if (_ball.Position.Y <= 0 ||
            _ball.Position.Y >= _graphics.GraphicsDevice.Viewport.Height - Globals.ballSize / 2f)
            _ball.MirrorAngle(false);

        _ball.Update(gameTime);

        for (int i = 0; i < _paddles.Length && i < (Globals.gameType == GameType.TwoPlayer ? 2 : 4); i++)
            _paddles[i].paddle.Update(gameTime);
    }
    private void DrawInGame(GameTime gameTime)
    {
        _ball.Draw(_spriteBatch, gameTime);

        for (int i = 0; i < _paddles.Length && i < (Globals.gameType == GameType.TwoPlayer ? 2 : 4); i++)
            _paddles[i].paddle.Draw(_spriteBatch, gameTime);

        DrawPaddleHearts();
    }
    private void DrawPaddleHearts()
    {
        const short heartDrawSize = 32;
        const short heartDrawGap = 8;
        
        for (int i = 0; i < _paddles.Length && i < (Globals.gameType == GameType.TwoPlayer ? 2 : 4); i++)
        {
            Vector2 drawStartPos = new();
            sbyte drawDirection = 0;
            switch (i)
            {
                case 0:
                    drawStartPos = new(0, 0);
                    drawDirection = 1;
                    break;
                case 1:
                    drawStartPos = new(_graphics.GraphicsDevice.Viewport.Width, 0);
                    drawDirection = -1;
                    break;
                case 2:
                    drawStartPos = new(0, _graphics.GraphicsDevice.Viewport.Height);
                    drawDirection = 1;
                    break;
                case 3:
                    drawStartPos = new(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
                    drawDirection = -1;
                    break;
                default:
                    Console.WriteLine("DrawPaddleHearts(): unhandled i in switch case");
                    break;
            }

            for (int j = 0; j < _paddles[i].health; i++)
            {
                Point drawPos = new(
                    (int)(drawStartPos.X + drawDirection * (j * (heartDrawGap + heartDrawSize))),
                    (int)drawStartPos.Y
                );
                _spriteBatch.Draw(
                    Globals.heartTexture,
                    new Rectangle(drawPos, new(heartDrawSize)),
                    Globals.playerColors[i]
                );
            }
        }
    }

    private void UpdateGameOver(GameTime gameTime)
    {

    }
    private void DrawGameOver(GameTime gameTime)
    {

    }
        
    private void DrawStringOnCenter(string text)
    {
        Vector2 position =
            new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f,
                _graphics.GraphicsDevice.Viewport.Height / 2f) - (Globals.font.MeasureString(text) / 2f);
        _spriteBatch.DrawString(Globals.font, text, position, Globals.textColor);
    }
}
