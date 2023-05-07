using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileMap;

namespace TES_TriangleEscapeSimulator
{
    public class Player
    {
        public Texture2D playerTexture;
        public Rectangle playerRectangle;

        public Vector2 playerOrigin;
        public Vector2 playerPosition;
        public float rotation;
        public Color backGroundColor = Color.White;

        public int hp;
        //toDO: implement powerUp;
        bool powerUp;
        //used for pixel perfect collision
        public Color[] textureData;

        public Vector2 playerVelocity;
        const float tangentialVelocity = 7f;
        float friction = 0.2f;

        public Player(Vector2 playerPosition, Texture2D texture)
        {
            this.playerTexture = texture;
            this.playerPosition = playerPosition;
            powerUp = false;
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            hp = 1;
        }

        public void Update(Map map, Map dmgmap, List<Tower> towers, List<MovingObject> movingObjects)
        {
            //update player position
            backGroundColor = Color.White;
            playerRectangle = new Rectangle((int)playerPosition.X-playerTexture.Width/2, (int)playerPosition.Y-playerTexture.Height/2,
                playerTexture.Width, playerTexture.Height);
            playerPosition += playerVelocity;
            playerOrigin = new Vector2(playerRectangle.Width / 2, playerRectangle.Height / 2);

            //player controls
            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) rotation -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) rotation += 0.1f;

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                float pvX = (float)Math.Cos(Math.PI / 180 * -90 + rotation) * tangentialVelocity;
                //Collision check X
                foreach (CollisionTiles tiles in map.collisionTiles)
                {
                    if (playerRectangle.IsTouchingLeft(tiles.rectangle, pvX))  {
                        pvX = 0;
                        playerPosition.X = tiles.rectangle.Left - tiles.rectangle.Width/2;
                    }
                    else if (playerRectangle.IsTouchingRight(tiles.rectangle, pvX))
                    {
                        pvX = 0;
                        playerPosition.X = tiles.rectangle.Right + tiles.rectangle.Height/2;
                    }
                }
                playerVelocity.X = pvX;

                float pvY = (float)Math.Sin(Math.PI / 180 * -90 + rotation) * tangentialVelocity;
                //Collision check Y
                foreach (CollisionTiles tiles in map.collisionTiles)
                {
                    if (playerRectangle.IsTouchingTop(tiles.rectangle, pvY))
                    {
                        pvY = 0;
                        playerPosition.Y = tiles.rectangle.Top - tiles.rectangle.Height/2;
                    }
                    else if (playerRectangle.IsTouchingBottom(tiles.rectangle, pvY))
                    {
                        pvY = 0;
                        playerPosition.Y = tiles.rectangle.Bottom + tiles.rectangle.Height/2;
                    }
                }
                playerVelocity.Y = pvY;
            }
            else if (playerVelocity != Vector2.Zero)
            {
                //gradually slows down player if there's no imminent collision;
                Vector2 i = playerVelocity;
                playerVelocity = i -= friction * i;
                //Collision check Y
                foreach (CollisionTiles tiles in map.collisionTiles)
                {
                    if (playerRectangle.IsTouchingTop(tiles.rectangle, playerVelocity.Y)) {
                        playerVelocity.Y = 0;
                        playerPosition.Y = tiles.rectangle.Top - tiles.rectangle.Height / 2;
                    } else if (playerRectangle.IsTouchingBottom(tiles.rectangle, playerVelocity.Y))
                    {
                        playerVelocity.Y = 0;
                        playerPosition.Y = tiles.rectangle.Bottom + tiles.rectangle.Height / 2;
                    }
                }
                //Collision check X
                foreach (CollisionTiles tiles in map.collisionTiles)
                    if (playerRectangle.IsTouchingLeft(tiles.rectangle, playerVelocity.X)) {
                        playerVelocity.X = 0;
                        playerPosition.X = tiles.rectangle.Left - tiles.rectangle.Width / 2;
                    } else if (playerRectangle.IsTouchingRight(tiles.rectangle, playerVelocity.X)) {
                        playerVelocity.X = 0;
                        playerPosition.X = tiles.rectangle.Right + tiles.rectangle.Height / 2;
                    }
            }

            //check for collision when space is pressed; static camera
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (playerRectangle.X + playerVelocity.X <= Game1.camera.centre.X)
                {
                    playerPosition.X = Game1.camera.centre.X + playerTexture.Width/2;
                } //left
                if (playerRectangle.Y + playerVelocity.Y <= Game1.camera.centre.Y)
                {
                    playerPosition.Y = Game1.camera.centre.Y + playerTexture.Height/2;
                } //top
                if (playerRectangle.X + playerVelocity.X >= Game1.camera.centre.X + Game1.screenwidth / Game1.camera.scale - playerTexture.Width)
                {
                    playerPosition.X = Game1.camera.centre.X + Game1.screenwidth / Game1.camera.scale - playerTexture.Width/2; 
                } //right
                if (playerRectangle.Y + playerVelocity.Y >= Game1.camera.centre.Y + Game1.screenheight / Game1.camera.scale - playerTexture.Height)
                {
                    //playerRectangle.Y = screenheight - playerTexture.Height;
                    playerPosition.Y = Game1.camera.centre.Y + Game1.screenheight / Game1.camera.scale - playerTexture.Height/2;
                } //bottom
            }

            //braking
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) playerVelocity = Vector2.Zero;

            if (powerUp == true)
            {
                //toDO
            }

            //check collision with dmg-inducing elements

            foreach(CollisionTiles dmgtile in dmgmap.collisionTiles)
            {
                if(playerRectangle.Intersects(dmgtile.rectangle))
                {
                    hp -= 1;
                }
            }

            foreach(Tower t in towers)
            {
                foreach(Bullets b in t.bullets)
                {
                    if(playerRectangle.Intersects(b.bulletRectangle))
                    {
                        hp -= 1;
                    }
                }
            }

            foreach (MovingObject mo in movingObjects)
            {
                if (PixelPerfectCollision.IntersectsPixel(playerRectangle, mo.rectangle, mo.textureData, mo.textureData))
                {
                    hp -= 1;
                }
            }
        } //Update close
    } //Close Class
} //Close Namespace
