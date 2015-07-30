using DinosaurLazers.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class LoadingScreen
    {
        public bool IsDone { get; private set; }

        SpriteFont font;
        Texture2D background, readyText;
        private int counter;
        private float healthAlphaChanger, chargeAlphaChanger, levelAlphaChanger, enemyAlphaChanger;
        private string health, charge, level, enemy;

        public LoadingScreen(GraphicsDeviceManager gdm, ContentManager cm)
        {
            font = cm.Load<SpriteFont>("Fonts/SubtextFont");

            health = "Healing";
            charge = "Recharging";
            level = "Creating Level";
            enemy = "Generating Alien Scum";

            readyText = cm.Load<Texture2D>("LoadingScreen/loading-screen-ready-text");


            background = new Texture2D(gdm.GraphicsDevice, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            background.SetData(ColorPicker.setTexture(background.Width, background.Height, Color.Black));
        }

        public void Update(GameTime gameTime)
        {
            if (counter == 0)
            {
                if (healthAlphaChanger < 255)
                healthAlphaChanger += 5;
                
                if (healthAlphaChanger == 65 || healthAlphaChanger == 130 || healthAlphaChanger == 195)
                {
                    health += ".";
                }
                else if (healthAlphaChanger == 255)
                {
                    health += " DONE!";
                    counter = 1;
                }
            }
            else if (counter == 1)
            {
                if (chargeAlphaChanger < 255)
                    chargeAlphaChanger += 5;

                if (chargeAlphaChanger == 65 || chargeAlphaChanger == 130 || chargeAlphaChanger == 195)
                {
                    charge += ".";
                }
                else if (chargeAlphaChanger == 255)
                {
                    charge += " DONE!";
                    counter = 2;
                }
            }
            else if (counter == 2)
            {
                if (levelAlphaChanger < 255)
                    levelAlphaChanger += 5;

                if (levelAlphaChanger == 65 || levelAlphaChanger == 130 || levelAlphaChanger == 195)
                {
                    level += ".";
                }
                else if (levelAlphaChanger == 255)
                {
                    level += " DONE!";
                    counter = 3;
                }
            }
            else if (counter == 3)
            {
                if (enemyAlphaChanger < 255)
                    enemyAlphaChanger += 5;

                if (enemyAlphaChanger == 65 || enemyAlphaChanger == 130 || enemyAlphaChanger == 195)
                {
                    enemy += ".";
                }
                else if (enemyAlphaChanger == 255)
                {
                    enemy += " DONE!";
                    counter = 4;
                }
            }
            else if (counter == 4)
            {
                IsDone = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.Black);
            sb.End();

            sb.Begin();
            if (counter >= 0)
            {
                sb.DrawString(font, health, new Vector2(150, 150), Color.White);
            }
            if (counter >= 1)
            {
                sb.DrawString(font, charge, new Vector2(300, 300), Color.White);
            }
            if (counter >= 2)
            {
                sb.DrawString(font, level, new Vector2(450, 450), Color.White);
            }
            if (counter >= 3)
            {
                sb.DrawString(font, enemy, new Vector2(600, 600), Color.White);
            }
            sb.End();

            sb.Begin();
            if (counter == 4)
            {
                sb.Draw(readyText, new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            sb.End();
        }
    }
}
