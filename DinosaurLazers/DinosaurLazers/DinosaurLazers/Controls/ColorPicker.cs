using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Controls
{
    public class ColorPicker
    {
        /// <summary>
        /// Takes in the height and width of a texture to assign a color
        /// array of that size to it, with each pixel in the array being
        /// the color passed in as an argument.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Color[] setTexture(int width, int height, Color c)
        {
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = c;
            return data;
        }
    }
}
