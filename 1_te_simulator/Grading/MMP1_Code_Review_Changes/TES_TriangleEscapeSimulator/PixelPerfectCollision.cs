using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TES_TriangleEscapeSimulator
{
    static class PixelPerfectCollision
    {
        public static bool IntersectsPixel(Rectangle rect1, Rectangle rect2, Color[] data1, Color[] data2)
        {
            if (rect1.Intersects(rect2))
            {
                //get the range for our pixel test;
                int top = Math.Max(rect1.Top, rect2.Top);
                int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
                int left = Math.Max(rect1.Left, rect2.Left);
                int right = Math.Min(rect1.Right, rect2.Right);

                //execute the pixel test;
                for (int y = top; y < bottom; y++)
                {
                    for (int x = left; x < right; x++)
                    {
                        Color color1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                        Color color2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                        //Images collide;
                        if (color1.A != 0 && color2.A != 0) return true;
                    }
                }
            }

            return false;
        }
    }
}
