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
    public class Lobby
    {
        private Texture2D background, titleText;
        public bool IsDone { get; set; }

        private Texture2D brontoBox, rexBox, pteraBox, triceraBox;
        private Texture2D keyboardOne, keyboardTwo, gamepadOne, gamepadTwo;
        private Texture2D playerOneOk, playerTwoOk;
        private Texture2D unselectedOne, unselectedTwo;
        private Texture2D bront, rex, ptery, cera;
        private Color c;

        private KeyboardState playerOneCurrentKey, playerOneOldKey, playerTwoCurrentKey, playerTwoOldKey;
        private GamePadState playerOneCurrentPad, playerOneOldPad, playerTwoCurrentPad, playerTwoOldPad;

        public Player.Chara playerOne { get; private set; }
        public Player.Chara playerTwo { get; private set; }
        public ChosenControl firstPlayerControl { get; private set; }
        public ChosenControl secondPlayerControl { get; private set; }

        private bool keyboardOneHighlighted, keyboardTwoHighlighted, gamepadOneHighlighted, gamepadTwoHighlighted,
                     brontoOneHighlighted, rexOneHighlighted, pteraOneHighlighted, triceraOneHighlighted,
                     brontoTwoHighlighted, rexTwoHighlighted, pteraTwoHighlighted, triceraTwoHighlighted;

        private SoundEffect moveMenuSound;
        private SoundEffect PteraSound;
        private SoundEffect triSound;
        private SoundEffect brontoSound;
        private SoundEffect trexSound;
        private SoundEffect menuDone;

        private BackgroundMusic bgMusic;
        private bool isBGMusicPlaying = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gdm"></param>
        /// <param name="cm"></param>
        /// <param name="firstPlayerControl"></param>
        public Lobby(GraphicsDeviceManager gdm, ContentManager cm, ChosenControl firstPlayerControl)
        {
            this.firstPlayerControl = firstPlayerControl;
            secondPlayerControl = ChosenControl.Unchosen;
            playerOne = Player.Chara.Unchosen;
            playerTwo = Player.Chara.Unchosen;

            gamepadTwoHighlighted = true;

            brontoOneHighlighted = true;
            rexTwoHighlighted = true;

            titleText = cm.Load<Texture2D>("Multiplayer Lobby/lobby-title-text");
            playerOneOk = cm.Load<Texture2D>("Multiplayer Lobby/player-one-ok");
            playerTwoOk = cm.Load<Texture2D>("Multiplayer Lobby/player-two-ok");

            keyboardOne = cm.Load<Texture2D>("Multiplayer Lobby/keyboard-graphic");
            keyboardTwo = cm.Load<Texture2D>("Multiplayer Lobby/keyboard-graphic");
            gamepadOne = cm.Load<Texture2D>("Multiplayer Lobby/gamepad-graphic");
            gamepadTwo = cm.Load<Texture2D>("Multiplayer Lobby/gamepad-graphic");

            brontoBox = cm.Load<Texture2D>("Multiplayer Lobby/bronto-box");
            rexBox = cm.Load<Texture2D>("Multiplayer Lobby/rex-box");
            pteraBox = cm.Load<Texture2D>("Multiplayer Lobby/ptera-box");
            triceraBox = cm.Load<Texture2D>("Multiplayer Lobby/tricera-box");

            bront = cm.Load<Texture2D>("CharacterSelectionScreen/Bronto");
            rex = cm.Load<Texture2D>("CharacterSelectionScreen/Trex");
            ptery = cm.Load<Texture2D>("CharacterSelectionScreen/Pteradactyl");
            cera = cm.Load<Texture2D>("CharacterSelectionScreen/Triceratops");

            background = new Texture2D(gdm.GraphicsDevice, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            background.SetData(ColorPicker.setTexture(background.Width, background.Height, Color.Gray));

            c.A = 180;
            c.R = 50;
            c.G = 50;
            c.B = 50;

            unselectedOne = new Texture2D(gdm.GraphicsDevice, 400, 400);
            unselectedTwo = new Texture2D(gdm.GraphicsDevice, 400, 400);
            unselectedOne.SetData(ColorPicker.setTexture(unselectedOne.Width, unselectedOne.Height, c));
            unselectedTwo.SetData(ColorPicker.setTexture(unselectedTwo.Width, unselectedTwo.Height, c));

            bgMusic = new BackgroundMusic(cm, BackgroundMusic.playAt.CharacterSelectScreen);
            moveMenuSound = cm.Load<SoundEffect>("Sounds/moveMenu");
            menuDone = cm.Load<SoundEffect>("Sounds/menuDone");
            PteraSound = cm.Load<SoundEffect>("Sounds/Ptera");
            triSound = cm.Load<SoundEffect>("Sounds/tri");
            brontoSound = cm.Load<SoundEffect>("Sounds/bronto");
            trexSound = cm.Load<SoundEffect>("Sounds/trex");
        }

        /// <summary>
        /// Checks inputs and updates visual components of the Mulitplayer Lobby
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (playerOne == Player.Chara.Unchosen || firstPlayerControl == ChosenControl.Unchosen)
            {
                if (firstPlayerControl == ChosenControl.Unchosen)
                {
                    FirstPlayerKeyboardInput(Keyboard.GetState());
                    if (GamePad.GetState(PlayerIndex.One).IsConnected)
                        FirstPlayerGamepadInput(GamePad.GetState(PlayerIndex.One));
                }
                else if (firstPlayerControl == ChosenControl.Keyboard)
                {
                    FirstPlayerKeyboardInput(Keyboard.GetState());
                }
                else if (firstPlayerControl == ChosenControl.Gamepad)
                {
                    FirstPlayerGamepadInput(GamePad.GetState(PlayerIndex.One));
                }
            }
            else if (playerOne != Player.Chara.Unchosen)
            {
                bool negation = false;
                if (firstPlayerControl == ChosenControl.Keyboard)
                {
                    playerOneCurrentKey = Keyboard.GetState();
                    if (OneKeyDeclinationPressed(playerOneCurrentKey, playerOneOldKey, true))
                    {
                        negation = true;
                    }
                }
                else if (firstPlayerControl == ChosenControl.Gamepad)
                {
                    playerOneCurrentPad = GamePad.GetState(PlayerIndex.One);
                    if (OnePadDeclinationPressed(playerOneCurrentPad, playerOneOldPad, true))
                    {
                        negation = true;
                    }
                }

                if (negation)
                {
                    if (playerOne == Player.Chara.Bronto)
                    {
                        brontoOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Trex)
                    {
                        rexOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Ptera)
                    {
                        pteraOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Cera)
                    {
                        triceraOneHighlighted = false;
                    }
                    playerOne = Player.Chara.Unchosen;
                }
            }

            if (playerTwo == Player.Chara.Unchosen || secondPlayerControl == ChosenControl.Unchosen)
            {
                if (secondPlayerControl == ChosenControl.Unchosen)
                {
                    SecondPlayerKeyboardInput(Keyboard.GetState());

                    if (!GamePad.GetState(PlayerIndex.Two).IsConnected)
                    {
                        if (GamePad.GetState(PlayerIndex.One).IsConnected && firstPlayerControl != ChosenControl.Gamepad)
                            SecondPlayerGamepadInput(GamePad.GetState(PlayerIndex.One));
                    }
                    else
                    {
                        SecondPlayerGamepadInput(GamePad.GetState(PlayerIndex.Two));
                    }

                }
                else if (secondPlayerControl == ChosenControl.Keyboard)
                {
                    SecondPlayerKeyboardInput(Keyboard.GetState());
                }
                else if (secondPlayerControl == ChosenControl.Gamepad)
                {
                    if (!GamePad.GetState(PlayerIndex.Two).IsConnected)
                    {
                        if (GamePad.GetState(PlayerIndex.One).IsConnected && firstPlayerControl != ChosenControl.Gamepad)
                            SecondPlayerGamepadInput(GamePad.GetState(PlayerIndex.One));
                    }
                    else
                    {
                        SecondPlayerGamepadInput(GamePad.GetState(PlayerIndex.Two));
                    }
                }
            }
            else if (playerTwo != Player.Chara.Unchosen)
            {
                bool negation = false;
                if (secondPlayerControl == ChosenControl.Keyboard)
                {
                    playerTwoCurrentKey = Keyboard.GetState();
                    if (OneKeyDeclinationPressed(playerTwoCurrentKey, playerTwoOldKey, false))
                    {
                        negation = true;
                    }
                }
                else if (secondPlayerControl == ChosenControl.Gamepad)
                {
                    if (firstPlayerControl != ChosenControl.Gamepad)
                    {
                        playerTwoCurrentPad = GamePad.GetState(PlayerIndex.One);
                    }
                    else
                    {
                        playerTwoCurrentPad = GamePad.GetState(PlayerIndex.Two);
                    }
                    if (OnePadDeclinationPressed(playerTwoCurrentPad, playerTwoOldPad, false))
                    {
                        negation = true;
                    }
                }

                if (negation)
                {
                    if (playerTwo == Player.Chara.Bronto)
                    {
                        brontoTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Trex)
                    {
                        rexTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Ptera)
                    {
                        pteraTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Cera)
                    {
                        triceraTwoHighlighted = false;
                    }
                    playerTwo = Player.Chara.Unchosen;
                }
            }

            if (!isBGMusicPlaying)
            {
                bgMusic.Play();
                isBGMusicPlaying = true;
            }

            if (playerOne != Player.Chara.Unchosen && playerTwo != Player.Chara.Unchosen)
            {
                this.IsDone = true;
            }
            
        }

        /// <summary>
        /// Draws the visual components of the Multiplayer Lobby
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.DarkGray);
            sb.End();

            sb.Begin();
            UpdatePlayerOneControlSelection(sb);
            UpdatePlayerTwoControlSelection(sb);
            sb.End();

            sb.Begin();
            UpdatePlayerOneCharacterSelection(sb);
            UpdatePlayerTwoCharacterSelection(sb);
            sb.End();

            sb.Begin();
            DrawGrayedOutSelectionOne(sb);
            DrawGrayedOutSelectionTwo(sb);
            sb.End();

            if (playerOne != Player.Chara.Unchosen)
            {
                sb.Begin();
                sb.Draw(playerOneOk, new Rectangle(0, 50, 1920, 1080), Color.White);
                sb.End();
            }
            if (playerTwo != Player.Chara.Unchosen)
            {
                sb.Begin();
                sb.Draw(playerTwoOk, new Rectangle(0, 50, 1920, 1080), Color.White);
                sb.End();
            }

            sb.Begin();
            sb.Draw(titleText, new Rectangle(0, 0, 1920, 1080), Color.White);
            sb.End();
        }

        #region UpdatePlayerControl

        /// <summary>
        /// Updates the highlight over P1's control
        /// </summary>
        /// <param name="sb"></param>
        private void UpdatePlayerOneControlSelection(SpriteBatch sb)
        {
            if (keyboardOneHighlighted || gamepadOneHighlighted)
            {
                if (keyboardOneHighlighted)
                {
                    sb.Draw(keyboardOne, new Rectangle(500, 500, 400, 400), Color.White);
                }
                else if (gamepadOneHighlighted)
                {
                    sb.Draw(gamepadOne, new Rectangle(500, 500, 400, 400), Color.White);
                }
            }
            else
            {
                if (firstPlayerControl == ChosenControl.Keyboard)
                {
                    sb.Draw(keyboardOne, new Rectangle(500, 500, 400, 400), Color.White);
                }
                else if (firstPlayerControl == ChosenControl.Gamepad)
                {
                    sb.Draw(gamepadOne, new Rectangle(500, 500, 400, 400), Color.White);
                }
            }
            
        }

        /// <summary>
        /// Updates the highlight over P2's control
        /// </summary>
        /// <param name="sb"></param>
        private void UpdatePlayerTwoControlSelection(SpriteBatch sb)
        {
            if (keyboardTwoHighlighted || gamepadTwoHighlighted)
            {
                if (keyboardTwoHighlighted)
                {
                    sb.Draw(keyboardOne, new Rectangle(1020, 500, 400, 400), Color.White);
                }
                else if (gamepadTwoHighlighted)
                {
                    sb.Draw(gamepadOne, new Rectangle(1020, 500, 400, 400), Color.White);
                }
            }
            else
            {
                if (secondPlayerControl == ChosenControl.Keyboard)
                {
                    sb.Draw(keyboardOne, new Rectangle(1020, 500, 400, 400), Color.White);
                }
                else if (secondPlayerControl == ChosenControl.Gamepad)
                {
                    sb.Draw(gamepadOne, new Rectangle(1020, 500, 400, 400), Color.White);
                }
            }
            
        }

        #endregion

        #region UpdateCharacterSelections

        /// <summary>
        /// Updates the highlight over P1's character
        /// </summary>
        /// <param name="sb"></param>
        private void UpdatePlayerOneCharacterSelection(SpriteBatch sb)
        {
            if (brontoOneHighlighted)
            {
                sb.Draw(brontoBox, new Rectangle(50, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(bront, new Rectangle(230, 640, 36, 184), Color.White);
                sb.End();
                sb.Begin();

            }
            else if (rexOneHighlighted)
            {
                sb.Draw(rexBox, new Rectangle(50, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(rex, new Rectangle(227, 640, 53, 183), Color.White);
                sb.End();
                sb.Begin();
            }
            else if (pteraOneHighlighted)
            {
                sb.Draw(pteraBox, new Rectangle(50, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(ptery, new Rectangle(160, 640, 180, 100), Color.White);
                sb.End();
                sb.Begin();
            }
            else if (triceraOneHighlighted)
            {
                sb.Draw(triceraBox, new Rectangle(50, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(cera, new Rectangle(218, 640, 61, 175), Color.White);
                sb.End();
                sb.Begin();
            }
        }

        /// <summary>
        /// Updates the highlight over P2's character
        /// </summary>
        /// <param name="sb"></param>
        private void UpdatePlayerTwoCharacterSelection(SpriteBatch sb)
        {
            if (brontoTwoHighlighted)
            {
                sb.Draw(brontoBox, new Rectangle(1470, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(bront, new Rectangle(1650, 640, 36, 184), Color.White);
                sb.End();
                sb.Begin();
            }
            else if (rexTwoHighlighted)
            {
                sb.Draw(rexBox, new Rectangle(1470, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(rex, new Rectangle(1647, 640, 53, 183), Color.White);
                sb.End();
                sb.Begin();
            }
            else if (pteraTwoHighlighted)
            {
                sb.Draw(pteraBox, new Rectangle(1470, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(ptery, new Rectangle(1580, 640, 180, 100), Color.White);
                sb.End();
                sb.Begin();
            }
            else if (triceraTwoHighlighted)
            {
                sb.Draw(triceraBox, new Rectangle(1470, 500, 400, 400), Color.White);
                sb.End();
                sb.Begin();
                sb.Draw(cera, new Rectangle(1638, 640, 61, 175), Color.White);
                sb.End();
                sb.Begin();
            }
        }

        #endregion

        #region UnselectedBoxes

        /// <summary>
        /// Draws the grayed out selection over the P1 option not currently being dealt with
        /// </summary>
        /// <param name="sb"></param>
        private void DrawGrayedOutSelectionOne(SpriteBatch sb)
        {
            if (keyboardOneHighlighted || gamepadOneHighlighted)
            {
                sb.Draw(unselectedOne, new Rectangle(50, 500, 400, 400), c);
            }
            else if (brontoOneHighlighted || rexOneHighlighted || pteraOneHighlighted || triceraOneHighlighted)
            {
                sb.Draw(unselectedOne, new Rectangle(500, 500, 400, 400), c);
            }
        }

        /// <summary>
        /// Draws the grayed out selection over the P2 option not currently being dealt with
        /// </summary>
        /// <param name="sb"></param>
        private void DrawGrayedOutSelectionTwo(SpriteBatch sb)
        {
            if (keyboardTwoHighlighted || gamepadTwoHighlighted)
            {
                sb.Draw(unselectedTwo, new Rectangle(1470, 500, 400, 400), c);
            }
            else if (brontoTwoHighlighted || rexTwoHighlighted || pteraTwoHighlighted || triceraTwoHighlighted)
            {
                sb.Draw(unselectedTwo, new Rectangle(1020, 500, 400, 400), c);
            }
        }

        #endregion

        #region SingleInputValidations

        /// <summary>
        /// Checks up movements on a Gamepad
        /// </summary>
        /// <param name="currentPad"></param>
        /// <param name="oldPad"></param>
        /// <returns></returns>
        private bool OnePadMovementUp(GamePadState currentPad, GamePadState oldPad)
        {
            if ((currentPad.IsButtonDown(Buttons.LeftThumbstickUp) || currentPad.IsButtonDown(Buttons.DPadUp)) && !(oldPad.IsButtonDown(Buttons.LeftThumbstickUp) || (oldPad.IsButtonDown(Buttons.DPadUp))))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks down movements on a GamePad
        /// </summary>
        /// <param name="currentPad"></param>
        /// <param name="oldPad"></param>
        /// <returns></returns>
        private bool OnePadMovementDown(GamePadState currentPad, GamePadState oldPad)
        {
            if ((currentPad.IsButtonDown(Buttons.LeftThumbstickDown) || currentPad.IsButtonDown(Buttons.DPadDown)) && !(oldPad.IsButtonDown(Buttons.LeftThumbstickDown) || (oldPad.IsButtonDown(Buttons.DPadDown))))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks up movements on a Keyboard
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="oldState"></param>
        /// <param name="firstPlayer"></param>
        /// <returns></returns>
        private bool OneKeyMovementUp(KeyboardState currentState, KeyboardState oldState, bool firstPlayer = false)
        {
            if (firstPlayer)
            {
                if (currentState.IsKeyDown(Keys.W) && !oldState.IsKeyDown(Keys.W))
                {
                    return true;
                }
            }
            else
            {
                if (currentState.IsKeyDown(Keys.P) && !oldState.IsKeyDown(Keys.P))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks down movements on a Keyboard
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="oldState"></param>
        /// <param name="firstPlayer"></param>
        /// <returns></returns>
        private bool OneKeyMovementDown(KeyboardState currentState, KeyboardState oldState, bool firstPlayer = false)
        {
            if (firstPlayer)
            {
                if (currentState.IsKeyDown(Keys.S) && !oldState.IsKeyDown(Keys.S))
                {
                    return true;
                }
            }
            else
            {
                if (currentState.IsKeyDown(Keys.OemSemicolon) && !oldState.IsKeyDown(Keys.OemSemicolon))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for Verifcation key presses (Keyboard - P1: F, P2: Enter)
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="oldState"></param>
        /// <param name="firstPlayer"></param>
        /// <returns></returns>
        private bool OneKeyVerificationPressed(KeyboardState currentState, KeyboardState oldState, bool firstPlayer = false)
        {
            if (firstPlayer)
            {
                if (currentState.IsKeyDown(Keys.F) && !oldState.IsKeyDown(Keys.F))
                {
                    return true;
                }
            }
            else
            {
                if (currentState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks for Verification pad presses (Gamepad - P1 & P2: A)
        /// </summary>
        /// <param name="currentPad"></param>
        /// <param name="oldPad"></param>
        /// <returns></returns>
        private bool OnePadVerificationPressed(GamePadState currentPad, GamePadState oldPad)
        {
            if (currentPad.IsButtonDown(Buttons.A) && !oldPad.IsButtonDown(Buttons.A))
            {
                return true;
            }
            return false;
        }

        private bool OneKeyDeclinationPressed(KeyboardState currentKey, KeyboardState oldKey, bool firstPlayer = false)
        {
            if (firstPlayer)
            {
                if ((currentKey.IsKeyDown(Keys.Tab) || currentKey.IsKeyDown(Keys.D)) && !(oldKey.IsKeyDown(Keys.Tab) || oldKey.IsKeyDown(Keys.D)))
                {
                    return true;
                }
            }
            else
            {
                if ((currentKey.IsKeyDown(Keys.Back) || currentKey.IsKeyDown(Keys.L)) && !(oldKey.IsKeyDown(Keys.Back) || oldKey.IsKeyDown(Keys.L)))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Checks to see if the user selected to go back in menu selections
        /// </summary>
        /// <param name="currentPad"></param>
        /// <param name="oldPad"></param>
        /// <returns></returns>
        private bool OnePadDeclinationPressed(GamePadState currentPad, GamePadState oldPad, bool firstPlayer = false)
        {
            if (currentPad.IsButtonDown(Buttons.B) && !oldPad.IsButtonDown(Buttons.B))
            {
                return true;
            }

            if (firstPlayer)
            {
                if ((currentPad.IsButtonDown(Buttons.DPadRight) || currentPad.IsButtonDown(Buttons.LeftThumbstickRight)) && !(oldPad.IsButtonDown(Buttons.DPadRight) || oldPad.IsButtonDown(Buttons.LeftThumbstickRight)))
                {
                    return true;
                }
            }
            else
            {
                if ((currentPad.IsButtonDown(Buttons.DPadLeft) || currentPad.IsButtonDown(Buttons.LeftThumbstickLeft)) && !(oldPad.IsButtonDown(Buttons.DPadLeft) || oldPad.IsButtonDown(Buttons.LeftThumbstickLeft)))
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

        #region UpdateBools

        private void UpdateUpControlOneBools()
        {
            if (keyboardOneHighlighted)
            {
                keyboardOneHighlighted = false;
                gamepadOneHighlighted = true;
            }
            else if (gamepadOneHighlighted)
            {
                gamepadOneHighlighted = false;
                keyboardOneHighlighted = true;
            }
        }

        private void UpdateUpControlTwoBools()
        {
            if (keyboardTwoHighlighted)
            {
                keyboardTwoHighlighted = false;
                gamepadTwoHighlighted = true;
            }
            else if (gamepadTwoHighlighted)
            {
                gamepadTwoHighlighted = false;
                keyboardTwoHighlighted = true;
            }
        }

        private void UpdateDownControlOneBools()
        {
            if (keyboardOneHighlighted)
            {
                keyboardOneHighlighted = false;
                gamepadOneHighlighted = true;
            }
            else if (gamepadOneHighlighted)
            {
                gamepadOneHighlighted = false;
                keyboardOneHighlighted = true;
            }
        }

        private void UpdateDownControlTwoBools()
        {
            if (keyboardTwoHighlighted)
            {
                keyboardTwoHighlighted = false;
                gamepadTwoHighlighted = true;
            }
            else if (gamepadTwoHighlighted)
            {
                gamepadTwoHighlighted = false;
                keyboardTwoHighlighted = true;
            }
        }

        private void UpdateUpCharacterOneBools()
        {
            if (brontoOneHighlighted)
            {
                if (!rexTwoHighlighted)
                {
                    rexOneHighlighted = true;
                }
                else
                {
                    pteraOneHighlighted = true;
                }
                brontoOneHighlighted = false;
            }
            else if (rexOneHighlighted)
            {
                if (!pteraTwoHighlighted)
                {
                    pteraOneHighlighted = true;
                }
                else
                {
                    triceraOneHighlighted = true;
                }
                rexOneHighlighted = false;
            }
            else if (pteraOneHighlighted)
            {
                if (!triceraTwoHighlighted)
                {
                    triceraOneHighlighted = true;
                }
                else
                {
                    brontoOneHighlighted = true;
                }
                pteraOneHighlighted = false;
            }
            else if (triceraOneHighlighted)
            {
                if (!brontoTwoHighlighted)
                {
                    brontoOneHighlighted = true;
                }
                else
                {
                    rexOneHighlighted = true;
                }
                triceraOneHighlighted = false;
            }
        }

        private void UpdateUpCharacterTwoBools()
        {
            if (brontoTwoHighlighted)
            {
                if (!rexOneHighlighted)
                {
                    rexTwoHighlighted = true;
                }
                else
                {
                    pteraTwoHighlighted = true;
                }
                brontoTwoHighlighted = false;
            }
            else if (rexTwoHighlighted)
            {
                if (!pteraOneHighlighted)
                {
                    pteraTwoHighlighted = true;
                }
                else
                {
                    triceraTwoHighlighted = true;
                }
                rexTwoHighlighted = false;
            }
            else if (pteraTwoHighlighted)
            {
                if (!triceraOneHighlighted)
                {
                    triceraTwoHighlighted = true;
                }
                else
                {
                    brontoTwoHighlighted = true;
                }
                pteraTwoHighlighted = false;
            }
            else if (triceraTwoHighlighted)
            {
                if (!brontoOneHighlighted)
                {
                    brontoTwoHighlighted = true;
                }
                else
                {
                    rexTwoHighlighted = true;
                }
                triceraTwoHighlighted = false;
            }
        }
        
        private void UpdateDownCharacterOneBools()
        {
            if (brontoOneHighlighted)
            {
                if (!triceraTwoHighlighted)
                {
                    triceraOneHighlighted = true;
                }
                else
                {
                    pteraOneHighlighted = true;
                }
                brontoOneHighlighted = false;
            }
            else if (rexOneHighlighted)
            {
                if (!brontoTwoHighlighted)
                {
                    brontoOneHighlighted = true;
                }
                else
                {
                    triceraOneHighlighted = true;
                }
                rexOneHighlighted = false;
            }
            else if (pteraOneHighlighted)
            {
                if (!rexTwoHighlighted)
                {
                    rexOneHighlighted = true;
                }
                else
                {
                    brontoOneHighlighted = true;
                }
                pteraOneHighlighted = false;
            }
            else if (triceraOneHighlighted)
            {
                if (!pteraTwoHighlighted)
                {
                    pteraOneHighlighted = true;
                }
                else
                {
                    rexOneHighlighted = true;
                }
                triceraOneHighlighted = false;
            }
        }

        private void UpdateDownCharacterTwoBools()
        {
            if (brontoTwoHighlighted)
            {
                if (!triceraOneHighlighted)
                {
                    triceraTwoHighlighted = true;
                }
                else
                {
                    pteraTwoHighlighted = true;
                }
                brontoTwoHighlighted = false;
            }
            else if (rexTwoHighlighted)
            {
                if (!brontoOneHighlighted)
                {
                    brontoTwoHighlighted = true;
                }
                else
                {
                    triceraTwoHighlighted = true;
                }
                rexTwoHighlighted = false;
            }
            else if (pteraTwoHighlighted)
            {
                if (!rexOneHighlighted)
                {
                    rexTwoHighlighted = true;
                }
                else
                {
                    brontoTwoHighlighted = true;
                }
                pteraTwoHighlighted = false;
            }
            else if (triceraTwoHighlighted)
            {
                if (!pteraOneHighlighted)
                {
                    pteraTwoHighlighted = true;
                }
                else
                {
                    rexTwoHighlighted = true;
                }
                triceraTwoHighlighted = false;
            }
        }

        /// <summary>
        /// Updates the selection for controls to determine if the focus changes to the characters
        /// </summary>
        /// <param name="firstPlayer"></param>
        private void UpdateControlVerificationSelection(bool firstPlayer = false)
        {
            if (firstPlayer)
            {
                if (keyboardOneHighlighted || gamepadOneHighlighted)
                {
                    if (keyboardOneHighlighted)
                    {
                        firstPlayerControl = ChosenControl.Keyboard;
                    }
                    else if (gamepadOneHighlighted)
                    {
                        firstPlayerControl = ChosenControl.Gamepad;
                    }

                    keyboardOneHighlighted = false;
                    gamepadOneHighlighted = false;
                }
            }
            else
            {
                if (keyboardTwoHighlighted || gamepadTwoHighlighted)
                {
                    if (keyboardTwoHighlighted)
                    {
                        secondPlayerControl = ChosenControl.Keyboard;
                    }
                    else if (gamepadTwoHighlighted)
                    {
                        secondPlayerControl = ChosenControl.Gamepad;
                    }

                    keyboardTwoHighlighted = false;
                    gamepadTwoHighlighted = false;
                }
            }
        }

        /// <summary>
        /// Updates the selection for characters to determine if "OK!" appears
        /// </summary>
        /// <param name="firstPlayer"></param>
        private void UpdateCharacterVerificationSelection(bool firstPlayer = false)
        {
            if (firstPlayer)
            {
                menuDone.Play();
                if (brontoOneHighlighted || rexOneHighlighted || pteraOneHighlighted || triceraOneHighlighted)
                {
                    if (brontoOneHighlighted)
                    {
                        playerOne = Player.Chara.Bronto;
                    }
                    else if (rexOneHighlighted)
                    {
                        playerOne = Player.Chara.Trex;
                    }
                    else if (pteraOneHighlighted)
                    {
                        playerOne = Player.Chara.Ptera;
                    }
                    else if (triceraOneHighlighted)
                    {
                        playerOne = Player.Chara.Cera;
                    }

                }
            }
            else
            {
                menuDone.Play();
                if (brontoTwoHighlighted || rexTwoHighlighted || pteraTwoHighlighted || triceraTwoHighlighted)
                {
                    if (brontoTwoHighlighted)
                    {
                        playerTwo = Player.Chara.Bronto;
                    }
                    else if (rexTwoHighlighted)
                    {
                        playerTwo = Player.Chara.Trex;
                    }
                    else if (pteraTwoHighlighted)
                    {
                        playerTwo = Player.Chara.Ptera;
                    }
                    else if (triceraTwoHighlighted)
                    {
                        playerTwo = Player.Chara.Cera;
                    }

                }
            }
        }
        
        #endregion

        #region P1 Inputs

        /// <summary>
        /// Input verification for P1 Keyboard
        /// </summary>
        /// <param name="state"></param>
        private void FirstPlayerKeyboardInput(KeyboardState state)
        {
            playerOneCurrentKey = state;

            if (OneKeyMovementUp(playerOneCurrentKey, playerOneOldKey, true))
            {
                moveMenuSound.Play();

                if (keyboardOneHighlighted || gamepadOneHighlighted)
                {
                    UpdateUpControlOneBools();
                }
                else
                {
                    UpdateUpCharacterOneBools();
                }
            }
            else if (OneKeyMovementDown(playerOneCurrentKey, playerOneOldKey, true))
            {
                moveMenuSound.Play();

                if (keyboardOneHighlighted || gamepadOneHighlighted)
                {
                    UpdateDownControlOneBools();
                }
                else
                {
                    UpdateDownCharacterOneBools();
                }
            }
            else if (OneKeyVerificationPressed(playerOneCurrentKey, playerOneOldKey, true))
            {
                moveMenuSound.Play();

                if (keyboardOneHighlighted || gamepadOneHighlighted)
                {
                    UpdateControlVerificationSelection(true);
                }
                else
                {
                    UpdateCharacterVerificationSelection(true);
                }
            }
            else if (OneKeyDeclinationPressed(playerOneCurrentKey, playerOneOldKey))
            {
                moveMenuSound.Play();

                if (playerOne != Player.Chara.Unchosen)
                {
                    if (playerOne == Player.Chara.Bronto)
                    {
                        brontoOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Trex)
                    {
                        rexOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Ptera)
                    {
                        pteraOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Cera)
                    {
                        triceraOneHighlighted = false;
                    }
                    playerOne = Player.Chara.Unchosen;
                }

                if (brontoOneHighlighted || rexOneHighlighted || pteraOneHighlighted || triceraOneHighlighted)
                {
                    playerOne = Player.Chara.Unchosen;
                    if (secondPlayerControl == ChosenControl.Gamepad)
                    {
                        gamepadOneHighlighted = true;
                    }
                    else if (secondPlayerControl == ChosenControl.Keyboard)
                    {
                        keyboardOneHighlighted = true;
                    }
                }
            }

            playerOneOldKey = playerOneCurrentKey;
        }

        /// <summary>
        /// Verification for P1 Gamepad
        /// </summary>
        /// <param name="padState"></param>
        private void FirstPlayerGamepadInput(GamePadState padState)
        {
            playerOneCurrentPad = padState;

            if (OnePadMovementUp(playerOneCurrentPad, playerOneOldPad))
            {
                moveMenuSound.Play();

                if (keyboardOneHighlighted || gamepadOneHighlighted)
                {
                    UpdateUpControlOneBools();
                }
                else
                {
                    UpdateUpCharacterOneBools();
                }
            }
            else if (OnePadMovementDown(playerOneCurrentPad, playerOneOldPad))
            {
                moveMenuSound.Play();

                if (keyboardOneHighlighted || gamepadOneHighlighted)
                {
                    UpdateDownControlOneBools();
                }
                else
                {
                    UpdateDownCharacterOneBools();
                }
            }
            else if (OnePadVerificationPressed(playerOneCurrentPad, playerOneOldPad))
            {
                moveMenuSound.Play();

                if (keyboardOneHighlighted || gamepadOneHighlighted)
                {
                    UpdateControlVerificationSelection(true);
                }
                else
                {
                    UpdateCharacterVerificationSelection(true);
                }
            }
            else if (OnePadDeclinationPressed(playerOneCurrentPad, playerOneOldPad, true))
            {
                moveMenuSound.Play();

                if (playerOne != Player.Chara.Unchosen)
                {
                    if (playerOne == Player.Chara.Bronto)
                    {
                        brontoOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Trex)
                    {
                        rexOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Ptera)
                    {
                        pteraOneHighlighted = true;
                    }
                    else if (playerOne == Player.Chara.Cera)
                    {
                        triceraOneHighlighted = false;
                    }
                    playerOne = Player.Chara.Unchosen;
                }

                if (brontoOneHighlighted || rexOneHighlighted || pteraOneHighlighted || triceraOneHighlighted)
                {
                    playerOne = Player.Chara.Unchosen;
                    if (secondPlayerControl == ChosenControl.Gamepad)
                    {
                        gamepadOneHighlighted = true;
                    }
                    else if (secondPlayerControl == ChosenControl.Keyboard)
                    {
                        keyboardOneHighlighted = true;
                    }
                }
            }
            

            playerOneOldPad = playerOneCurrentPad;
        }
        #endregion

        #region P2 Inputs

        /// <summary>
        /// Verification for P2 Keyboard
        /// </summary>
        /// <param name="state"></param>
        private void SecondPlayerKeyboardInput(KeyboardState state)
        {
            playerTwoCurrentKey = state;

            if (OneKeyMovementUp(playerTwoCurrentKey, playerTwoOldKey))
            {
                moveMenuSound.Play();

                if (keyboardTwoHighlighted || gamepadTwoHighlighted)
                {
                    UpdateUpControlTwoBools();
                }
                else
                {
                    UpdateUpCharacterTwoBools();
                }
            }
            else if (OneKeyMovementDown(playerTwoCurrentKey, playerTwoOldKey))
            {
                moveMenuSound.Play();

                if (keyboardTwoHighlighted || gamepadTwoHighlighted)
                {
                    UpdateDownControlTwoBools();
                }
                else
                {
                    UpdateDownCharacterTwoBools();
                }
            }
            else if (OneKeyVerificationPressed(playerTwoCurrentKey, playerTwoOldKey))
            {
                moveMenuSound.Play();

                if (keyboardTwoHighlighted || gamepadTwoHighlighted)
                {
                    UpdateControlVerificationSelection();
                }
                else
                {
                    UpdateCharacterVerificationSelection();
                }
            }
            else if (OneKeyDeclinationPressed(playerTwoCurrentKey, playerTwoOldKey))
            {
                moveMenuSound.Play();

                if (playerTwo != Player.Chara.Unchosen)
                {
                    if (playerTwo == Player.Chara.Bronto)
                    {
                        brontoTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Trex)
                    {
                        rexTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Ptera)
                    {
                        pteraTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Cera)
                    {
                        triceraTwoHighlighted = false;
                    }
                    playerTwo = Player.Chara.Unchosen;
                }

                if (brontoTwoHighlighted || rexTwoHighlighted || pteraTwoHighlighted || triceraTwoHighlighted)
                {
                    playerTwo = Player.Chara.Unchosen;
                    if (secondPlayerControl == ChosenControl.Gamepad)
                    {
                        gamepadTwoHighlighted = true;
                    }
                    else if (secondPlayerControl == ChosenControl.Keyboard)
                    {
                        keyboardTwoHighlighted = true;
                    }
                }
            }

            playerTwoOldKey = playerTwoCurrentKey;
        }

        /// <summary>
        /// Verification for P2 Gamepad
        /// </summary>
        /// <param name="padState"></param>
        private void SecondPlayerGamepadInput(GamePadState padState)
        {
            playerTwoCurrentPad = padState;

            if (OnePadMovementUp(playerTwoCurrentPad, playerTwoOldPad))
            {
                moveMenuSound.Play();

                if (keyboardTwoHighlighted || gamepadTwoHighlighted)
                {
                    UpdateUpControlTwoBools();
                }
                else
                {
                    UpdateUpCharacterTwoBools();
                }
            }
            else if (OnePadMovementDown(playerTwoCurrentPad, playerTwoOldPad))
            {
                moveMenuSound.Play();

                if (keyboardTwoHighlighted || gamepadTwoHighlighted)
                {
                    UpdateDownControlTwoBools();
                }
                else
                {
                    UpdateDownCharacterTwoBools();
                }
            }
            else if (OnePadVerificationPressed(playerTwoCurrentPad, playerTwoOldPad))
            {
                moveMenuSound.Play();

                if (keyboardTwoHighlighted || gamepadTwoHighlighted)
                {
                    UpdateControlVerificationSelection();
                }
                else
                {
                    UpdateCharacterVerificationSelection();
                }
            }
            else if (OnePadDeclinationPressed(playerTwoCurrentPad, playerTwoOldPad))
            {
                moveMenuSound.Play();

                if (playerTwo != Player.Chara.Unchosen)
                {
                    if (playerTwo == Player.Chara.Bronto)
                    {
                        brontoTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Trex)
                    {
                        rexTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Ptera)
                    {
                        pteraTwoHighlighted = true;
                    }
                    else if (playerTwo == Player.Chara.Cera)
                    {
                        triceraTwoHighlighted = false;
                    }
                    playerTwo = Player.Chara.Unchosen;
                }

                if (brontoTwoHighlighted || rexTwoHighlighted || pteraTwoHighlighted || triceraTwoHighlighted)
                {
                    playerTwo = Player.Chara.Unchosen;
                    if (secondPlayerControl == ChosenControl.Gamepad)
                    {
                        gamepadTwoHighlighted = true;
                    }
                    else if (secondPlayerControl == ChosenControl.Keyboard)
                    {
                        keyboardTwoHighlighted = true;
                    }
                }
            }

            playerTwoOldPad = playerTwoCurrentPad;
        }
        #endregion
    }
}