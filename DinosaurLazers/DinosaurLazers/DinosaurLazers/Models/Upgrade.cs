using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using DinosaurLazers.Controls;
using Microsoft.Xna.Framework.Media;

namespace DinosaurLazers.Models
{
    public class Upgrade
    {
        private bool _complete = false;
        public bool IsComplete
        {
            get
            {
                return _complete;
            }
            private set
            {
                _complete = value;
            }
        }

        private Texture2D _backGround;
        private Color mainBoxColor;
        private Color subBoxColor;
        private int windowWidth, windowHeight, choiceBoxWidth, choiceBoxHeight;
        private float upgradeBoxVelocity, upgradeTextVelocity,
                      upgradeTextShadowVelocity, choiceBoxVelocity,
                      choiceTitleVelocity, choiceNumberVelocity,
                      choiceCostVelocity, finishVelocity,
                      finishTextVelocity, pointVelocity, playerContentVelocity;
        public bool Stationary { get; set; }
        private bool _entering = true;
        private bool _exiting = false;

        private Texture2D _healthRect;
        private Rectangle _healthData;
        private bool _healthSelected = false;
        private Texture2D _chargeRect;
        private Rectangle _chargeData;
        private bool _chargeSelected = false;
        private Texture2D _powerRect;
        private Rectangle _powerData;
        private bool _powerSelected = false;

        private bool contentIsVisible = false;

        private Texture2D _highlighted;
        private Rectangle _highlightedData;
        private Color _highlightColor;

        private Texture2D _finishButton;
        private Rectangle _finishData;
        private bool _finishSelected;

        private Color healthColor;
        private Color chargeColor;
        private Color powerColor;

        private GraphicsDeviceManager gdm;
        private Player playerOne, playerTwo;
        private Sprite playerOneSprite, playerTwoSprite;

        private KeyboardState _currentKeyPressed, _oldKeyPressed;
        private GamePadState _currentPadState, _oldPadState;
        private double invalidTransactionHighlightTimer = 0;

        private int exitingVelocityUnit;

        private string playerContent;
        private SpriteFont title;
        private SpriteFont subTitleFont;
        private SpriteFont finishedFont;
        private bool invalidTransaction = false;
        private bool firstPlayerSelection = true;

        private SoundEffect moveMenuSound;
        private SoundEffect selectSound;
        private SoundEffect cantSound;
        private SoundEffect doneSound;

        private Song BGMusic;
        private bool MusicIsPlaying = false;

        public Upgrade(GraphicsDeviceManager gdm, SpriteBatch sb, ContentManager cm, Player playerOne, Sprite spriteOne, Player playerTwo = null, Sprite spriteTwo = null)
        {
            this.gdm = gdm;
            this.playerOne = playerOne;
            playerOneSprite = spriteOne;
            this.playerTwo = playerTwo;
            playerTwoSprite = spriteTwo;

            playerContent = "PLAYER 1";

            windowWidth = gdm.PreferredBackBufferWidth - 100;
            windowHeight = gdm.PreferredBackBufferHeight - 150;

            
            choiceBoxWidth = 540;
            choiceBoxHeight = (windowHeight/2);

            SetContentVelocities();

            title = cm.Load<SpriteFont>("Fonts/DefaultFont"); 
            subTitleFont = cm.Load<SpriteFont>("Fonts/SubtextFont");
            finishedFont = cm.Load<SpriteFont>("Fonts/FinishButtonFont");

            moveMenuSound = cm.Load<SoundEffect>("Sounds/moveMenu");
            selectSound = cm.Load<SoundEffect>("Sounds/menuSelect");
            cantSound = cm.Load<SoundEffect>("Sounds/menuCant");
            doneSound = cm.Load<SoundEffect>("Sounds/menuDone");

            _backGround = new Texture2D(gdm.GraphicsDevice, windowWidth, windowHeight);
            _healthRect = new Texture2D(gdm.GraphicsDevice, choiceBoxWidth, choiceBoxHeight);
            _chargeRect = new Texture2D(gdm.GraphicsDevice, choiceBoxWidth, choiceBoxHeight);
            _powerRect = new Texture2D(gdm.GraphicsDevice, choiceBoxWidth, choiceBoxHeight);
            _finishButton = new Texture2D(gdm.GraphicsDevice, 400, 150);

            mainBoxColor = SetColor(150, 50, 50, 50);

            subBoxColor = mainBoxColor;
            subBoxColor.A = 240;

            _highlightColor = SetColor(150, 200, 200, 200);
            _healthSelected = true;

            _backGround.SetData(ColorPicker.setTexture(windowWidth, windowHeight, mainBoxColor));
            _healthRect.SetData(ColorPicker.setTexture(choiceBoxWidth, choiceBoxHeight, subBoxColor));
            _chargeRect.SetData(ColorPicker.setTexture(choiceBoxWidth, choiceBoxHeight, subBoxColor));
            _powerRect.SetData(ColorPicker.setTexture(choiceBoxWidth, choiceBoxHeight, subBoxColor));
            
            _finishButton.SetData(ColorPicker.setTexture(150, 400, subBoxColor));

            healthColor = SetColor(175, 230, 80, 80);
            chargeColor = SetColor(190, 100, 100, 255);
            powerColor = SetColor(175, 80, 230, 80);

            BGMusic = cm.Load<Song>("Sounds/Music/BeatBossBGMusic");

            exitingVelocityUnit = 0;
        }

