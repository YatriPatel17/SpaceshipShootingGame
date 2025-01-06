using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipShootingGame
{
    public class UFOManager
    {
        // declaring variables 
        public Vector2 Position;
        public int Speed;
        public static int Radius = 40;

        // declaring and initializing random for random ufo creation
        private static Random random = new Random();

        // UFOManager constructor
        public UFOManager(int startX, int speed)
        {
            Position = new Vector2(startX, random.Next(100, 500));
            this.Speed = speed;
        }

        public void Update()
        {
            // updation position.x
            Position.X -= Speed;
        }

        // getting boundry for UFO
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Radius, Radius);
            }
        }
    }

    public class MultipleUFOManager
    {
        // declaring variables and initializing them
        public List<UFO> UFOs = new List<UFO>();
        private Random random = new Random();
        private int ufoInterval = 1000;
        private int lastCreationTime = 0;
        public int missedUfos { get; private set; }

        // resetting UFOs
        public void Reset()
        {
            UFOs.Clear();
            missedUfos = 0;
        }

        public void UpdateUFOs(GameTime gameTime)
        {
            // creating UFOs randomly and removing them if the reaches left bounry 
            lastCreationTime += gameTime.ElapsedGameTime.Milliseconds;

            if (lastCreationTime >= ufoInterval)
            {
                UFOs.Add(new UFO(new Vector2(1200, random.Next(65, 500))));
                lastCreationTime = 0;
            }

            for (int i = UFOs.Count - 1; i >= 0; i--)
            {
                UFOs[i].Update();
                if (UFOs[i].Position.X < -UFO.Radius)
                {
                    UFOs.RemoveAt(i);
                    missedUfos++;
                }
            }
        }

        // draw method to draw UFOs
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach (var ufo in UFOs)
            {
                ufo.Draw(spriteBatch, texture);
            }
        }
    }
}
