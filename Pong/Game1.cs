using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = Globals.isFullScreen;
            _graphics.PreferredBackBufferWidth = Globals.windowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.windowSize.Y;

            Window.Title = "Pong - 5463947 & 9392998";

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

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
            Globals.spriteBatch.Begin();

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

            Globals.spriteBatch.End();
            base.Draw(gameTime);
        }

        private void UpdateMainMenu(GameTime gameTime)
        {

        }
        private void DrawMainMenu(GameTime gameTime)
        {
            DrawStringOnCenter("Welkom bij PONG! Druk op <SPACE> om te beginnen!");
        }

        private void UpdateInGame(GameTime gameTime)
        {

        }
        private void DrawInGame(GameTime gameTime)
        {

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
            Globals.spriteBatch.DrawString(Globals.font, text, position, Globals.textColor);
        }
    }
}

