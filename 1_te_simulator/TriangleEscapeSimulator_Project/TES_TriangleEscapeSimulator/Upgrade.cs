using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TES_TriangleEscapeSimulator
{
    class Upgrade
    {
        //Not yet implemented;
        Texture2D texture;
        Vector2 position;
        Vector2 origin;
        Rectangle rectangle;
        bool depleted;

        public Upgrade(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            depleted = false;
        }

        public void Update(Player player)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (rectangle.Intersects(player.playerRectangle))
            {
                depleted = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (depleted != true)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
