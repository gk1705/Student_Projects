using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TES_TriangleEscapeSimulator
{
    public class SwitchandExit
    {
        Texture2D switchTexture;
        Texture2D exitTexture;

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        Vector2 switchPosition;
        Vector2 switchOrigin;

        Vector2 exitPosition;
        Vector2 exitOrigin;

        Rectangle switchRectangle;
        Rectangle exitRectangle;

        //On entry (depending if switch was activated) move the player to location [postEntryPosition];
        Vector2 postEntryPosition;
        public bool isActive;

        public SwitchandExit(Vector2 switchPosition, Vector2 exitPosition, Vector2 postEntryPosition)
        {
            this.switchTexture = Content.Load<Texture2D>("key");
            this.switchPosition = switchPosition;

            this.exitTexture = Content.Load<Texture2D>("exitClose");
            this.exitPosition = exitPosition;

            this.postEntryPosition = postEntryPosition;
            isActive = false;
        }

        public void Update(Player player)
        {
            switchRectangle = new Rectangle((int)switchPosition.X, (int)switchPosition.Y, switchTexture.Width, switchTexture.Height);
            exitRectangle = new Rectangle((int)exitPosition.X, (int)exitPosition.Y, exitTexture.Width, exitTexture.Height);

            if (isActive == false)
            {
                this.switchTexture = Content.Load<Texture2D>("key");
                this.exitTexture = Content.Load<Texture2D>("exitClose");
            }
            
            //Switching the switch;
            if (player.playerRectangle.Intersects(switchRectangle))
            {
                //maybe change the color/texture to give visual info to the player?
                this.switchTexture = Content.Load<Texture2D>("empty");
                this.exitTexture = Content.Load<Texture2D>("exitOpen");
                isActive = true;
            }
            //beam me [somewhere] scotty;
            if (player.playerRectangle.Intersects(exitRectangle) && isActive)
            {
                player.playerPosition = postEntryPosition;
                player.playerVelocity = Vector2.Zero;
            } 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(switchTexture, switchPosition, null, Color.White, 0f, switchOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(exitTexture, exitPosition, null, Color.White, 0f, exitOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
