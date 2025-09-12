using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Ball _ball;

        public Game1()
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
                Globals.gameState = GameState.InGame;
        }
        private void DrawMainMenu(GameTime gameTime)
        {
            DrawStringOnCenter("Welkom bij PONG! Druk op <SPACE> om te beginnen!");
        }

        private void UpdateInGame(GameTime gameTime)
        {
            _ball.Update(gameTime);
            // For debugging
            if (Keyboard.GetState().IsKeyDown(Keys.A) && gameTime.TotalGameTime.Milliseconds % 500 == 0)
                _ball.MirrorAngle(Keyboard.GetState().IsKeyDown(Keys.D));
        }
        private void DrawInGame(GameTime gameTime)
        {
            _ball.Draw(_spriteBatch, gameTime);
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
}

