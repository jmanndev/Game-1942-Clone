using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _1942
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int windowHeight = 800;
        int windowWidth = 500;

        SpriteFont font;

        Texture2D playerTexture;
        Player player;

        Texture2D bulletTexture;
        List<Bullet> bulletList = new List<Bullet>();

        Texture2D enemyTexture;
        List<Enemy> enemyList = new List<Enemy>();
        int enemyHeight = 20;
        int enemyWidth = 20;
        float enemySpawnTime = 0.4f;
        float enemySpawnCooldownTime = 0.0f;

        Random rand = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
            playerTexture = Content.Load<Texture2D>("plane");
            bulletTexture = Content.Load<Texture2D>("bullet");
            enemyTexture = Content.Load<Texture2D>("ball");

            player = new Player(graphics, this, playerTexture, new Vector2(300,300));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            player.Update(gameTime);

            for (int i = bulletList.Count-1; i >=0 ; i--)
            {
                bulletList[i].Update(gameTime);

                if (bulletList[i].position.X < 0 ||
                    bulletList[i].position.X > graphics.PreferredBackBufferWidth ||
                    bulletList[i].position.Y < 0 ||
                    bulletList[i].position.Y > graphics.PreferredBackBufferHeight)
                {
                    bulletList.RemoveAt(i);
                }
            }

            enemySpawnCooldownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (enemySpawnCooldownTime < 0.0f)
            {
                SpawnEnemy();
                enemySpawnCooldownTime = enemySpawnTime;
            }

            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                enemyList[i].Update(gameTime);

                if (enemyList[i].position.Y > graphics.PreferredBackBufferHeight)
                {
                    enemyList.RemoveAt(i);
                }
            }

            CheckCollisions();
            CheckEnemyHealth();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            player.Draw(spriteBatch);
            spriteBatch.DrawString(font, player.getScore().ToString(), new Vector2(10, 10), Color.White);


            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                bulletList[i].Draw(spriteBatch);
            }

            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                enemyList[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Shoot(Vector2 pos, Vector2 dir, float speed, int strength)
        {
            Bullet b = new Bullet(bulletTexture, pos, dir, speed, strength);
            bulletList.Add(b);
        }

        void SpawnEnemy()
        {
            float randomX = rand.Next(0, graphics.PreferredBackBufferWidth - enemyWidth);
            float randomY = rand.Next(-100, 0);
            Enemy e = new Enemy(enemyTexture, new Vector2(randomX, randomY), enemyWidth, enemyHeight);
            enemyList.Add(e);
        }

        void CheckCollisions()
        {
            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                for (int j = enemyList.Count - 1; j >= 0; j--)
                {
                  //  float distance = (bulletList[i].position - enemyList[j].position).Length();
                   // if (distance < (bulletList[i].height / 2.0f + enemyList[j].height / 2.0f))
                    if (bulletList[i].rectangle.Intersects(enemyList[j].rectangle))
                    {
                        enemyList[j].Hit(bulletList[i].getStrength());
                        bulletList.RemoveAt(i);

                        break;
                    }
                }
            }
        }

        void CheckEnemyHealth()
        {
            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                if (enemyList[i].health <= 0)
                {
                    enemyList.RemoveAt(i);
                    player.raiseScore(1);
                }
            }
        }
    }
}
