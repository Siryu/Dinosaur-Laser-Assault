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

namespace DinosaurLazers.Models
{
    public class PlayerShots
    {
        public List<Vector2> shotsOne, shotsTwo;
        Texture2D playerOneShotImage, playerTwoShotImage;
        Player dinosaurOne, dinosaurTwo;
        SoundEffect playerShot;

        Sprite playerOne, playerTwo;

        public PlayerShots(ContentManager content, Player dinoOne, Sprite playerOneSprite, Player dinoTwo = null, Sprite playerTwoSprite = null)
        {
            shotsOne = new List<Vector2>();
            playerOneShotImage = content.Load<Texture2D>("Images/laserPurple");
            playerShot = content.Load<SoundEffect>("Sounds/laser");
            dinosaurOne = dinoOne;
            playerOne = playerOneSprite;

            if (dinoTwo != null && playerTwoSprite != null)
            {
                shotsTwo = new List<Vector2>();
                playerTwoShotImage = content.Load<Texture2D>("Images/laserBlue");
                dinosaurTwo = dinoTwo;
                playerTwo = playerTwoSprite;
            }
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {

            for (int i = shotsOne.Count - 1; i >= 0; i--)
            {
                shotsOne[i] -= Vector2.UnitY * gameTime.ElapsedGameTime.Milliseconds;

                if (shotsOne[i].Y < -50)
                {
                    shotsOne.RemoveAt(i);
                }
            }

            if (shotsTwo != null)
            {
                for (int i = shotsTwo.Count - 1; i >= 0; i--)
                {
                    shotsTwo[i] -= Vector2.UnitY * gameTime.ElapsedGameTime.Milliseconds;

                    if (shotsTwo[i].Y < -50)
                    {
                        shotsTwo.RemoveAt(i);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector2 shot in shotsOne)
            {
                spriteBatch.Draw(playerOneShotImage, shot, Color.White);
            }

            if (shotsTwo != null)
            {
                foreach (Vector2 shot in shotsTwo)
                {
                    spriteBatch.Draw(playerTwoShotImage, shot, Color.White);
                }
            }
        }

        public void FireOne(Player dinosaur)
        {
            if (dinosaur.LasersPowerLevel == 1)
            {
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * 4);
            }

            else if (dinosaur.LasersPowerLevel == 2)
            {
                if (dinosaur.dinoChosen == Player.Chara.Ptera)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 - 40));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 + 20));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Trex)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 - 10));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 + 20));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Cera)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 - 10));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 + 12));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Bronto)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 5));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 5));
                }
            }

            else if (dinosaur.LasersPowerLevel == 3)
            {
                if (dinosaur.dinoChosen == Player.Chara.Ptera)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 - 43));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 + 17));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 - 37));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 + 23));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Trex)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 - 7));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 + 17));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 - 13));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 + 23));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Cera)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 - 7));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 + 9));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 - 13));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 + 15));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Bronto)
                {
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 8));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 8));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 2));
                    shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 2));
                }
            }

            else if (dinosaur.LasersPowerLevel == 4)
            {
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 20));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 20));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 40 - Vector2.UnitX * 50);
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 40 + Vector2.UnitX * 40);
                shotsOne.Add(playerOne.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 - 20));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 + 20));
            }

            else if (dinosaur.LasersPowerLevel == 5)
            {
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 20));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 20));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 40 - Vector2.UnitX * 50);
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 40 + Vector2.UnitX * 40);
                shotsOne.Add(playerOne.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 - 20));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 + 20));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 40));
                shotsOne.Add(playerOne.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 40));
            }

            playerShot.Play();
        }

        public void FireTwo(Player dinosaur)
        {
            if (dinosaur.LasersPowerLevel == 1)
            {
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * 4);
            }

            else if (dinosaur.LasersPowerLevel == 2)
            {
                if (dinosaur.dinoChosen == Player.Chara.Ptera)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 - 40));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 + 20));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Trex)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 - 10));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 + 20));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Cera)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 - 10));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 + 12));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Bronto)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 5));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 5));
                }
            }

            else if (dinosaur.LasersPowerLevel == 3)
            {
                if (dinosaur.dinoChosen == Player.Chara.Ptera)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 - 43));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 + 17));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 - 37));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 30 - Vector2.UnitX * (4 + 23));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Trex)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 - 7));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 + 17));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 - 13));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 45 - Vector2.UnitX * (4 + 23));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Cera)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 - 7));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 + 9));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 - 13));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 85 - Vector2.UnitX * (4 + 15));
                }
                else if (dinosaur.dinoChosen == Player.Chara.Bronto)
                {
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 8));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 8));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 2));
                    shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 2));
                }
            }

            else if (dinosaur.LasersPowerLevel == 4)
            {
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 20));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 20));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 40 - Vector2.UnitX * 50);
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 40 + Vector2.UnitX * 40);
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 - 20));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 + 20));
            }

            else if (dinosaur.LasersPowerLevel == 5)
            {
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 20));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 20));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 40 - Vector2.UnitX * 50);
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 40 + Vector2.UnitX * 40);
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 - 20));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * (100 - 80) - Vector2.UnitX * (4 + 20));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 - 40));
                shotsTwo.Add(playerTwo.Position - Vector2.UnitY * 100 - Vector2.UnitX * (4 + 40));
            }

            playerShot.Play();
        }

    }
}
