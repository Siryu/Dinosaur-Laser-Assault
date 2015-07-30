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
    public class Death
    {
        private GraphicsDeviceManager gdm;
        private Sprite playerOneSprite, playerTwoSprite;

        private KeyboardState currentKey, oldKey;
        private GamePadState currentPad, oldPad;

        private Texture2D gameOverText, buttonTexture;
        private int opacityCount;
        private float gameOverOpacity;
        private SpriteFont font;
        private Vector2 titleButtonPosition;
        private Color c;

        public bool TitleSelected { get; private set; }

        public Death(GraphicsDeviceManager gdm, ContentManager cm, Sprite playerOneSprite, Sprite playerTwoSprite)
        {
            this.gdm = gdm;
            this.playerOneSprite = playerOneSprite;
            if (playerTwoSprite != null)
            {
                this.playerTwoSprite = playerTwoSprite;
            }
            font = cm.Load<SpriteFont>("Fonts/SubtextFont");
            titleButtonPosition = new Vector2(0, -300);
            gameOverText = cm.Load<Texture2D>("GameOver/game-over");
            gameOverOpacity = 0;
            opacityCount = 0;

            c.A = 180;
            c.R = 50;
            c.G = 50;
            c.B = 50;

            buttonTexture = new Texture2D(gdm.GraphicsDevice, 800, 150);
            buttonTexture.SetData(ColorPicker.setTexture(buttonTexture.Width, buttonTexture.Height, c));
        }

        public void Update(GameTime gameTime)
        {
            if (gameOverOpacity < 1)
            {
                opacityCount += 1;
                gameOverOpacity = opacityCount / 255f;
            }

            if (playerOneSprite.Position.Y < gdm.PreferredBackBufferHeight + 200)
            {
                Vector2 movement = Vector2.Zero;
                movement += Vector2.UnitY * .05f;
                playerOneSprite.Movement += movement;

                playerOneSprite.Update(gameTime);
                movement.X = 0f;
                movement.Y = 0f;
                playerOneSprite.Movement = movement;
                playerOneSprite.Update(gameTime);
            }
            else if (playerTwoSprite != null)
            {
                if (playerTwoSprite.Position.Y < gdm.PreferredBackBufferHeight + 200)
                {
                    Vector2 movement = Vector2.Zero;
                    movement += Vector2.UnitY * .05f;
                    playerTwoSprite.Movement += movement;

                    playerTwoSprite.Update(gameTime);
                    movement.X = 0f;
                    movement.Y = 0f;
                    playerTwoSprite.Movement = movement;
                    playerTwoSprite.Update(gameTime);
                }
            }

            if (gameOverOpacity >= 150 / 255f)
            {
                titleButtonPosition = new Vector2(550, 800);
            }

            if (titleButtonPosition != null && titleButtonPosition.Y > 0)
            {
                CheckInputs();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(gameOverText, new Rectangle(0, 0, 1920, 1080), Color.White * gameOverOpacity);
            sb.End();

            if (titleButtonPosition != null && titleButtonPosition.Y > 0)
            {
                sb.Begin();
                sb.Draw(buttonTexture, new Rectangle(530, 780, 870, 100), c);
                sb.End();

                sb.Begin();
                sb.Draw(buttonTexture, new Rectangle(525, 775, 870, 100), Color.White);
                sb.End();

                sb.Begin();
                sb.Draw(buttonTexture, new Rectangle(525, 775, 870, 100), c);
                sb.End();

                sb.Begin();
                sb.DrawString(font, "RETURN TO TITLE SCREEN", titleButtonPosition, Color.White);
                sb.End();
            }
        }

        private void CheckInputs()
        {
            currentKey = Keyboard.GetState();
            currentPad = GamePad.GetState(PlayerIndex.One);

            if (currentKey.IsKeyDown(Keys.F) || currentKey.IsKeyDown(Keys.Enter) || currentKey.IsKeyDown(Keys.Space) || currentPad.IsButtonDown(Buttons.A))
            {
                if (!(oldKey.IsKeyDown(Keys.F) || oldKey.IsKeyDown(Keys.Enter) || oldKey.IsKeyDown(Keys.Space) || oldPad.IsButtonDown(Buttons.A)))
                {
                    TitleSelected = true;
                }
            }


            oldKey = currentKey;
            oldPad = currentPad;
        }
    }
}
