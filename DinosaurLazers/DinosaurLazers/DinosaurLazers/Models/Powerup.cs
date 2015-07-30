using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public enum PowerupType
    {
        Health, Charge, Power
    }

    public class Powerup
    {
        public Texture2D powerupTexture;
        public PowerupType type;
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Powerup(GraphicsDeviceManager gdm, SpriteBatch sb, ContentManager cm, Vector2 position)
        {
            this.position = position;
            Random rng = new Random();
            double rand = rng.NextDouble();
            if (rand < 0.9)
            {
                powerupTexture = cm.Load<Texture2D>("Images/health-powerup");
                type = PowerupType.Health;
            }
            else
            {
                powerupTexture = cm.Load<Texture2D>("Images/charge-powerup");
                type = PowerupType.Charge;
            }

        }

        public void Update(GameTime gameTime)
        {
            position.Y += (float)(gameTime.ElapsedGameTime.Milliseconds / 6f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            spriteBatch.Draw(powerupTexture, new Rectangle((int)position.X, (int)position.Y, 50, 50), Color.White);
            //spriteBatch.End();
        }
    }
}
