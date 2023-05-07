using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TES_TriangleEscapeSimulator
{
    public class Node
    {
        public bool walkable
        {
            get;
            set;
        }

        public int gridX
        {
            get;
            set;
        }

        public int gridY
        {
            get;
            set;
        }

        public int gCost
        {
            get;
            set;
        }

        public int hCost
        {
            get;
            set;
        }

        public Node parent
        {
            get;
            set;
        }

        public Vector2 position
        {
            get;
            set;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public Node(bool walkable, int gridX, int gridY, Vector2 position)
        {
            this.walkable = walkable;
            this.gridX = gridX;
            this.gridY = gridY;
            this.position = position;
        }
    }
}
