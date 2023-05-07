using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileMap
{
    public class Tiles
    {
        public Texture2D texture
        {
            get;
            protected set;
        }
        public Rectangle rectangle
        {
            get;
            protected set;
        }
        public int id
        {
            get;
            protected set;
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

    public class CollisionTiles : Tiles
    {
        public CollisionTiles(int i, Rectangle newRectangle)
        {
            //for example: load Tile1, Tile2, and so on
            //id serves as an identifier to destinguish between dmg- and impassable-tiles;
            texture = Content.Load<Texture2D>("Tile" + i);
            this.rectangle = newRectangle;
            id = i;
        }
    }
}
