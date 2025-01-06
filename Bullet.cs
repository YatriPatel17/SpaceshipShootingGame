using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceshipShootingGame
{
    public class Bullet
    {
        // declaring + initializing variables
        public Vector2 Position;
        private const float Speed = 10f;

        // Bullet constructor
        public Bullet(Vector2 initialPosition)
        {
            Position = initialPosition;
        }

        // updating posotion.x
        public void Update()
        {
            Position.X += Speed;
        }

        // getting boundry
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 10, 5);

        // draw method to draw bullet
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
