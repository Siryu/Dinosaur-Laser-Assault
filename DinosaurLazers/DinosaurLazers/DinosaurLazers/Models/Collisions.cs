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
    public class Collisions
    {
        public List<Powerup> powerups = new List<Powerup>();
        List<Sprite> enemies;
        List<AnimatedSprite> explosions;
        EnemyShots enemyShots;
        PlayerShots playerShots;
        Sprite playerOneSprite, playerTwoSprite;
        Player dinosaurOne, dinosaurTwo;
        SoundEffect laserPowerupSound, healthPowerupSound, enemyHit, playerHit;
        public SoundEffect explosionSound;

        GraphicsDeviceManager gdm;
        SpriteBatch sb;
        ContentManager cm;

        public Texture2D explodeImage;
        //TextController _textController;
        bool onMarshObject;

        float _halfScreenHeight;

        public Collisions(ContentManager content, List<AnimatedSprite> explosions)
        {
            explodeImage = content.Load<Texture2D>("Images/explosion_sheet");
            this.explosions = explosions;
        }

        public void Update(Game game,ContentManager content, SpriteBatch spriteBatch, SpriteFont font, Player playerOne, List<Sprite> enemies, EnemyShots enemyShots, PlayerShots playerShots, List<Sprite> randomObjects, List<HealthBar> healthBars, Sprite playerOneSprite, GraphicsDeviceManager graphics, Player playerTwo = null, Sprite playerTwoSprite = null)
        {
            this.enemies = enemies;
            this.enemyShots = enemyShots;
            this.playerShots = playerShots;
            this.playerOneSprite = playerOneSprite;
            this.dinosaurOne = playerOne;
            this.playerTwoSprite = playerTwoSprite;
            this.dinosaurTwo = playerTwo;
            _halfScreenHeight = graphics.PreferredBackBufferHeight / 1.5f;

            gdm = graphics;
            sb = spriteBatch;
            cm = content;

            explosionSound = cm.Load<SoundEffect>("Sounds/explosion");
            laserPowerupSound = cm.Load<SoundEffect>("Sounds/Few"); 
            healthPowerupSound = cm.Load<SoundEffect>("Sounds/Few");
            enemyHit = cm.Load<SoundEffect>("Sounds/Few");
            playerHit = cm.Load<SoundEffect>("sounds/Punch");

            if (!playerOneSprite.IsInvincible)
            {
                CheckIfEnemyHitsPlayer();
            }

            CheckIfPowerupHitsPlayer();

            CheckIfPlayerHitSomething(game, spriteBatch, content, healthBars);

            checkIfPlayerIsHitByGroundItem(randomObjects);

        }

        private void CheckIfPlayerHitSomething(Game game, SpriteBatch spriteBatch, ContentManager Content, List<HealthBar> healthBars)
        {
            for (int enemyCounter = enemies.Count - 1; enemyCounter >= 0; enemyCounter--)
            {
                Sprite enemy = enemies[enemyCounter];
                enemy.IsHit = false;
                for (int i = playerShots.shotsOne.Count - 1; i >= 0; i--) 
                {
                    //don't check for collision below half screen height
                    if (playerShots.shotsOne[i].Y > _halfScreenHeight)
                        continue;

                    if ((Vector2.DistanceSquared(playerShots.shotsOne[i], enemy.Position) < ((enemy.width * enemy.height) / 11f)))
                    {
                        enemy.IsHit = true;
                        enemy._currentHealth -= 10;
                        playerShots.shotsOne.RemoveAt(i);
                        if (enemy._currentHealth <= 0)
                        {
                            AnimatedSprite explode = new AnimatedSprite(12, 0, 0, 134, 134, game, explodeImage,
                                enemy.Position,
                                spriteBatch, new Enemies.EnemyType(Content, Enemies.EnemyType.Etype.Simple));
                            explode.Name = "Explode";
                            explode.delay = 75f;

                            healthBars.RemoveAt(healthBars.Count - 1);

                            explosionSound.Play();
                            dinosaurOne.Points += enemy.enemyType.PointValue;
                            Random r = new Random();
                            if (r.NextDouble() > 0.75)
                            {
                                Vector2 powerupStartingPosition = enemy.Position;
                                Powerup p = new Powerup(gdm, sb, cm, powerupStartingPosition);
                                powerups.Add(p);
                            }

                            enemies.RemoveAt(enemyCounter);
                            explosions.Add(explode);
                        }
                        return;
                    }
                }
            }

            if (playerShots.shotsTwo != null)
            {

                for (int enemyCounter = enemies.Count - 1; enemyCounter >= 0; enemyCounter--)
                {
                    Sprite enemy = enemies[enemyCounter];
                    enemy.IsHit = false;
                    for (int i = playerShots.shotsTwo.Count - 1; i >= 0; i--)
                    {
                        //don't check for collision below half screen height
                        if (playerShots.shotsTwo[i].Y > _halfScreenHeight)
                            continue;

                        if ((Vector2.DistanceSquared(playerShots.shotsTwo[i], enemy.Position) < ((enemy.width * enemy.height) / 11f)))
                        {
                            enemy.IsHit = true;
                            enemy._currentHealth -= 10;
                            playerShots.shotsTwo.RemoveAt(i);
                            if (enemy._currentHealth <= 0)
                            {
                                AnimatedSprite explode = new AnimatedSprite(12, 0, 0, 134, 134, game, explodeImage,
                                    enemy.Position,
                                    spriteBatch, new Enemies.EnemyType(Content, Enemies.EnemyType.Etype.Simple));
                                explode.Name = "Explode";
                                explode.delay = 75f;

                                healthBars.RemoveAt(healthBars.Count - 1);

                                explosionSound.Play();
                                dinosaurTwo.Points += enemy.enemyType.PointValue;
                                Random r = new Random();
                                if (r.NextDouble() > 0.75)
                                {
                                    Vector2 powerupStartingPosition = enemy.Position;
                                    Powerup p = new Powerup(gdm, sb, cm, powerupStartingPosition);
                                    powerups.Add(p);
                                }

                                enemies.RemoveAt(enemyCounter);
                                explosions.Add(explode);
                            }
                            return;
                        }
                    }
                }
            }
        }

        private void CheckIfEnemyHitsPlayer()
        {
            float _borderOverPlayer = _halfScreenHeight - playerOneSprite.Texture.Height;
            foreach (Vector2 shot in enemyShots.list.Keys)
            {
                 if (shot.Y < _borderOverPlayer)
                    continue;

                 if (Vector2.DistanceSquared(shot, playerOneSprite.Position) < (playerOneSprite.width  + playerOneSprite.height) * 5)
                 {
                     if (!playerOneSprite.IsInvincible)
                     {
                         if (dinosaurOne._laserPowerLevel > 1)
                         {
                             //dinosaur._laserPowerLevel -= 1;
                         }

                         dinosaurOne._currentHealth -= 10;
                         playerHit.Play();
                         enemyShots.list.Remove(shot);
                     }

                     playerOneSprite.InvincibleTimeLeft = 2000;
                     break;
                 }

                 if (playerTwoSprite != null)
                 {
                     if (Vector2.DistanceSquared(shot, playerTwoSprite.Position) < ((playerTwoSprite as AnimatedSprite).width * (playerTwoSprite as AnimatedSprite).height) / 2.7f)
                     {
                         if (!playerTwoSprite.IsInvincible)
                         {
                             if (dinosaurTwo._laserPowerLevel > 1)
                             {
                                 //dinosaur._laserPowerLevel -= 1;
                             }

                             dinosaurTwo._currentHealth -= 10;
                             playerHit.Play();
                             enemyShots.list.Remove(shot);
                         }

                         playerTwoSprite.InvincibleTimeLeft = 2000;
                         break;
                     }
                 }
            }
        }

        private void checkIfPlayerIsHitByGroundItem(List<Sprite> randomObjects)
        {
            float _borderOverPlayer = _halfScreenHeight - playerOneSprite.Texture.Height;
            onMarshObject = false;
            
            foreach (Sprite rObj in randomObjects)
            {
                if (rObj.Position.Y < _borderOverPlayer)
                    continue;

                if (Vector2.DistanceSquared(rObj.Position, playerOneSprite.Position) < (float)((rObj.Texture.Width * rObj.Texture.Height) * rObj.Scale / 3))
                {
                    if (rObj.objectType == ObjectType.Spikes)
                    {
                        if (!playerOneSprite.IsInvincible)
                        {
                            dinosaurOne._currentHealth -= 5;
                            playerHit.Play();
                            playerOneSprite.InvincibleTimeLeft = 2000;
                        }
                    }
                    else if (rObj.objectType == ObjectType.Marsh && !onMarshObject)
                    {
                        playerOneSprite.IsInMarsh = true;
                        onMarshObject = true;
                    }
                }

                if (playerTwoSprite != null)
                {
                    if (Vector2.DistanceSquared(rObj.Position, playerTwoSprite.Position) < (float)((rObj.Texture.Width * rObj.Texture.Height) * rObj.Scale / 3))
                    {
                        if (rObj.objectType == ObjectType.Spikes)
                        {
                            if (!playerTwoSprite.IsInvincible)
                            {
                                dinosaurTwo._currentHealth -= 5;
                                playerHit.Play();
                                playerTwoSprite.InvincibleTimeLeft = 2000;
                            }
                        }
                        else if (rObj.objectType == ObjectType.Marsh && !onMarshObject)
                        {
                            playerTwoSprite.IsInMarsh = true;
                            onMarshObject = true;
                        }
                    }
                }
            }
            if (!onMarshObject && playerOneSprite.IsInMarsh)
            {
                playerOneSprite.IsInMarsh = false;
            }
            if (playerTwoSprite != null)
            {
                if (!onMarshObject && playerTwoSprite.IsInMarsh)
                {
                    playerTwoSprite.IsInMarsh = false;
                }
            }
        }

        private void CheckIfPowerupHitsPlayer()
        {
            float _borderOverPlayer = _halfScreenHeight - playerOneSprite.Texture.Height;
            float playerWidth = playerOneSprite.Texture.Width;
            float playerHeight = playerOneSprite.Texture.Height;

            foreach (Powerup powerup in powerups)
            {

                float distanceBetweenPowerupAndPlayer = powerup.Position.X - playerOneSprite.Position.X;
                if (powerup.Position.Y < _borderOverPlayer)
                    continue;

                if (ObjectsColliding(powerup, playerOneSprite))
                {
                    
                    if (powerup.type == PowerupType.Health)
                    {
                        if (dinosaurOne._currentHealth < dinosaurOne.MaxHealth)
                        {
                            dinosaurOne._currentHealth += 10;
                            healthPowerupSound.Play();
                            if (dinosaurOne._currentHealth > dinosaurOne.MaxHealth)
                                dinosaurOne._currentHealth = (int)dinosaurOne.MaxHealth;
                        }
                        else
                        {
                            dinosaurOne.Points += 50;
                        }
                    }
                    else if (powerup.type == PowerupType.Charge)
                    {
                        if (dinosaurOne._currentCharge < dinosaurOne.MaxLaserCharge)
                        {
                            laserPowerupSound.Play();
                            dinosaurOne._currentCharge += 50;
                            if (dinosaurOne._currentCharge > dinosaurOne.MaxLaserCharge)
                                dinosaurOne._currentCharge = (int)dinosaurOne.MaxLaserCharge;
                        }
                        else
                        {
                            dinosaurOne.Points += 50;
                        }
                    }

                    powerups.Remove(powerup);
                    

                    if (powerup.Position.Y > gdm.PreferredBackBufferHeight)
                    {
                        powerups.Remove(powerup);
                    }

                    break;
                }

                if (playerTwoSprite != null)
                {
                    if (ObjectsColliding(powerup, playerTwoSprite))
                    {
                        
                        if (powerup.type == PowerupType.Health)
                        {
                            if (dinosaurTwo._currentHealth < dinosaurTwo.MaxHealth)
                            {
                                dinosaurTwo._currentHealth += 10;
                                healthPowerupSound.Play();
                                if (dinosaurTwo._currentHealth > dinosaurTwo.MaxHealth)
                                    dinosaurTwo._currentHealth = (int)dinosaurTwo.MaxHealth;
                            }
                            else
                            {
                                dinosaurTwo.Points += 50;
                            }
                        }
                        else if (powerup.type == PowerupType.Charge)
                        {
                            if (dinosaurTwo._currentCharge < dinosaurTwo.MaxLaserCharge)
                            {
                                laserPowerupSound.Play();
                                dinosaurTwo._currentCharge += 50;
                                if (dinosaurTwo._currentCharge > dinosaurTwo.MaxLaserCharge)
                                    dinosaurTwo._currentCharge = (int)dinosaurTwo.MaxLaserCharge;
                            }
                            else
                            {
                                dinosaurTwo.Points += 50;
                            }
                        }

                        powerups.Remove(powerup);
                        

                        if (powerup.Position.Y > gdm.PreferredBackBufferHeight)
                        {
                            powerups.Remove(powerup);
                        }

                        break;
                    }
                }
            }
        }

        private bool ObjectsColliding(Powerup powerup, Sprite dinosaur)
        {
            float powerupLeft = powerup.Position.X;
            float powerupRight = powerupLeft + 50;
            float powerupTop = powerup.Position.Y;
            float powerupBottom = powerupTop + 50;

            float dinosaurLeft = dinosaur.Position.X - 30;
            float dinosaurRight = dinosaurLeft + 60;
            float dinosaurTop = dinosaur.Position.Y - 60;
            float dinosaurBottom = dinosaurTop + 120;

            if (powerupBottom >= dinosaurTop)
            {
                if (powerupTop <= dinosaurBottom)
                {
                    if (powerupLeft <= dinosaurRight && powerupRight >= dinosaurLeft)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
