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
    public class Background
    {

        private float _Yoffset;
        private Level.LevelSelected _level;
        private Texture2D _backGround;

        public Background(Level.LevelSelected level, GraphicsDevice gd, SpriteBatch sb, ContentManager cm)
        {
            this._level = level;
            _backGround = CreateBackground(gd, sb, cm);
        }

        public void Update(GameTime gameTime, float scrollSpeed)
        {
            _Yoffset += scrollSpeed;

            if (_Yoffset >= -256)
            {
                _Yoffset -= 256; 
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
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    sb.Draw(cm.Load<Texture2D>("Backgrounds/" + _level.ToString()), new Vector2(x * 400, y * 256), Color.White);
                }
            }

            sb.End();
            //reset the GraphicsDevice to draw on the backbuffer (directly to the backbuffer)
            gd.SetRenderTarget(null);
            

            return (Texture2D)target;
        }
    }
}
