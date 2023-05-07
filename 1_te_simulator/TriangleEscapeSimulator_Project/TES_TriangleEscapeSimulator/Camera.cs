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
    public class Camera
    {
        public Matrix transform;
        public Vector2 centre;
        public float scale = 1.5f;
        Viewport view;

        public Camera(Viewport view)
        {
            this.view = view;
        }

        public void Update(GameTime gameTime, Game1 tes, Player player, Map map)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //static cam
            }
            else
            {
                centre.X = ((player.playerPosition.X + player.playerTexture.Width / 2) - (view.Width / 2) / scale) - 32;
                centre.Y = ((player.playerPosition.Y + player.playerTexture.Height / 2) - (view.Height / 2) / scale) - 16;

                //avoid camera boundary break;
                if (centre.X < 0) centre.X = 0;
                if (centre.Y < 0) centre.Y = 0;
                if (centre.Y + view.Height / scale > map.height) centre.Y = map.height - view.Height / scale;
                if (centre.X + view.Width / scale > map.width) centre.X = map.width - view.Width / scale;

                if (scale < 1f) scale = 1f;
                if (scale > 2f) scale = 2f;

                //button input enables scaling;
                if (Keyboard.GetState().IsKeyDown(Keys.U))
                {
                    scale -= 0.01f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    scale += 0.01f;
                }

                transform = Matrix.CreateTranslation(new Vector3(-centre, 0)) * Matrix.CreateScale(scale);
            }
        }
    }
}
