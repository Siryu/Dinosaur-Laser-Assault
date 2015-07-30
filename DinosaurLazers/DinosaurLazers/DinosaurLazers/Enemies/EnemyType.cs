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

namespace DinosaurLazers.Enemies
{
    public class EnemyType
    {

        public float spacing;
        public int life;
        public float speed;
        public Texture2D image;
        public Etype type;
        public int PointValue { get; private set; }
        public bool TargetPlayerOne { get; private set; }

        public enum Etype
        {
            Player,
            Simple,
            Bomber, 
            Gunner,
            Darting,
            Blinking, 
            Hard,

            Level1MidBoss,
            Level2MidBoss,
            Level3MidBoss,
            Level4MidBoss,
            Level5MidBoss,

            Level1Boss,
            Level2Boss,
            Level3Boss,
            Level4Boss,
            Level5Boss
        }

        public EnemyType(ContentManager content, Etype type)
        {
            this.type = type;

            if (type == Etype.Simple)
            {
                createSimple(content);
            }
            else if (type == Etype.Gunner)
            {
                createGunner(content);
            }
            else if (type == Etype.Bomber)
            {
                createBomber(content);
            }
            else if (type == Etype.Darting)
            {
                createDarting(content);
            }
            else if (type == Etype.Blinking)
            {
                createBlinking(content);
            }
            else if (type == Etype.Hard)
            {
                createHard(content);
            }
            else if (type == Etype.Level1MidBoss)
            {
                createLevel1MidBoss(content);
            }
            else if (type == Etype.Level1Boss)
            {
                createLevel5Boss(content);
            }
            else if (type == Etype.Level2MidBoss)
            {
                createLevel2MidBoss(content);
            }
            else if (type == Etype.Level2Boss)
            {
                createLevel2Boss(content);
            }
            else if (type == Etype.Level3MidBoss)
            {
                createLevel3MidBoss(content);
            }
            else if (type == Etype.Level3Boss)
            {
                createLevel3Boss(content);
            }
            else if (type == Etype.Level4MidBoss)
            {
                createLevel4MidBoss(content);
            }
            else if (type == Etype.Level4Boss)
            {
                createLevel4Boss(content);
            }
            else if (type == Etype.Level5MidBoss)
            {
                createLevel5MidBoss(content);
            }
            else if (type == Etype.Level5Boss)
            {
                createLevel5Boss(content);
            }
        }

        private void createLevel2MidBoss(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .6f;
            this.life = 400;
            this.PointValue = 500;
            this.image = content.Load<Texture2D>("Images/dudeman_spritesheet");
        }

        private void createLevel3MidBoss(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .6f;
            this.life = 400;
            this.PointValue = 500;
            this.image = content.Load<Texture2D>("Images/dudeman_spritesheet");
        }

        private void createLevel4MidBoss(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .6f;
            this.life = 400;
            this.PointValue = 500;
            this.image = content.Load<Texture2D>("Images/dudeman_spritesheet");
        }

        private void createLevel5MidBoss(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .6f;
            this.life = 400;
            this.PointValue = 500;
            this.image = content.Load<Texture2D>("Images/dudeman_spritesheet");
        }

        private void createLevel2Boss(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .2f;
            this.life = 6000;
            this.PointValue = 2000;
            this.image = content.Load<Texture2D>("Enemies/tankdinosaur");
        }

        private void createLevel3Boss(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .7f;
            this.life = 4000;
            this.PointValue = 2000;
            this.image = content.Load<Texture2D>("Enemies/spino_spritesheet");
        }

        private void createLevel4Boss(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .4f;
            this.life = 7000;
            this.PointValue = 2000;
            this.image = content.Load<Texture2D>("Enemies/stego_spritesheet");
        }

        private void createSimple(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .4f;
            this.life = 10;
            this.PointValue = 10;
            this.image = content.Load<Texture2D>("Enemies/simple");
        }

        private void createGunner(ContentManager content)
        {
            this.spacing = 70;
            this.speed = .7f;
            this.life = 50;
            this.PointValue = 50;
            this.image = content.Load<Texture2D>("Enemies/gunner");
        }

        private void createBomber(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .3f;
            this.life = 100;
            this.PointValue = 75;
            this.image = content.Load<Texture2D>("Enemies/bomber");
        }

        private void createDarting(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .5f;
            this.life = 30;
            this.PointValue = 50;
            this.image = content.Load<Texture2D>("Enemies/darting");
        }

        private void createBlinking(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .4f;
            this.life =50;
            this.PointValue = 50;
            this.image = content.Load<Texture2D>("Enemies/blinking");
        }

