using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileMap
{
    static class RectangleHelper
    {
        public static bool IsTouchingLeft(this Rectangle r1, Rectangle r2, float velocityX)
        {
            return r1.Right + velocityX > r2.Left &&
                   r1.Left < r2.Left &&
                   r1.Bottom > r2.Top &&
                   r1.Top < r2.Bottom;
        }

        public static bool IsTouchingRight(this Rectangle r1, Rectangle r2, float velocityX)
        {
            return r1.Left + velocityX < r2.Right &&
                   r1.Right > r2.Right &&
                   r1.Bottom > r2.Top &&
                   r1.Top < r2.Bottom;
        }

        public static bool IsTouchingTop(this Rectangle r1, Rectangle r2, float velocityY)
        {
            return r1.Bottom + velocityY > r2.Top &&
                   r1.Top < r2.Top &&
                   r1.Right > r2.Left &&
                   r1.Left < r2.Right;
        }

        public static bool IsTouchingBottom(this Rectangle r1, Rectangle r2, float velocityY)
        {
            return r1.Top + velocityY < r2.Bottom &&
                   r1.Bottom > r2.Bottom &&
                   r1.Right > r2.Left &&
                   r1.Left < r2.Right;
        }
    }
}
