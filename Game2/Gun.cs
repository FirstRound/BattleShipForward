using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class Gun
    {
        public Texture2D gun_texture;
        public Rectangle gun_rect;
        private int speed = 10;
        private int health = 1000;
        public int points = 0;
        private int screen_height;
        private int screen_width;
        public Vector2 gun_origin;
        public Vector2 gun_position;
        public float gun_rotation;

        public Gun(Texture2D gun, int width, int height)
        {
            gun_texture = gun;
            screen_height = height;
            screen_width = width;
            gun_rect = new Rectangle(0, height - gun.Height-50, 150, 100);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gun_texture, gun_rect, null, Color.White, gun_rotation, gun_origin, SpriteEffects.None, 0);
        }

        public void MoveLeft()
        {
            if (gun_rect.X > -gun_texture.Width / 4)
                gun_rect.X -= speed;
        }

        public void MoveRight()
        {
            if (gun_rect.X <= screen_width - gun_texture.Width/2)
                gun_rect.X += speed;
        }

        public void SpeedUp() {
            speed+=1;
        }
    }
}
