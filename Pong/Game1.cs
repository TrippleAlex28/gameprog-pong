using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
public class Game1 : Game
{
    public GraphicsDeviceManager graphics;
    public SpriteBatch spriteBatch;

    Paddle[] playerPaddles = new Paddle[4];

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.IsFullScreen = false;
        graphics.PreferredBackBufferWidth = 640;
        graphics.PreferredBackBufferHeight = 480;

        Window.Title = "Pong - 5463947 & 9392998";

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        Globals.paddleTexture = Content.Load<Texture2D>("sprites/paddle");
        Globals.ballTexture = Content.Load<Texture2D>("sprites/ball");
        Globals.font = Content.Load<SpriteFont>("default_font");

        // create paddles and add them to the array, for some reason doing the loadcontent actions of paddles in the constructor caused an error so it's like this    
        playerPaddles[0] = new Paddle(this, PaddleMovementDirection.Vertical, false, Keys.S, Keys.W, Color.Blue);
        playerPaddles[1] = new Paddle(this, PaddleMovementDirection.Vertical, true, Keys.Down, Keys.Up, Color.Red);
        playerPaddles[2] = new Paddle(this, PaddleMovementDirection.Horizontal, false, Keys.I, Keys.U, Color.Green);
        playerPaddles[3] = new Paddle(this, PaddleMovementDirection.Horizontal, true, Keys.B, Keys.V, Color.Purple);
        for (int i = 0; i < playerPaddles.Length; i++)
        {
            playerPaddles[i].LoadContent();
        }
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
        spriteBatch.Begin();

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

        spriteBatch.End();
        base.Draw(gameTime);
    }

    private void UpdateMainMenu(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Globals.continueKey))
            Globals.gameState = GameState.InGame;
    }
    private void DrawMainMenu(GameTime gameTime)
    {
        DrawStringOnCenter("Welkom bij PONG! Druk op <SPACE> om te beginnen!");
    }

    private void UpdateInGame(GameTime gameTime)
    {
        for (int i = 0; i < playerPaddles.Length; i++)
        {
            playerPaddles[i].Update(gameTime);
        }
    }
    private void DrawInGame(GameTime gameTime)
    {
         for (int i = 0; i < playerPaddles.Length; i++)
        {
            playerPaddles[i].Draw(gameTime);
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
            new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f,
                graphics.GraphicsDevice.Viewport.Height / 2f) - (Globals.font.MeasureString(text) / 2f);
        spriteBatch.DrawString(Globals.font, text, position, Globals.textColor);
    }
}
