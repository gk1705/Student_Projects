using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileMap;

namespace TES_TriangleEscapeSimulator
{
    public class MovingObject
    {
        Texture2D texture;
        Vector2 position;
        Vector2 origin;
        public Rectangle rectangle
        {
            get;
            private set;
        }
        int hspeed;
        int vspeed;
        public Color[] textureData
        {
            get;
            private set;
        }

        public MovingObject(Texture2D texture, Vector2 position, int hspeed, int vspeed)
        {
            this.texture = texture;
            this.position = position;
            this.hspeed = hspeed;
            this.vspeed = vspeed;
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        public void Update(Map map)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            bool reverse = false;
            foreach(CollisionTiles tile in map.collisionTiles)
            {
                //reverse moving object if collision with tiles;
                if ((rectangle.IsTouchingTop(tile.rectangle, vspeed) ||
                    rectangle.IsTouchingBottom(tile.rectangle, vspeed) ||
                    rectangle.IsTouchingLeft(tile.rectangle, hspeed) ||
                    rectangle.IsTouchingRight(tile.rectangle, hspeed)) &&
                    tile.id == 1)
                {
                    reverse = true;
                }
            }
            if (reverse == true)
            {
                    hspeed = -hspeed;
                    vspeed = -vspeed;
            }
            position += new Vector2(hspeed, vspeed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