        private void createHard(ContentManager content)
        {
            this.spacing = 300;
            this.speed = .4f;
            this.life = 150;
            this.PointValue = 150;
            this.image = content.Load<Texture2D>("Enemies/hard");
            Random r = new Random();
            double rand = r.NextDouble();
            if (rand > .5)
            {
                TargetPlayerOne = true;
            }
        }

        private void createLevel1MidBoss(ContentManager content)
        {
            this.spacing = 1;
            this.speed = .1f;
            this.life = 1000;
            this.PointValue = 500;
            this.image = content.Load<Texture2D>("Enemies/midboss");
        }

        private void createLevel5Boss(ContentManager content)
        {
            this.spacing = 1;
            this.speed = .1f;
            this.life = 10000;
            this.PointValue = 1000;
            this.image = content.Load<Texture2D>("Enemies/boss");
        }

        public Vector2 Update(EnemyType enemyType, Vector2 position, Vector2 movement, GraphicsDeviceManager graphics, Sprite playerOneSprite, Sprite playerTwoSprite = null, Player one = null, Player two = null)
        {
            Vector2 AIMovement = Vector2.Zero;

            if (enemyType.type == Etype.Simple)
            {
                AIMovement = simpleAIMovement(graphics, position, movement);
            }
            else if (enemyType.type == Etype.Gunner)
            {
                AIMovement = gunnerAIMovement(graphics, position, movement);
            }
            else if (enemyType.type == Etype.Bomber)
            {
                AIMovement = bomberAIMovement(graphics, position, movement);
            }
            else if (enemyType.type == Etype.Darting)
            {
                AIMovement = dartingAIMovement(graphics, position, movement);
            }
            else if (type == Etype.Blinking)
            {
                // update ai
                AIMovement = createBlinking(graphics, position, movement);
            }
            else if (enemyType.type == Etype.Hard)
            {
                AIMovement = hardAIMovement(graphics, position, movement, enemyType.speed, playerOneSprite, playerTwoSprite, one, two);
            }
            else if (type == Etype.Level1MidBoss)
            {
                // update ai
                AIMovement = createLevel1MidBoss(graphics, position, movement);
            }
            else if (type == Etype.Level1Boss)
            {
                AIMovement = createLevel1Boss(graphics, position, movement);
            }
            else if (type == Etype.Level2MidBoss)
            {
                AIMovement = createLevel2MidBoss(graphics, position, movement);
            }
            else if (type == Etype.Level3Boss)
            {
                AIMovement = createLevel3Boss(graphics, position, movement);
            }
            return AIMovement;
        }

        private Vector2 createLevel2MidBoss(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            return movement;
        }

        private Vector2 createLevel3Boss(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            return movement;
        }

        private Vector2 createLevel1Boss(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            if (position.Y > 350)
                movement.Y *= -1;
            
            return movement;
        }

        private Vector2 createLevel1MidBoss(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            return movement;
        }

        private Vector2 createBlinking(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            return movement;
        }

        private Vector2 dartingAIMovement(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            return movement;
        }

        private Vector2 hardAIMovement(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement, float speed ,Sprite playerOneSprite, Sprite playerTwoSprite = null, Player one = null, Player two = null)
        {
            if (playerTwoSprite == null)
            {
                TargetPlayerOne = true;
            }
            if (TargetPlayerOne)
            {
                if (position.X > playerOneSprite.Position.X + 10 && position.X < graphics.PreferredBackBufferWidth)
                {
                    movement.X = -speed;
                }
                else if (position.X < playerOneSprite.Position.X - 10 && position.X > 0)
                {
                    movement.X = speed;
                }

                if (one.Health == 0 && playerTwoSprite != null)
                {
                    TargetPlayerOne = false;
                }
            }
            else
            {
                if (playerTwoSprite != null)
                {
                    if (position.X > playerTwoSprite.Position.X + 10 && position.X < graphics.PreferredBackBufferWidth)
                    {
                        movement.X = -speed;
                    }
                    else if (position.X < playerTwoSprite.Position.X - 10 && position.X > 0)
                    {
                        movement.X = speed;
                    }

                    if (two.Health == 0)
                    {
                        TargetPlayerOne = true;
                    }
                }
            }


            return movement;
        }

        private Vector2 bomberAIMovement(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            return movement;
        }

        private Vector2 gunnerAIMovement(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            return movement;
        }

        private Vector2 simpleAIMovement(GraphicsDeviceManager graphics, Vector2 position, Vector2 movement)
        {
            if (position.Y > 100)
            {
                movement.Y *= -1;
            }

            return movement;
        }
    }
}
