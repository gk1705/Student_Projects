using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using TileMap;
using BloomPostprocess;
using System.IO;
using TiledSharp;

namespace TES_TriangleEscapeSimulator
{
    //TILED was used under the GNU GENERAL PUBLIC LICENSE;
    //The tiled-sharp library was used under the Apache License;

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int screenwidth = 1024;
        public static int screenheight = 768;
        public static float totalGameTime = 0;

        enum GameState
        {
            MainMenu,
            NameInput,
            HowTo,
            ChooseLevel,
            Credits,
            Playing,
            Highscore,
            Won
        }

        //Game Elements;
        GameState CurrentGameState;
        Button bPlay;
        Button bCredits;
        Button bMenu;
        Button bHowTo;
        SpriteFont DistanceMeter;
        SpriteFont Timer;
        SpriteFont NameFont;
        int distance = 0;
        //To store the scores;
        public readonly string highScoresFilename = "highscores.lst";
        string fullpath;

        //Player-Related;
        public static Camera camera;
        Player player;
        NameInputManager playerName;

        //Map;
        Map map;
        //TmxMaps;
        TmxMap tmxMap1;
        TmxMap tmxMap2;
        TmxMap tmxMap3;
        TmxMap tmxMap4;
        TmxMap tmxMap5;
        TmxMap tmxMap6;
        //TmxMapLoader stores all the tiles and objects we intend to load on our map;
        TmxMapLoader tmxLoader;

        //Lvl-Buttons;
        Button blvl1;
        Button blvl2;
        Button blvl3;
        Button blvl4;
        Button blvl5;
        Button blvl6;
        List<Button> buttons;

        //Overlay Screens;
        bool paused = false;
        bool gameOver = false;
        bool won = false;
        Texture2D pauseTexture;
        Rectangle pauseRectangle;
        Texture2D gameOverTexture;
        Rectangle gameOverRectangle;
        Texture2D winTexture;
        Rectangle winRectangle;

        //For the bloom effect
        BloomComponent bloom;

        //PathFinder
        PathFinding.PathFinder finder; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            fullpath = Path.Combine(Content.RootDirectory, highScoresFilename);

            bloom = new BloomComponent(this);
            Components.Add(bloom);
            //Default
            //bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
            //Custom
            bloom.Settings = new BloomSettings(null, 0.25f, 0.3f, 2, 1, 2, 1);
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
            this.IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = screenheight;
            graphics.PreferredBackBufferWidth = screenwidth;
            graphics.ApplyChanges();

            #region Highscore
            //path to savegame
            //Checke whether file exists
            if (!File.Exists(fullpath))
            {
                //Fake file :)
                //Create artificial save data
                HighScoreData data = new HighScoreData(3);
                data.playerName[0] = "Gab";
                data.time[0] = 79;
                data.distance[0] = 217;

                data.playerName[1] = "Ben";
                data.time[1] = 84;
                data.distance[1] = 301;

                data.playerName[2] = "Stefan";
                data.time[2] = 91;
                data.distance[2] = 305;

                HighScore.SaveHighScores(data, fullpath);
            }
            #endregion

            tmxMap1 = new TmxMap("LVLs/map1.tmx");
            tmxMap2 = new TmxMap("LVLs/map2.tmx");
            tmxMap3 = new TmxMap("LVLs/map3.tmx");
            tmxMap4 = new TmxMap("LVLs/map4.tmx");
            tmxMap5 = new TmxMap("LVLs/map5.tmx");
            tmxMap6 = new TmxMap("LVLs/map6.tmx");

            CurrentGameState = GameState.NameInput;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            TmxMapLoader.Content = Content;
            Tiles.Content = Content;
            Tower.Content = Content;
            SwitchandExit.Content = Content;
            HighScore.Content = Content;

            // Create a new SpriteBatch, which can be used to draw textures;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load Map beforehand;
            //tmxLoader.LoadMap();

            // TODO: use this.Content to load your game content here;
            camera = new Camera(GraphicsDevice.Viewport);
            player = new Player(/*tmxLoader.playerStartPosition*/ Vector2.Zero, Content.Load<Texture2D>("trianglefinal"));
            map = new Map();

            //Name Input;
            playerName = new NameInputManager();

