using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace TES_TriangleEscapeSimulator
{
    class TmxMapLoader
    {
        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }
        public TmxMap tmxMap
        {
            get;
            private set;
        }
        public Vector2 playerStartPosition
        {
            get;
            private set;
        }
        public List<Tower> towers
        {
            get;
            private set;
        }
        public List<MovingObject> movingObjects
        {
            get;
            private set;
        }
        public List<SwitchandExit> swExits
        {
            get;
            private set;
        }
        public int[,] mapTiles
        {
            get;
            private set;
        }

        public TmxMapLoader(TmxMap tmxMap)
        {
            this.tmxMap = tmxMap;
            playerStartPosition = Vector2.Zero;
            towers = new List<Tower>();
            movingObjects = new List<MovingObject>();
            swExits = new List<SwitchandExit>();
            mapTiles = new int[tmxMap.Height, tmxMap.Width];
        }

        public void LoadMap()
        {
            mapTiles = new int[tmxMap.Height, tmxMap.Width];
            //Setting up the grid;
            int x = 0, y = 0;
            foreach (TmxLayerTile tmxtile in tmxMap.Layers[0].Tiles)
            {
                //if x is smaler than width -> add tiles, reset if otherwise;
                mapTiles[x, y] = tmxtile.Gid;
                y++;
                if (y == tmxMap.Width)
                {
                    x++;
                    y = 0;
                }
            }

            //Setting up the objects;
            foreach (TmxObject obj in tmxMap.ObjectGroups["Objektebene 1"].Objects)
            {
                if (obj.Name.Contains("Switch") || obj.Name.Contains("Exit") || obj.Name.Contains("Port"))
                {
                    string item = obj.Name.Substring(0, obj.Name.Length - 1);
                    string id = obj.Name.Substring(obj.Name.Length - 1, 1);

                    switch(item)
                    {
                        case "Switch":
                            swExits.Add(new SwitchandExit(new Vector2((float)obj.X, (float)obj.Y), Vector2.Zero, Vector2.Zero));
                            break;

                        case "Exit":
                            swExits[int.Parse(id)].exitPosition = new Vector2((float)obj.X, (float)obj.Y);
                            break;

                        case "Port":
                            swExits[int.Parse(id)].postEntryPosition = new Vector2((float)obj.X, (float)obj.Y);
                            break;
                    }
                }
                else
                {
                    switch (obj.Name)
                    {
                        case "Player":
                            playerStartPosition = new Vector2((float)obj.X, (float)obj.Y);
                            break;

                        case "Tower":
                            towers.Add(new Tower(Content.Load<Texture2D>("tower"), new Vector2((float)obj.X + 32, (float)obj.Y + 32)));
                            break;

                        case "Spike1":
                            movingObjects.Add(new MovingObject(Content.Load<Texture2D>("spikequad"), new Vector2((float)obj.X, (float)obj.Y), 5, 0));
                            break;

                        case "Spike2":
                            movingObjects.Add(new MovingObject(Content.Load<Texture2D>("spikequad_2"), new Vector2((float)obj.X, (float)obj.Y), 5, 0));
                            break;

                        case "Spike3":
                            movingObjects.Add(new MovingObject(Content.Load<Texture2D>("spikequad_3"), new Vector2((float)obj.X, (float)obj.Y), 0, 2));
                            break;
                    }
                }
            } //end foreach;
        } //end loadmap;
    } //end class;
}
