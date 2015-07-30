using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using DinosaurLazers.GameStates;
using Microsoft.Xna.Framework.Audio;
using DinosaurLazers.Enemies;

namespace DinosaurLazers.Models
{
    public class Level
    {
        public enum LevelSelected
        { 
            Level1, Level2, Level3, Level4, Level5
        }
        public bool boss = false;
        public bool midBoss = false;

        public float ScrollSpeed { get; set; }
        public LevelSelected levelSelected;

        public string Name { get; set; }
        public Background background { get; set; }
        public Border border { get; set; }
        public List<Clouds> clouds { get; set; }
        public long DistanceTravelled { get; set; }

        public List<Sprite> randomObjects { get; set; }
        public List<Enemy> enemies { get; set; }
        public List<Sprite> enemySprites { get; set; }
        private List<EnemyGroup> groups;

        public List<HealthBar> healthBars;
        private Random rand;


        SoundEffect bossSound;
        public BackgroundMusic BGMusic;
        public BackgroundMusic BossBGMusic;
        bool IsBossMusicPlaying = false;
        bool playBossSound = false;

        ContentManager Content;
        //GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;

        public Level(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager content, LevelSelected levelSelected, List<Sprite> randomObjects)
        {
            this.Content = content;

            rand = new Random();
            this.randomObjects = new List<Sprite>();
            enemies = new List<Enemy>();
            enemySprites = new List<Sprite>();
            groups = new List<EnemyGroup>();
            healthBars = new List<HealthBar>();

            this.levelSelected = levelSelected;
            background = new Background(levelSelected, graphics.GraphicsDevice, spriteBatch, content);

            bossSound = content.Load<SoundEffect>("Sounds/Wow");

            if (levelSelected == LevelSelected.Level1)
            {
                BGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level1);
                BossBGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level1Boss);
                border = new Border(levelSelected, graphics.GraphicsDevice, spriteBatch, content);
                //clouds = null;
            }
            else if (levelSelected == LevelSelected.Level2)
            {
                BGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level2);
                BossBGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level2Boss);
                border = null;
                clouds = new List<Clouds> { new Clouds(levelSelected, graphics.GraphicsDevice, spriteBatch, content, rand),
                                            new Clouds(levelSelected, graphics.GraphicsDevice, spriteBatch, content, rand),
                                            new Clouds(levelSelected, graphics.GraphicsDevice, spriteBatch, content, rand),
                                            new Clouds(levelSelected, graphics.GraphicsDevice, spriteBatch, content, rand),
                                            new Clouds(levelSelected, graphics.GraphicsDevice, spriteBatch, content, rand),
                                            new Clouds(levelSelected, graphics.GraphicsDevice, spriteBatch, content, rand),
                                            new Clouds(levelSelected, graphics.GraphicsDevice, spriteBatch, content, rand) };
            }
            else if (levelSelected == LevelSelected.Level3)
            {
                BGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level3);
                BossBGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level3Boss);
                border = null;
                //clouds = null;
            }
            else if (levelSelected == LevelSelected.Level4)
            {
                BGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level4);
                BossBGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level4Boss);
                border = null;
            }
            else if (levelSelected == LevelSelected.Level5)
            {
                BGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level4);
                BossBGMusic = new BackgroundMusic(content, BackgroundMusic.playAt.Level5Boss);
                border = null;
            }

            foreach (Sprite rObj in randomObjects)
            {
                rObj.Position = new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth - 50),rand.Next(-1000, -100));
            }

            this.randomObjects = randomObjects;
        }

        public GameState Update(Game game, GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, List<Sprite> enemies)
        {
            GameState gameState = GameState.PlayingLevel;


            if (!boss && !midBoss)
            {
                DistanceTravelled += 1;
                ScrollSpeed = gameTime.ElapsedGameTime.Milliseconds / 6f;
            }
            else
            {
                ScrollSpeed = 0;

                if (playBossSound == false && boss)
                {
                    playBossSound = true;

                    bossSound.Play();
                }
                if (boss)
                {
                    if (!IsBossMusicPlaying)
                    {
                        BossBGMusic.Play();
                        IsBossMusicPlaying = true;
                    }
                }
                else
                {
                    playBossSound = false;
                }
            }
            
            background.Update(gameTime, ScrollSpeed);

            if (levelSelected == LevelSelected.Level1)
            {
                border.Update(gameTime, ScrollSpeed);
            }
            else if (levelSelected == LevelSelected.Level2)
            {
                foreach (Clouds cloud in clouds)
                {
                    cloud.Update(gameTime, graphics, ScrollSpeed);
                }
            }

            EnemyGroup deleteMe = null;
            foreach (EnemyGroup eg in groups)
            {
                eg.Update(DistanceTravelled, this);

                if (eg.group.Count == 0)
                {
                    deleteMe = eg;
                }   
            }

            if (deleteMe != null)
            {
                this.groups.Remove(deleteMe);
            }

            foreach (Enemy e in this.enemies)
            {
                Sprite newEnemy = null;

                if (e.isAnimatedSprite)
                {
                    if (e.type.type == EnemyType.Etype.Level2MidBoss)
                    {
                        newEnemy = new AnimatedSprite(5, 0, 0, 85, 87, game, e.Image, e.Position, spriteBatch, e.type);
                    }
                    else if (e.type.type == EnemyType.Etype.Level2Boss)
                    {
                        newEnemy = new AnimatedSprite(3, 0, 0, 193, 377, game, e.Image, e.Position, spriteBatch, e.type);
                    }
                    else if (e.type.type == EnemyType.Etype.Level3Boss)
                    {
                        newEnemy = new AnimatedSprite(7, 0, 0, 189, 377, game, e.Image, e.Position, spriteBatch, e.type);
                    }
                    else if (e.type.type == EnemyType.Etype.Level4Boss)
                    {
                        newEnemy = new AnimatedSprite(3, 0, 0, 192, 377, game, e.Image, e.Position, spriteBatch, e.type);
                    }
                }
                else
                {
                    newEnemy = new Sprite(game, e.Image, e.Position, spriteBatch, e.type);
                }

                healthBars.Add(new HealthBar(Content, graphics, newEnemy.Position));
                newEnemy.Movement = e.Movement;
                enemies.Add(newEnemy);
            }

            // used to move random items down the screen
            Vector2 movement = new Vector2();
            movement = Vector2.Zero;
            movement.Y = ScrollSpeed;

            foreach (Sprite rObj in randomObjects)
            {
                rObj.Position += movement;

                if (rObj.Position.Y >= graphics.PreferredBackBufferHeight + 200)
                {
                    rObj.Position = new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth - 50), rand.Next(-1000, -100));
                }
            }

            this.enemies.Clear();

            if (enemies.Count == 0)
            {
                if (midBoss)
                    midBoss = false;
                else if (boss)
                {
                    boss = false;
                    if (this.levelSelected != Level.LevelSelected.Level5)
                    {
                        gameState = GameState.EndOfLevel;
                    }
                    else
                    {
                        gameState = GameState.Credits;
                    }
                }
            }
            return gameState;
        }

        public void AddGroup(EnemyGroup group)
        {
            this.groups.Add(group);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GameTime gameTime)
        {
            background.Draw(spriteBatch);

            foreach (Sprite rObj in randomObjects)
            {
                rObj.Draw(gameTime);
            }

            if (levelSelected == LevelSelected.Level1)
            {
                border.Draw(spriteBatch);
            }
            

           // spriteBatch.DrawString(font, this.DistanceTravelled.ToString(), new Vector2(400, 1000), Color.White);
        }
    }
}