            //Menu Buttons;
            bPlay = new Button(Content.Load<Texture2D>("ButtonPlay"), graphics.GraphicsDevice);
            bPlay.SetPosition(new Vector2(200f, 500f));
            bCredits = new Button(Content.Load<Texture2D>("ButtonCredits"), graphics.GraphicsDevice);
            bCredits.SetPosition(new Vector2(100f, 600f));
            bMenu = new Button(Content.Load<Texture2D>("ButtonMenu"), graphics.GraphicsDevice);
            bMenu.SetPosition(new Vector2(650f, 600f));
            bHowTo = new Button(Content.Load<Texture2D>("ButtonHowTo"), graphics.GraphicsDevice);
            bHowTo.SetPosition(new Vector2(400f, 400f));

            //Lvl-Select Buttons;
            buttons = new List<Button>();
            blvl1 = new Button(Content.Load<Texture2D>("ButtonLVL1"), graphics.GraphicsDevice);
            blvl1.SetPosition(new Vector2(220f, 210f));
            buttons.Add(blvl1);
            blvl2 = new Button(Content.Load<Texture2D>("ButtonLVL2"), graphics.GraphicsDevice);
            blvl2.SetPosition(new Vector2(520f, 210f));
            buttons.Add(blvl2);
            blvl3 = new Button(Content.Load<Texture2D>("ButtonLVL3"), graphics.GraphicsDevice);
            blvl3.SetPosition(new Vector2(220f, 310f));
            buttons.Add(blvl3);
            blvl4 = new Button(Content.Load<Texture2D>("ButtonLVL4"), graphics.GraphicsDevice);
            blvl4.SetPosition(new Vector2(520f, 310f));
            buttons.Add(blvl4);
            blvl5 = new Button(Content.Load<Texture2D>("ButtonLVL5"), graphics.GraphicsDevice);
            blvl5.SetPosition(new Vector2(220f, 410f));
            buttons.Add(blvl5);
            blvl6 = new Button(Content.Load<Texture2D>("ButtonLVL6"), graphics.GraphicsDevice);
            blvl6.SetPosition(new Vector2(520f, 410f));
            buttons.Add(blvl6);

            //Score
            DistanceMeter = Content.Load<SpriteFont>(@"Fonts\PandoraLimiter");
            Timer = Content.Load<SpriteFont>(@"Fonts\Timer");
            NameFont = Content.Load<SpriteFont>(@"Fonts\PandoraLimiter2");

            //Pause Screen
            pauseTexture = Content.Load<Texture2D>("pscreen");
            pauseRectangle = new Rectangle(0, 0, pauseTexture.Width, pauseTexture.Height);
            //GameOver Screen
            gameOverTexture = Content.Load<Texture2D>("goscreen");
            gameOverRectangle = new Rectangle(0, 0, gameOverTexture.Width, gameOverTexture.Height);
            //Win Screen
            winTexture = Content.Load<Texture2D>("Credits_finished");
            winRectangle = new Rectangle(0, 0, winTexture.Width, winTexture.Height);

            //Testing the astar algorithm;
            finder = new PathFinding.PathFinder();
            int[,] tempmap = new int[,]
            {
                    {0, 1, 0, 0, 0, 0, 0, 0},
                    {0, 1, 0, 0, 0, 1, 1, 0},
                    {0, 0, 1, 0, 0, 1, 0, 0},
                    {0, 0, 1, 1, 0, 1, 0, 0},
                    {0, 0, 0, 0, 0, 0, 1, 0},
            };

            foreach (Node n in finder.FindPath(new Vector2(320, 2), new Vector2(0, 0), tempmap))
                Debug.WriteLine(n.gridY + " | " + n.gridX);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
            //We'll need the state to check if we hover over a button;
            MouseState mouse = Mouse.GetState();
            //Debug.WriteLine("X: {0}, Y: {1}", mouse.X, mouse.Y);

            //Define Current State of Play:
            switch (CurrentGameState) {
                //The player is able to input a name at the beginning, which would later be used when saving the highscore;
                case GameState.NameInput:
                    playerName.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) CurrentGameState = GameState.MainMenu;
                    break;

