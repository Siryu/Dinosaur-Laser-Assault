using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class FadeScreen
    {
        public int Opacity { get; set; }
        private Texture2D screen;
        private Color c;
        private float updatedOpacity;

        public FadeScreen(GraphicsDeviceManager gdm)
        {
            screen = new Texture2D(gdm.GraphicsDevice, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            Opacity = 0;
            updatedOpacity = 0;
            c.A = 255;
            c.R = 20;
            c.G = 20;
            c.B = 20;
            screen.SetData(setTexture(gdm.PreferredBackBufferHeight, gdm.PreferredBackBufferWidth));
        }

        private Color[] setTexture(int height, int width)
        {
            Color[] data = new Color[height * width];
            for (int i = 0; i < data.Length; ++i) data[i] = c;
            return data;
        }

        public void Fade(GameTime gameTime, int opacityChange)
        {
            Opacity += opacityChange;
            updatedOpacity = Opacity / 255f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(screen, new Rectangle(0, 0, screen.Width, screen.Height), c * updatedOpacity);
            sb.End();
        }
    }
}
