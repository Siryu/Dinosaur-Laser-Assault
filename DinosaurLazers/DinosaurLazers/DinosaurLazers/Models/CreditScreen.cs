using DinosaurLazers.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class CreditScreen
    {
        private Texture2D background, creditText;
        private int count;
        private string endMessage = "", otherMessage = "";
        private string baseMessage = "THEIR REVENGE ACHIEVED,", restOfMessage = "THE DINOSAURS WENT TO REST.";
        private bool firstDone, secondDone;
        public bool TextDone { get; set; }
        public bool BeginBackgroundCreditScroll { get; set; }
        private SpriteFont font;
        //private SoundEffect textBloop;
        private BackgroundMusic music;

        public CreditScreen(GraphicsDeviceManager gdm, ContentManager cm)
        {
            background = new Texture2D(gdm.GraphicsDevice, 1920, 1080);
            background.SetData(ColorPicker.setTexture(background.Width, background.Height, Color.Black));
            creditText = cm.Load<Texture2D>("Images/credit-text");
            font = cm.Load<SpriteFont>("Fonts/FinishButtonFont");
            //textBloop = cm.Load<SoundEffect>("Sounds/Text-Noise");
        }

        public void Update(GameTime gameTime)
        {
            if (!TextDone)
            {
                WriteText();
            }
            
        }

        private void WriteText()
        {
            if (!firstDone)
            {
                count++;
            }
            else
            {
                if (!secondDone)
                {
                    count++;
                }
            }

            if (count % 5 == 0 && !firstDone)
            {
                try
                {
                    endMessage += baseMessage.ElementAt((count / 5) - 1);
                    //textBloop.Play();
                }
                catch (Exception e)
                {
                    string s = e.ToString();
                    firstDone = true;
                    count = 0;
                }
            }

            if ((count % 5 == 0 && count >= 5) && !secondDone && firstDone)
            {
                try
                {
                    otherMessage += restOfMessage.ElementAt((count / 5) - 1);
                    //textBloop.Play();
                }
                catch (Exception e)
                {
                    string s = e.ToString();
                    secondDone = true;
                }
            }

            if (firstDone && secondDone)
            {
                TextDone = true;
            }
        }

        public void Draw(SpriteBatch sb, Level currentLevel)
        {
            

            if (!BeginBackgroundCreditScroll)
            {
                sb.Begin();
                sb.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.Black);
                sb.End();

                if (!string.IsNullOrEmpty(endMessage))
                {
                    sb.Begin();
                    sb.DrawString(font, endMessage, new Vector2(25, 500), Color.White);
                    sb.End();
                }
                if (!string.IsNullOrEmpty(otherMessage))
                {
                    sb.Begin();
                    sb.DrawString(font, otherMessage, new Vector2(50, 600), Color.White);
                    sb.End();
                }
            }
            else
            {
                sb.Begin();
                currentLevel.background.Draw(sb);
                sb.End();
                sb.Begin();
                currentLevel.border.Draw(sb);
                sb.End();
                sb.Begin();
                sb.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.Black * .4f);
                sb.End();
                sb.Begin();
                sb.Draw(creditText, new Rectangle(0, 0, 1920, 1080), Color.White);
                sb.End();
            }
        }
    }
}