                case GameState.MainMenu:
                    if (bPlay.isClicked) CurrentGameState = GameState.ChooseLevel;
                    if (bCredits.isClicked) CurrentGameState = GameState.Credits;
                    if (bHowTo.isClicked) CurrentGameState = GameState.HowTo;
                    bPlay.Update(mouse);
                    bCredits.Update(mouse);
                    bHowTo.Update(mouse);
                    break;

                case GameState.HowTo:
                    if (bMenu.isClicked) CurrentGameState = GameState.MainMenu;
                    bMenu.Update(mouse);
                    break;

                case GameState.ChooseLevel:
                    if (bMenu.isClicked) CurrentGameState = GameState.MainMenu;
                    bMenu.Update(mouse);
                    //Choose the lvl by clicking the related button;
                    if (blvl1.isClicked) LoadLevel(tmxMap1);
                    if (blvl2.isClicked) LoadLevel(tmxMap2);
                    if (blvl3.isClicked) LoadLevel(tmxMap3);
                    if (blvl4.isClicked) LoadLevel(tmxMap4);
                    if (blvl5.isClicked) LoadLevel(tmxMap5);
                    if (blvl6.isClicked) LoadLevel(tmxMap6);

                    foreach (Button b in buttons) b.Update(mouse);
                    break;

                case GameState.Credits:
                    if (bMenu.isClicked) CurrentGameState = GameState.MainMenu;
                    bMenu.Update(mouse);
                    break;

                case GameState.Playing:
                    if (gameOver)
                    {
                        gameOverRectangle = new Rectangle((int)camera.centre.X, (int)camera.centre.Y, (int)(gameOverTexture.Width / camera.scale), (int)(gameOverTexture.Height / camera.scale));
                        if (Keyboard.GetState().IsKeyDown(Keys.Q)) Exit();
                        if (Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            distance = 0;
                            ResetGame(player, tmxLoader.swExits);
                            gameOver = false;
                        }
                    }
                    else if (!paused && !won)
                    {
                        totalGameTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
                        if (Keyboard.GetState().IsKeyDown(Keys.P))
                        {
                            paused = true;
                        }
                        //Run updates
                        player.Update(map, tmxLoader.towers, tmxLoader.movingObjects);
                        foreach(Tower t in tmxLoader.towers) t.Update(player, map, (float)gameTime.ElapsedGameTime.TotalSeconds);
                        foreach(MovingObject mo in tmxLoader.movingObjects) mo.Update(map);
                        foreach(SwitchandExit se in tmxLoader.swExits) se.Update(player);
                        camera.Update(gameTime, this, player, map);
                        distance += (int) player.playerVelocity.Length();
                        //Game Over?
                        if (player.hp <= 0) gameOver = true;
                    } else if (paused)
                    {
                        pauseRectangle = new Rectangle((int)camera.centre.X, (int)camera.centre.Y,(int) (pauseTexture.Width / camera.scale), (int) (pauseTexture.Height / camera.scale));
                        if (Keyboard.GetState().IsKeyDown(Keys.C)) paused = false;
                        if (Keyboard.GetState().IsKeyDown(Keys.Q)) Exit();
                        if (Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            distance = 0;
                            ResetGame(player, tmxLoader.swExits);
                            paused = false;
                        }
                    }
                    //win condition, last exit was touched, (out of map);
                    if (player.playerPosition.X < Vector2.Zero.X || player.playerPosition.Y < Vector2.Zero.Y)
                    {
                        CurrentGameState = GameState.Won;
                    }
                    //break play-state;
                    break;

