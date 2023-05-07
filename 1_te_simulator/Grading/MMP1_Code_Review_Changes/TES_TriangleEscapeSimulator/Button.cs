using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TES_TriangleEscapeSimulator
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        Color color = new Color(255, 255, 255, 255);
        Vector2 size;

        public Button(Texture2D texture, GraphicsDevice graphics)
        {
            this.texture = texture;
            //depending on the viewport size we divide the width and height;
            size = new Vector2(graphics.Viewport.Width/4, graphics.Viewport.Height/8);
        }

        private bool down;
        private ButtonState mouseState;
        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (mouseRectangle.Intersects(rectangle) && mouseState != mouse.LeftButton)
            {
                //if the button is fully visible;
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                //ensures blinking;
                if (down) color.A += 3;
                else color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    mouseState = mouse.LeftButton;
                }
            }
            //if mouse is away from button;
            else if (color.A < 255)
            {
                color.A += 3;
                isClicked = false;
                //No alternative; This ensures that button is only pressed once;
                mouseState = mouse.MiddleButton;
            }
        }

        public void SetPosition(Vector2 newPositon)
        {
            position = newPositon;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
