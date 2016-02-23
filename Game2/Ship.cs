using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class Ship
    {
        private Texture2D ship_texture;
        public Rectangle ship_rect;
        private int screen_height;
        private int screen_width;
        private int velocity;
        private int health = 100;
        public int score = 0;
        public bool is_visible;
        private int cur_level;
        private static  Random rnd = new Random();

        public Ship(Texture2D ship, int level, int width, int height)
        {
            cur_level = level;
            ship_texture = ship;
            screen_height = height;
            screen_width = width;
            ship_rect = new Rectangle(rnd.Next(0, width-ship.Width), rnd.Next(200, height - ship.Height-100), 200, 200);
            if (ship_rect.X < width/2)
            {
                velocity = rnd.Next(cur_level, 2 * cur_level);
            }
            else
                velocity = rnd.Next(-2 * cur_level, cur_level);
            is_visible = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship_texture, ship_rect, Color.White);
        }

        public void Move()
        {
            if (ship_rect.X <= -ship_texture.Width)
            {
                Game1.health -= 70+5*cur_level;
                if (rnd.Next(0, 2) == 0)
                {
                    velocity = rnd.Next(cur_level, 2 * cur_level);
                    ship_rect.X = screen_width;
                }
                else
                {
                    velocity = rnd.Next(-2 * cur_level, cur_level);
                    ship_rect.X = -ship_texture.Width;
                }
            }
            if (ship_rect.X > screen_width + ship_texture.Width)
            {
                Game1.health -= 70 + 5 * cur_level;
                if (rnd.Next(0, 2) == 0)
                {
                    velocity = rnd.Next(cur_level, 2 * cur_level);
                    ship_rect.X = screen_width;
                }
                else
                {
                    velocity = rnd.Next(-2 * cur_level, cur_level);
                    ship_rect.X = -ship_texture.Width;
                }
            }

            ship_rect.X -= velocity;
        }

        public void Damage(int damage=20) 
        {
            damage -= 2*(cur_level / 3);
            if (damage <= 10)
                damage = 10;
            health -= damage;
            is_visible = isAlive();
        }

        private bool isAlive() 
        {
            return health > 0;
        }
    }
}
