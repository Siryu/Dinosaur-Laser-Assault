using DinosaurLazers.GameStates;
using DinosaurLazers.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Controls
{
    public class GamepadInput
    {
        GamePadState _currentInput;
        GamePadState _oldInput;
        Vector2 _movement;

        public void Update(GamePadState state, Sprite player, Player dino, PlayerShots playerShots, GraphicsDeviceManager graphics, GameTime gameTime, GameState gameState)
        {
            _oldInput = _currentInput;
            _currentInput = state;

            _movement = Vector2.Zero;

            if ((_currentInput.IsButtonUp(Buttons.LeftThumbstickRight)) && player.Position.X > 50)
            {
                _movement -= Vector2.UnitX * .7f;
            }
            if ((_currentInput.IsButtonUp(Buttons.LeftThumbstickLeft)) && player.Position.X < graphics.PreferredBackBufferWidth - 50)
            {
                _movement += Vector2.UnitX * .7f;
            }
            if ((_currentInput.IsButtonUp(Buttons.LeftThumbstickDown)) && player.Position.Y > (graphics.PreferredBackBufferHeight / 2) + 100)
            {
                _movement -= Vector2.UnitY * .5f;
            }
            if ((_currentInput.IsButtonUp(Buttons.LeftThumbstickUp)) && player.Position.Y < graphics.PreferredBackBufferHeight - player.Texture.Height)
            {
                _movement += Vector2.UnitY * .5f;
            }

            // Jamie Key
            if (_currentInput.IsButtonDown(Buttons.Y) && !_oldInput.IsButtonDown(Buttons.Y))
            {
                if (dino._laserPowerLevel < 5)
                    dino._laserPowerLevel += 1;
                else
                    dino._laserPowerLevel = 1;
            }

            if (_currentInput.Buttons.A  == ButtonState.Pressed)
            {
                if (dino.LaserCharge > 0)
                {
                    dino._currentCharge -= 1;

                    if (dino.PlayerIndex == PlayerIndex.One)
                    {
                        playerShots.FireOne(dino);
                    }
                    else if (dino.PlayerIndex == PlayerIndex.Two)
                    {
                        playerShots.FireTwo(dino);
                    }
                }
            }

            if (player.IsInMarsh)
            {
                player.Movement += _movement / 3;
            }
            else
            {
                player.Movement += _movement;
            }
            player.Update(gameTime);
            _movement.X = 0f;
            _movement.Y = 0f;
            player.Movement = _movement;
            player.Update(gameTime);
        }

        private bool WasJustPressed(Buttons buttonToCheck)
        {
            return _oldInput.IsButtonUp(buttonToCheck) && _currentInput.IsButtonDown(buttonToCheck);
        }
    }
}
