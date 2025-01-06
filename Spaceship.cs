using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceshipShootingGame
{
    public class Spaceship
    {
        // declaring variables
        public Vector2 Position;

        // Spaceship constructor
        public Spaceship(Vector2 initialPosition)
        {
            Position = initialPosition;
        }

        // getting boundry
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 70, 70);
            }
        }

        // updating spaceship position according to the key press
        public void Update(KeyboardState keyboardState, Viewport viewport)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Position.Y -= 5;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                Position.Y += 5;
            }

            Position.Y = MathHelper.Clamp(Position.Y, 0, viewport.Height - 100);
        }

        // draw method to draw spaceship
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
