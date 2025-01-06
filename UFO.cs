using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace SpaceshipShootingGame
{
    public class UFO
    {
        // declaring variables + initializing them
        public Vector2 Position;
        private const float Speed = 3f;
        public static int Radius = 40;

        // UFO constructor
        public UFO(Vector2 initialPosition)
        {
            Position = initialPosition;
        }

        public void Update()
        {
            // updating positon.x
            Position.X -= Speed;
        }

        // getting boundry
        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, 50, 50); }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
