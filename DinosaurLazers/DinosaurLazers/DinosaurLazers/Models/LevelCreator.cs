using DinosaurLazers.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class LevelCreator
    {
        private ContentManager Content;

        private EnemyType simple, bombers, gunners, darting, hards, midBoss, boss, level2midboss, level2boss, level3midboss, level3boss, level4midboss, level4boss, level5midboss, level5boss;

        public LevelCreator(ContentManager Content)
        {
            this.Content = Content;

            simple = new EnemyType(Content, EnemyType.Etype.Simple);
            bombers = new EnemyType(Content, EnemyType.Etype.Bomber);
            gunners = new EnemyType(Content, EnemyType.Etype.Gunner);
            darting = new EnemyType(Content, EnemyType.Etype.Darting);
            hards = new EnemyType(Content, EnemyType.Etype.Hard);
            midBoss = new EnemyType(Content, EnemyType.Etype.Level1MidBoss);
            boss = new EnemyType(Content, EnemyType.Etype.Level1Boss);
            level2midboss = new EnemyType(Content, EnemyType.Etype.Level2MidBoss);
            level2boss = new EnemyType(Content, EnemyType.Etype.Level2Boss);
            level3midboss = new EnemyType(Content, EnemyType.Etype.Level3MidBoss);
            level3boss = new EnemyType(Content, EnemyType.Etype.Level3Boss);
            level4midboss = new EnemyType(Content, EnemyType.Etype.Level4MidBoss);
            level4boss = new EnemyType(Content, EnemyType.Etype.Level4Boss);
            level5midboss = new EnemyType(Content, EnemyType.Etype.Level5MidBoss);
            level5boss = new EnemyType(Content, EnemyType.Etype.Level5Boss);
        }

        public Level CreateLevel1(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            Texture2D texture1 = Content.Load<Texture2D>("Images/Spikes");
            Sprite spikes = new Sprite(game, texture1, Vector2.Zero, spriteBatch, new EnemyType(Content, EnemyType.Etype.Simple));
            spikes.objectType = ObjectType.Spikes;
            spikes.Color = Color.White;
            Texture2D texture2 = Content.Load<Texture2D>("Images/swamp2");//SmallGreenMarsh");
            Sprite marsh = new Sprite(game, texture2, Vector2.Zero, spriteBatch, new EnemyType(Content, EnemyType.Etype.Simple));
            marsh.objectType = ObjectType.Marsh;

            List<Sprite> randomObjects = new List<Sprite>();
            randomObjects.Add(spikes);
            randomObjects.Add(marsh);

            Level level1 = new Level(graphics, spriteBatch, Content, Level.LevelSelected.Level1, randomObjects);
            level1.Name = "Level1";

            EnemyGroup eg1 = new EnemyGroup(Content, graphics, simple, 100, 2, Enemy.Direction.Left);
            EnemyGroup eg2 = new EnemyGroup(Content, graphics, simple, 100, 2, Enemy.Direction.Right);
            EnemyGroup eg3 = new EnemyGroup(Content, graphics, bombers, 300, 1, Enemy.Direction.Left);
            EnemyGroup eg4 = new EnemyGroup(Content, graphics, bombers, 300, 1, Enemy.Direction.Right);
            EnemyGroup eg5 = new EnemyGroup(Content, graphics, simple, 700, 2, Enemy.Direction.Left);
            EnemyGroup eg6 = new EnemyGroup(Content, graphics, simple, 700, 2, Enemy.Direction.Right);
            EnemyGroup eg7 = new EnemyGroup(Content, graphics, gunners, 1000, 2, Enemy.Direction.Right, false, true);
            EnemyGroup eg8 = new EnemyGroup(Content, graphics, gunners, 1000, 2, Enemy.Direction.Left, false, true);

            EnemyGroup eg9 = new EnemyGroup(Content, graphics, bombers, 1100, 2, Enemy.Direction.Left);
            EnemyGroup eg10 = new EnemyGroup(Content, graphics, bombers, 1100, 2, Enemy.Direction.Right);
            EnemyGroup eg11 = new EnemyGroup(Content, graphics, gunners, 1350, 1, Enemy.Direction.Right);
            EnemyGroup eg12 = new EnemyGroup(Content, graphics, gunners, 1600, 1, Enemy.Direction.Left);
            EnemyGroup eg13 = new EnemyGroup(Content, graphics, hards, 1600, 2, Enemy.Direction.Right);
            EnemyGroup eg14 = new EnemyGroup(Content, graphics, midBoss, 2000, 1, Enemy.Direction.Top, true); //change 1000 back to 2000

            level1.AddGroup(eg1);
            level1.AddGroup(eg2);
            level1.AddGroup(eg3);
            level1.AddGroup(eg4);
            level1.AddGroup(eg5);
            level1.AddGroup(eg6);
            level1.AddGroup(eg7);
            level1.AddGroup(eg8);

            level1.AddGroup(eg9);
            level1.AddGroup(eg10);
            level1.AddGroup(eg11);
            level1.AddGroup(eg12);
            level1.AddGroup(eg13);
            level1.AddGroup(eg14);

            return level1;
        }

        public Level CreateLevel2(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            Texture2D texture1 = Content.Load<Texture2D>("Images/Spikes");
            Sprite spikes = new Sprite(game, texture1, Vector2.Zero, spriteBatch, new EnemyType(Content, EnemyType.Etype.Simple));
            spikes.objectType = ObjectType.Spikes;
            spikes.Color = Color.White;
            Texture2D texture2 = Content.Load<Texture2D>("Images/oil-spill");
            Sprite marsh = new Sprite(game, texture2, Vector2.Zero, spriteBatch, new EnemyType(Content, EnemyType.Etype.Simple));
            marsh.objectType = ObjectType.Marsh;

            List<Sprite> randomObjects = new List<Sprite>();
            randomObjects.Add(spikes);
            randomObjects.Add(marsh);

            Level level2 = new Level(graphics, spriteBatch, Content, Level.LevelSelected.Level2, randomObjects);

            level2.Name = "Level2";

            EnemyGroup eg1 = new EnemyGroup(Content, graphics, simple, 100, 3, Enemy.Direction.Left);
            EnemyGroup eg2 = new EnemyGroup(Content, graphics, bombers, 100, 3, Enemy.Direction.Right);
            EnemyGroup eg3 = new EnemyGroup(Content, graphics, bombers, 300, 3, Enemy.Direction.Left);
            EnemyGroup eg4 = new EnemyGroup(Content, graphics, bombers, 300, 3, Enemy.Direction.Right);
            EnemyGroup eg5 = new EnemyGroup(Content, graphics, simple, 700, 5, Enemy.Direction.Left);
            EnemyGroup eg6 = new EnemyGroup(Content, graphics, simple, 700, 5, Enemy.Direction.Right);
            EnemyGroup eg7 = new EnemyGroup(Content, graphics, hards, 1000, 2, Enemy.Direction.Right, false, true);
            EnemyGroup eg8 = new EnemyGroup(Content, graphics, level2midboss, 1000, 3, Enemy.Direction.Left, false, true);

            EnemyGroup eg9 = new EnemyGroup(Content, graphics, gunners, 1100, 3, Enemy.Direction.Left);
            EnemyGroup eg10 = new EnemyGroup(Content, graphics, gunners, 1100, 3, Enemy.Direction.Right);
            EnemyGroup eg11 = new EnemyGroup(Content, graphics, bombers, 1350, 3, Enemy.Direction.Right);
            EnemyGroup eg12 = new EnemyGroup(Content, graphics, hards, 1600, 3, Enemy.Direction.Left);
            EnemyGroup eg13 = new EnemyGroup(Content, graphics, hards, 1600, 3, Enemy.Direction.Right);

            EnemyGroup eg14 = new EnemyGroup(Content, graphics, level2boss, 2000, 1, Enemy.Direction.Right, true);

            level2.AddGroup(eg1);
            level2.AddGroup(eg2);
            level2.AddGroup(eg3);
            level2.AddGroup(eg4);
            level2.AddGroup(eg5);
            level2.AddGroup(eg6);
            level2.AddGroup(eg7);
            level2.AddGroup(eg8);

            level2.AddGroup(eg9);
            level2.AddGroup(eg10);
            level2.AddGroup(eg11);
            level2.AddGroup(eg12);
            level2.AddGroup(eg13);
            level2.AddGroup(eg14);

            return level2;
        }

        public Level CreateLevel3(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            Texture2D texture1 = Content.Load<Texture2D>("Images/lava");
            Sprite spikes = new Sprite(game, texture1, Vector2.Zero, spriteBatch, new EnemyType(Content, EnemyType.Etype.Simple));
            spikes.objectType = ObjectType.Spikes;
            spikes.Color = Color.White;

            List<Sprite> randomObjects = new List<Sprite>();
            randomObjects.Add(spikes);

            Level level3 = new Level(graphics, spriteBatch, Content, Level.LevelSelected.Level3, randomObjects);

            level3.Name = "Level3";

            //Wave1
            EnemyGroup eg1 = new EnemyGroup(Content, graphics, simple, 100, 10, Enemy.Direction.Left);
            EnemyGroup eg2 = new EnemyGroup(Content, graphics, simple, 100, 10, Enemy.Direction.Right);
            EnemyGroup eg3 = new EnemyGroup(Content, graphics, bombers, 300, 5, Enemy.Direction.Left);
            EnemyGroup eg4 = new EnemyGroup(Content, graphics, bombers, 300, 5, Enemy.Direction.Right);
            EnemyGroup eg5 = new EnemyGroup(Content, graphics, simple, 700, 15, Enemy.Direction.Left);
            EnemyGroup eg6 = new EnemyGroup(Content, graphics, simple, 700, 15, Enemy.Direction.Right);

            //Midboss
            EnemyGroup eg7 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Right, false, true);
            EnemyGroup eg8 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Left, false, true);
            EnemyGroup eg9 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Top, false, true);

            //Wave 2
            EnemyGroup eg10 = new EnemyGroup(Content, graphics, gunners, 1100, 5, Enemy.Direction.Left);
            EnemyGroup eg11 = new EnemyGroup(Content, graphics, gunners, 1100, 5, Enemy.Direction.Right);
            EnemyGroup eg12 = new EnemyGroup(Content, graphics, bombers, 1350, 5, Enemy.Direction.Right);
            EnemyGroup eg13 = new EnemyGroup(Content, graphics, bombers, 1350, 5, Enemy.Direction.Left);
            EnemyGroup eg14 = new EnemyGroup(Content, graphics, hards, 1600, 3, Enemy.Direction.Left);
            EnemyGroup eg15 = new EnemyGroup(Content, graphics, hards, 1600, 3, Enemy.Direction.Right);
            EnemyGroup eg16 = new EnemyGroup(Content, graphics, darting, 1600, 3, Enemy.Direction.Left);
            EnemyGroup eg17 = new EnemyGroup(Content, graphics, darting, 1600, 3, Enemy.Direction.Right);

            //Boss
            EnemyGroup eg18 = new EnemyGroup(Content, graphics, level3boss, 2000, 1, Enemy.Direction.Left, true);

            level3.AddGroup(eg1);
            level3.AddGroup(eg2);
            level3.AddGroup(eg3);
            level3.AddGroup(eg4);
            level3.AddGroup(eg5);
            level3.AddGroup(eg6);
            level3.AddGroup(eg7);
            level3.AddGroup(eg8);
            level3.AddGroup(eg9);

            level3.AddGroup(eg10);
            level3.AddGroup(eg11);
            level3.AddGroup(eg12);
            level3.AddGroup(eg13);
            level3.AddGroup(eg14);
            level3.AddGroup(eg15);
            level3.AddGroup(eg16);
            level3.AddGroup(eg17);
            level3.AddGroup(eg18);

            return level3;
        }

        public Level CreateLevel4(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            Texture2D texture1 = Content.Load<Texture2D>("Images/lava");
            Sprite spikes = new Sprite(game, texture1, Vector2.Zero, spriteBatch, new EnemyType(Content, EnemyType.Etype.Simple));
            spikes.objectType = ObjectType.Spikes;
            spikes.Color = Color.White;

            List<Sprite> randomObjects = new List<Sprite>();
            randomObjects.Add(spikes);

            Level level4 = new Level(graphics, spriteBatch, Content, Level.LevelSelected.Level4, randomObjects);

            level4.Name = "Level4";

            //Wave1
            EnemyGroup eg1 = new EnemyGroup(Content, graphics, simple, 100, 15, Enemy.Direction.Left);
            EnemyGroup eg2 = new EnemyGroup(Content, graphics, simple, 100, 15, Enemy.Direction.Right);
            EnemyGroup eg3 = new EnemyGroup(Content, graphics, bombers, 300, 10, Enemy.Direction.Left);
            EnemyGroup eg4 = new EnemyGroup(Content, graphics, bombers, 300, 10, Enemy.Direction.Right);
            EnemyGroup eg5 = new EnemyGroup(Content, graphics, simple, 700, 15, Enemy.Direction.Left);
            EnemyGroup eg6 = new EnemyGroup(Content, graphics, simple, 700, 15, Enemy.Direction.Right);

            //Midboss
            EnemyGroup eg7 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Right, false, true);
            EnemyGroup eg8 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Left, false, true);
            EnemyGroup eg9 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Top, false, true);

            //Wave 2
            EnemyGroup eg10 = new EnemyGroup(Content, graphics, gunners, 1100, 10, Enemy.Direction.Left);
            EnemyGroup eg11 = new EnemyGroup(Content, graphics, gunners, 1100, 10, Enemy.Direction.Right);
            EnemyGroup eg12 = new EnemyGroup(Content, graphics, bombers, 1350, 10, Enemy.Direction.Right);
            EnemyGroup eg13 = new EnemyGroup(Content, graphics, bombers, 1350, 10, Enemy.Direction.Left);
            EnemyGroup eg14 = new EnemyGroup(Content, graphics, hards, 1600, 5, Enemy.Direction.Left);
            EnemyGroup eg15 = new EnemyGroup(Content, graphics, hards, 1600, 5, Enemy.Direction.Right);
            EnemyGroup eg16 = new EnemyGroup(Content, graphics, darting, 1600, 3, Enemy.Direction.Left);
            EnemyGroup eg17 = new EnemyGroup(Content, graphics, darting, 1600, 3, Enemy.Direction.Right);

            //Boss
            EnemyGroup eg18 = new EnemyGroup(Content, graphics, level4boss, 2000, 1, Enemy.Direction.Right, true);
            EnemyGroup eg20 = new EnemyGroup(Content, graphics, hards, 2000, 3, Enemy.Direction.Left, true);
            EnemyGroup eg21 = new EnemyGroup(Content, graphics, midBoss, 2000, 2, Enemy.Direction.Top, true);
            EnemyGroup eg22 = new EnemyGroup(Content, graphics, hards, 2000, 3, Enemy.Direction.Right, true);

            level4.AddGroup(eg1);
            level4.AddGroup(eg2);
            level4.AddGroup(eg3);
            level4.AddGroup(eg4);
            level4.AddGroup(eg5);
            level4.AddGroup(eg6);
            level4.AddGroup(eg7);
            level4.AddGroup(eg8);
            level4.AddGroup(eg9);

            level4.AddGroup(eg10);
            level4.AddGroup(eg11);
            level4.AddGroup(eg12);
            level4.AddGroup(eg13);
            level4.AddGroup(eg14);
            level4.AddGroup(eg15);
            level4.AddGroup(eg16);
            level4.AddGroup(eg17);
            level4.AddGroup(eg18);
            level4.AddGroup(eg20);
            level4.AddGroup(eg21);
            level4.AddGroup(eg22);

            return level4;
        }

        public Level CreateLevel5(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            Texture2D texture1 = Content.Load<Texture2D>("Images/Spikes");
            Sprite spikes = new Sprite(game, texture1, Vector2.Zero, spriteBatch, new EnemyType(Content, EnemyType.Etype.Simple));
            spikes.objectType = ObjectType.Spikes;
            spikes.Color = Color.White;

            List<Sprite> randomObjects = new List<Sprite>();
            randomObjects.Add(spikes);

            Level level5 = new Level(graphics, spriteBatch, Content, Level.LevelSelected.Level5, randomObjects);

            level5.Name = "Level5";

            //Wave1
            EnemyGroup eg1 = new EnemyGroup(Content, graphics, simple, 100, 30, Enemy.Direction.Left);
            EnemyGroup eg2 = new EnemyGroup(Content, graphics, simple, 200, 30, Enemy.Direction.Right);
            EnemyGroup eg3 = new EnemyGroup(Content, graphics, bombers, 300, 15, Enemy.Direction.Left);
            EnemyGroup eg4 = new EnemyGroup(Content, graphics, bombers, 400, 15, Enemy.Direction.Right);
            EnemyGroup eg5 = new EnemyGroup(Content, graphics, gunners, 500, 20, Enemy.Direction.Right);
            EnemyGroup eg6 = new EnemyGroup(Content, graphics, gunners, 600, 20, Enemy.Direction.Left);
            EnemyGroup eg7 = new EnemyGroup(Content, graphics, simple, 700, 30, Enemy.Direction.Left);
            EnemyGroup eg8 = new EnemyGroup(Content, graphics, simple, 800, 30, Enemy.Direction.Right);

            //Midboss
            EnemyGroup eg9 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Right, false, true);
            EnemyGroup eg10 = new EnemyGroup(Content, graphics, level2midboss, 1000, 3, Enemy.Direction.Left, false, true);
            EnemyGroup eg11 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Top, false, true);
            EnemyGroup eg12 = new EnemyGroup(Content, graphics, level2midboss, 1000, 3, Enemy.Direction.Right, false, true);
            EnemyGroup eg13 = new EnemyGroup(Content, graphics, midBoss, 1000, 1, Enemy.Direction.Left, false, true);

            //Wave 2
            EnemyGroup eg14 = new EnemyGroup(Content, graphics, gunners, 1100, 25, Enemy.Direction.Left);
            EnemyGroup eg15 = new EnemyGroup(Content, graphics, gunners, 1200, 25, Enemy.Direction.Right);
            EnemyGroup eg16 = new EnemyGroup(Content, graphics, bombers, 1300, 20, Enemy.Direction.Right);
            EnemyGroup eg17 = new EnemyGroup(Content, graphics, bombers, 1400, 20, Enemy.Direction.Left);
            EnemyGroup eg18 = new EnemyGroup(Content, graphics, hards, 1500, 15, Enemy.Direction.Left);
            EnemyGroup eg19 = new EnemyGroup(Content, graphics, hards, 1600, 15, Enemy.Direction.Right);
            EnemyGroup eg20 = new EnemyGroup(Content, graphics, darting, 1700, 10, Enemy.Direction.Left);
            EnemyGroup eg21 = new EnemyGroup(Content, graphics, darting, 1800, 10, Enemy.Direction.Right);

            //Boss
            EnemyGroup eg22 = new EnemyGroup(Content, graphics, boss, 2000, 1, Enemy.Direction.Left, true);
            EnemyGroup eg23 = new EnemyGroup(Content, graphics, boss, 2000, 3, Enemy.Direction.Top, true);
            EnemyGroup eg24 = new EnemyGroup(Content, graphics, boss, 2000, 1, Enemy.Direction.Right, true);
            EnemyGroup eg25 = new EnemyGroup(Content, graphics, level3boss, 2000, 1, Enemy.Direction.Left, true);
            EnemyGroup eg26 = new EnemyGroup(Content, graphics, level3boss, 2000, 1, Enemy.Direction.Right, true);

            level5.AddGroup(eg1);
            level5.AddGroup(eg2);
            level5.AddGroup(eg3);
            level5.AddGroup(eg4);
            level5.AddGroup(eg5);
            level5.AddGroup(eg6);
            level5.AddGroup(eg7);
            level5.AddGroup(eg8);
            level5.AddGroup(eg9);
            level5.AddGroup(eg10);
            level5.AddGroup(eg11);
            level5.AddGroup(eg12);
            level5.AddGroup(eg13);

            level5.AddGroup(eg14);
            level5.AddGroup(eg15);
            level5.AddGroup(eg16);
            level5.AddGroup(eg17);
            level5.AddGroup(eg18);
            level5.AddGroup(eg19);
            level5.AddGroup(eg20);
            level5.AddGroup(eg21);
            level5.AddGroup(eg22);
            level5.AddGroup(eg23);
            level5.AddGroup(eg24);
            level5.AddGroup(eg25);
            level5.AddGroup(eg26);

            return level5;
        }
    }
}
