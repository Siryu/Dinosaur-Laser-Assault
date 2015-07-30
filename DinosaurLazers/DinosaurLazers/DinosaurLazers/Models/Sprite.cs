using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DinosaurLazers.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinosaurLazers.Models
{
    public class Sprite : DrawableGameComponent
    {
        public bool IsInvincible { get { return InvincibleTimeLeft > 0; } }
        public bool IsVisible { get { return InvincibleTimeLeft < 1000; } }
        public float InvincibleTimeLeft { get; set; }
        public float ShotRotation { get; set; }

        public EnemyType enemyType { get; set; }
        public ObjectType objectType { get; set; }
        public bool IsInMarsh { get; set; }

        public SpriteBatch SpriteBatch { get; protected set; }
        protected Vector2 _textureSize, _halfTextureSize;
        public Color Color { get; set; }
        protected Texture2D _texture;

        public int bulletCount = 0;
        public bool OKFire = false;
        public bool IsHit = false;

        public string Name = "";

        private int _maxHealth;
        public int _currentHealth;

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

        public Texture2D Texture
        {
            get { return _texture; }
            set {
                _texture = value;
                _textureSize = new Vector2(_texture.Width, _texture.Height);
                _halfTextureSize = _textureSize / 2;
                float heightWidthAverage = Math.Abs(_texture.Width - _texture.Height) / 2 + Math.Min(_texture.Height, _texture.Width);
                BoundingSphereRadius = heightWidthAverage *.8f;
                
            }
        }

        public Vector2 Position { get; set; }
        public Vector2 Movement { get; set; }
        public float Opacity { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float RotationPerUpdate { get; set; }

        public int width, height;

        public float BoundingSphereRadius { get; set; }

        public Sprite(Game game, Texture2D texture, Vector2 position, SpriteBatch batch, float scale = 1, float opacity = 1, float rotation = 0, float rotationPerUpdate = 0) : base(game)
        {
            Texture = texture;
            Position = position;
            SpriteBatch = batch;
            Scale = scale;
            Opacity = opacity;
            Rotation = rotation;
            RotationPerUpdate = rotationPerUpdate;
            Color = Color.White;
            enemyType = null;

            this.width = texture.Width;
            this.height = texture.Height;

            _currentHealth = _maxHealth;
        }


        public Sprite(Game game, Texture2D texture, Vector2 position, SpriteBatch batch, EnemyType type, float scale = 1, float opacity = 1, float rotation = 0, float rotationPerUpdate = 0) : base(game)
        {
            Texture = texture;
            Position = position;
            SpriteBatch = batch;
            Scale = scale;
            Opacity = opacity;
            Rotation = rotation;
            RotationPerUpdate = rotationPerUpdate;
            Color = Color.White;
            enemyType = type;

            this.width = texture.Width;
            this.height = texture.Height;

            _maxHealth = type.life;

            _currentHealth = _maxHealth;
        }

        public override void Draw(GameTime gameTime)
        {

            float opacity = 1;

            if (InvincibleTimeLeft > 1000)
            {
                opacity = 0.2f;
            }
            else if (InvincibleTimeLeft > 0)
            {
                opacity = ((int)(gameTime.TotalGameTime.Milliseconds / 100)) % 2 == 0 ? .5f : 1f;
            }


            //SpriteBatch.Draw(Texture, Position, Color.White * opacity);
            //base.Draw(gameTime);
            //Rectangle destRect = new Rectangle((int)(Position.X + .5f - _halfTextureSize.X), (int)(Position.Y + .5f - _halfTextureSize.Y), (int)Texture.Width, (int)Texture.Height);
            if (this.IsHit)
            {
                SpriteBatch.Draw(_texture, Position, null, Color.IndianRed * opacity, Rotation, _halfTextureSize, Scale, SpriteEffects.None, 0);
            }
            else
            {
                SpriteBatch.Draw(_texture, Position, null, Color * opacity, Rotation, _halfTextureSize, Scale, SpriteEffects.None, 0);
            }
        }
        


        public override void Update(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //Rotation += RotationPerUpdate * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            InvincibleTimeLeft -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, Sprite playerOneSprite, Sprite playerTwoSprite, Player one, Player two)
        {
            base.Update(gameTime);
            if (this.enemyType != null)
            {
                Movement = enemyType.Update(enemyType, Position, Movement, graphics, playerOneSprite, playerTwoSprite, one, two);
            }

            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            InvincibleTimeLeft -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
