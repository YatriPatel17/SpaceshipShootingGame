using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace SpaceshipShootingGame
{
    public class Game1 : Game
    {
        // declaring variables + initializing 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D spaceshipTexture, ufoTexture, bulletTexture;
        private SpriteFont font, font1;

        private Spaceship spaceship;
        private List<Bullet> bullets;
        private MultipleUFOManager ufoManager;

        private int score = 0;
        private double elapsedTime = 0;
        private bool gameOver = false;
        private int missedUfos = 0;

        private List<ParallaxLayer> parallaxLayers;
        private float cameraSpeed = 100f;

        private Texture2D instructionImage;
        private Texture2D contactPageImage;

        private Song backgroundSound;
        private SoundEffect menuSound;
        private SoundEffectInstance menuSoundInstance;
        private SoundEffect shootingSound;
        private SoundEffect gameOverSound;

        // enum for menu
        private enum GameState
        {
            StartGame,
            Instructions,
            ContactPage,
            Playing,
            Quit
        }

        private GameState currentState = GameState.StartGame;
        private KeyboardState previousKeyboardState;

        private string[] menuOption = { "Start Game", "Instruction", "Contact Page", "Quit" };
        private int selectedOption = 0;

        private SpriteFont menuFont;
        private Texture2D menuBackgroundTexture;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // setting screen widht and height
            _graphics.PreferredBackBufferWidth = 800; // width
            _graphics.PreferredBackBufferHeight = 600; // height
            _graphics.ApplyChanges();

            // making mouse visible
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // initializing some lists and creating objects
            spaceship = new Spaceship(new Vector2(50, GraphicsDevice.Viewport.Height / 2));
            bullets = new List<Bullet>();
            ufoManager = new MultipleUFOManager();

            previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // setting screen height
            int screenHeight = GraphicsDevice.Viewport.Height;

            // loading images and sounds
            var moonTexture = Content.Load<Texture2D>("Images/Moon");
            // Reference
            // Author unknown. (n.d.). [Image description, e.g., "Abstract artwork featuring geometric shapes"]. DeviantArt. Retrieved December 14, 2024, from https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/47d382cf-6dc7-4e1d-a02b-6e5aa6172908/d4wzi9f-c09c5888-af36-4334-b002-3b40e14a272d.png

            var bgTexture = Content.Load<Texture2D>("Images/bg");
            // Reference
            // Author unknown. (n.d.). Stars in a dark blue sky at night. Freepik. Retrieved December 14, 2024, from https://www.freepik.com/premium-photo/stars-dark-blue-sky-night_7224165.htm

            parallaxLayers = new List<ParallaxLayer>()
            {
                new ParallaxLayer(bgTexture, 0.2f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, new Vector2(0, 0)),
                new ParallaxLayer(moonTexture, 0.5f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, new Vector2(GraphicsDevice.Viewport.Width, 10))
            };

            spaceshipTexture = Content.Load<Texture2D>("Images/spaceship");
            // Reference
            // Author unknown. (n.d.). [Image of ___]. Pinterest. Retrieved December 14, 2024, from https://i.pinimg.com/originals/23/63/42/2363425d573122e2d4378a8181aca37a.png

            bulletTexture = Content.Load<Texture2D>("Images/bullet");
            // Reference
            // Author unknown. (n.d.). [Image of Glenos-G 160 bullet]. Fandom. Retrieved December 14, 2024, from https://vignette3.wikia.nocookie.net/commando2/images/8/84/Glenos-G_160_bullet.png/revision/latest?cb=20120731090012

            ufoTexture = Content.Load<Texture2D>("Images/smallufo");
            // Reference
            // Author unknown. (n.d.). [Image of UFO alien spaceship on transparent background]. Vecteezy. Retrieved December 14, 2024, from https://static.vecteezy.com/system/resources/previews/027/125/687/non_2x/ufo-alien-isolated-on-transparent-background-futuristic-ufo-spaceship-generative-ai-png.png

            font = Content.Load<SpriteFont>("Fonts/font");
            font1 = Content.Load<SpriteFont>("Fonts/font1");

            instructionImage = Content.Load<Texture2D>("Images/instructionPage");
            contactPageImage = Content.Load<Texture2D>("Images/contactPage");

            menuSound = Content.Load<SoundEffect>("Sounds/menuSound");
            // Reference
            // Author unknown. (n.d.). Victory awaits in the gaming universe [rock music]. Pixabay. Retrieved December 14, 2024, from https://pixabay.com/music/rock-victory-awaits-in-the-gaming-universe-astronaut-265184/
            menuSoundInstance = menuSound.CreateInstance();
            menuSoundInstance.IsLooped = true;

            backgroundSound = this.Content.Load<Song>("Sounds/Background");
            // Reference
            // Author unknown. (n.d.). Sci-fi background [ambient music]. Pixabay. Retrieved December 14, 2024, from https://pixabay.com/music/ambient-sci-fi-background-258999/

            shootingSound = Content.Load<SoundEffect>("Sounds/shooting");
            // Reference
            // Author unknown. (n.d.). Shooting star sound effect. Pixabay. Retrieved December 14, 2024, from https://pixabay.com/sound-effects/shooting-star-2-104073/

            gameOverSound = Content.Load<SoundEffect>("Sounds/gameover");
            // Reference
            // Pixabay. (n.d.). Game over [Audio file]. Pixabay. https://pixabay.com/sound-effects/game-over-160612/

            menuBackgroundTexture = Content.Load<Texture2D>("Images/bg");

            // loading background sound 
            MediaPlayer.Play(backgroundSound);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // updating game according to the selected menu option
            switch (currentState)
            {
                case GameState.StartGame:
                    if (menuSoundInstance.State != SoundState.Playing)
                    {
                        menuSoundInstance.Play();
                    }
                    MediaPlayer.Stop();
                    UpdateMenu(keyboardState);
                    break;
                case GameState.Playing:
                    if (menuSoundInstance.State == SoundState.Playing)
                    {
                        menuSoundInstance.Stop();
                    }
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(backgroundSound);
                    }
                    UpdateGame(gameTime, keyboardState);
                    break;
                case GameState.Instructions:
                case GameState.ContactPage:
                    if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
                    {
                        currentState = GameState.StartGame;
                    }
                    break;

                default:
                    UpdateGame(gameTime, keyboardState);
                    break;
            }

            previousKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        private void UpdateMenu(KeyboardState keyboardState)
        {
            // selecting appropriate menu option according to the key press
            if (keyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                selectedOption = (selectedOption + 1) % menuOption.Length;
            }
            if (keyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                selectedOption = (selectedOption - 1 + menuOption.Length) % menuOption.Length;
            }

            // performing the appropriate game action according to the selected option
            if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                switch (menuOption[selectedOption])
                {
                    case "Start Game":
                        ResetGame();
                        currentState = GameState.Playing;
                        break;
                    case "Instruction":
                        currentState = GameState.Instructions;
                        break;
                    case "Contact Page":
                        currentState = GameState.ContactPage;
                        break;
                    case "Quit":
                        Exit();
                        break;
                }
            }
        }

        private void UpdateGame(GameTime gameTime, KeyboardState keyboardState)
        {
            // on esc key press, returning back to start game option
            if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                currentState = GameState.StartGame;
                return;
            }

            // if the game is over then returning
            if (gameOver) return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // updating parallaxLayer
            foreach (var layer in parallaxLayers)
            {
                layer.Update(deltaTime, cameraSpeed);
            }

            spaceship.Update(keyboardState, GraphicsDevice.Viewport);

            // shooting bullet + playing shooting sound
            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                if (bullets.Count == 0 || bullets[bullets.Count - 1].Position.X > spaceship.Position.X + 50)
                {
                    bullets.Add(new Bullet(spaceship.Position + new Vector2(50, spaceshipTexture.Height / 2 - bulletTexture.Height / 2)));
                    shootingSound.Play();
                }
            }

            // removing bullets
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();
                if (bullets[i].Position.X > GraphicsDevice.Viewport.Width)
                {
                    bullets.RemoveAt(i);
                }
            }

            ufoManager.UpdateUFOs(gameTime);

            // if bullet touches ufo the removing ufo + bullet, also updating the score
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                for (int j = ufoManager.UFOs.Count - 1; j >= 0; j--)
                {
                    if (bullets[i].Bounds.Intersects(ufoManager.UFOs[j].Bounds))
                    {
                        bullets.RemoveAt(i);
                        ufoManager.UFOs.RemoveAt(j);
                        score++;
                        break;
                    }
                }
            }

            // if the player misses 10 UFOs then ending the game
            missedUfos = ufoManager.missedUfos;
            if (missedUfos >= 10)
            {
                gameOver = true;
                gameOverSound.Play();
            }

            // if the bullet touches the UFO then ending the game
            foreach (var ufo in ufoManager.UFOs)
            {
                if (ufo.Bounds.Intersects(spaceship.Bounds))
                {
                    gameOver = true;
                    gameOverSound.Play();
                }
            }
        }

        private void GetMenu()
        {
            // setting menu
            _spriteBatch.Draw(menuBackgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            Vector2 position = new Vector2(100, 100);
            for (int i = 0; i < menuOption.Length; i++)
            {
                Color color = (i == selectedOption) ? Color.Green : Color.White;
                _spriteBatch.DrawString(font, menuOption[i], position, color);
                position.Y += 40;
            }
        }

        private void DrawGame()
        {
            // drawing parallaxLayers
            foreach (var layer in parallaxLayers)
            {
                layer.Draw(_spriteBatch);
            }

            // drawing spaceship
            spaceship.Draw(_spriteBatch, spaceshipTexture);

            // drawing bullets
            foreach (var bullet in bullets)
            {
                bullet.Draw(_spriteBatch, bulletTexture);
            }

            // drawing UFOs
            ufoManager.Draw(_spriteBatch, ufoTexture);

            // drawing score, time and missed ufos
            _spriteBatch.DrawString(font, $"Score: {score}", new Vector2(GraphicsDevice.Viewport.Width - 150, 10), Color.White);
            _spriteBatch.DrawString(font, $"Time: {Math.Floor(elapsedTime)}s", new Vector2(GraphicsDevice.Viewport.Width - 150, 40), Color.White);
            _spriteBatch.DrawString(font, $"Missed UFOs: {missedUfos}", new Vector2(GraphicsDevice.Viewport.Width - 150, 70), Color.White);

            // drawing game over string
            if (gameOver)
            {
                string gameOverText = $"Game Over! Your Score: {score}, and you missed {missedUfos} UFOs.";
                Vector2 textSize = font.MeasureString(gameOverText);
                _spriteBatch.DrawString(font1, gameOverText, new Vector2((GraphicsDevice.Viewport.Width - textSize.X) / 2, GraphicsDevice.Viewport.Height / 2), Color.Red);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            // drawing menu
            switch (currentState)
            {
                case GameState.StartGame:
                    GetMenu();
                    break;

                case GameState.Instructions:
                    _spriteBatch.Draw(instructionImage, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    _spriteBatch.DrawString(font, "Press ESC to go back", new Vector2(10, 10), Color.White);
                    break;

                case GameState.ContactPage:
                    _spriteBatch.Draw(contactPageImage, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    _spriteBatch.DrawString(font, "Press ESC to go back", new Vector2(10, 10), Color.White);
                    break;

                default:
                    DrawGame();
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // resetting the game
        private void ResetGame()
        {
            score = 0;
            missedUfos = 0;
            gameOver = false;
            spaceship = new Spaceship(new Vector2(50, GraphicsDevice.Viewport.Height / 2));
            bullets.Clear();
            ufoManager.Reset();
        }
    }
}