        /// <summary>
        /// Used as a way of setting a color to your choosing, using the proper order of
        /// byte information needed for a color (alpha, red, green, blue).
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        private Color SetColor(byte alpha, byte red, byte green, byte blue)
        {
            Color c = new Color();
            c.A = alpha;
            c.R = red;
            c.G = green;
            c.B = blue;
            return c;
        }

        /// <summary>
        /// Initializes all of the velocities of every moving
        /// piece of the Upgrade screen
        /// </summary>
        private void SetContentVelocities()
        {
            upgradeBoxVelocity = -100 - windowHeight;
            upgradeTextVelocity = -50 - windowHeight;
            upgradeTextShadowVelocity = -45 - windowHeight;
            playerContentVelocity = choiceBoxHeight - 110;
            choiceBoxVelocity = choiceBoxHeight - 50;
            choiceTitleVelocity = choiceBoxHeight;
            choiceNumberVelocity = choiceBoxHeight + 80;
            choiceCostVelocity = choiceBoxHeight + 160;
            finishVelocity = choiceBoxHeight + 310;
            finishTextVelocity = choiceBoxHeight + 335;
            pointVelocity = choiceBoxHeight + 335;
        }

        /// <summary>
        /// Adjusts the velocity of the incoming elements to
        /// swiftly come in and smoothly stop in place.
        /// </summary>
        /// <returns></returns>
        private float EnteringVelocity()
        {
            if (upgradeBoxVelocity >= 50 && upgradeTextVelocity >= 100 && upgradeTextShadowVelocity >=105)
            {
                return 14f;
            }
            else if (upgradeBoxVelocity >= 40 && upgradeTextVelocity >= 90 && upgradeTextShadowVelocity >= 95)
            {
                return 10f;
            }
            else if (upgradeBoxVelocity >= 15 && upgradeTextVelocity >= 65 && upgradeTextShadowVelocity >= 70)
            {
                return 6f;
            }
            else if (upgradeBoxVelocity >= -25 && upgradeTextVelocity >= 25 && upgradeTextShadowVelocity >= 30)
            {
                return 2f;
            }
            else if (upgradeBoxVelocity >= -65 && upgradeTextVelocity >= -15 && upgradeTextShadowVelocity >= -10)
            {
                return 1.25f;
            }
            else
            {
                return .5f;
            }
        }

        /// <summary>
        /// Updates the velocities of all Upgrade screen elements that are leaving the screen,
        /// giving them a smooth start and a swift exit.
        /// </summary>
        /// <returns></returns>
        private float ExitingVelocity()
        {
            exitingVelocityUnit++;
            if (exitingVelocityUnit < 5)
            {
                return 14f;
            }
            if (exitingVelocityUnit < 7)
            {
                return 10f;
            }
            if (exitingVelocityUnit < 10)
            {
                return 6f;
            }
            if (exitingVelocityUnit < 15)
            {
                return 2f;
            }
            if (exitingVelocityUnit < 20)
            {
                return 1.25f;
            }
            return .5f;
        }

