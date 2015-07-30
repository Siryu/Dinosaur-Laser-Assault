using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DinosaurLazers.Models;
using DinosaurLazers.Controls;
using DinosaurLazers.Enemies;
using DinosaurLazers.GameStates;

namespace DinosaurLazers
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GameState gameState;
        GameState previousState;

        Upgrade upgradeScreen;
        StartScreen startScreen;
        CharacterSelectionScreen characterScreen;
        Lobby multiplayerLobby;
        Death deathScreen;
        FadeScreen fadeScreen;
        StartLevel levelBeginning;
        PauseScreen pauseScreen;
        LoadingScreen loadingScreen;
        CreditScreen creditScreen;

        LevelCreator levelCreator;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // load up the level here
        Random rand = new Random();
        Level currentLevel;
       // Background background;
        List<Sprite> enemies;
        List<AnimatedSprite> explosions;
        ShotBehavior shotBehavior;
        EnemyShots enemyShots;
        PlayerShots playerShots;
        HealthBar hbOne, hbTwo;
        HealthBar bossLifeBar;
        ChargeMeter cbOne, cbTwo;
        BackgroundMusic backGroundMusic;
        Texture2D playerOneMeterTitle, playerTwoMeterTitle, meterLabel;
        Texture2D playerOneMarker, playerTwoMarker;
      
        Player dinosaurOne, dinosaurTwo;
        AnimatedSprite playerOneSprite, playerTwoSprite;
        Collisions collisions;
        SpriteFont font;

        int playerOneCounter = 0, playerTwoCounter = 0;
        private int levelIncrement;

        private bool singlePlayer;
        private bool explodedOne, explodedTwo;

        KeyboardInput input;
        GamepadInput padInput;

        //TextController textController;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = true;

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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            gameState = GameState.TitleScreen;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            backGroundMusic = new BackgroundMusic(Content, BackgroundMusic.playAt.Level1);

            startScreen = new StartScreen(graphics, spriteBatch, Content);
            fadeScreen = new FadeScreen(graphics);

            enemies = new List<Sprite>();
            explosions = new List<AnimatedSprite>();

            collisions = new Collisions(Content, explosions);
            input = new KeyboardInput();
            padInput = new GamepadInput();

            hbOne = new HealthBar(Content, graphics, new Vector2(50, graphics.PreferredBackBufferHeight - 125));
            cbOne = new ChargeMeter(Content, graphics, new Vector2(50, graphics.PreferredBackBufferHeight - 75));
            meterLabel = Content.Load<Texture2D>("Images/meter-labels");
            playerOneMeterTitle = Content.Load<Texture2D>("Images/meter-player-one");
            playerTwoMeterTitle = Content.Load<Texture2D>("Images/meter-player-two");
            playerOneMarker = Content.Load<Texture2D>("Images/P1-Marker");
            playerTwoMarker = Content.Load<Texture2D>("Images/P2-Marker");

            bossLifeBar = new HealthBar(Content, graphics, new Vector2(graphics.PreferredBackBufferWidth / 2 - 140, 50));
            bossLifeBar.IsVisible = false;

            font = Content.Load<SpriteFont>("Font");

            levelCreator = new LevelCreator(Content);

            pauseScreen = new PauseScreen(graphics, Content);

            creditScreen = new CreditScreen(graphics, Content);
        }

        protected void LoadCharacter()
        {
            if (startScreen.SinglePlayer)
            {
                singlePlayer = true;
                dinosaurOne = new Player(Content, graphics, characterScreen.Character, 1);
                dinosaurOne.ControlScheme = startScreen.ControlScheme;

                ValidateCharacter(1);

                playerOneSprite.Position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight - 250);
            }
            else if (startScreen.MultiPlayer)
            {
                singlePlayer = false;
                dinosaurOne = new Player(Content, graphics, multiplayerLobby.playerOne, 1);
                dinosaurTwo = new Player(Content, graphics, multiplayerLobby.playerTwo, 2);
                dinosaurOne.ControlScheme = multiplayerLobby.firstPlayerControl;
                dinosaurTwo.ControlScheme = multiplayerLobby.secondPlayerControl;

                ValidateCharacter(1);
                ValidateCharacter(2);

                playerOneSprite.Position = new Vector2(graphics.PreferredBackBufferWidth / 3, graphics.PreferredBackBufferHeight - 250);
                playerTwoSprite.Position = new Vector2(graphics.PreferredBackBufferWidth - (graphics.PreferredBackBufferWidth / 3), graphics.PreferredBackBufferHeight - 250);
            }

            playerShots = new PlayerShots(Content, dinosaurOne, playerOneSprite, dinosaurTwo, playerTwoSprite);
            shotBehavior = new ShotBehavior(Content);
            enemyShots = new EnemyShots(Content);

            CreateScreens();
            GenerateLevel();
        }

        private void ValidateCharacter(int player)
        {
            if (player == 1)
            {
                if (dinosaurOne.dinoChosen == Player.Chara.Bronto)
                {
                    playerOneSprite = new AnimatedSprite(12, 0, 0, 38, 183, this, dinosaurOne.Image, dinosaurOne.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }
                else if (dinosaurOne.dinoChosen == Player.Chara.Trex)
                {
                    playerOneSprite = new AnimatedSprite(12, 0, 0, 69, 181, this, dinosaurOne.Image, dinosaurOne.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }
                else if (dinosaurOne.dinoChosen == Player.Chara.Ptera)
                {
                    playerOneSprite = new AnimatedSprite(7, 0, 0, 195, 118, this, dinosaurOne.Image, dinosaurOne.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }
                else if (dinosaurOne.dinoChosen == Player.Chara.Cera)
                {
                    playerOneSprite = new AnimatedSprite(7, 0, 0, 62, 175, this, dinosaurOne.Image, dinosaurOne.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }
            }
            else if (player == 2)
            {
                if (dinosaurTwo.dinoChosen == Player.Chara.Bronto)
                {
                    playerTwoSprite = new AnimatedSprite(12, 0, 0, 38, 183, this, dinosaurTwo.Image, dinosaurTwo.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }
                else if (dinosaurTwo.dinoChosen == Player.Chara.Trex)
                {
                    playerTwoSprite = new AnimatedSprite(12, 0, 0, 69, 181, this, dinosaurTwo.Image, dinosaurTwo.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }
                else if (dinosaurTwo.dinoChosen == Player.Chara.Ptera)
                {
                    playerTwoSprite = new AnimatedSprite(7, 0, 0, 195, 118, this, dinosaurTwo.Image, dinosaurTwo.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }
                else if (dinosaurTwo.dinoChosen == Player.Chara.Cera)
                {
                    playerTwoSprite = new AnimatedSprite(7, 0, 0, 62, 175, this, dinosaurTwo.Image, dinosaurTwo.Position, spriteBatch, new EnemyType(Content, EnemyType.Etype.Player));
                }

                hbTwo = new HealthBar(Content, graphics, new Vector2(graphics.PreferredBackBufferWidth - 330, graphics.PreferredBackBufferHeight - 125));
                cbTwo = new ChargeMeter(Content, graphics, new Vector2(graphics.PreferredBackBufferWidth - 330, graphics.PreferredBackBufferHeight - 75));
            }
        }

        private void CreateScreens()
        {
            upgradeScreen = new Upgrade(graphics, spriteBatch, Content, dinosaurOne, playerOneSprite, dinosaurTwo, playerTwoSprite);
            deathScreen = new Death(graphics, Content, playerOneSprite, playerTwoSprite);
            levelBeginning = new StartLevel(graphics, Content, currentLevel);
            loadingScreen = new LoadingScreen(graphics, Content);
            levelIncrement++;
            levelBeginning.LevelCount+=levelIncrement;
        }

        private void GenerateLevel()
        {
            if (levelBeginning.LevelCount == 1)
            {
                currentLevel = levelCreator.CreateLevel1(this, graphics, spriteBatch);
            }
            else if (levelBeginning.LevelCount == 2)
            {
                currentLevel = levelCreator.CreateLevel2(this, graphics, spriteBatch);
            }
            else if (levelBeginning.LevelCount == 3)
            {
                currentLevel = levelCreator.CreateLevel3(this, graphics, spriteBatch);
            }
            else if (levelBeginning.LevelCount == 4)
            {
                currentLevel = levelCreator.CreateLevel4(this, graphics, spriteBatch);
            }
            else if (levelBeginning.LevelCount == 5)
            {
                currentLevel = levelCreator.CreateLevel5(this, graphics, spriteBatch);
            }
            levelBeginning.level = currentLevel;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            gameState = GameState.TitleScreen;
            previousState = gameState;

            upgradeScreen = null;
            startScreen = null;
            characterScreen = null;
            multiplayerLobby = null;
            deathScreen = null;
            levelBeginning = null;
            pauseScreen = null;
            loadingScreen = null;

            levelCreator = null;

            Random rand = new Random();
            currentLevel = null;

            enemies = null;
            explosions = null;
            shotBehavior = null;
            enemyShots = null;
            playerShots = null;
            hbOne = null;
            hbTwo = null;
            bossLifeBar = null;
            cbOne = null;
            cbTwo = null;
            backGroundMusic = null;
            playerOneMeterTitle = null;
            playerTwoMeterTitle = null;
            meterLabel = null;
            playerOneMarker = null;
            playerTwoMarker = null;

            dinosaurOne = null;
            dinosaurTwo = null;
            playerOneSprite = null;
            playerTwoSprite = null;
            collisions = null;
            font = null;

            playerOneCounter = 0;
            playerTwoCounter = 0;
            levelIncrement = 0;

            singlePlayer = false;
            explodedOne = false;
            explodedTwo = false;

            input = null;
            padInput = null;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (gameState == GameState.TitleScreen)
            {
                previousState = GameState.TitleScreen;
                startScreen.Update(gameTime, 1);

                if (startScreen.SinglePlayer || startScreen.MultiPlayer)
                {
                    if (fadeScreen.Opacity <= 255)
                    {
                        if (fadeScreen.Opacity != 255)
                        {
                            MediaPlayer.Volume -= .07f;
                            fadeScreen.Fade(gameTime, 15);
                        }
                        else
                        {
                            if (startScreen.SinglePlayer)
                            {
                                gameState = GameState.CharacterSelection;
                                characterScreen = new CharacterSelectionScreen(graphics, Content);
                            }
                            else if (startScreen.MultiPlayer)
                            {
                                gameState = GameState.MultiplayerLobby;
                                multiplayerLobby = new Lobby(graphics, Content, startScreen.ControlScheme);
                            }
                            MediaPlayer.Volume = 1f;
                        }
                    }
                    else
                    {
                        if (fadeScreen != null)
                        {
                            if (fadeScreen.Opacity > 0)
                            {
                                fadeScreen.Fade(gameTime, -1);
                            }
                        }
                    }
                }
            }
            else if (gameState == GameState.CharacterSelection)
            {
                previousState = GameState.CharacterSelection;
                characterScreen.Update(gameTime);

                if (characterScreen.IsDone)
                {
                    if (fadeScreen.Opacity <= 255)
                    {
                        if (fadeScreen.Opacity != 255)
                        {
                            fadeScreen.Fade(gameTime, 15);
                            MediaPlayer.Volume -= .06f;
                        }
                        else
                        {
                            LoadCharacter();
                            gameState = GameState.StartLevel; // .Credits; // 
                            characterScreen = null;
                            startScreen = null;
                        }
                    }
                }
                else
                {
                    if (fadeScreen.Opacity > 0)
                    {
                        fadeScreen.Fade(gameTime, -5);
                    }
                }
            }
            else if (gameState == GameState.MultiplayerLobby)
            {
                previousState = GameState.MultiplayerLobby;
                multiplayerLobby.Update(gameTime);

                if (multiplayerLobby.IsDone)
                {
                    if (fadeScreen.Opacity <= 255)
                    {
                        if (fadeScreen.Opacity != 255)
                        {
                            fadeScreen.Fade(gameTime, 15);
                            MediaPlayer.Volume -= .06f;
                        }
                        else
                        {
                            LoadCharacter();
                            gameState = GameState.StartLevel;
                            characterScreen = null;
                            startScreen = null;
                        }
                    }
                }
                else
                {
                    if (fadeScreen.Opacity > 0)
                    {
                        fadeScreen.Fade(gameTime, -5);
                    }
                }
            }
            else if (gameState == GameState.StartLevel)
            {
                previousState = GameState.StartLevel;
                if (fadeScreen.Opacity > 0)
                {
                    MediaPlayer.Volume += .07f;
                    fadeScreen.Fade(gameTime, -5);
                }
                if (fadeScreen.Opacity == 0)
                {
                    MediaPlayer.Volume = 1f;
                }

                float scrollSpeed = gameTime.ElapsedGameTime.Milliseconds / 6f;
                currentLevel.background.Update(gameTime, scrollSpeed);

                levelBeginning.Update(gameTime);
                if (levelIncrement == 1)
                    currentLevel.border.Update(gameTime, scrollSpeed);

                playerOneSprite.Update(gameTime);
                if (playerTwoSprite != null)
                {
                    playerTwoSprite.Update(gameTime);
                }

                if (levelBeginning.IsDone)
                {
                    gameState = GameState.PlayingLevel;
                }
            }
            else if (gameState == GameState.PlayingLevel)
            {
                previousState = GameState.PlayingLevel;

                if (dinosaurOne._currentHealth > 0)
                {
                    if (dinosaurOne.ControlScheme == ChosenControl.Keyboard)
                    {
                        input.Update(Keyboard.GetState(), playerOneSprite, dinosaurOne, playerShots, graphics, gameTime, gameState);
                    }
                    else if (dinosaurOne.ControlScheme == ChosenControl.Gamepad)
                    {
                        padInput.Update(GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None), playerOneSprite, dinosaurOne, playerShots, graphics, gameTime, gameState);
                    }
                }
                
                if (dinosaurTwo != null)
                {
                    if (dinosaurTwo._currentHealth > 0)
                    {
                        if (dinosaurTwo.ControlScheme == ChosenControl.Keyboard)
                        {
                            input.Update(Keyboard.GetState(), playerTwoSprite, dinosaurTwo, playerShots, graphics, gameTime, gameState);
                        }
                        else if (dinosaurTwo.ControlScheme == ChosenControl.Gamepad)
                        {
                            if (dinosaurOne.ControlScheme != ChosenControl.Gamepad)
                            {
                                padInput.Update(GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None), playerTwoSprite, dinosaurTwo, playerShots, graphics, gameTime, gameState);
                            }
                            else
                            {
                                padInput.Update(GamePad.GetState(PlayerIndex.Two, GamePadDeadZone.None), playerTwoSprite, dinosaurTwo, playerShots, graphics, gameTime, gameState);
                            }
                        }
                    }
                }

                hbOne.Update(dinosaurOne.Health);
                cbOne.Update(dinosaurOne.LaserCharge);
                if (playerTwoSprite != null)
                {
                    hbTwo.Update(dinosaurTwo.Health);
                    cbTwo.Update(dinosaurTwo.LaserCharge);
                }

                int counter = 0;
                foreach (Sprite s in enemies)
                {
                    currentLevel.healthBars[counter++].Update(s.Health, s.Position);

                    if (s.enemyType.type == Enemies.EnemyType.Etype.Level1Boss ||
                        s.enemyType.type == Enemies.EnemyType.Etype.Level2Boss ||
                        s.enemyType.type == Enemies.EnemyType.Etype.Level3Boss)
                    {
                        bossLifeBar.IsVisible = true;
                        bossLifeBar.Update(s.Health);
                    }
                    else
                    {
                        bossLifeBar.IsVisible = false;
                    }
                }

                enemyShots.Update(gameTime, graphics, playerOneSprite, playerTwoSprite);
                playerShots.Update(gameTime, graphics);

                gameState = currentLevel.Update(this, gameTime, spriteBatch, graphics, this.enemies);

                foreach (Sprite enemy in enemies)
                {
                    if (enemy.GetType() == (typeof(AnimatedSprite)))
                    {
                        (enemy as AnimatedSprite).Update(gameTime);
                    }
                    else
                    {
                        enemy.Update(gameTime, graphics, playerOneSprite, playerTwoSprite, dinosaurOne, dinosaurTwo);
                    }

                    if ((enemy.Movement.X < 0 && enemy.Position.X <= 100) || (enemy.Movement.X > 0 && enemy.Position.X >= graphics.PreferredBackBufferWidth - 100))
                    {
                        enemy.Movement = new Vector2(enemy.Movement.X * -1, enemy.Movement.Y);
                    }

                    if ((enemy.Movement.Y < 0 && enemy.Position.Y <= 50) || (enemy.Movement.Y > 0 && enemy.Position.Y >= graphics.PreferredBackBufferHeight / 1.5))
                    {
                        enemy.Movement = new Vector2(enemy.Movement.X, enemy.Movement.Y * -1);
                    }
                    

                    shotBehavior.Update(graphics, enemy, enemyShots);
                }

                AnimatedSprite removeMe = null;
                foreach (AnimatedSprite ex in explosions)
                {
                    ex.Update(gameTime);

                    if (ex.frames >= ex.frameTotal)
                    {
                        removeMe = ex;
                    }
                }
                explosions.Remove(removeMe);

                foreach (Powerup powerUp in collisions.powerups)
                {
                    if (powerUp != null)
                    {
                        powerUp.Update(gameTime);
                    }
                }



                collisions.Update(this, Content, spriteBatch, font, dinosaurOne, enemies, enemyShots, playerShots, currentLevel.randomObjects, currentLevel.healthBars, playerOneSprite, graphics, dinosaurTwo, playerTwoSprite);

                if (dinosaurOne._currentCharge < dinosaurOne.MaxLaserCharge)
                    playerOneCounter += 1;
                if (playerOneCounter >= 9)
                {
                    dinosaurOne._currentCharge++;
                    playerOneCounter = 0;
                }

                if (dinosaurTwo != null)
                {
                    if (dinosaurTwo._currentCharge < dinosaurTwo.MaxLaserCharge)
                        playerTwoCounter += 1;
                    if (playerTwoCounter >= 9)
                    {
                        dinosaurTwo._currentCharge++;
                        playerTwoCounter = 0;
                    }
                }

                if (singlePlayer)
                {
                    if (dinosaurOne._currentHealth <= 0)
                    {
                        hbOne.Update(dinosaurOne.Health);
                        
                        gameState = GameState.Death;
                    }
                }
                else
                {
                    if (dinosaurOne._currentHealth <= 0 && dinosaurTwo._currentHealth <= 0)
                    {
                        hbOne.Update(dinosaurOne.Health);
                        hbTwo.Update(dinosaurTwo.Health);
                        
                        gameState = GameState.Death;
                    }
                    else if (dinosaurOne._currentHealth <= 0 || dinosaurTwo._currentHealth <= 0)
                    {
                        if (dinosaurOne._currentHealth <= 0)
                        {
                            hbOne.Update(0);
                            if (playerOneSprite.Position.Y < graphics.PreferredBackBufferHeight + 200)
                            {
                                if (!explodedOne)
                                {
                                    explodedOne = true;
                                    collisions.explosionSound.Play();
                                }
                                AnimatedSprite explode = new AnimatedSprite(12, 0, 0, 134, 134, this, collisions.explodeImage,
                                playerOneSprite.Position,
                                spriteBatch, new Enemies.EnemyType(Content, Enemies.EnemyType.Etype.Simple));
                                explode.Name = "Explode";
                                explosions.Add(explode);

                                Vector2 movement = Vector2.Zero;
                                movement += Vector2.UnitY * .05f;
                                playerOneSprite.Movement += movement;

                                playerOneSprite.Update(gameTime);
                                movement.X = 0f;
                                movement.Y = 0f;
                                playerOneSprite.Movement = movement;
                                playerOneSprite.Update(gameTime);
                            }
                        }
                        else if (dinosaurTwo._currentHealth <= 0)
                        {
                            hbTwo.Update(0);
                            if (playerTwoSprite.Position.Y < graphics.PreferredBackBufferHeight + 200)
                            {
                                if (!explodedTwo)
                                {
                                    explodedTwo = true;
                                    collisions.explosionSound.Play();
                                }
                                AnimatedSprite explode = new AnimatedSprite(12, 0, 0, 134, 134, this, collisions.explodeImage,
                                playerTwoSprite.Position,
                                spriteBatch, new Enemies.EnemyType(Content, Enemies.EnemyType.Etype.Simple));
                                explode.Name = "Explode";
                                explosions.Add(explode);

                                Vector2 movement = Vector2.Zero;
                                movement += Vector2.UnitY * .05f;
                                playerTwoSprite.Movement += movement;

                                playerTwoSprite.Update(gameTime);
                                movement.X = 0f;
                                movement.Y = 0f;
                                playerTwoSprite.Movement = movement;
                                playerTwoSprite.Update(gameTime);
                            }
                        }
                    }
                }
                
            }

            else if (gameState == GameState.Death)
            {
                previousState = GameState.Death;

                

                if (playerTwoSprite != null)
                {
                    AnimatedSprite explode = new AnimatedSprite(12, 0, 0, 134, 134, this, collisions.explodeImage,
                                playerTwoSprite.Position,
                                spriteBatch, new Enemies.EnemyType(Content, Enemies.EnemyType.Etype.Simple));
                    explode.Name = "Explode";

                    if (!explodedOne)
                    {
                        collisions.explosionSound.Play();
                        explodedOne = true;
                    }
                    explosions.Add(explode);

                    AnimatedSprite explodeOne = new AnimatedSprite(12, 0, 0, 134, 134, this, collisions.explodeImage,
                                playerOneSprite.Position,
                                spriteBatch, new Enemies.EnemyType(Content, Enemies.EnemyType.Etype.Simple));
                    explode.Name = "Explode";
                    if (!explodedTwo)
                    {
                        collisions.explosionSound.Play();
                        explodedTwo = true;
                    }
                    explosions.Add(explodeOne);
                }
                else
                {
                    AnimatedSprite explode = new AnimatedSprite(12, 0, 0, 134, 134, this, collisions.explodeImage,
                                playerOneSprite.Position,
                                spriteBatch, new Enemies.EnemyType(Content, Enemies.EnemyType.Etype.Simple));
                    explode.Name = "Explode";
                    if (!explodedOne)
                    {
                        collisions.explosionSound.Play();
                        explodedOne = true;
                    }
                    explosions.Add(explode);
                }

                AnimatedSprite removeMe = null;
                foreach (AnimatedSprite ex in explosions)
                {
                    ex.Update(gameTime);

                    if (ex.frames >= ex.frameTotal)
                    {
                        removeMe = ex;
                    }
                }
                explosions.Remove(removeMe);

                if (fadeScreen.Opacity <= 180)
                {
                    fadeScreen.Fade(gameTime, 1);
                }

                deathScreen.Update(gameTime);

                

                enemyShots.Update(gameTime, graphics, playerOneSprite, playerTwoSprite);
                playerShots.Update(gameTime, graphics);
                foreach (Powerup powerUp in collisions.powerups)
                {
                    if (powerUp != null)
                    {
                        powerUp.Update(gameTime);
                    }
                }
                collisions.Update(this, Content, spriteBatch, font, dinosaurOne, enemies, enemyShots, playerShots, currentLevel.randomObjects, currentLevel.healthBars, playerOneSprite, graphics, dinosaurTwo, playerTwoSprite);

                if (deathScreen.TitleSelected)
                {
                    if (fadeScreen.Opacity < 255)
                    {
                        fadeScreen.Fade(gameTime, 5);
                        MediaPlayer.Volume -= .06f;
                    }
                    else if (fadeScreen.Opacity >= 255)
                    {
                        UnloadContent();
                        LoadContent();
                    }
                }
            }
            else if (gameState == GameState.EndOfLevel)
            {
                previousState = GameState.EndOfLevel;

                enemyShots.list.Clear();

                playerOneSprite.Update(gameTime);
                if (playerTwoSprite != null)
                {
                    playerTwoSprite.Update(gameTime);
                }

                currentLevel.Update(this, gameTime, spriteBatch, graphics, enemies);
                //enemyShots.Update(gameTime, graphics, playerOneSprite, playerTwoSprite);
                playerShots.Update(gameTime, graphics);
                upgradeScreen.Update(gameTime);

                foreach (Powerup powerUp in collisions.powerups)
                {
                    if (powerUp != null)
                    {
                        powerUp.Update(gameTime);
                    }
                }

                AnimatedSprite removeMe = null;
                foreach (AnimatedSprite ex in explosions)
                {
                    ex.Update(gameTime);

                    if (ex.frames >= ex.frameTotal)
                    {
                        removeMe = ex;
                    }
                }
                explosions.Remove(removeMe);

                collisions.Update(this, Content, spriteBatch, font, dinosaurOne, enemies, enemyShots, playerShots, currentLevel.randomObjects, currentLevel.healthBars, playerOneSprite, graphics, dinosaurTwo, playerTwoSprite);

                if (upgradeScreen.IsComplete)
                {
                    if (fadeScreen.Opacity <= 255)
                    {
                        if (fadeScreen.Opacity != 255)
                        {
                            fadeScreen.Fade(gameTime, 15);
                        }
                        else
                        {
                            if (playerTwoSprite == null)
                            {
                                playerOneSprite.Position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight - 250);
                                dinosaurOne.Replenish();
                            }
                            else
                            {
                                playerOneSprite.Position = new Vector2(graphics.PreferredBackBufferWidth / 3, graphics.PreferredBackBufferHeight - 250);
                                playerTwoSprite.Position = new Vector2(graphics.PreferredBackBufferWidth - (graphics.PreferredBackBufferWidth / 3), graphics.PreferredBackBufferHeight - 250);
                                dinosaurOne.Replenish();
                                hbOne.Update(dinosaurOne.Health);
                                dinosaurTwo.Replenish();
                                hbTwo.Update(dinosaurTwo.Health);
                            }

                            if (currentLevel.levelSelected != Level.LevelSelected.Level5)
                            {
                                gameState = GameState.LoadingScreen;
                            }
                        }
                    }
                }
            }
            else if (gameState == GameState.LoadingScreen)
            {
                previousState = GameState.LoadingScreen;
                loadingScreen.Update(gameTime);

                if (loadingScreen.IsDone)
                {
                    if (fadeScreen.Opacity <= 255)
                    {
                        if (fadeScreen.Opacity != 255)
                        {
                            fadeScreen.Fade(gameTime, 5);
                            MediaPlayer.Volume -= .07f;
                        }
                        else
                        {
                            gameState = GameState.StartLevel;
                            CreateScreens();
                            GenerateLevel();
                            MediaPlayer.Volume = 1f;
                        }
                    }
                }
                else
                {
                    if (fadeScreen.Opacity > 0)
                    {
                        fadeScreen.Fade(gameTime, -5);
                    }
                }
            }
            else if (gameState == GameState.Pause)
            {
                pauseScreen.Update(gameTime);
                MediaPlayer.Volume = .25f;
                if (pauseScreen.QuitSelected)
                {
                    this.Exit();
                }
                else if (pauseScreen.ResumeSelected)
                {
                    pauseScreen.ResumeSelected = false;
                    gameState = previousState;
                    MediaPlayer.Volume = 1f;
                }
                else if (pauseScreen.TitleSelected)
                {
                    if (fadeScreen.Opacity <= 255)
                    {
                        if (fadeScreen.Opacity != 255)
                        {
                            fadeScreen.Fade(gameTime, 15);
                            MediaPlayer.Volume -= .06f;
                        }
                        else
                        {
                            UnloadContent();
                            LoadContent();
                        }
                    }
                }
            }
            else if (gameState == GameState.Credits)
            {
                previousState = GameState.Credits;

                creditScreen.Update(gameTime);

                if (creditScreen.TextDone && !creditScreen.BeginBackgroundCreditScroll)
                {
                    if (fadeScreen.Opacity < 255)
                    {
                        fadeScreen.Fade(gameTime, 5);
                    }
                    else if (fadeScreen.Opacity == 255)
                    {
                        creditScreen.BeginBackgroundCreditScroll = true;
                        levelBeginning.LevelCount = 1;
                        GenerateLevel();
                    }
                }
                else if (creditScreen.BeginBackgroundCreditScroll)
                {
                    if (fadeScreen.Opacity > 0)
                    {
                        fadeScreen.Fade(gameTime, -5);
                    }
                    currentLevel.background.Update(gameTime, 2);
                    currentLevel.border.Update(gameTime, 3);
                }
                else
                {
                    if (fadeScreen.Opacity > 0)
                    {
                        fadeScreen.Fade(gameTime, -5);
                    }
                }
            }

        // Allows the game to exit
        // Gamepad option to exit
        //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        //{
        //    this.Exit();
        //}

        // allows keyboard exit on esc keypress
        if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None).IsButtonDown(Buttons.Start))
        {
            gameState = GameState.Pause;
        }
        base.Update(gameTime);
    }
        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (gameState == GameState.TitleScreen)
            {
                startScreen.Draw(spriteBatch);

                if (startScreen.SinglePlayer || startScreen.MultiPlayer || fadeScreen.Opacity >= 0) //////this might throw an error
                {
                    fadeScreen.Draw(spriteBatch);
                }

            }
            else if (gameState == GameState.CharacterSelection)
            {
                characterScreen.Draw(spriteBatch);

                if (characterScreen.IsDone)
                {
                    fadeScreen.Draw(spriteBatch);
                }
                else if (fadeScreen.Opacity > 0)
                {
                    fadeScreen.Draw(spriteBatch);
                }
            }
            else if (gameState == GameState.MultiplayerLobby)
            {
                multiplayerLobby.Draw(spriteBatch);

                if (multiplayerLobby.IsDone)
                {
                    fadeScreen.Draw(spriteBatch);
                }
                else if (fadeScreen.Opacity > 0)
                {
                    fadeScreen.Draw(spriteBatch);
                }
            }
            else if (gameState == GameState.StartLevel)
            {
                spriteBatch.Begin();
                currentLevel.background.Draw(spriteBatch);
                playerOneSprite.Draw(gameTime);
                if (playerTwoSprite != null)
                {
                    playerTwoSprite.Draw(gameTime);
                }
                spriteBatch.End();
                spriteBatch.Begin();
                if (levelIncrement == 1)
                    currentLevel.border.Draw(spriteBatch);
                spriteBatch.End();
                levelBeginning.Draw(spriteBatch);
            }
            else if (gameState == GameState.Pause)
            {
                #region Previous State Checking
                if (previousState == GameState.TitleScreen)
                {
                    startScreen.Draw(spriteBatch);
                }
                else if (previousState == GameState.CharacterSelection)
                {
                    characterScreen.Draw(spriteBatch);
                }
                else if (gameState == GameState.MultiplayerLobby)
                {
                    multiplayerLobby.Draw(spriteBatch);
                }
                else if (previousState == GameState.StartLevel)
                {
                    spriteBatch.Begin();
                    currentLevel.background.Draw(spriteBatch);
                    playerOneSprite.Draw(gameTime);
                    if (playerTwoSprite != null)
                    {
                        playerTwoSprite.Draw(gameTime);
                    }
                    spriteBatch.End();
                    spriteBatch.Begin();
                    if (levelIncrement == 1)
                        currentLevel.border.Draw(spriteBatch);
                    spriteBatch.End();
                    levelBeginning.Draw(spriteBatch);
                }
                else if (previousState == GameState.PlayingLevel)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    currentLevel.Draw(spriteBatch, font, gameTime);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                    enemyShots.Draw(spriteBatch);
                    playerShots.Draw(spriteBatch);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);

                    if (gameState != GameState.EndOfLevel && gameState != GameState.StartLevel)
                    {
                        if (playerTwoSprite != null)
                        {
                            spriteBatch.DrawString(font, "Points: " + dinosaurOne.Points.ToString(), new Vector2(600, 1000), Color.White);
                            spriteBatch.DrawString(font, "Points: " + dinosaurTwo.Points.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 800, graphics.PreferredBackBufferHeight - 80), Color.White);
                            spriteBatch.DrawString(font, "Total Points:", new Vector2(((graphics.PreferredBackBufferWidth / 2) - 120), graphics.PreferredBackBufferHeight - 120), Color.White);
                            spriteBatch.DrawString(font, (dinosaurOne.Points + dinosaurTwo.Points).ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - 50, graphics.PreferredBackBufferHeight - 80), Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(font, "Total Points:", new Vector2(((graphics.PreferredBackBufferWidth / 2) - 120), graphics.PreferredBackBufferHeight - 120), Color.White);
                            spriteBatch.DrawString(font, dinosaurOne.Points.ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - 40, 1000), Color.White);
                        }
                        hbOne.Draw(spriteBatch);
                        cbOne.Draw(spriteBatch);
                        spriteBatch.Draw(playerOneMeterTitle, new Rectangle(50, graphics.PreferredBackBufferHeight - 175, 280, 50), Color.White);
                        spriteBatch.Draw(meterLabel, new Rectangle(340, graphics.PreferredBackBufferHeight - 125, 100, 90), Color.White);
                        if (playerTwoSprite != null)
                        {
                            hbTwo.Draw(spriteBatch);
                            cbTwo.Draw(spriteBatch);
                            spriteBatch.Draw(playerTwoMeterTitle, new Rectangle(graphics.PreferredBackBufferWidth - 330, graphics.PreferredBackBufferHeight - 175, 280, 50), Color.White);
                            spriteBatch.Draw(meterLabel, new Rectangle(graphics.PreferredBackBufferWidth - 440, graphics.PreferredBackBufferHeight - 125, 100, 90), Color.White);
                        }
                    }
                    playerOneSprite.Draw(gameTime);
                    if (playerTwoSprite != null)
                    {
                        playerTwoSprite.Draw(gameTime);
                    }

                    foreach (Sprite enemy in enemies)
                    {
                        enemy.Draw(gameTime);
                    }

                    foreach (Powerup powerUp in collisions.powerups)
                    {
                        powerUp.Draw(spriteBatch);
                    }
                    spriteBatch.End();

                    spriteBatch.Begin();
                    foreach (AnimatedSprite ex in explosions)
                    {
                        ex.Draw(gameTime);
                    }
                    spriteBatch.End();

                    spriteBatch.Begin();
                    if (currentLevel.border != null)
                    {
                        currentLevel.border.Draw(spriteBatch);
                    }
                    base.Draw(gameTime);
                    spriteBatch.End();

                    if (bossLifeBar.IsVisible)
                    {
                        spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                        bossLifeBar.Draw(spriteBatch);
                        spriteBatch.End();
                    }
                }
                else if (previousState == GameState.EndOfLevel)
                {
                    upgradeScreen.Draw(spriteBatch);
                }
                else if (previousState == GameState.Death)
                {
                    deathScreen.Draw(spriteBatch);
                }
                #endregion

                pauseScreen.Draw(spriteBatch);
                if (pauseScreen.TitleSelected)
                {
                    if (fadeScreen.Opacity <= 255)
                    {
                        if (fadeScreen.Opacity != 255)
                        {
                            fadeScreen.Draw(spriteBatch);
                        }
                    }
                }
            }
            else
            {
                if (gameState != GameState.Credits)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    currentLevel.Draw(spriteBatch, font, gameTime);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                    enemyShots.Draw(spriteBatch);
                    playerShots.Draw(spriteBatch);
                    spriteBatch.End();
                }
                

                
                if (gameState != GameState.Credits)
                {
                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                    playerOneSprite.Draw(gameTime);
                    if (dinosaurOne.dinoChosen != Player.Chara.Ptera)
                    {
                        spriteBatch.Draw(playerOneMarker, new Rectangle((int)playerOneSprite.Position.X - (playerOneSprite.width / 2), (int)playerOneSprite.Position.Y - (playerOneSprite.height / 2) - 50, 50, 50), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(playerOneMarker, new Rectangle((int)playerOneSprite.Position.X - 20, (int)playerOneSprite.Position.Y - (playerOneSprite.height / 2) - 50, 50, 50), Color.White);
                    }

                    if (playerTwoSprite != null)
                    {
                        playerTwoSprite.Draw(gameTime);
                        if (dinosaurTwo.dinoChosen != Player.Chara.Ptera)
                        {
                            spriteBatch.Draw(playerTwoMarker, new Rectangle((int)playerTwoSprite.Position.X - (playerTwoSprite.width / 2), (int)playerTwoSprite.Position.Y - (playerTwoSprite.height / 2) - 50, 50, 50), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(playerTwoMarker, new Rectangle((int)playerTwoSprite.Position.X - 20, (int)playerTwoSprite.Position.Y - (playerTwoSprite.height / 2) - 50, 50, 50), Color.White);
                        }
                    }

                    int counter = 0;
                    foreach (Sprite enemy in enemies)
                    {
                        enemy.Draw(gameTime);
                        currentLevel.healthBars[counter++].Draw(spriteBatch, enemy);
                    }

                    foreach (Powerup powerUp in collisions.powerups)
                    {
                        powerUp.Draw(spriteBatch);
                    }

                    spriteBatch.End();

                    spriteBatch.Begin();

                    foreach (AnimatedSprite ex in explosions)
                    {
                        ex.Draw(gameTime);
                    }
                    spriteBatch.End();
                }


                if (currentLevel.levelSelected == Level.LevelSelected.Level1 && gameState != GameState.Credits)
                {
                    spriteBatch.Begin();
                    if (currentLevel.border != null)
                    {
                        currentLevel.border.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                }

                if (currentLevel.levelSelected == Level.LevelSelected.Level2)
                {
                    spriteBatch.Begin();
                    foreach (Clouds cloud in currentLevel.clouds)
                    {
                        cloud.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                }

                spriteBatch.Begin();
                base.Draw(gameTime);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                if (gameState != GameState.EndOfLevel && gameState != GameState.StartLevel && gameState != GameState.Credits)
                {
                    if (playerTwoSprite != null)
                    {
                        spriteBatch.DrawString(font, "Points: " + dinosaurOne.Points.ToString(), new Vector2(600, 1000), Color.White);
                        spriteBatch.DrawString(font, "Points: " + dinosaurTwo.Points.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 800, graphics.PreferredBackBufferHeight - 80), Color.White);
                        spriteBatch.DrawString(font, "Total Points:", new Vector2(((graphics.PreferredBackBufferWidth / 2) - 120), graphics.PreferredBackBufferHeight - 120), Color.White);
                        spriteBatch.DrawString(font, (dinosaurOne.Points + dinosaurTwo.Points).ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - 50, graphics.PreferredBackBufferHeight - 80), Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, "Total Points:", new Vector2(((graphics.PreferredBackBufferWidth / 2) - 120), graphics.PreferredBackBufferHeight - 120), Color.White);
                        spriteBatch.DrawString(font, dinosaurOne.Points.ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - 40, 1000), Color.White);
                    }
                    hbOne.Draw(spriteBatch);
                    cbOne.Draw(spriteBatch);
                    spriteBatch.Draw(playerOneMeterTitle, new Rectangle(50, graphics.PreferredBackBufferHeight - 175, 280, 50), Color.White);
                    spriteBatch.Draw(meterLabel, new Rectangle(340, graphics.PreferredBackBufferHeight - 125, 100, 90), Color.White);
                    if (playerTwoSprite != null)
                    {
                        hbTwo.Draw(spriteBatch);
                        cbTwo.Draw(spriteBatch);
                        spriteBatch.Draw(playerTwoMeterTitle, new Rectangle(graphics.PreferredBackBufferWidth - 330, graphics.PreferredBackBufferHeight - 175, 280, 50), Color.White);
                        spriteBatch.Draw(meterLabel, new Rectangle(graphics.PreferredBackBufferWidth - 440, graphics.PreferredBackBufferHeight - 125, 100, 90), Color.White);
                    }
                }
                spriteBatch.End();

                if (bossLifeBar.IsVisible)
                {
                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                    bossLifeBar.Draw(spriteBatch);
                    spriteBatch.End();
                }

                if (gameState == GameState.EndOfLevel)
                {
                    upgradeScreen.Draw(spriteBatch);
                }

                if (gameState == GameState.LoadingScreen)
                {
                    loadingScreen.Draw(spriteBatch);
                }

                if (fadeScreen.Opacity > 0)
                {
                    fadeScreen.Draw(spriteBatch);
                }

                if (gameState == GameState.Death)
                {
                    deathScreen.Draw(spriteBatch);
                }

                if (gameState == GameState.Credits)
                {
                    creditScreen.Draw(spriteBatch, currentLevel);
                    if (fadeScreen.Opacity >= 0 && creditScreen.TextDone)
                    {
                        fadeScreen.Draw(spriteBatch);
                    }
                }
            }
        }

        
    }
}
