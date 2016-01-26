using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System;
using System.Timers;

namespace Augenblick
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Augenblick : Game
    {
        public static Augenblick GameInstance;
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private MazeGrid GameGrid;
        private bool rotationsEnabled;
        private float inspectionTime;
        private float solveTime;
        private Difficulty currentDifficulty;

        private Timer timer;

        public Augenblick()
        {
            GameInstance = this;
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
            SetWindowSize(720, 576); // 720, 576
            IsMouseVisible = true;
            MouseHandler.MouseButtonsEnabled = true;

            CreateTitleScreen();
        }

        #region game flow

        private void CreateTitleScreen()
        {
            CreateMaze(Difficulty.Normal);
        }

        private void CreateMaze(Difficulty diff)
        {
            LevelParameters prefs;
            switch (diff)
            {
                case Difficulty.Easy:
                    prefs = GameConstants.Easy;
                    break;
                case Difficulty.Normal:
                    prefs = GameConstants.Normal;
                    break;
                case Difficulty.Hard:
                    prefs = GameConstants.Hard;
                    break;
                case Difficulty.Impossible:
                    prefs = GameConstants.Impossible;
                    break;
                default:
                    prefs = GameConstants.Normal;
                    break;
            }

            Layer.ClearAllLayers();
            MouseHandler.ClearMouseEvents();

            GameGrid = MazeGrid.Generate(prefs.SideLength);
            GameGrid.GridVisible = true;

            Layer.AddToDraw(GameGrid);
            Layer.AddToUpdate(GameGrid);

            rotationsEnabled = prefs.RotationsEnabled;
            inspectionTime = prefs.InspectionTime * 1000; // timerille millisekunteina
            solveTime = prefs.SolveTime * 1000;
            currentDifficulty = diff;

            timer = new Timer(inspectionTime);

            StartInspection();
        }

        private void StartInspection()
        {
            Timer controlDelay = new Timer(inspectionTime * GameConstants.MinimumInspectionTimePercentage);
            controlDelay.Elapsed += delegate
            {
                MouseHandler.LeftMouseClicked += delegate
                {
                    timer.Stop();
                    RotateGrid();
                };

                controlDelay.Stop();
            };
            controlDelay.Start();


            timer = new Timer();
            timer.Interval = inspectionTime;
            timer.Elapsed += delegate
            {
                timer.Stop();
                RotateGrid();
            };
            timer.Start();
        }

        private void RotateGrid()
        {
            MouseHandler.ClearMouseEvents();
            GameGrid.WallsVisible = false;

            if (rotationsEnabled)
            {
                switch (RandomGen.NextInt(0, 4))
                {
                    case 0:
                        StartSolve();
                        break;
                    case 1:
                        GameGrid.Rotate(Rotation.QuarterClockwise, GameConstants.RotationTime);
                        GameGrid.RotationFinished += StartSolve;
                        break;
                    case 2:
                        GameGrid.Rotate(Rotation.QuarterCounterClockwise, GameConstants.RotationTime);
                        GameGrid.RotationFinished += StartSolve;
                        break;
                    case 3:
                        GameGrid.Rotate(Rotation.Half, GameConstants.RotationTime);
                        GameGrid.RotationFinished += StartSolve;
                        break;
                    default:
                        break;
                }
            }
            else StartSolve();


        }

        private void StartSolve()
        {
            GameGrid.AddStartPosition();

            Timer controlDelay = new Timer(200);
            controlDelay.Elapsed += delegate
            {
                SetSolveControls();
                controlDelay.Stop();
            };
            controlDelay.Start();

            timer = new Timer();
            timer.Interval = solveTime;
            timer.Elapsed += delegate
            {
                timer.Stop();
                SolveFinished(GameGrid.VerifySelectedRoute());
            };
            timer.Start();
        }

        private void SolveFinished(Tuple<bool, Point> result)
        {
            timer.Stop();
            MouseHandler.ClearMouseEvents();

            GameGrid.WallsVisible = true;

            timer = new Timer();
            timer.Interval = 1000.0f;
            timer.Elapsed += delegate
            {
                MouseHandler.LeftMouseClicked +=
                    delegate
                    {
                        CreateMaze(currentDifficulty);
                    };
                timer.Stop();
            };
            timer.Start();

            if (result.Item1 == false) // meni väärin
                SolveFailed(result);
            else
                SolveSucceeded(result);
        }

        private void SolveFailed(Tuple<bool, Point> result)
        {
            GameGrid.Grid[result.Item2.X, result.Item2.Y].Blink(GameConstants.BlinkingColor, GameConstants.BlinkingSpeed);
        }

        private void SolveSucceeded(Tuple<bool, Point> result)
        {

        }

        #endregion


        private void SetSolveControls()
        {
            MouseHandler.LeftMouseClicked += delegate(Vector2 pos)
            {
                Point? cell = GameGrid.GetPointByCoordinates(pos);
                if (cell != null)
                {
                    Point last = GameGrid.SelectedRoute[GameGrid.SelectedRoute.Count - 1];

                    if (cell.Value.X != last.X && cell.Value.Y != last.Y) // ei saa laittaa viistosti
                        return;

                    // bestest code ever :D
                    if (last.X < cell.Value.X)
                        for (int i = last.X; i <= cell.Value.X; i++)
                        {
                            GameGrid.SelectedRoute.Add(new Point(i, cell.Value.Y));
                        }
                    if (last.X > cell.Value.X)
                        for (int i = last.X; i >= cell.Value.X; i--)
                        {
                            GameGrid.SelectedRoute.Add(new Point(i, cell.Value.Y));
                        }
                    if (last.Y < cell.Value.Y)
                        for (int i = last.Y; i <= cell.Value.Y; i++)
                        {
                            GameGrid.SelectedRoute.Add(new Point(cell.Value.X, i));
                        }
                    if (last.Y > cell.Value.Y)
                        for (int i = last.Y; i >= cell.Value.Y; i--)
                        {
                            GameGrid.SelectedRoute.Add(new Point(cell.Value.X, i));
                        }

                    if (GameGrid.Grid[cell.Value.X, cell.Value.Y].Type == CellType.End)
                        SolveFinished(GameGrid.VerifySelectedRoute());
                }
            };
            MouseHandler.RightMouseClicked += delegate(Vector2 pos)
            {
                Point? cell = GameGrid.GetPointByCoordinates(pos);
                if (cell != null)
                {
                    int index = GameGrid.SelectedRoute.IndexOf((Point)cell);
                    if (index != -1 && index != 0)
                        GameGrid.SelectedRoute.RemoveRange(index, GameGrid.SelectedRoute.Count - index);
                }
            };
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