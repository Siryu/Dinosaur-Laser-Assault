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
namespace DinosaurLazers.Models
{
    public class HealthBar
    {
        private Texture2D container, healthbar;
        private Vector2 position;
        private int fullHealth;
        private int currentHealth;
        private Color barColor;

        public bool IsVisible { get; set; }

        public HealthBar(ContentManager content, GraphicsDeviceManager gdm, Vector2 position)
        {
            this.position = position;
            LoadContent(content);
            fullHealth = healthbar.Width;
            currentHealth = fullHealth;
        }
        private void LoadContent(ContentManager content)
        {
            container = content.Load<Texture2D>("Images/healthBar");
            healthbar = content.Load<Texture2D>("Images/health");
        }

        public void Update(float health)
        {
            //currentHealth = (int)(health * fullHealth);
            if (health >= 0)
                currentHealth = (int)(health * fullHealth);

            HealthColor();
        }

        public void Update(float health, Vector2 position)
        {
            HealthColor();
            if (currentHealth >= 0)
                currentHealth = (int)(health * fullHealth);

            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(container, position, Color.White);
            spriteBatch.Draw(healthbar, position, new Rectangle((int) position.X, (int)position.Y, currentHealth, healthbar.Height), barColor);
        }

        public void Draw(SpriteBatch spriteBatch, Sprite enemySprite)
        {
            float scale = 0.2f;
            Vector2 _spacing = new Vector2(enemySprite.Position.X - ((this.healthbar.Width / 2) * scale), enemySprite.Position.Y + (enemySprite.height / 1.8f));
            spriteBatch.Draw(container, _spacing, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(healthbar, _spacing, new Rectangle((int)position.X, (int)position.Y, currentHealth, healthbar.Height), barColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        private void HealthColor()
        {
            if (currentHealth >= healthbar.Width * 0.75)
            {
                barColor = Color.Green;
            }
            else if (currentHealth >= healthbar.Width * 0.25)
            {
                barColor = Color.Yellow;
            }
            else
            {
                barColor = Color.Red;
            }
        }
    }
}
