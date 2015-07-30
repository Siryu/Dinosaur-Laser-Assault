using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DinosaurLazers.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DinosaurLazers.Models
{
    public class EnemyShots
    {
        public Dictionary<Vector2, Sprite> list;
        Texture2D simpleImage, gunnerImage, bomberImage, dartingImage, blinkingImage, hardImage, level1MidBossImage, level1BossImage, level2midboss, level2boss, level3boss, level4boss;
        SoundEffect enemyLaser, bidew, blop, few;

        SoundEffect sound = null;

        public EnemyShots(ContentManager content)
        {
            list = new Dictionary<Vector2,Sprite>();
            simpleImage = content.Load<Texture2D>("Images/redLazer");
            bomberImage = content.Load<Texture2D>("Images/laserRedShot");
            gunnerImage = content.Load<Texture2D>("Images/gunnerbullets");
            blinkingImage = content.Load<Texture2D>("Images/blinkerbullets");
            enemyLaser = content.Load<SoundEffect>("Sounds/enemylaser");
            level2midboss = content.Load<Texture2D>("Images/firespear");
            level2boss = content.Load<Texture2D>("Enemies/tank_spike");
            level3boss = content.Load<Texture2D>("Enemies/fireball");
            level4boss = content.Load<Texture2D>("Enemies/stego_ammo");
            bidew = content.Load<SoundEffect>("Sounds/Bidew");
            blop = content.Load<SoundEffect>("Sounds/Blop");
            few = content.Load<SoundEffect>("Sounds/Few");
            dartingImage = simpleImage;
            hardImage = simpleImage;
            level1MidBossImage = bomberImage;
            level1BossImage = dartingImage;
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, Sprite playerOneSprite, Sprite playerTwoSprite = null)
        {
            // X for equation
            float rotation = (float)Math.PI / 64;

            for (int i = 0; i < list.Keys.Count; i++)
            {
                Vector2 shotoriginal = list.Keys.ElementAt(i);
                Sprite s = list[shotoriginal];
                Vector2 shotNew = shotoriginal;
                list.Remove(shotoriginal);
                float bulletspeed = .5f;



                if (s.enemyType.type == EnemyType.Etype.Simple)
                {
                    bulletspeed = .5f;
                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                    if (playerTwoSprite != null)
                    {
                        shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerTwoSprite.Position.X - shotNew.X);
                    }
                }
                else if (s.enemyType.type == EnemyType.Etype.Bomber)
                {
                    bulletspeed = .3f;
                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;

                    // A Sin (Bx - C) + D
                    s.ShotRotation += rotation;
                    if (s.ShotRotation >= 2 * Math.PI)
                    {
                        s.ShotRotation = 0;
                    }
                    shotNew += Vector2.UnitX * 5 * (float)Math.Sin(s.ShotRotation);
                }
                else if (s.enemyType.type == EnemyType.Etype.Gunner)
                {
                    bulletspeed = 1f;


                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                    if (playerTwoSprite != null)
                    {
                        shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerTwoSprite.Position.X - shotNew.X);
                    }
                }
                else if (s.enemyType.type == EnemyType.Etype.Blinking)
                {
                    bulletspeed = .5f;

                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                    if (playerTwoSprite != null)
                    {
                        shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerTwoSprite.Position.X - shotNew.X);
                    }
                }
                else if (s.enemyType.type == EnemyType.Etype.Darting)
                {
                    bulletspeed = .4f;
                   
                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                    if (playerTwoSprite != null)
                    {
                        shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerTwoSprite.Position.X - shotNew.X);
                    }
                }
                else if (s.enemyType.type == EnemyType.Etype.Hard)
                {
                    bulletspeed = 1f;
                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (s.enemyType.type == EnemyType.Etype.Level1MidBoss)
                {
                    bulletspeed = .9f;
                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                }

                else if (s.enemyType.type == EnemyType.Etype.Level1Boss)
                {
                    bulletspeed = 1.2f;

                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                }
                else if (s.enemyType.type == EnemyType.Etype.Level2MidBoss)
                {
                    bulletspeed = .3f;

                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                }
                else if (s.enemyType.type == EnemyType.Etype.Level2Boss)
                {
                    bulletspeed = .3f;

                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                }
                else if (s.enemyType.type == EnemyType.Etype.Level3Boss)
                {
                    bulletspeed = 1f;

                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                }
                else if (s.enemyType.type == EnemyType.Etype.Level4Boss)
                {
                    bulletspeed = .5f;

                    shotNew += Vector2.UnitY * bulletspeed * gameTime.ElapsedGameTime.Milliseconds;
                    shotNew += Vector2.UnitX * .1f * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(playerOneSprite.Position.X - shotNew.X);
                }
                if(!list.Keys.Contains(shotNew))
                    list.Add(shotNew, s);

                if (shotNew.Y > graphics.PreferredBackBufferHeight + 10)
                {
                    list.Remove(shotNew);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector2 shot in list.Keys)
            {
                Sprite sprite;
                Texture2D image = null;
                
                list.TryGetValue(shot, out sprite);
                if (sprite.enemyType.type == EnemyType.Etype.Simple)
                {
                    image = simpleImage;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Bomber)
                {
                    image = bomberImage;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Gunner)
                {
                    image = gunnerImage;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Level1MidBoss)
                {
                    image = gunnerImage;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Darting)
                {
                    image = dartingImage;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Blinking)
                {
                    image = blinkingImage;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Hard)
                {
                    image = hardImage;
                    sound = bidew;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Level1Boss)
                {
                    image = level1BossImage;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Level2MidBoss)
                {
                    image = level2midboss;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Level2Boss)
                {
                    image = level2boss;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Level3Boss)
                {
                    image = level3boss;
                }
                else if (sprite.enemyType.type == EnemyType.Etype.Level4Boss)
                {
                    image = level4boss;
                }
                spriteBatch.Draw(image, shot, Color.White);
            }
        }

        public void AddShot(Sprite enemy)
        {
           // if more than 1 bullet, or 
            
            if(!this.list.Keys.Contains(enemy.Position + Vector2.UnitY * 30))
            {
                if (enemy.Name != "Explode")
                {
                    list.Add(enemy.Position + Vector2.UnitY * 30, enemy);

                    if (enemy.enemyType.type == EnemyType.Etype.Level1Boss)
                    {
                        Vector2 leftShot = enemy.Position;
                        Vector2 rightShot = enemy.Position;
                        leftShot.Y += 210;
                        leftShot.X -= 440;
                        rightShot.Y += 210;
                        rightShot.X += 440;

                        if (!this.list.Keys.Contains(leftShot) && !this.list.Keys.Contains(rightShot))
                        {
                            list.Add(leftShot, enemy);
                            list.Add(rightShot, enemy);
                        }
                    }
                }
            }
        }
    }
}
