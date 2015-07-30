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
    public class AnimatedSprite : Sprite
    {

        public float elapsed;
        public int frames = 0;
        public int frameTotal;
        public int startX, startY;
        public float delay = 200f;

        public AnimatedSprite(int frameTotal, int startX, int startY, int width, int height, Game game, Texture2D texture, Vector2 position, SpriteBatch batch, EnemyType type)
            : base(game, texture, position, batch, type)
        {
            this.frameTotal = frameTotal;
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;

            
        }

        public override void Update(GameTime gameTime)
        {
            
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                if (frames >= frameTotal)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float opacity = 1;

            if (InvincibleTimeLeft > 1000)
            {
                opacity = 0.2f;
            }
            else if (InvincibleTimeLeft > 0)
            {
                opacity = ((int)(gameTime.TotalGameTime.Milliseconds / 100)) % 2 == 0 ? .5f : 1f;
            }
            if (IsHit)
            {
                SpriteBatch.Draw(_texture, new Vector2(Position.X - width / 2, Position.Y - height / 2), new Rectangle(width * frames, 0, width, height), Color.IndianRed * opacity);
            }
            else
            {
                SpriteBatch.Draw(_texture, new Vector2(Position.X - width / 2, Position.Y - height / 2), new Rectangle(width * frames, 0, width, height), Color * opacity);
            }

        }
    }
}
