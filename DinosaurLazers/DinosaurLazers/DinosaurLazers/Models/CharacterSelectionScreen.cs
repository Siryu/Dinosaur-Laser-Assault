using DinosaurLazers.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class CharacterSelectionScreen
    {
        public bool IsDone { get; set; }
        public Player.Chara Character { get; private set; }
        private KeyboardState currentKey, oldKey;
        private GamePadState currentPadState, oldPadState;

        private Texture2D background, caption, menuBackgroundRect, finishedButton;
        private Texture2D brontoRect, rexRect,
                          pteraRect, triceraRect,
                          highlightedRect;
        private Texture2D brontSprite, rexSprite, pterySprite, ceraSprite;
        private Rectangle highlightedData;
        private Color menuItemColor;
        private bool brontoSelected, rexSelected, pteraSelected, triceraSelected;

        private GraphicsDeviceManager gdm;

        private SpriteFont subTitleFont;
        private SpriteFont finishedFont;

        private SoundEffect moveMenuSound;
        private SoundEffect PteraSound;
        private SoundEffect triSound;
        private SoundEffect brontoSound;
        private SoundEffect trexSound;

        private BackgroundMusic bgMusic;
        private bool isBGMusicPlaying = false;

        public CharacterSelectionScreen(GraphicsDeviceManager gdm, ContentManager cm)
        {
            this.gdm = gdm;

            background = new Texture2D(gdm.GraphicsDevice, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            menuBackgroundRect = new Texture2D(gdm.GraphicsDevice, gdm.PreferredBackBufferWidth - 100, gdm.PreferredBackBufferHeight - 100);
            caption = cm.Load<Texture2D>("CharacterSelectionScreen/CharacterSelectionScreen-Caption");

            brontoRect = new Texture2D(gdm.GraphicsDevice, 400, 400);
            rexRect = new Texture2D(gdm.GraphicsDevice, 400, 400);
            pteraRect = new Texture2D(gdm.GraphicsDevice, 400, 400);
            triceraRect = new Texture2D(gdm.GraphicsDevice, 400, 400);
            finishedButton = new Texture2D(gdm.GraphicsDevice, 400, 150);
            brontoSelected = true;

            brontSprite = cm.Load<Texture2D>("CharacterSelectionScreen/Bronto");
            rexSprite = cm.Load<Texture2D>("CharacterSelectionScreen/Trex");
            pterySprite = cm.Load<Texture2D>("CharacterSelectionScreen/Pteradactyl");
            ceraSprite = cm.Load<Texture2D>("CharacterSelectionScreen/Triceratops");

            moveMenuSound = cm.Load<SoundEffect>("Sounds/moveMenu");
            PteraSound = cm.Load<SoundEffect>("Sounds/Ptera");
            triSound = cm.Load<SoundEffect>("Sounds/tri");
            brontoSound = cm.Load<SoundEffect>("Sounds/bronto");
            trexSound = cm.Load<SoundEffect>("Sounds/trex");

            subTitleFont = cm.Load<SpriteFont>("Fonts/SubtextFont");
            finishedFont = cm.Load<SpriteFont>("Fonts/FinishButtonFont");

            menuItemColor.A = 150;
            menuItemColor.R = 50;
            menuItemColor.G = 50;
            menuItemColor.B = 50;

            background.SetData(ColorPicker.setTexture(background.Width, background.Height, Color.DarkGray));
            menuBackgroundRect.SetData(ColorPicker.setTexture(menuBackgroundRect.Width, menuBackgroundRect.Height, menuItemColor));
            brontoRect.SetData(ColorPicker.setTexture(brontoRect.Width, brontoRect.Height, menuItemColor));
            rexRect.SetData(ColorPicker.setTexture(rexRect.Width, rexRect.Height, menuItemColor));
            pteraRect.SetData(ColorPicker.setTexture(pteraRect.Width, pteraRect.Height, menuItemColor));
            triceraRect.SetData(ColorPicker.setTexture(triceraRect.Width, triceraRect.Height, menuItemColor));
            finishedButton.SetData(ColorPicker.setTexture(finishedButton.Width, finishedButton.Height, menuItemColor));

            bgMusic = new BackgroundMusic(cm, BackgroundMusic.playAt.CharacterSelectScreen);
        }

        public void Update(GameTime gameTime)
        {
            if (!isBGMusicPlaying)
            {
                bgMusic.Play();
                isBGMusicPlaying = true;
            }

            CheckInput(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None));
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.DarkGray);
            sb.End();

            sb.Begin();
            sb.Draw(menuBackgroundRect, new Rectangle(50, 50, menuBackgroundRect.Width, menuBackgroundRect.Height), Color.DarkGray);
            sb.End();

            UpdateHighlightTexture();
            UpdateHighlightRectangle();

            sb.Begin();
            sb.Draw(caption, new Rectangle(0, 50, 1920, 1080), Color.White);
            sb.Draw(brontoRect, new Rectangle(105, 405, brontoRect.Width, brontoRect.Height), Color.DarkGray);
            sb.Draw(rexRect, new Rectangle(545, 405, rexRect.Width, rexRect.Height), Color.DarkGray);
            sb.Draw(pteraRect, new Rectangle(985, 405, pteraRect.Width, pteraRect.Height), Color.DarkGray);
            sb.Draw(triceraRect, new Rectangle(1425, 405, triceraRect.Width, triceraRect.Height), Color.DarkGray);
            //sb.Draw(finishedButton, new Rectangle(765, 845, finishedButton.Width, finishedButton.Height), Color.DarkGray);
            sb.End();

            sb.Begin();
            sb.Draw(highlightedRect, highlightedData, Color.White);
            sb.End();

            sb.Begin();
            sb.Draw(brontoRect, new Rectangle(100, 400, brontoRect.Width, brontoRect.Height), Color.DarkGray);
            sb.Draw(rexRect, new Rectangle(540, 400, rexRect.Width, rexRect.Height), Color.DarkGray);
            sb.Draw(pteraRect, new Rectangle(980, 400, pteraRect.Width, pteraRect.Height), Color.DarkGray);
            sb.Draw(triceraRect, new Rectangle(1420, 400, triceraRect.Width, triceraRect.Height), Color.DarkGray);
            //sb.Draw(finishedButton, new Rectangle(760, 840, finishedButton.Width, finishedButton.Height), Color.DarkGray);
            sb.End();

            sb.Begin();
            sb.DrawString(subTitleFont, "BRONT", new Vector2(190, 450), Color.White);
            sb.Draw(brontSprite, new Rectangle(275, 550, 36, 184), Color.White);
            sb.DrawString(subTitleFont, "REX", new Vector2(680, 450), Color.White);
            sb.Draw(rexSprite, new Rectangle(715, 550, 53, 183), Color.White);
            sb.DrawString(subTitleFont, "PTERY", new Vector2(1080, 450), Color.White);
            sb.Draw(pterySprite, new Rectangle(1090, 580, 180, 100), Color.White);
            sb.DrawString(subTitleFont, "CERA", new Vector2(1540, 450), Color.White);
            sb.Draw(ceraSprite, new Rectangle(1590, 550, 61, 175), Color.White);
            //sb.DrawString(finishedFont, "DONE", new Vector2(800, 865), Color.White);
            sb.End();
        }

        private void UpdateHighlightTexture()
        {
            highlightedRect = new Texture2D(gdm.GraphicsDevice, 400, 400);
            highlightedRect.SetData(ColorPicker.setTexture(highlightedRect.Width, highlightedRect.Height, Color.White));
        }

        private void UpdateHighlightRectangle()
        {
            if (brontoSelected)
            {
                highlightedData = new Rectangle(100, 400, brontoRect.Width, brontoRect.Height);
            }
            else if (rexSelected)
            {
                highlightedData = new Rectangle(540, 400, rexRect.Width, rexRect.Height);
            }
            else if (pteraSelected)
            {
                highlightedData = new Rectangle(980, 400, pteraRect.Width, pteraRect.Height);
            }
            else if (triceraSelected)
            {
                highlightedData = new Rectangle(1420, 400, triceraRect.Width, triceraRect.Height);
            }
            else
            {
                //A default that will most likely never be called, but implemented just to be sure
                highlightedData = new Rectangle(0, 0, 0, 0);
            }
        }

        private void CheckInput(KeyboardState state, GamePadState padState)
        {
            currentKey = state;
            currentPadState = padState;

            if (ProperLeftInput())
            {
                moveMenuSound.Play();

                if (brontoSelected)
                {
                    brontoSelected = false;
                    rexSelected = true;
                }
                else if (rexSelected)
                {
                    rexSelected = false;
                    pteraSelected = true;
                }
                else if (pteraSelected)
                {
                    pteraSelected = false;
                    triceraSelected = true;
                }
                else if (triceraSelected)
                {
                    triceraSelected = false;
                    brontoSelected = true;
                }
            }
            if (((currentKey.IsKeyDown(Keys.A) || currentKey.IsKeyDown(Keys.Left)) ||
                currentPadState.IsButtonDown(Buttons.LeftThumbstickLeft) || currentPadState.IsButtonDown(Buttons.DPadLeft)) &&
                !(oldKey.IsKeyDown(Keys.A) || oldKey.IsKeyDown(Keys.Left) ||
                oldPadState.IsButtonDown(Buttons.LeftThumbstickLeft) || oldPadState.IsButtonDown(Buttons.DPadLeft)))
            {
                moveMenuSound.Play();

                if (brontoSelected)
                {
                    brontoSelected = false;
                    triceraSelected = true;
                }
                else if (rexSelected)
                {
                    rexSelected = false;
                    brontoSelected = true;
                }
                else if (pteraSelected)
                {
                    pteraSelected = false;
                    rexSelected = true;
                }
                else if (triceraSelected)
                {
                    triceraSelected = false;
                    pteraSelected = true;
                }
            }
            if ((currentKey.IsKeyDown(Keys.Enter) || currentKey.IsKeyDown(Keys.Space) || currentKey.IsKeyDown(Keys.F)) || currentPadState.IsButtonDown(Buttons.A)) //turn || back to && once majority of selection is ready to test
            {
                if (brontoSelected)
                {
                    brontoSound.Play();
                    Character = Player.Chara.Bronto;
                }
                else if (rexSelected)
                {
                    trexSound.Play();
                    Character = Player.Chara.Trex;
                }
                else if (pteraSelected)
                {
                    PteraSound.Play();
                    Character = Player.Chara.Ptera;
                }
                else if (triceraSelected)
                {
                    triSound.Play();
                    Character = Player.Chara.Cera;
                }

                IsDone = true;
            }

            oldKey = currentKey;
            oldPadState = currentPadState;
        }

        private bool ProperLeftInput()
        {
            if ((currentKey.IsKeyDown(Keys.D) || currentKey.IsKeyDown(Keys.Right) || 
                currentPadState.IsButtonDown(Buttons.LeftThumbstickRight) || currentPadState.IsButtonDown(Buttons.DPadRight)) &&
                !(oldKey.IsKeyDown(Keys.D) || oldKey.IsKeyDown(Keys.Right) ||
                oldPadState.IsButtonDown(Buttons.LeftThumbstickRight) || oldPadState.IsButtonDown(Buttons.DPadRight)))
            {
                return true;
            }
            return false;
        }
    }
}
