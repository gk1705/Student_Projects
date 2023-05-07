using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using TileMap;
using BloomPostprocess;
using System.IO;

namespace TES_TriangleEscapeSimulator
{
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
            Credits,
            Playing,
            Highscore,
        }

        //Game Elements
        GameState CurrentGameState = GameState.MainMenu;
        Button bPlay;
        Button bCredits;
        Button bMenu;
        SpriteFont DistanceMeter;
        SpriteFont Timer;
        int distance = 0;
        //To store the scores;
        public readonly string highScoresFilename = "highscores.lst";
        string fullpath;

        //Player-Related
        public static Camera camera;
        Player player;
        NameInputManager playerName;

        //Map
        Map map;
        Map dmgmap;

        //Towers
        List<Tower> towers = new List<Tower>();
        Tower tower1;
        Tower tower2;
        Tower tower3;
        Tower tower4;
        Tower tower5;
        Tower tower6;
        Tower tower7;

        //Moving Objects
        List<MovingObject> movingObjects = new List<MovingObject>();
        MovingObject mobject1;
        MovingObject mobject2;
        MovingObject mobject3;
        MovingObject mobject4;
        MovingObject mobject5;

        //Switches and Exits
        List<SwitchandExit> swExits = new List<SwitchandExit>();
        SwitchandExit swexit1;
        SwitchandExit swexit2;
        SwitchandExit swexit3;
        SwitchandExit swexit4;

        //Backgrounds
        /*Texture2D tutorialTexture;
        Vector2 tutorialPosition;*/

        //Overlay Screens
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

            // TODO: use this.Content to load your game content here
            camera = new Camera(GraphicsDevice.Viewport);
            player = new Player(new Vector2(580f, 2150f), Content.Load<Texture2D>("trianglefinal"));
            map = new Map();
            dmgmap = new Map();

            //tutorialTexture = Content.Load<Texture2D>("Tutorial");
            //tutorialPosition = new Vector2(330f, 1985f);

            Tiles.Content = Content;
            Tower.Content = Content;
            SwitchandExit.Content = Content;
            HighScore.Content = Content;

            //Name Input
            playerName = new NameInputManager();

            #region handplaced content
            //Standard Collision Tiles
            map.Generate(new int[,] {
                 {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                 {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1},
                 {1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
                 {1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1},
                 {1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                 {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                 {1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                 {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                 {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                 {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                 {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                 {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                 {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
                 {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
                 {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
                 {0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                                    }, 64);

            //Overlay for DMG-Tiles
            dmgmap.Generate(new int[,] {
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3, 3, 0},
                 {0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3, 3, 0},
                 {0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0},
                 {0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 3, 3, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    }, 64);

            //Towers
            tower1 = new Tower(Content.Load<Texture2D>("tower"), new Vector2(1122f, map.height - 160f));
            tower2 = new Tower(Content.Load<Texture2D>("tower"), new Vector2(288f, 1552f));
            tower3 = new Tower(Content.Load<Texture2D>("tower"), new Vector2(map.width - 224f, 1512f));
            tower4 = new Tower(Content.Load<Texture2D>("tower"), new Vector2(352f, 632f));
            tower5 = new Tower(Content.Load<Texture2D>("tower"), new Vector2(642f, 416f));
            tower6 = new Tower(Content.Load<Texture2D>("tower"), new Vector2(map.width - 414f, 1019f));
            tower7 = new Tower(Content.Load<Texture2D>("tower"), new Vector2(map.width - 288f, 288f));
            towers.Add(tower1);
            towers.Add(tower2);
            towers.Add(tower3);
            towers.Add(tower4);
            towers.Add(tower5);
            towers.Add(tower6);
            towers.Add(tower7);

            //Moving Objects
            mobject1 = new MovingObject(Content.Load<Texture2D>("spikequad_2"), new Vector2(700f, 1152f), 5, 0);
            mobject2 = new MovingObject(Content.Load<Texture2D>("spikequad_3"), new Vector2(1216f, 900f), 0, 5);
            mobject3 = new MovingObject(Content.Load<Texture2D>("spikequad"), new Vector2(500f, 64f), 5, 0);
            mobject4 = new MovingObject(Content.Load<Texture2D>("spikequad"), new Vector2(800f, 128f), 5, 0);
            mobject5 = new MovingObject(Content.Load<Texture2D>("spikequad_3"), new Vector2(64f, 64f), 0, 2);
            movingObjects.Add(mobject1);
            movingObjects.Add(mobject2);
            movingObjects.Add(mobject3);
            movingObjects.Add(mobject4);
            movingObjects.Add(mobject5);

            //(Switch)Keys and Exits
            swexit1 = new SwitchandExit(new Vector2(895f, 2365f), new Vector2(350f, 2000f), new Vector2(400f, 1850f));
            swexit2 = new SwitchandExit(new Vector2(map.width - 192, 1215f), new Vector2(64f, 1152f), new Vector2(132f, 1000f));
            swexit3 = new SwitchandExit(new Vector2(320f, 1015f), new Vector2(256f, 768f), new Vector2(550f, 1015f));
            swexit4 = new SwitchandExit(new Vector2(64f, 64f), new Vector2(896f, 384f), new Vector2(-100f, -100f));
            //swexit4 = new SwitchandExit(new Vector2(256f + 64f, 416f + 32f), new Vector2(896f, 384f), new Vector2(-100f, -100f));
            swExits.Add(swexit1);
            swExits.Add(swexit2);
            swExits.Add(swexit3);
            swExits.Add(swexit4);

            #endregion

            //Menu Buttons
            bPlay = new Button(Content.Load<Texture2D>("ButtonPlay"), graphics.GraphicsDevice);
            bPlay.SetPosition(new Vector2(200f, 500f));
            bCredits = new Button(Content.Load<Texture2D>("ButtonCredits"), graphics.GraphicsDevice);
            bCredits.SetPosition(new Vector2(100f, 600f));
            bMenu = new Button(Content.Load<Texture2D>("ButtonMenu"), graphics.GraphicsDevice);
            bMenu.SetPosition(new Vector2(650f, 600f));

            //Score
            DistanceMeter = Content.Load<SpriteFont>(@"Fonts\PandoraLimiter");
            Timer = Content.Load<SpriteFont>(@"Fonts\Timer");

            //Pause Screen
            pauseTexture = Content.Load<Texture2D>("pscreen");
            pauseRectangle = new Rectangle(0, 0, pauseTexture.Width, pauseTexture.Height);
            //GameOver Screen
            gameOverTexture = Content.Load<Texture2D>("goscreen");
            gameOverRectangle = new Rectangle(0, 0, gameOverTexture.Width, gameOverTexture.Height);
            //Win Screen
            winTexture = Content.Load<Texture2D>("Credits_finished");
            winRectangle = new Rectangle(0, 0, winTexture.Width, winTexture.Height);
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

            //For debug purposes only
            MouseState mouse = Mouse.GetState();
            Debug.WriteLine("X: {0}, Y: {1}", mouse.X, mouse.Y);

            //Used to define current state of play
            switch (CurrentGameState) {
                case GameState.MainMenu:
                    if (bPlay.isClicked) CurrentGameState = GameState.Playing;
                    if (bCredits.isClicked) CurrentGameState = GameState.Credits;
                    bPlay.Update(mouse);
                    bCredits.Update(mouse);
                    playerName.Update(gameTime);
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
                            ResetGame(player, swExits);
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
                        player.Update(map, dmgmap, towers, movingObjects);
                        foreach(Tower t in towers) t.Update(player, map, (float)gameTime.ElapsedGameTime.TotalSeconds);
                        foreach(MovingObject mo in movingObjects) mo.Update(map);
                        foreach(SwitchandExit se in swExits) se.Update(player);
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
                            ResetGame(player, swExits);
                            paused = false;
                        }
                    }
                    //win condition, last exit was touched;
                    if (player.playerPosition == new Vector2(-100f, -100f))
                    {
                        winRectangle = new Rectangle((int)camera.centre.X, (int)camera.centre.Y, (int)(winTexture.Width / camera.scale), (int)(winTexture.Height / camera.scale));
                        won = true;
                    }

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
                    dmgmap.Draw(spriteBatch);
                    foreach (Tower t in towers)
                    {
                        t.Draw(spriteBatch);
                        t.DrawBullets(spriteBatch);
                    }
                    foreach(MovingObject mo in movingObjects)
                    {
                        mo.Draw(spriteBatch);
                    }
                    foreach(SwitchandExit se in swExits)
                    {
                        se.Draw(spriteBatch);
                    }
                    //spriteBatch.Draw(tutorialTexture, tutorialPosition, Color.White);
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
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Rectangle(0, 0, screenwidth, screenheight), Color.White);
                    bPlay.Draw(spriteBatch);
                    bCredits.Draw(spriteBatch);
                    spriteBatch.DrawString(Timer, "Current player: " + playerName.name, Vector2.Zero, Color.Black);
                    break;

                case GameState.Playing:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                         camera.transform);
                    if (!gameOver && !paused)
                    {
                        //timer is drawn top left corner;
                        spriteBatch.DrawString(Timer,"Time: " + totalGameTime.ToString("0.00"), new Vector2(camera.centre.X, camera.centre.Y), Color.White);
                    }
                    //drawn near the player character
                    spriteBatch.DrawString(DistanceMeter, (distance / 100).ToString(), player.playerPosition - new Vector2(player.playerTexture.Width / 2 + 12, player.playerTexture.Height / 2 + 12), Color.White);
                    if (paused)
                    {
                        spriteBatch.Draw(pauseTexture, pauseRectangle, Color.White);
                    }
                    else if (gameOver)
                    {
                        spriteBatch.Draw(gameOverTexture, gameOverRectangle, Color.White);
                    }
                    if (won)
                    {
                        spriteBatch.Draw(winTexture, winRectangle, Color.White);
                    }
                    break;

                case GameState.Credits:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Credits"), new Rectangle(0, 0, screenwidth, screenheight), Color.White);
                    bMenu.Draw((spriteBatch));
                    spriteBatch.End();
                    break;
            }
            spriteBatch.End();
        } //End Draw

        void ResetGame(Player player, List<SwitchandExit> swExits)
        {
            player.hp = 1;
            player.playerVelocity = Vector2.Zero;
            player.playerPosition = new Vector2(580f, 2150f);
            foreach (SwitchandExit se in swExits) se.isActive = false;
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
    } //End Class
}