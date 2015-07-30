using DinosaurLazers.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class PauseScreen
    {
        private KeyboardState currentKey, oldKey;
        private GamePadState currentPadState, oldPadState;

        private Texture2D pauseBackground, pauseText, resumeButton, quitButton, titleButton;
        private bool resumeHighlighted, quitHighlighted, titleHighlighted;

        public bool ResumeSelected { get; set; }
        public bool QuitSelected { get; set; }
        public bool TitleSelected { get; set; }

        private Texture2D highlightedOption;
        private Rectangle highlightedData;

        private Color c;
        
        public PauseScreen(GraphicsDeviceManager gdm, ContentManager cm)
        {
            pauseBackground = new Texture2D(gdm.GraphicsDevice, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            resumeButton = new Texture2D(gdm.GraphicsDevice, 800, 100);
            titleButton = new Texture2D(gdm.GraphicsDevice, 800, 100);
            quitButton = new Texture2D(gdm.GraphicsDevice, 800, 100);

            highlightedOption = new Texture2D(gdm.GraphicsDevice, 800, 100);
            highlightedData = new Rectangle(560, 485, 800, 100);

            pauseText = cm.Load<Texture2D>("PauseScreen/pause-menu-text-redone");

            resumeHighlighted = true;

            c.A = 210;
            c.R = 60;
            c.G = 60;
            c.B = 60;

            pauseBackground.SetData(ColorPicker.setTexture(pauseBackground.Width, pauseBackground.Height, c));
            resumeButton.SetData(ColorPicker.setTexture(resumeButton.Width, resumeButton.Height, c));
            titleButton.SetData(ColorPicker.setTexture(titleButton.Width, titleButton.Height, c));
            quitButton.SetData(ColorPicker.setTexture(quitButton.Width, quitButton.Height, c));
            highlightedOption.SetData(ColorPicker.setTexture(highlightedOption.Width, highlightedOption.Height, Color.White));
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardInput(Keyboard.GetState());
            if (GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None).IsConnected)
            {
                CheckGamePadInput(GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None));
            }

            if (resumeHighlighted)
            {
                highlightedData = new Rectangle(560, 485, 800, 100);
            }
            else if (titleHighlighted)
            {
                highlightedData = new Rectangle(560, 610, 800, 100);
            }
            else if (quitHighlighted)
            {
                highlightedData = new Rectangle(560, 735, 800, 100);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(pauseBackground, new Rectangle(0, 0, 1920, 1080), c);
            sb.End();

            sb.Begin();
            sb.Draw(resumeButton, new Rectangle(565, 490, 800, 100), c);
            sb.Draw(titleButton, new Rectangle(565, 615, 800, 100), c);
            sb.Draw(quitButton, new Rectangle(565, 740, 800, 100), c);
            sb.End();

            if (highlightedData != null)
            {
                sb.Begin();
                sb.Draw(highlightedOption, highlightedData, Color.White);
                sb.End();
            }

            sb.Begin();
            sb.Draw(resumeButton, new Rectangle(560, 485, 800, 100), c);
            sb.Draw(titleButton, new Rectangle(560, 610, 800, 100), c);
            sb.Draw(quitButton, new Rectangle(560, 735, 800, 100), c);
            sb.End();

            sb.Begin();
            sb.Draw(pauseText, new Rectangle(0, 20, 1920, 1080), Color.White);
            sb.End();
        }

        private void CheckKeyboardInput(KeyboardState ks)
        {
            currentKey = ks;

            if ((currentKey.IsKeyDown(Keys.S) || currentKey.IsKeyDown(Keys.Down)) && !(oldKey.IsKeyDown(Keys.S) || oldKey.IsKeyDown(Keys.Down)))
            {
                if (titleHighlighted)
                {
                    titleHighlighted = false;
                    quitHighlighted = true;
                }
                else if (resumeHighlighted)
                {
                    resumeHighlighted = false;
                    titleHighlighted = true;
                }
            }
            if ((currentKey.IsKeyDown(Keys.W) || currentKey.IsKeyDown(Keys.Up)) && !(oldKey.IsKeyDown(Keys.W) || oldKey.IsKeyDown(Keys.Up)))
            {
                if (titleHighlighted)
                {
                    titleHighlighted = false;
                    resumeHighlighted = true;
                }
                else if (quitHighlighted)
                {
                    quitHighlighted = false;
                    titleHighlighted = true;
                }
            }

            if ((currentKey.IsKeyDown(Keys.Enter) || currentKey.IsKeyDown(Keys.Space) || currentKey.IsKeyDown(Keys.F)) && !(oldKey.IsKeyDown(Keys.Enter) || oldKey.IsKeyDown(Keys.Space) || oldKey.IsKeyDown(Keys.F)))
            {
                if (resumeHighlighted)
                    ResumeSelected = true;

                if (quitHighlighted)
                    QuitSelected = true;

                if (titleHighlighted)
                    TitleSelected = true;
            }

            oldKey = currentKey;
        }

        private void CheckGamePadInput(GamePadState gps)
        {
            currentPadState = gps;

            if ((currentPadState.IsButtonDown(Buttons.LeftThumbstickDown) || currentPadState.IsButtonDown(Buttons.DPadDown)) && !(oldPadState.IsButtonDown(Buttons.LeftThumbstickDown) || oldPadState.IsButtonDown(Buttons.DPadDown)))
            {
                if (titleHighlighted)
                {
                    titleHighlighted = false;
                    quitHighlighted = true;
                }
                else if (resumeHighlighted)
                {
                    resumeHighlighted = false;
                    titleHighlighted = true;
                }
            }
            if ((currentPadState.IsButtonDown(Buttons.LeftThumbstickUp) || currentPadState.IsButtonDown(Buttons.DPadUp)) && !(oldPadState.IsButtonDown(Buttons.LeftThumbstickUp) || oldPadState.IsButtonDown(Buttons.DPadUp)))
            {
                if (titleHighlighted)
                {
                    titleHighlighted = false;
                    resumeHighlighted = true;
                }
                else if (quitHighlighted)
                {
                    quitHighlighted = false;
                    titleHighlighted = true;
                }
            }

            if (currentPadState.IsButtonDown(Buttons.A) && !oldPadState.IsButtonDown(Buttons.A))
            {
                if (resumeHighlighted)
                    ResumeSelected = true;

                if (quitHighlighted)
                    QuitSelected = true;

                if (titleHighlighted)
                    TitleSelected = true;
            }

            oldPadState = currentPadState;
        }
    }
}
