using DinosaurLazers.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Enemies
{
    public class ShotBehavior
    {
        Random rand = new Random();
        SoundEffect gunnerSound, bomberSound, simpleSound;

        public ShotBehavior(ContentManager Content)
        {
            simpleSound = Content.Load<SoundEffect>("Sounds/simpleSound");
            gunnerSound = simpleSound;
            bomberSound = simpleSound;
        }

        public void Update(GraphicsDeviceManager graphics, Sprite enemy, EnemyShots enemyShots)
        {
            


            if (rand.NextDouble() < 0.01 && enemy.Position.X > -10 && enemy.Position.X < graphics.PreferredBackBufferWidth + 10 && enemy.Name != "Explode")
            {
                enemy.OKFire = true;
                enemy.bulletCount = 0;
            }

            if (enemy.enemyType.type == EnemyType.Etype.Gunner && enemy.OKFire)
            {
                if (enemy.bulletCount < 7)
                {
                    gunnerSound.Play();
                    enemyShots.AddShot(enemy);
                    enemy.bulletCount++;
                }
                else
                {
                    enemy.OKFire = false;
                }
            }
            else if (enemy.enemyType.type == EnemyType.Etype.Level1Boss && enemy.OKFire)
            {
                if (enemy.bulletCount < 2)
                {
                    simpleSound.Play();
                    enemyShots.AddShot(enemy);
                    enemy.bulletCount++;
                }
                else
                {
                    enemy.OKFire = false;
                }
            }
            else if (enemy.OKFire)
            {
                simpleSound.Play();
                enemyShots.AddShot(enemy);
                enemy.OKFire = false;
            }
        }

    }
}