        /// <summary>
        /// Updates the Upgrade class, and checks to see if the menu is stationary or not.
        /// If it isn't stationary, moving elements enter and lock in place or leave accordingly.
        /// If it is stationary, the selection highlight is determined by which box it's
        /// hovering over, and is adjusted accordingly in size.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!Stationary)
            {
                if (_entering)
                {
                    float f = EnteringVelocity();

                    upgradeBoxVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                    upgradeTextVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                    upgradeTextShadowVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);


                    if (upgradeBoxVelocity >= 50 && upgradeTextVelocity >= 100  && upgradeTextShadowVelocity >= 105 && !Stationary)
                    {
                        Stationary = !Stationary;
                        _entering = !_entering;
                    }
                }
                else if (_exiting)
                {
                    if (upgradeBoxVelocity < gdm.PreferredBackBufferHeight)
                    {
                        float f = ExitingVelocity();

                        upgradeBoxVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        upgradeTextVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        upgradeTextShadowVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        playerContentVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        choiceBoxVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        choiceTitleVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        choiceNumberVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        choiceCostVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        finishVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        finishTextVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                        pointVelocity += (float)(gameTime.ElapsedGameTime.Milliseconds / f);
                    }
                    else
                    {
                        if (playerOneSprite.Position.Y > -200)
                        {
                            Vector2 movement = Vector2.Zero;
                            movement -= Vector2.UnitY * .8f;
                            playerOneSprite.Movement += movement;

                            playerOneSprite.Update(gameTime);
                            movement.X = 0f;
                            movement.Y = 0f;
                            playerOneSprite.Movement = movement;
                            playerOneSprite.Update(gameTime);
                        }
                        else
                        {
                            if ((playerTwoSprite != null && playerTwo != null) && playerTwo.Health > 0)
                            {
                                
                                if (firstPlayerSelection)
                                {
                                    SetContentVelocities();
                                    Stationary = false;
                                    _entering = true;
                                    contentIsVisible = false;
                                    _exiting = false;
                                    playerContent = "PLAYER 2";
                                    _healthSelected = true;
                                    _chargeSelected = false;
                                    _powerSelected = false;
                                    _finishSelected = false;
                                    firstPlayerSelection = false;
                                }
                                else if (playerTwoSprite.Position.Y > -200)
                                {
                                    Vector2 movement = Vector2.Zero;
                                    movement -= Vector2.UnitY * .8f;
                                    playerTwoSprite.Movement += movement;

                                    playerTwoSprite.Update(gameTime);
                                    movement.X = 0f;
                                    movement.Y = 0f;
                                    playerTwoSprite.Movement = movement;
                                    playerTwoSprite.Update(gameTime);
                                }
                                else
                                {
                                    _complete = true;
                                }
                            }
                            else
                            {
                                _complete = true;
                            }
                        }
                        
                    }
                }
                
            }
            else
            {
                if (!MusicIsPlaying)
                {
                    MusicIsPlaying = true;
                    MediaPlayer.Volume = .7f;
                    MediaPlayer.Play(BGMusic);
                }

                if (firstPlayerSelection)
                {
                    if (playerOne.ControlScheme == ChosenControl.Keyboard)
                    {
                        CheckInput(Keyboard.GetState(), playerOne);
                    }
                    else if (playerOne.ControlScheme == ChosenControl.Gamepad)
                    {
                        CheckPadInput(GamePad.GetState(playerOne.PlayerIndex), playerOne);
                    }
                }
                else if (playerTwo != null && !firstPlayerSelection)
                {
                    if (playerTwo.ControlScheme == ChosenControl.Keyboard)
                    {
                        CheckInput(Keyboard.GetState(), playerTwo);
                    }
                    else if (playerTwo.ControlScheme == ChosenControl.Gamepad)
                    {
                        if (playerOne.ControlScheme != ChosenControl.Gamepad)
                        {
                            CheckPadInput(GamePad.GetState(PlayerIndex.One), playerTwo);
                        }
                        else
                        {
                            CheckPadInput(GamePad.GetState(PlayerIndex.Two), playerTwo);
                        }
                    }
                }
                

                if (_healthSelected || _chargeSelected || _powerSelected)
                {
                    _highlighted = new Texture2D(gdm.GraphicsDevice, choiceBoxWidth, choiceBoxHeight);
                }
                else
                {
                    _highlighted = new Texture2D(gdm.GraphicsDevice, 400, 150);
                }
            }


            

        }
        
        /// <summary>
        /// Draws every element needed by the Upgrade screen.
        /// Depending on which box is highlighted, _highlighted changes in size and position.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(_backGround, new Rectangle(50,(int)upgradeBoxVelocity,windowWidth,windowHeight), mainBoxColor);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(title, "UPGRADES", new Vector2(130, (int)upgradeTextShadowVelocity), subBoxColor);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(title, "UPGRADES", new Vector2(125, (int)upgradeTextVelocity), Color.White);
            spriteBatch.End();

            if (Stationary)
            {
                contentIsVisible = true;
            }

            if (contentIsVisible)
            {
                Color c = subBoxColor;
                c.A = 150;

                _healthData = new Rectangle(100, (int)choiceBoxVelocity, choiceBoxWidth, choiceBoxHeight - 150);
                _chargeData = new Rectangle(690, (int)choiceBoxVelocity, choiceBoxWidth, choiceBoxHeight - 150);
                _powerData = new Rectangle(1280, (int)choiceBoxVelocity, choiceBoxWidth, choiceBoxHeight - 150);
                _finishData = new Rectangle(1350, (int)finishVelocity, 400, 150);

                if (!_finishSelected)
                {
                    _highlighted = new Texture2D(gdm.GraphicsDevice, choiceBoxWidth, choiceBoxHeight);
                    _highlighted.SetData(ColorPicker.setTexture(choiceBoxHeight, choiceBoxWidth, _highlightColor));
                    
                    if (_healthSelected)
                    {
                        _healthRect.SetData(ColorPicker.setTexture(choiceBoxHeight, choiceBoxWidth, c));
                    }
                    else if (_chargeSelected)
                    {
                        _chargeRect.SetData(ColorPicker.setTexture(choiceBoxHeight, choiceBoxWidth, c));
                    }
                    else if (_powerSelected)
                    {
                        _powerRect.SetData(ColorPicker.setTexture(choiceBoxHeight, choiceBoxWidth, c));
                    }

                }
                else
                {
                    _finishButton.SetData(ColorPicker.setTexture(150,400, c));
                    _highlighted = new Texture2D(gdm.GraphicsDevice, 400, 150);
                    _highlighted.SetData(ColorPicker.setTexture(150, 400, _highlightColor));
                }


                if (_healthSelected)
                {
                    _highlightedData = new Rectangle(100, choiceBoxHeight - 50, choiceBoxWidth, choiceBoxHeight - 150);
                }
                else if (_chargeSelected)
                {
                    _highlightedData = new Rectangle(690, choiceBoxHeight - 50, choiceBoxWidth, choiceBoxHeight - 150);
                }
                else if (_powerSelected)
                {
                    _highlightedData = new Rectangle(1280, choiceBoxHeight - 50, choiceBoxWidth, choiceBoxHeight - 150);
                }
                else if (_finishSelected)
                {
                    _highlightedData = new Rectangle(1350, (int)finishVelocity, 400, 150);
                }
                else
                {
                    _highlightedData = new Rectangle(0, 0, 0, 0);
                }

                spriteBatch.Begin();

                spriteBatch.Draw(_healthRect, new Rectangle(105, (int)choiceBoxVelocity + 5, choiceBoxWidth, choiceBoxHeight - 150), subBoxColor);
                spriteBatch.Draw(_chargeRect, new Rectangle(695, (int)choiceBoxVelocity + 5, choiceBoxWidth, choiceBoxHeight - 150), subBoxColor);
                spriteBatch.Draw(_powerRect, new Rectangle(1285, (int)choiceBoxVelocity + 5, choiceBoxWidth, choiceBoxHeight - 150), subBoxColor);
                spriteBatch.Draw(_finishButton, new Rectangle(1355, (int)finishVelocity + 5, 400, 150), subBoxColor);
                if (firstPlayerSelection)
                {
                    spriteBatch.DrawString(finishedFont, "POINTS: " + playerOne.Points, new Vector2(365, pointVelocity + 5), subBoxColor);
                }
                else
                {
                    spriteBatch.DrawString(finishedFont, "POINTS: " + playerTwo.Points, new Vector2(365, pointVelocity + 5), subBoxColor);
                }

                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.Draw(_highlighted, _highlightedData, _highlightColor);
                spriteBatch.End();

                
                
                spriteBatch.Begin();

                spriteBatch.Draw(_healthRect, _healthData, subBoxColor);
                spriteBatch.Draw(_chargeRect, _chargeData, subBoxColor);
                spriteBatch.Draw(_powerRect, _powerData, subBoxColor);
                spriteBatch.Draw(_finishButton, _finishData, subBoxColor);
                spriteBatch.DrawString(subTitleFont, playerContent, new Vector2(818, playerContentVelocity + 3), subBoxColor);
                spriteBatch.End();

                spriteBatch.Begin();
                if (firstPlayerSelection)
                    DrawMenuText(spriteBatch, playerOne);
                else
                    DrawMenuText(spriteBatch, playerTwo);
                spriteBatch.End();

            }
        }

        private void DrawMenuText(SpriteBatch spriteBatch, Player dinosaur)
        {
            spriteBatch.DrawString(subTitleFont, playerContent, new Vector2(815, playerContentVelocity), Color.White);
            spriteBatch.DrawString(subTitleFont, "HEALTH", new Vector2(240, choiceTitleVelocity), healthColor);
            spriteBatch.DrawString(subTitleFont, dinosaur.MaxHealth + " --> " + (dinosaur.MaxHealth + 25), new Vector2(215, choiceNumberVelocity), healthColor);
            spriteBatch.DrawString(subTitleFont, "COST: " + dinosaur.HealthUpgradeCost, new Vector2(225, choiceCostVelocity), Color.White);

            spriteBatch.DrawString(subTitleFont, "CHARGE", new Vector2(830, choiceTitleVelocity), chargeColor);
            spriteBatch.DrawString(subTitleFont, dinosaur.MaxLaserCharge + " --> " + (dinosaur.MaxLaserCharge + 25), new Vector2(805, choiceNumberVelocity), chargeColor);
            spriteBatch.DrawString(subTitleFont, "COST: " + dinosaur.ChargeUpgradeCost, new Vector2(815, choiceCostVelocity), Color.White);

            spriteBatch.DrawString(subTitleFont, "POWER LEVEL", new Vector2(1330, choiceTitleVelocity), powerColor);
            if (dinosaur.LasersPowerLevel < 4)
            {
                spriteBatch.DrawString(subTitleFont, dinosaur.LasersPowerLevel + " --> " + (dinosaur.LasersPowerLevel + 1), new Vector2(1460, choiceNumberVelocity), powerColor);
                spriteBatch.DrawString(subTitleFont, "COST: " + dinosaur.PowerUpgradeCost, new Vector2(1395, choiceCostVelocity), Color.White);
            }
            else if (dinosaur.LasersPowerLevel == 4)
            {
                spriteBatch.DrawString(subTitleFont, dinosaur.LasersPowerLevel + " --> MAX", new Vector2(1410, choiceNumberVelocity), powerColor);
                spriteBatch.DrawString(subTitleFont, "COST: " + dinosaur.PowerUpgradeCost, new Vector2(1395, choiceCostVelocity), Color.White);
            }
            else if (dinosaur.LasersPowerLevel == 5)
            {
                spriteBatch.DrawString(subTitleFont, "MAX", new Vector2(1490, choiceNumberVelocity), powerColor);
                spriteBatch.DrawString(subTitleFont, "COST: -----", new Vector2(1395, choiceCostVelocity), Color.White);
            }
            

            if (invalidTransaction)
            {
                invalidTransactionHighlightTimer += 1;
                spriteBatch.DrawString(finishedFont, "POINTS: " + dinosaur.Points, new Vector2(360, pointVelocity), Color.Red);
                if (invalidTransactionHighlightTimer >= 60)
                {
                    invalidTransactionHighlightTimer = 0;
                    invalidTransaction = false;
                }
            }
            else
            {
                spriteBatch.DrawString(finishedFont, "POINTS: " + dinosaur.Points, new Vector2(360, pointVelocity), Color.White);
            }

            spriteBatch.DrawString(finishedFont, "DONE", new Vector2(1390, finishTextVelocity), Color.White);
        }

        /// <summary>
        /// Keyboard input checking used during Update.
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void CheckInput(KeyboardState state, Player dinosaur)
        {
            _currentKeyPressed = state;
            if (firstPlayerSelection)
            {
                if ((_currentKeyPressed.IsKeyDown(Keys.A) || _currentKeyPressed.IsKeyDown(Keys.Left)) && !(_oldKeyPressed.IsKeyDown(Keys.A) || _oldKeyPressed.IsKeyDown(Keys.Left)))
                {
                    LeftInput();
                }
                if ((_currentKeyPressed.IsKeyDown(Keys.D) || _currentKeyPressed.IsKeyDown(Keys.Right)) && !(_oldKeyPressed.IsKeyDown(Keys.D) || _oldKeyPressed.IsKeyDown(Keys.Right)))
                {
                    RightInput();
                }
                if ((_currentKeyPressed.IsKeyDown(Keys.W) || _currentKeyPressed.IsKeyDown(Keys.Up)) && !(_oldKeyPressed.IsKeyDown(Keys.W) || _oldKeyPressed.IsKeyDown(Keys.Up)))
                {
                    UpInput();
                }
                if ((_currentKeyPressed.IsKeyDown(Keys.S) || _currentKeyPressed.IsKeyDown(Keys.Down)) && !(_oldKeyPressed.IsKeyDown(Keys.S) || _oldKeyPressed.IsKeyDown(Keys.Down)))
                {
                    DownInput();
                }
            }
            else
            {
                if (_currentKeyPressed.IsKeyDown(Keys.L) && !_oldKeyPressed.IsKeyDown(Keys.L))
                {
                    LeftInput();
                }
                if (_currentKeyPressed.IsKeyDown(Keys.OemQuotes) && !_oldKeyPressed.IsKeyDown(Keys.OemQuotes))
                {
                    RightInput();
                }
                if (_currentKeyPressed.IsKeyDown(Keys.P) && !_oldKeyPressed.IsKeyDown(Keys.P))
                {
                    UpInput();
                }
                if (_currentKeyPressed.IsKeyDown(Keys.OemSemicolon) && !_oldKeyPressed.IsKeyDown(Keys.OemSemicolon))
                {
                    DownInput();
                }
            }
            
            if ((_currentKeyPressed.IsKeyDown(Keys.Enter) || _currentKeyPressed.IsKeyDown(Keys.Space) || _currentKeyPressed.IsKeyDown(Keys.F)) && !(_oldKeyPressed.IsKeyDown(Keys.Enter) || _oldKeyPressed.IsKeyDown(Keys.Space) || _oldKeyPressed.IsKeyDown(Keys.F)))
            {
                MenuOptionSelected(dinosaur);
            }

            _oldKeyPressed = _currentKeyPressed;
        }

        public void CheckPadInput(GamePadState padState, Player dinosaur)
        {
            _currentPadState = padState;


            if ((_currentPadState.IsButtonDown(Buttons.LeftThumbstickLeft) || _currentPadState.IsButtonDown(Buttons.DPadLeft)) && !(_oldPadState.IsButtonDown(Buttons.LeftThumbstickLeft) || _oldPadState.IsButtonDown(Buttons.DPadLeft)))
            {
                LeftInput();
            }
            if ((_currentPadState.IsButtonDown(Buttons.LeftThumbstickRight) || _currentPadState.IsButtonDown(Buttons.DPadRight)) && !(_oldPadState.IsButtonDown(Buttons.LeftThumbstickRight) || _oldPadState.IsButtonDown(Buttons.DPadRight)))
            {
                RightInput();
            }
            if ((_currentPadState.IsButtonDown(Buttons.LeftThumbstickDown) || _currentPadState.IsButtonDown(Buttons.DPadDown)) && !(_oldPadState.IsButtonDown(Buttons.LeftThumbstickDown) || _oldPadState.IsButtonDown(Buttons.DPadDown)))
            {
                DownInput();
            }
            if ((_currentPadState.IsButtonDown(Buttons.LeftThumbstickUp) || _currentPadState.IsButtonDown(Buttons.DPadUp)) && !(_oldPadState.IsButtonDown(Buttons.LeftThumbstickUp) || _oldPadState.IsButtonDown(Buttons.DPadUp)))
            {
                UpInput();
            }

            if (_currentPadState.IsButtonDown(Buttons.A) && !(_oldPadState.IsButtonDown(Buttons.A)))
            {
                MenuOptionSelected(dinosaur);
            }

            _oldPadState = _currentPadState;
        }

        private void LeftInput()
        {
            if (_chargeSelected)
            {
                _healthSelected = true;
                _chargeSelected = false;
                moveMenuSound.Play();
            }
            else if (_powerSelected)
            {
                _chargeSelected = true;
                _powerSelected = false;
                moveMenuSound.Play();
            }
        }

        private void RightInput()
        {
            if (_chargeSelected)
            {
                _powerSelected = true;
                _chargeSelected = false;
                moveMenuSound.Play();

            }
            else if (_healthSelected)
            {
                _chargeSelected = true;
                _healthSelected = false;
                moveMenuSound.Play();
            }
        }

        private void UpInput()
        {
            if (_finishSelected)
            {
                _chargeSelected = true;
                _finishSelected = false;
                moveMenuSound.Play();
            }
        }

        private void DownInput()
        {
            if (!_finishSelected)
            {
                _healthSelected = false;
                _chargeSelected = false;
                _powerSelected = false;
                _finishSelected = true;
                moveMenuSound.Play();
            }
        }

        private void MenuOptionSelected(Player dinosaur)
        {
            if (_finishSelected)
            {
                doneSound.Play();

                Stationary = !Stationary;
                _exiting = true;
            }
            else if (_healthSelected)
            {
                if (dinosaur.Points >= dinosaur.HealthUpgradeCost)
                {
                    selectSound.Play();

                    dinosaur.Points -= dinosaur.HealthUpgradeCost;
                    dinosaur.MaxHealth = dinosaur.MaxHealth + 25;
                    dinosaur.HealthUpgradeCost++;
                }
                else
                {
                    cantSound.Play();

                    invalidTransaction = true;
                }
            }
            else if (_chargeSelected)
            {
                if (dinosaur.Points >= dinosaur.ChargeUpgradeCost)
                {
                    selectSound.Play();

                    dinosaur.Points -= dinosaur.ChargeUpgradeCost;
                    dinosaur.MaxLaserCharge = dinosaur.MaxLaserCharge + 25;
                    dinosaur.ChargeUpgradeCost++;
                }
                else
                {
                    cantSound.Play();

                    invalidTransaction = true;
                }
            }
            else if (_powerSelected)
            {
                if (dinosaur.Points >= dinosaur.PowerUpgradeCost && dinosaur._laserPowerLevel != 5)
                {
                    selectSound.Play();

                    dinosaur.Points -= dinosaur.PowerUpgradeCost;
                    dinosaur.LasersPowerLevel = dinosaur.LasersPowerLevel + 1;
                    dinosaur.PowerUpgradeCost++;
                }
                else
                {
                    cantSound.Play();

                    invalidTransaction = true;
                }
            }
        }
    }
}
