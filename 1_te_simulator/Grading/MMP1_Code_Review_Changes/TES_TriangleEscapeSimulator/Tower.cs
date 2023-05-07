using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace TES_TriangleEscapeSimulator
{
    struct Point
    {
        public int x
        {
            get;
            private set;
        }
        public int y
        {
            get;
            private set;
        }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Tower
    {
        Texture2D texture;
        Vector2 position;
        Vector2 origin;
        Rectangle rectangle;
        float shootTimer;
        float rotation;

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public List<Bullets> bullets
        {
            get;
            private set;
        }

        public Tower(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            bullets = new List<Bullets>();
        }

        public void Update(Player player, Map map, float elapsedTime)
        {
            rectangle = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2,
                texture.Width, texture.Height);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);

            if (Vector2.Distance(player.playerPosition, position) < 400)
            { 
                //Check if tower sees player;
                if (!CheckViewObstruction(player, map))
                {
                    shootTimer += elapsedTime;
                    //Makes the tower face the player;
                    rotation = (float)Math.Atan2(player.playerPosition.Y - position.Y, player.playerPosition.X - position.X);
                }
            }
            //Condition for shooting
            if (Vector2.Distance(player.playerPosition, position) < 300 && shootTimer > 2)
            {
                Shoot(ReturnNormVec(player.playerPosition - position));
                shootTimer = 0;
            }
            UpdateBullets(map);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }

        public void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach(Bullets bullet in bullets)
            {
                spriteBatch.Draw(bullet.texture, bullet.position, null, Color.White, 0f, bullet.origin, 1f, SpriteEffects.None, 0f);
            }
        }

        //distvec = direction the bullet flies;
        private void Shoot(Vector2 distvec)
        {
            Bullets newBullet = new Bullets(Content.Load<Texture2D>("bullet_real"));
            newBullet.velocity = distvec * 4f;
            //Spawn bullet inside of tower;
            newBullet.position = position - new Vector2(this.texture.Width/8, this.texture.Height/8) + newBullet.velocity;
            newBullet.isVisible = true;

            //Two bullets foreach tower as a limit;
            if (bullets.Count < 2)
            {
                bullets.Add(newBullet);
            }
        }

        private void UpdateBullets(Map map)
        {
            foreach (Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                //If bullet is out of range or if it hit a tile: remove;
                foreach(CollisionTiles tiles in map.collisionTiles)
                {
                    if (tiles.id == 1 && bullet.bulletRectangle.Intersects(tiles.rectangle)) bullet.isVisible = false;
                }
                if (Vector2.Distance(bullet.position, position) > 300)
                {
                    bullet.isVisible = false;
                }
                bullet.Update();
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        private Vector2 ReturnNormVec(Vector2 vec)
        {
            float length = (float) Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
            return new Vector2(vec.X / length, vec.Y / length);
        }

        private bool CheckViewObstruction(Player player, Map map)
        {
            //Check if theres an intersection in the tower's field of view;
            bool intersection = false;
            //those player and tower wont change mid frame;
            Point p1 = new Point((int)player.playerPosition.X, (int)player.playerPosition.Y);
            Point p2 = new Point((int)position.X, (int)position.Y);

            foreach (CollisionTiles t in map.collisionTiles)
            {
                //Get all rectangle points;
                Point r1 = new Point(t.rectangle.X, t.rectangle.Y);
                Point r2 = new Point(t.rectangle.X + t.texture.Width, t.rectangle.Y);
                Point r3 = new Point(t.rectangle.X, t.rectangle.Y + t.texture.Height);
                Point r4 = new Point(t.rectangle.X + texture.Height, t.rectangle.Y + t.texture.Width);

                //now we test the intersection against all lines of the rectangle;
                if (Intersection(p1, p2, r1, r2) ||
                    Intersection(p1, p2, r1, r3) ||
                    Intersection(p1, p2, r2, r4) ||
                    Intersection(p1, p2, r3, r4))
                {
                    intersection = true;
                    break;
                }
                intersection = false;
            }

            return intersection;
        }

        //Check for obstruction in field of view;
        //The idea for this was taken from: http://www.dcs.gla.ac.uk/~pat/52233/slides/Geometry1x1.pdf
        //Firstly, we check if the p2 lies on segment p1 and p3;
        private bool IsOnLineSegment(Point p1, Point p2, Point p3)
        {
            if (p2.x <= Math.Max(p1.x, p3.x) &&
                p2.x >= Math.Min(p1.x, p3.x) &&
                p2.y <= Math.Max(p1.y, p3.y) &&
                p2.y >= Math.Min(p1.y, p3.y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //check if segment stands clockwise, counterclockwise or collinear to point;
        //the sign of the evaluation determines the case;
        //we basically test two gradient triangles against each other;
        //if positive: clock;
        //if negative: cclock;
        //if zero: collinear;
        //hypotenuse1: (p2.y - p1.y) / (p2.x - p1.x);
        //hypotenuse2: (p3.y - p2.y) / (p3.x - p2.x);
        //equate and rearrange;
        //(p2.y - p1.y) * (p3.x - p2.x) - (p3.y - p2.y) * (p2.x - p1.x);
        
        private int GetOrientation(Point p1, Point p2, Point p3)
        {
            int orientation = (p2.y - p1.y) * (p3.x - p2.x) - (p3.y - p2.y) * (p2.x - p1.x);
            //three cases: 1, 2, 3;
            if (orientation > 0) return 1;
            else if (orientation < 0) return 2;
            else return 3;
        }

        private bool Intersection(Point p1, Point p2, Point p3, Point p4)
        {
            //Segment 1:
            int orientation1 = GetOrientation(p1, p2, p3);
            int orientation2 = GetOrientation(p1, p2, p4);
            //Segment 1:
            int orientation3 = GetOrientation(p3, p4, p1);
            int orientation4 = GetOrientation(p3, p4, p2);

            //four cases to handl;
            //normal intersection:
            if (orientation1 != orientation2 && orientation3 != orientation4)
            {
                return true;
            }
            //collinear cases:
            //segment 1 and 2 are collinear and p3 lies on segment 1;
            if (orientation1 == 3 && IsOnLineSegment(p1, p3, p2)) {
                return true;
            }
            //segment 1 and 2 are collinear and p4 lies on segment 1;
            if (orientation2 == 3 && IsOnLineSegment(p1, p4, p2))
            {
                return true;
            }
            //segment 1 and 2 are collinear and p1 lies on segment 2;
            if (orientation3 == 3 && IsOnLineSegment(p3, p1, p4)) {
                return true;
            }
            //segment 1 and 2 are collinear and p2 lies on segment 2;
            if (orientation4 == 3 && IsOnLineSegment(p3, p2, p4))
            {
                return true;
            }
            //no intersection;
            return false;
        }
    } //Close tower class;
}