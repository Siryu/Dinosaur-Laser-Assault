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
    public class Enemy
    {
        public enum Direction
        {
            Left, Right, Top
        }

        public bool isAnimatedSprite = false;

        private int _maxHealth;
        public int _currentHealth;

        static Random rand = new Random();
        public static int index = 1;

        public EnemyType type;
        public Vector2 Position 
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }
        public Vector2 Movement
        {
            get
            {
                return movement;
            }
            set
            {
                movement = value;
            }
        }
        public Texture2D Image { get; set; }

        Vector2 position;
        Vector2 movement;
        //float enemySpacing = 100;
        //float enemySpacing = MathHelper.Clamp(3000, 70, 1000);

        public Enemy(ContentManager content, GraphicsDeviceManager graphics,EnemyType type, Direction direction = Direction.Left )
        {
            this.type = type;
            Image = type.image;

            _maxHealth = 50;
            _currentHealth = _maxHealth;

            Position = Vector2.Zero;
            position.Y = 50;
            Movement = Vector2.Zero;

            if (direction == Direction.Right)
            {
                position.X += graphics.PreferredBackBufferWidth + 300 + type.spacing * (index + 1);
                movement = Vector2.UnitX * -type.speed;
                movement.Y = (float)(rand.NextDouble() * type.speed - .2f);
            }
            else if (direction == Direction.Left)
            {
                position.X = -300 - type.spacing * (index + 1);
                movement = Vector2.UnitX * type.speed;
                movement.Y = (float)(rand.NextDouble() * type.speed - .2f);
            }
            else
            {
                position.X = (type.spacing * (index + 1)) * graphics.PreferredBackBufferWidth / 2;
                movement = Vector2.UnitX / 2;
                movement.Y = (float)(type.speed * rand.NextDouble());
            }
            //position.X = graphics.PreferredBackBufferWidth / 2;
            index++;
        }
    }
}
