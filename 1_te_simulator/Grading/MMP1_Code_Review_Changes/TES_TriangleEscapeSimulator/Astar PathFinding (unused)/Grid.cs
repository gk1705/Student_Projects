using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileMap;

namespace TES_TriangleEscapeSimulator
{
    class Grid
    {
        public Node[,] grid
        {
            get;
            set;
        }

        public Grid(int[,] map)
        {
            grid = new Node[map.GetLength(0), map.GetLength(1)];
            for(int y = 0; y < map.GetLength(0); y++)
            {
                for(int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == 0)
                    {
                        grid[y, x] = new Node(true, x, y, new Vector2(x * 64, y * 64));
                    }
                    else
                    {
                        grid[y, x] = new Node(false, x, y, new Vector2(x * 64, y * 64));
                    }
                }
            }
        }

        public Node GetNodeFromPosition(Vector2 position)
        {
            int x = (int) position.X / 64;
            int y = (int) position.Y / 64;

            return grid[y, x];
        }

        public List<Node> GetNeighbourNodes(Node node)
        {
            //doesnt return diagonal ones;
            List<Node> neighbours = new List<Node>();
            if (node.gridY - 1 >= 0)
                neighbours.Add(grid[node.gridY - 1, node.gridX]);
            if (node.gridY + 1 <= grid.GetLength(0) - 1)
                neighbours.Add(grid[node.gridY + 1, node.gridX]);

            if (node.gridX - 1 >= 0)
                neighbours.Add(grid[node.gridY, node.gridX - 1]);
            if (node.gridX + 1 <= grid.GetLength(1) - 1)
                neighbours.Add(grid[node.gridY, node.gridX + 1]);

            return neighbours;
        }
    }
}
