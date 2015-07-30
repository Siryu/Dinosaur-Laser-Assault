using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DinosaurLazers.Models;
using DinosaurLazers.GameStates;

namespace DinosaurLazers.Controls
{
    public class KeyboardInput
    {
        KeyboardState _currentKeyPressed;
        KeyboardState _oldKeyPressed;
        Vector2 _movement;
        
        public void Update(KeyboardState state, Sprite player, Player dino, PlayerShots playerShots, GraphicsDeviceManager graphics, GameTime gameTime, GameState gameState)
        {
            _oldKeyPressed = _currentKeyPressed;
            _currentKeyPressed = state;

            _movement = Vector2.Zero;

            if (dino.PlayerIndex == PlayerIndex.One)
            {
                if ((_currentKeyPressed.IsKeyDown(Keys.A) || _currentKeyPressed.IsKeyDown(Keys.Left)) && player.Position.X > 50)
                {
                    _movement -= Vector2.UnitX * .7f;
                }
                if ((_currentKeyPressed.IsKeyDown(Keys.D) || _currentKeyPressed.IsKeyDown(Keys.Right)) && player.Position.X < graphics.PreferredBackBufferWidth - 50)
                {
                    _movement += Vector2.UnitX * .7f;
                }
                if ((_currentKeyPressed.IsKeyDown(Keys.W) || _currentKeyPressed.IsKeyDown(Keys.Up)) && player.Position.Y > (graphics.PreferredBackBufferHeight / 2) + 100)
                {
                    _movement -= Vector2.UnitY * .5f;
                }
                if ((_currentKeyPressed.IsKeyDown(Keys.S) || _currentKeyPressed.IsKeyDown(Keys.Down)) && player.Position.Y < graphics.PreferredBackBufferHeight - player.Texture.Height)
                {
                    _movement += Vector2.UnitY * .5f;
                }
            }
            else if (dino.PlayerIndex == PlayerIndex.Two)
            {
                if (_currentKeyPressed.IsKeyDown(Keys.L) && player.Position.X > 50)
                {
                    _movement -= Vector2.UnitX * .7f;
                }
                if (_currentKeyPressed.IsKeyDown(Keys.OemQuotes)&& player.Position.X < graphics.PreferredBackBufferWidth - 50)
                {
                    _movement += Vector2.UnitX * .7f;
                }
                if (_currentKeyPressed.IsKeyDown(Keys.P) && player.Position.Y > (graphics.PreferredBackBufferHeight / 2) + 100)
                {
                    _movement -= Vector2.UnitY * .5f;
                }
                if (_currentKeyPressed.IsKeyDown(Keys.OemSemicolon) && player.Position.Y < graphics.PreferredBackBufferHeight - player.Texture.Height)
                {
                    _movement += Vector2.UnitY * .5f;
                }
            }

            // Jamie Key
            if(_currentKeyPressed.IsKeyDown(Keys.J) && !_oldKeyPressed.IsKeyDown(Keys.J))
            {
                if (dino._laserPowerLevel < 5)
                    dino._laserPowerLevel += 1;
                else
                    dino._laserPowerLevel = 1;
            }

            if (_currentKeyPressed.IsKeyDown(Keys.Space) || _currentKeyPressed.IsKeyDown(Keys.F) || _currentKeyPressed.IsKeyDown(Keys.Enter))
            {
                if (dino.LaserCharge > 0)
                {
                    

                    if (dino.PlayerIndex == PlayerIndex.One)
                    {
                        if (_currentKeyPressed.IsKeyDown(Keys.F) || _currentKeyPressed.IsKeyDown(Keys.Space))
                        {
                            playerShots.FireOne(dino);
                            dino._currentCharge -= 1;
                        }
                    }
                    else if (dino.PlayerIndex == PlayerIndex.Two)
                    {
                        if (_currentKeyPressed.IsKeyDown(Keys.Enter))
                        {
                            playerShots.FireTwo(dino);
                            dino._currentCharge -= 1;
                        }
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

        bool WasJustPressed(Keys keyToCheck)
        {
            return _oldKeyPressed.IsKeyUp(keyToCheck) && _currentKeyPressed.IsKeyDown(keyToCheck);
        }
    }
}
