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
    public class ChargeMeter
    {
        private Texture2D container, chargebar;
        private Vector2 position;
        private int fullcharge;
        private int currentCharge;
        private Color barColor;

        public ChargeMeter(ContentManager content, GraphicsDeviceManager gdm, Vector2 position)
        { 
            LoadContent(content);
            fullcharge = chargebar.Width;
            currentCharge = fullcharge;
            this.position = position;
            
        }
        private void LoadContent(ContentManager content)
        {
            container = content.Load<Texture2D>("Images/healthBar");
            chargebar = content.Load<Texture2D>("Images/health");
        }

        public void Update(float charge)
        {
            ChargeColor();
            currentCharge = (int)(charge * chargebar.Width);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(chargebar, position, new Rectangle((int)position.X, (int)position.Y, currentCharge, chargebar.Height), barColor);
            spriteBatch.Draw(container, position, Color.White);
        }

        private void ChargeColor()
        {
            if (currentCharge >= chargebar.Width * 0.75)
            {
                barColor = Color.Blue;
            }
            else if (currentCharge >= chargebar.Width * 0.25)
            {
                barColor = Color.CornflowerBlue;
            }
            else
            {
                barColor = Color.AliceBlue;
            }
        }
    }
}
