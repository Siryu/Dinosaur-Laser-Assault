using DinosaurLazers.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class StartLevel
    {
        public bool IsDone { get; set; }

        private GraphicsDeviceManager gdm;
        public int LevelCount { get; set; }

        private float backRectOpacity;
        private int rectOpacityDelay = 255;

        private float textOpacity;
        private int textOpacityDelay;

        private bool levelTextDone;
        private bool beginDone;
        private bool BGMusicPlaying = false;

        private int displayDelay;
        private int repetitions;

        public Level level { get; set; }

        private SpriteFont font;

        private Texture2D opaqueRect;

        private Color backColor;

        public StartLevel(GraphicsDeviceManager gdm, ContentManager cm, Level currentLevel)
        {
            this.gdm = gdm;
            opaqueRect = new Texture2D(gdm.GraphicsDevice, 1920, 680);

            font = cm.Load<SpriteFont>("Fonts/DefaultFont");
            this.level = currentLevel;

            backRectOpacity = 180/255f;
            backColor.A = 180;
            backColor.R = 40;
            backColor.G = 40;
            backColor.B = 40;
            opaqueRect.SetData(ColorPicker.setTexture(opaqueRect.Width, opaqueRect.Height, Color.Black));
        }

        public void Update(GameTime gameTime)
        {
            //wait 3 seconds
            if (!BGMusicPlaying)
            {
                MediaPlayer.Volume = 1f;
                level.BGMusic.Play();
                BGMusicPlaying = true;
            }
            if (!beginDone)
            {
                if (displayDelay < 60)
                {
                    displayDelay++;
                }
                else if (displayDelay >= 120)
                {
                    if (textOpacity > 0) //start to fade out
                    {
                        textOpacityDelay -= 15;
                        textOpacity = textOpacityDelay / 255f;
                    }
                    else if (textOpacity <= 0)
                    {
                        textOpacity = 0;
                        displayDelay = 0; //reset timer;
                        if (!beginDone && repetitions == 1)
                        {
                            beginDone = true;
                        }
                        if (!levelTextDone && repetitions == 0)
                        {
                            levelTextDone = true;
                            repetitions++;
                        }

                    }
                }
                else if (displayDelay >= 60) //start to fade in
                {
                    if (textOpacity < 1)
                    {
                        textOpacityDelay += 15;
                        textOpacity = textOpacityDelay / 255f;
                    }
                    else if (textOpacity >= 1)
                    {
                        displayDelay++; //wait one second

                    }

                }
            }
            else if (beginDone)
            {
                if (backRectOpacity > 0)
                {
                    rectOpacityDelay -= 15;
                    backRectOpacity = rectOpacityDelay / 255f;
                }
                else if (backRectOpacity <= 0)
                {
                    backRectOpacity = 0;
                    IsDone = true;
                }
            }
            
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(opaqueRect, new Rectangle(0, 200, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight - 400), backColor * backRectOpacity);
            sb.End();
            sb.Begin();
            if (!levelTextDone)
            {
                sb.DrawString(font, "LEVEL " + LevelCount, new Vector2(400, 400), Color.White * textOpacity);
            }
            else
            {
                sb.DrawString(font, "BEGIN", new Vector2(450, 400), Color.White * textOpacity);
            }
            sb.End();
            
        }


    }
}
