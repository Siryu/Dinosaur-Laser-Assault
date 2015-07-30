using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinosaurLazers.Models
{
    public class TextSprite : Sprite
    {
        public float MsLifeLeft { get; set; }
        public float OriginalLifeInMiliseconds { get; set; }
        public float BeginScale { get; set; }
        public float EndScale { get; set; }

        float _scaleGrowthPerMilisecond;

        internal TextSprite(Game game, Texture2D texture, Vector2 position, SpriteBatch spritebatch, Color color, float beginScale = 1, float endScale = 1, float msLifeLength = 400)
            : base(game, texture, position, spritebatch, new Enemies.EnemyType(game.Content, Enemies.EnemyType.Etype.Simple))
        {
            OriginalLifeInMiliseconds = msLifeLength;
            BeginScale = beginScale;
            EndScale = endScale;
            Color = color;
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            MsLifeLeft -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            base.Update(gameTime);
            Scale += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _scaleGrowthPerMilisecond;
            Position -= Vector2.One * 70 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (MsLifeLeft <= 0)
            {
                this.Enabled = false;
                this.Visible = false;
            }
        }

        public void Reset()
        {
            MsLifeLeft = OriginalLifeInMiliseconds;
            Scale = BeginScale;
            _scaleGrowthPerMilisecond = (EndScale - BeginScale) / MsLifeLeft;
        }

    }
}
