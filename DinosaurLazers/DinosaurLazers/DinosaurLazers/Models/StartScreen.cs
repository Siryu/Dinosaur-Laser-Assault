using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DinosaurLazers.Controls;

namespace DinosaurLazers.Models
{
    public class StartScreen
    {
        private Texture2D background, title, gameModeText, dino, landscape;
        private Texture2D smallBush, largeBush;
        private Texture2D fire, backgroundFire;

        public bool SinglePlayer { get; set; }
        public bool MultiPlayer { get; set; }

        public ChosenControl ControlScheme { get; set; }

        private KeyboardState currentKey, oldKey;
        private GamePadState currentPadInput, oldPadInput;

        private GraphicsDeviceManager gdm;
        private SpriteBatch sb;
        private ContentManager cm;

        private Texture2D highlightedRect;
        private Rectangle highlightedData;
        private Color highlightColor;

        private bool singleSelected, multiSelected;

        private float _fireOffset;
        private float _backgroundFireOffset;

        BackgroundMusic bgMusic;
        bool bgMusicIsPlaying = false;


        public StartScreen(GraphicsDeviceManager gdm, SpriteBatch sb, ContentManager cm)
        {
            this.gdm = gdm;
            this.sb = sb;
            this.cm = cm;
            background = cm.Load<Texture2D>("TitleScreen/title-screen");
            title = cm.Load<Texture2D>("TitleScreen/DinoLaserAssaultTitle");
            dino = cm.Load<Texture2D>("TitleScreen/title-dino");
            landscape = cm.Load<Texture2D>("TitleScreen/title-foreground");
            smallBush = cm.Load<Texture2D>("TitleScreen/dry-bush-small");
            largeBush = cm.Load<Texture2D>("TitleScreen/dry-bush-large");
            gameModeText = cm.Load<Texture2D>("TitleScreen/player-option-text");
            bgMusic = new BackgroundMusic(cm, BackgroundMusic.playAt.Opening);

            highlightColor.A = 50;
            highlightColor.R = 255;
            highlightColor.G = 255;
            highlightColor.B = 255;

            singleSelected = true;
            highlightedRect = new Texture2D(gdm.GraphicsDevice, 400, 100);
            highlightedRect.SetData(ColorPicker.setTexture(highlightedRect.Width, highlightedRect.Height, highlightColor));

            fire = CreateMovingFire(8, "light-fire", 256, gdm.PreferredBackBufferHeight - 180, 300);
            backgroundFire = CreateMovingFire(16, "fire", 128, gdm.PreferredBackBufferHeight - 300, 150);
        }

        public void Update(GameTime gameTime, float scrollSpeed)
        {
            if (!bgMusicIsPlaying)
            {
                this.bgMusic.Play();
                bgMusicIsPlaying = true;
            }

            _fireOffset -= scrollSpeed;
            _backgroundFireOffset -= scrollSpeed;

            if (_fireOffset <= -256)
            {
                _fireOffset += 256; 
            }
            if (_backgroundFireOffset <= -128)
            {
                _backgroundFireOffset += 128;
            }
            ValidateInputs();
            
            UpdateHighlightData();

        }