                case GameState.Won:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) CurrentGameState = GameState.ChooseLevel;
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //what needs to be drawn with the custom bloom effect
            bloom.BeginDraw();
            GraphicsDevice.Clear(Color.TransparentBlack);
            
            //used to define state of play
            switch (CurrentGameState)
            {
                case GameState.Playing:
                    bloom.BeginDraw();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                         camera.transform);
                    map.Draw(spriteBatch);
                    foreach (Tower t in tmxLoader.towers)
                    {
                        t.Draw(spriteBatch);
                        t.DrawBullets(spriteBatch);
                    }
                    foreach(MovingObject mo in tmxLoader.movingObjects)
                    {
                        mo.Draw(spriteBatch);
                    }
                    foreach(SwitchandExit se in tmxLoader.swExits)
                    {
                        se.Draw(spriteBatch);
                    }
                    if (!gameOver)
                    {
                        spriteBatch.Draw(player.playerTexture, player.playerPosition, null, Color.White,
                        player.rotation, player.playerOrigin, 1f, SpriteEffects.None, 0);
                    }
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);

            //what needs to be drawn without bloom without bloom
            switch (CurrentGameState)
            {
                case GameState.NameInput:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("NameInputBG"), new Rectangle(0, 0, screenwidth, screenheight), Color.White);
                    spriteBatch.DrawString(NameFont, "Name:  " + playerName.name, new Vector2(screenheight / 2f - 250f, screenwidth / 2f - 50f), Color.White);
                    break;

                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Rectangle(0, 0, screenwidth, screenheight), Color.White);
                    bPlay.Draw(spriteBatch);
                    bCredits.Draw(spriteBatch);
                    bHowTo.Draw(spriteBatch);
                    spriteBatch.DrawString(Timer, "Current player: " + playerName.name, new Vector2(20f, 20f), Color.Black);
                    break;

                case GameState.HowTo:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("HowTo"), new Rectangle(0, 0, screenwidth, screenheight), Color.White);
                    bMenu.Draw((spriteBatch));
                    break;

                case GameState.ChooseLevel:
                    spriteBatch.Begin();
                    bMenu.Draw((spriteBatch));
                    foreach (Button b in buttons) b.Draw(spriteBatch);
                    break;

                case GameState.Playing:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                         camera.transform);
                    if (!gameOver && !paused)
                    {
                        //timer is drawn top left corner;
                        spriteBatch.DrawString(Timer,"Time: " + totalGameTime.ToString("0.00"), new Vector2(camera.centre.X, camera.centre.Y), Color.White);
                    }
                    //drawn near the player character;
                    spriteBatch.DrawString(DistanceMeter, (distance / 100).ToString(), player.playerPosition - new Vector2(player.playerTexture.Width / 2 + 12, player.playerTexture.Height / 2 + 12), Color.White);
                    if (paused)
                    {
                        spriteBatch.Draw(pauseTexture, pauseRectangle, Color.White);
                    }
                    else if (gameOver)
                    {
                        spriteBatch.Draw(gameOverTexture, gameOverRectangle, Color.White);
                    }
                    break;

                case GameState.Credits:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Credits"), new Rectangle(0, 0, screenwidth, screenheight), Color.White);
                    bMenu.Draw((spriteBatch));
                    spriteBatch.End();
                    break;

                case GameState.Won:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Credits_finished"), new Rectangle(0, 0, screenwidth, screenheight), Color.White);
                    spriteBatch.End();
                    break;
            }
            spriteBatch.End();
        } //End Draw

        void ResetGame(Player player, List<SwitchandExit> swExits)
        {
            player.hp = 1;
            player.playerVelocity = Vector2.Zero;
            player.playerPosition = tmxLoader.playerStartPosition;
            foreach (SwitchandExit se in tmxLoader.swExits) se.isActive = false;
            totalGameTime = 0;
        }

        void SaveHighScore()
        {
            //We load the data to check and then save our score;
            HighScoreData data = HighScore.LoadHighScores(fullpath);

            int scoreIndex = -1;
            for(int i = 0; i < data.count; i++)
            {
                if (totalGameTime < data.time[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                //We found a new highscore. Now we need to swap;
                for (int i = data.count - 1; i > scoreIndex; i--)
                {
                    data.playerName[i] = data.playerName[i - 1];
                    data.time[i] = data.time[i - 1];
                    data.distance[i] = data.distance[i - 1];
                }
            }

            //Insert the new score;
            data.playerName[scoreIndex] = playerName.name;
            data.time[scoreIndex] = totalGameTime;
            data.distance[scoreIndex] = distance;

            HighScore.SaveHighScores(data, fullpath);
        }

        void LoadLevel(TmxMap tmxMap)
        {
            tmxLoader = new TmxMapLoader(tmxMap);
            tmxLoader.LoadMap();
            CurrentGameState = GameState.Playing;
            player.playerPosition = tmxLoader.playerStartPosition;
            map.Generate(tmxLoader.mapTiles, 64);
        }
    } //End Class
}