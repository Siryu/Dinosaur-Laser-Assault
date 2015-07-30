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
    public class EnemyGroup
    {
        public List<Enemy> group;
        bool isBoss;
        bool isMidBoss;
        long distanceToPop;

        public EnemyGroup(ContentManager content, GraphicsDeviceManager graphics, EnemyType enemyType, long distance, int enemyCount = 3, Enemy.Direction entryDirection = Enemy.Direction.Left, bool isBoss = false, bool isMidBoss = false)
        {
            distanceToPop = distance;
            this.isBoss = isBoss;
            this.isMidBoss = isMidBoss;
            group = new List<Enemy>();
            for (int i = 0; i < enemyCount; i++)
            {
                Enemy enemy = new Enemy(content, graphics, enemyType, entryDirection);
                if (enemyType.type == EnemyType.Etype.Level2MidBoss || enemyType.type == EnemyType.Etype.Level2Boss || enemyType.type == EnemyType.Etype.Level3Boss || enemyType.type == EnemyType.Etype.Level4Boss)
                {
                    enemy.isAnimatedSprite = true;
                }
                group.Add(enemy);
            }
            Enemy.index = 1;
        }
        
        public void Update(long distanceTravelled, Level level)
        {
            if (distanceTravelled >= distanceToPop)
            {
                foreach(Enemy e in group)
                {
                    level.enemies.Add(e);
                }
                if (this.isBoss)
                    level.boss = true;
                else if (this.isMidBoss)
                    level.midBoss = true;
                this.group.Clear();
            }
        }
    }
}
