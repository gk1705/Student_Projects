using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TileMap
{
    public class Map
    {
        public List<CollisionTiles> collisionTiles = new List<CollisionTiles>();

        public int width;
        public int height;
        
        //64 x 64 for the tiles;
        //2d Array determines which textures are loaded;
        public void Generate(int[,] map, int size)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                for(int y = 0; y < map.GetLength(0); y++)
                {
                    //determine what texture we're going to be loading; row then col to pick the tile;
                    int number = map[y, x];

                    if (number > 0)
                    {
                        //0 no tile, else tile + i;
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size)));
                    }

                    //size of the actual map;
                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(CollisionTiles tile in collisionTiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
