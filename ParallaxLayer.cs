using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceshipShootingGame
{
    public class ParallaxLayer
    {
        // declaring variables
        public Texture2D texture { get; }
        public float ScrollSpeed { get; }
        public Vector2 Position;

        private int screenWidth;
        private int screenHeight;

        // ParallaxLayer constructor
        public ParallaxLayer(Texture2D texture, float scrollSpeed, int screenWidth, int screenHeight, Vector2 initialPosition)
        {
            this.texture = texture;
            this.ScrollSpeed = scrollSpeed;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.Position = initialPosition;
        }

        public void Update(float deltaTime, float cameraSpeed)
        {
            Position = new Vector2(Position.X - cameraSpeed * ScrollSpeed * deltaTime, Position.Y);

            // handling background image and the moon
            if (Position.X <= -texture.Width && !texture.Name.Contains("Moon"))
            {
                Position.X = 0;
            }
            else if (texture.Name.Contains("Moon") && Position.X + texture.Width <= 0)
            {
                Position.X = screenWidth;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // calculating scale to make the background cover the screen
            float scaleX = (float)screenWidth / texture.Width;
            float scaleY = (float)screenHeight / texture.Height;

            if (texture.Name.Contains("Moon"))
            {
                // drawing the moon at its position 
                spriteBatch.Draw(texture, Position, Color.White);
            }
            else
            {
                // drawing the background which covers the entire screen
                spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, new Vector2(Position.X + texture.Width * scaleX, Position.Y), null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
        }
    }
}
