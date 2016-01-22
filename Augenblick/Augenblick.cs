using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Augenblick
{


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Augenblick : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private MazeGrid GameGrid;

        public Augenblick()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Begin();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void Begin()
        {
            GameGrid = MazeGrid.Generate(6);
            GameGrid.GridVisible = true;
            // GameGrid.Grid[1, 2].CellColor = Color.White;
            Layer.AddToDraw(GameGrid);
            Layer.AddToUpdate(GameGrid);

            IsMouseVisible = true;

            MouseHandler.LeftMouseClicked += delegate(Vector2 pos)
            {
                GameGrid.GetCellByCoordinates(pos).CellColor = Color.Blue;
            };
            MouseHandler.RightMouseClicked += delegate(Vector2 pos)
            {
                GameGrid.Rotate(Rotation.QuarterClockwise);
            };
            KeyboardHandler.ListenedKeys.Add(Keys.Space);
            KeyboardHandler.KeyPressed += delegate(Keys k)
            {
                GameGrid.Rotate(Rotation.QuarterClockwise);
            };

            SetWindowSize(720, 576);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseHandler.Update(gameTime);
            KeyboardHandler.Update(gameTime);

            Layer.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GameConstants.BackgroundColor);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp);

            for (int i = 0; i < Layer.Layers.Length; i++)
            {
                Layer.Layers[i].Draw(spriteBatch);
            }

            base.Draw(gameTime);
            spriteBatch.End();
        }

        void SetWindowSize(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;

            graphics.ApplyChanges();
        }
    }
}