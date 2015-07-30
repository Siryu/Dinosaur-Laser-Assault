using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class Border
    {
        private float _Yoffset;
        private Level.LevelSelected _level;
        private Texture2D _backGround;

        public Border(Level.LevelSelected level, GraphicsDevice gd, SpriteBatch sb, ContentManager cm)
        {
            this._level = level;
            _backGround = CreateBackground(gd, sb, cm);
        }

        public void Update(GameTime gameTime, float scrollSpeed)
        {
            _Yoffset += scrollSpeed;

            if (_Yoffset >= -512)
            {
                _Yoffset -= 512; 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(_backGround, Vector2.UnitY * _Yoffset, Color.White);
        }

        private Texture2D CreateBackground(GraphicsDevice gd, SpriteBatch sb, ContentManager cm)
        {
            RenderTarget2D target = new RenderTarget2D(gd, 2048, 2048);
            //tell the GraphicsDevice we want to render to the gamesMenu rendertarget (an in-memory buffer)
            gd.SetRenderTarget(target);

            //clear the background
            gd.Clear(Color.Transparent);

            //begin drawing
            sb.Begin();
     
            for (int y = 0; y < 4; y++)
            {
                sb.Draw(cm.Load<Texture2D>("Backgrounds/TreeBorder"), new Vector2(0 - 350, y * 512), Color.White);
                sb.Draw(cm.Load<Texture2D>("Backgrounds/TreeBorder"), new Vector2(1920 - 250, y * 512), Color.White);
            }
     
            sb.End();
            //reset the GraphicsDevice to draw on the backbuffer (directly to the backbuffer)
            gd.SetRenderTarget(null);
            

            return (Texture2D)target;
        }
    }
}
