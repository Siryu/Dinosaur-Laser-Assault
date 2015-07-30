using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class Clouds
    {
        private float _Yoffset;
        private float _Xoffset;
        private Level.LevelSelected _level;
        private Texture2D cloud;
        private Random rand;

        public Clouds(Level.LevelSelected level, GraphicsDevice gd, SpriteBatch sb, ContentManager cm, Random rand)
        {
            this._level = level;
            cloud = cm.Load<Texture2D>("Backgrounds/Clouds");
            this.rand = rand;
            this._Yoffset = rand.Next(-1000, -600);
            this._Xoffset = rand.Next(2000, 3000);
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, float scrollSpeed)
        {
            _Yoffset += scrollSpeed;
            _Xoffset--;

            if (_Yoffset >= graphics.PreferredBackBufferHeight + 100)
            {
                _Yoffset -= graphics.PreferredBackBufferHeight + rand.Next(100 + cloud.Height, 1000); 
            }
            if (_Xoffset <= 0 - cloud.Width)
            {
                _Xoffset += graphics.PreferredBackBufferWidth + cloud.Width + rand.Next(100, 1000);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cloud, new Vector2(_Xoffset, _Yoffset), Color.Tan);
        }
    }
}
