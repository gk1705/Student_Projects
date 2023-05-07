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

        public List<Bullets> bullets = new List<Bullets>();

        public Tower(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Update(Player player, Map map, float elapsedTime)
        {
            rectangle = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2,
                texture.Width, texture.Height);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            if (Vector2.Distance(player.playerPosition, position) < 400)
            {
                //Makes the tower face the player;
                rotation = (float)Math.Atan2(player.playerPosition.Y - position.Y, player.playerPosition.X - position.X);
            }
            shootTimer += elapsedTime;
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
        public void Shoot(Vector2 distvec)
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

        public void UpdateBullets(Map map)
        {
            foreach (Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                //If bullet is out of range or if it hit a tile: remove;
                foreach(CollisionTiles tiles in map.collisionTiles)
                {
                    if (bullet.bulletRectangle.Intersects(tiles.rectangle)) bullet.isVisible = false;
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

        public Vector2 ReturnNormVec(Vector2 vec)
        {
            float length = (float) Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
            return new Vector2(vec.X / length, vec.Y / length);
        }

        /*public float ReturnAngle(Vector2 vec1, Vector2 vec2)
        {
            return (float) Math.Acos((Vector2.Dot(vec1, vec2) / Vector2.Dot(ReturnNormVec(vec1), ReturnNormVec(vec2))));
        }*/
    }
}