using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _1942
{
    public class Player
    {
        public Game1 game;
        GraphicsDeviceManager graphics;

        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        
        //STATS
        float speed = 5.0f;
        int width = 64;
        int height = 64;
        int score;

        float shootingTime = 0.2f;
        float shootingCooldownTime = 0.1f;

        int bulletGroupMax = 3;
        int bulletGroup = 0;
        float bulletSpeed = 7.0f;
        int bulletStrength = 30;

        //CONTROLS
        public Keys upKey = Keys.W;
        public Keys downKey = Keys.S;
        public Keys leftKey = Keys.A;
        public Keys rightKey = Keys.D;
        public Keys shootKey = Keys.Space;

        public Player(GraphicsDeviceManager graphics, Game1 game, Texture2D texture, Vector2 position)
        {
            this.graphics = graphics;
            this.texture = texture;
            this.position = position;
            this.game = game;
            score = 0;
            UpdateRectangle();
        }

        public void Update(GameTime gameTime)
        {
            UpdateControls(gameTime);
            UpdateRectangle();
            Collisions();
        }

        void UpdateRectangle()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        void UpdateControls(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(upKey))
                position.Y -= speed;

            if (Keyboard.GetState().IsKeyDown(downKey))
                position.Y += speed;

            if (Keyboard.GetState().IsKeyDown(leftKey))
                position.X -= speed;

            if (Keyboard.GetState().IsKeyDown(rightKey))
                position.X += speed;


            
            //shooting
            shootingCooldownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Keyboard.GetState().IsKeyDown(shootKey))
            {

                Shoot(gameTime);
            }
        }

        void Shoot(GameTime gameTime)
        {
            if (shootingCooldownTime < 0.0f)
            {
                game.Shoot(position, new Vector2(0, -1), bulletSpeed, bulletStrength);
                bulletGroup++;

                if (bulletGroup >= bulletGroupMax)
                {
                    shootingCooldownTime = shootingTime;
                    bulletGroup = 0;
                }
            }

        }

        void Collisions()
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > graphics.PreferredBackBufferWidth - width)
                position.X = graphics.PreferredBackBufferWidth - width;
            if (position.Y > graphics.PreferredBackBufferHeight - height)
                position.Y = graphics.PreferredBackBufferHeight - height;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(width / 2, 0);
            spriteBatch.Draw(texture, new Rectangle((int)(rectangle.X - offset.X), (int)(rectangle.Y - offset.Y), width, height), Color.Orange);
        }

        public void raiseScore(int number)
        {
            score += number;
        }

        public int getScore()
        {
            return score;
        }
    }
}
