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
    public class Bullet
    {
        Texture2D texture;
        public Vector2 position;
        Vector2 direction;
        public Rectangle rectangle;

        float speed;
        public int height = 4;
        int width = 4;
        int strength;

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, float speed, int strength)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.strength = strength;
            UpdateRectangle();
        }

        void UpdateRectangle()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
        }


        public void Update(GameTime gameTime)
        {
            position += direction * speed;
            UpdateRectangle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(width / 2, height / 2);
            spriteBatch.Draw(texture, new Rectangle((int)(rectangle.X - offset.X), (int)(rectangle.Y - offset.Y), width, height), Color.White);
        }

        public int getStrength()
        {
            return strength;
        }
    }
}