        private void ValidateInputs()
        {
            CheckInput(Keyboard.GetState());
            if (GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None).IsConnected)
            {
                CheckGamePad(GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None));
            }
        }

        private void UpdateHighlightData()
        {
            if (singleSelected)
            {
                highlightedData = new Rectangle(40, 470, 930, 120);
            }
            else if (multiSelected)
            {
                highlightedData = new Rectangle(40, 615, 850, 120);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(background, new Rectangle(0,0,gdm.PreferredBackBufferWidth,gdm.PreferredBackBufferHeight), Color.White);
            sb.End();

            sb.Begin();
            sb.Draw(landscape, new Rectangle(0, 0, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight), Color.White);
            sb.End();

            sb.Begin();
            sb.Draw(backgroundFire, Vector2.UnitX * -_backgroundFireOffset, Color.White);
            sb.End();

            sb.Begin();
            sb.Draw(title, new Rectangle(-70, -90, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight), Color.White);
            sb.Draw(smallBush, new Rectangle(-60, 750, 250, 188), Color.White);
            sb.Draw(dino, new Rectangle(200, 80, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight), Color.White);
            sb.End();

            sb.Begin();
            sb.Draw(fire, Vector2.UnitX * _fireOffset, Color.White);
            sb.End();

            if (highlightedData != null)
            {
                sb.Begin();
                sb.Draw(highlightedRect, highlightedData, highlightColor * .3f);
                sb.End();
            }

            sb.Begin();
            sb.Draw(gameModeText, new Rectangle(-120, -50, 1920, 1080), Color.White);
            sb.End();

            sb.Begin();
            sb.Draw(largeBush, new Rectangle(1650, 825, 500, 375), Color.White);
            sb.End();
        }

        private Texture2D CreateMovingFire(int numberOfGraphics, string file, float xSize, float ySize, float graphicHeight)
        {
            RenderTarget2D target = new RenderTarget2D(gdm.GraphicsDevice, 2048, 2048);
            //tell the GraphicsDevice we want to render to the gamesMenu rendertarget (an in-memory buffer)
            gdm.GraphicsDevice.SetRenderTarget(target);

            //clear the background
            gdm.GraphicsDevice.Clear(Color.Transparent);

            //begin drawing
            sb.Begin();
            for (int x = 0; x < numberOfGraphics; x++)
            {
                sb.Draw(cm.Load<Texture2D>("TitleScreen/"+file), new Vector2((x * xSize), ySize), Color.White);
            }

            sb.End();
            //reset the GraphicsDevice to draw on the backbuffer (directly to the backbuffer)
            gdm.GraphicsDevice.SetRenderTarget(null);

            return (Texture2D)target;
        }

        private void CheckInput(KeyboardState state)
        {
            currentKey = state;

            if ((currentKey.IsKeyDown(Keys.S) || currentKey.IsKeyDown(Keys.Down) || currentKey.IsKeyDown(Keys.OemSemicolon)) && !(oldKey.IsKeyDown(Keys.S) || oldKey.IsKeyDown(Keys.Down) || oldKey.IsKeyDown(Keys.OemSemicolon)))
            {
                singleSelected = false;
                multiSelected = true;
            }
            if ((currentKey.IsKeyDown(Keys.W) || currentKey.IsKeyDown(Keys.Up) || currentKey.IsKeyDown(Keys.P)) && !(oldKey.IsKeyDown(Keys.W) || oldKey.IsKeyDown(Keys.Up) || oldKey.IsKeyDown(Keys.P)))
            {
                singleSelected = true;
                multiSelected = false;
            }
            if ((currentKey.IsKeyDown(Keys.Enter) || currentKey.IsKeyDown(Keys.Space) || currentKey.IsKeyDown(Keys.F)) && !(oldKey.IsKeyDown(Keys.Enter) || oldKey.IsKeyDown(Keys.Space) || oldKey.IsKeyDown(Keys.F)))
            {
                if (singleSelected)
                {
                    SinglePlayer = true;
                }
                else if (multiSelected)
                {
                    MultiPlayer = true;
                }
                ControlScheme = ChosenControl.Keyboard;
            }

            oldKey = currentKey;
        }

        private void CheckGamePad(GamePadState state)
        {
            currentPadInput = state;

            if ((currentPadInput.IsButtonDown(Buttons.LeftThumbstickDown) || currentPadInput.IsButtonDown(Buttons.DPadDown)) && !(oldPadInput.IsButtonDown(Buttons.LeftThumbstickDown) || oldPadInput.IsButtonDown(Buttons.DPadDown)))
            {
                singleSelected = false;
                multiSelected = true;
            }
            if ((currentPadInput.IsButtonDown(Buttons.LeftThumbstickUp) || currentPadInput.IsButtonDown(Buttons.DPadUp)) && !(oldPadInput.IsButtonDown(Buttons.LeftThumbstickUp) || oldPadInput.IsButtonDown(Buttons.DPadUp)))
            {
                singleSelected = true;
                multiSelected = false;
            }
            if (currentPadInput.IsButtonDown(Buttons.A) && !oldPadInput.IsButtonDown(Buttons.A))
            {
                if (singleSelected)
                {
                    SinglePlayer = true;
                }
                else if (multiSelected)
                {
                    MultiPlayer = true;
                }
                ControlScheme = ChosenControl.Gamepad;
            }

            oldPadInput = currentPadInput;
        }
    }
}
