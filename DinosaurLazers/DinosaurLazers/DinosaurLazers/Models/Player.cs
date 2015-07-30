using DinosaurLazers.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class Player 
    {
        public enum Chara
        {
            Bronto,
            Trex,
            Ptera,
            Cera,
            Unchosen
        }
        public Chara dinoChosen;

        private int _maxHealth;
        public int _currentHealth;
        private int _maxCharge;
        public int _currentCharge;

        public Vector2 _halfTextureSize;

        public int _laserPowerLevel;

        public float LaserCharge
        {
            get
            {
                return (float)_currentCharge / (float)_maxCharge;
            }
            set
            {
                
            }
        }
        public float MaxLaserCharge
        {
            get
            {
                return _maxCharge;
            }
            set
            {
                _maxCharge = (int)value;
            }
        }
        public int CurrentLazerCharge 
        { 
            get
            {
                return _currentCharge;
            }
            set
            {
                _currentCharge = value;
            }
        }

        public Texture2D Image { get; set; }
        public Texture2D TextureLeft { get; protected set; }
        public Texture2D TextureRight { get; protected set; }
        public Texture2D TextureShield { get; protected set; }


        Vector2 position;
        Vector2 movement;


        public int Points { get; set; }
        public float Health
        {
            get
            {
               return (float)_currentHealth / (float)_maxHealth;
            }
            set
            {
                
            }
        }
        public float MaxHealth 
        { 
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = (int)value;
            }
        }
      
        readonly Random _rnd = new Random();

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }
        public Vector2 Movement
        {
            get
            {
                return movement;
            }
            set
            {
                movement = value;
            }
        }

        // Vector2 _halfShieldSize;

        private int _healthUpgradeCost = 100;
        public int HealthUpgradeCost
        {
            get
            {
                return _healthUpgradeCost;
            }
            set
            {
                _healthUpgradeCost = (int)(_healthUpgradeCost * 1.25);
            }
        }

        private int _chargeUpgradeCost = 100;
        public int ChargeUpgradeCost
        {
            get
            {
                return _chargeUpgradeCost;
            }
            set
            {
                _chargeUpgradeCost = (int)(_chargeUpgradeCost * 1.25);
            }
        }

        private int _powerUpgradeCost = 300;
        public int PowerUpgradeCost
        {
            get
            {
                return _powerUpgradeCost;
            }
            set
            {
                if (_laserPowerLevel == maxPowerLevelPossible)
                {
                    _powerUpgradeCost = 0;
                }
                else
                {
                    _powerUpgradeCost = _powerUpgradeCost * 2;
                }
            }
        }

        private int maxPowerLevelPossible = 5;

        public int LasersPowerLevel
        {
            get { return _laserPowerLevel; }
            set
            {
                if (_laserPowerLevel < maxPowerLevelPossible)
                    _laserPowerLevel = _laserPowerLevel + 1;
            }
        }

        public ChosenControl ControlScheme { get; set; }
        public PlayerIndex PlayerIndex { get; private set; }


        public Player(ContentManager content, GraphicsDeviceManager graphics, Chara dinoChosen, int index)
        {
            this.dinoChosen = dinoChosen;

            Image = content.Load<Texture2D>("Player/" + dinoChosen.ToString());

            _halfTextureSize = new Vector2(Image.Width / 2, Image.Height / 2);

            ControlScheme = ChosenControl.Gamepad;
            if (index == 1)
            {
                PlayerIndex = PlayerIndex.One;
            }
            else
            {
                PlayerIndex = PlayerIndex.Two;
            }

            Position = Vector2.Zero;
            Movement = Vector2.Zero;

            _laserPowerLevel = 1;// 4; change back to 4 for debug lasers
            Points = 0;
            if (dinoChosen == Chara.Bronto)
            {
                _maxHealth = 100;
                _maxCharge = 100;
            }
            else if (dinoChosen == Chara.Trex)
            {
                _maxHealth = 125;
                _maxCharge = 125;
            }
            else if (dinoChosen == Chara.Ptera)
            {
                _maxHealth = 75;
                _maxCharge = 150;
            }
            else if (dinoChosen == Chara.Cera)
            {
                _maxHealth = 150;
                _maxCharge = 75;
            }

            _currentHealth = _maxHealth;
            CurrentLazerCharge = _maxCharge;
        }

        public void Replenish()
        {
            _currentHealth = _maxHealth;
            CurrentLazerCharge = _maxCharge;
        }
    }
}
