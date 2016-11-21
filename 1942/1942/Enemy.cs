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
    class Enemy
    {
        Texture2D texture;
        public Vector2 position;
        public Rectangle rectangle;
        
        //STATS
        float speed = 5.0f;
        int width;
        public int height = 64;
        public int health = 100;

        public Enemy(Texture2D texture, Vector2 position, int width, int height)
        {
            this.height = height;
            this.width = width;
            this.texture = texture;
            this.position = position;
            UpdateRectangle();
        }

        void UpdateRectangle()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public void Update(GameTime gameTime)
        {
            position.Y += speed;
            UpdateRectangle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)rectangle.X, (int)rectangle.Y, width, height), Color.Aqua);
        }

        public void Hit(int damage)
        {
            health -= damage;
        }
    }
}
