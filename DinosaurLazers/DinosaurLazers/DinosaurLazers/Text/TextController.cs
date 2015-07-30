using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinosaurLazers.Models
{
    public class TextController : DrawableGameComponent
    {
        Dictionary<string, Texture2D> _textTextures = new Dictionary<string, Texture2D>();
        List<TextSprite> _activeSprites = new List<TextSprite>();
        List<TextSprite> _inactiveSprites = new List<TextSprite>();
        private Color _textColor;
        public static SpriteFont Font { get; set; }
        private SpriteBatch _spriteBatch;
        GraphicsDeviceManager _graphics;
        static Random _rnd = new Random();


        public TextController(Game game, GraphicsDeviceManager graphics, SpriteBatch spritebatch, Color textColor, SpriteFont font, params string[] stringsToPreload)
            : base(game)
        {
            _spriteBatch = spritebatch;
            _graphics = graphics;
            _textColor = textColor;
            Font = font;
            Initialize();
            foreach (var stringToLoad in stringsToPreload)
            {
                GetTexture(stringToLoad);
            }
        }


        public void AddTextSprite(Vector2 position, string text)
        {
            TextSprite sprite = GetSprite(position, text, _textColor);
            _activeSprites.Add(sprite);
        }

        public void AddTextSprite(Vector2 position, string text, Color color, float beginScale = 1, float endScale = 1, float msLifeLength = 400)
        {
            TextSprite sprite = GetSprite(position, text, color, beginScale, endScale, msLifeLength);
            _activeSprites.Add(sprite);
        }


        private TextSprite GetSprite(Vector2 position, string text, Color color, float beginScale = 1, float endScale = 1, float msLifeLength = 400)
        {

            TextSprite sprite = null;
            if (_inactiveSprites.Count > 0)
            {
                sprite = _inactiveSprites[0];
                _inactiveSprites.RemoveAt(0);
                sprite.Texture = GetTexture(text);
                sprite.Position = position;
                sprite.Enabled = true;
                sprite.Opacity = 1;
                sprite.Color = color;
                sprite.BeginScale = beginScale;
                sprite.EndScale = endScale;
                sprite.OriginalLifeInMiliseconds = msLifeLength;
                sprite.Reset();
            }
            else
            {
                sprite = new TextSprite(Game, GetTexture(text), position, _spriteBatch, color, beginScale, endScale, msLifeLength);
            }
            return sprite;

        }


        public Texture2D GetTexture(string text)
        {
            if (!_textTextures.ContainsKey(text))
            {
                _textTextures.Add(text, CreateTexture(text));
            }
            return _textTextures[text];

        }

        private Texture2D CreateTexture(string text)
        {
            Vector2 textureSize = Font.MeasureString(text);
            RenderTarget2D target = new RenderTarget2D(Game.GraphicsDevice, (int)textureSize.X, (int)textureSize.Y);
            //tell the GraphicsDevice we want to render to the gamesMenu rendertarget (an in-memory buffer)
            GraphicsDevice.SetRenderTarget(target);

            //clear the background
            GraphicsDevice.Clear(Color.Transparent);
            Vector2 drawPosition = (new Vector2(textureSize.X, textureSize.Y) - textureSize) / 2;
            //begin drawing
            _spriteBatch.Begin();
            _spriteBatch.DrawString(Font, text, drawPosition, Color.White);
            _spriteBatch.End();
            //reset the GraphicsDevice to draw on the backbuffer (directly to the backbuffer)
            GraphicsDevice.SetRenderTarget(null);

            return (Texture2D)target;

        }


        private Point GetSmallestTextureSizePowerOfTwo(string text)
        {
            Vector2 size = Font.MeasureString(text);
            int power = 8;
            while (Math.Pow(2, power) < Math.Max(size.X, size.Y))
            {
                power++;
            }
            return new Point((int)Math.Pow(2, power), (int)Math.Pow(2, power));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = _activeSprites.Count - 1; i >= 0; i--)
            {
                TextSprite doneSprite = _activeSprites[i];
                if (!doneSprite.Enabled)
                {
                    _activeSprites.RemoveAt(i);
                    _inactiveSprites.Add(doneSprite);
                }
            }

            foreach (var item in _activeSprites)
            {
                item.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (var text in _activeSprites)
            {
                text.Draw(gameTime);
            }
        }

        public void Clear()
        {
            _activeSprites.Clear();
        }

    }
}
