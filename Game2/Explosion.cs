using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class Explosion
    {
        private Texture2D explose_texture;
        private Rectangle explose_rect;
        private Vector2 origin_position;
        private Vector2 position;
        private Vector2 velocity;
        public bool is_visible;

        private int frame_height;
        private int count_frames = 3;
        private int frame_width;
        private int current_frame;
        private double timer;
        private double interval = 50;

        public Explosion(Texture2D explose, int frames, Vector2 explose_pos, int width, int height)
        {
            position = explose_pos;
            count_frames = frames;
            explose_texture = explose;
            frame_height = height;
            frame_width = width;
            is_visible = true;
        }

        public void Update(GameTime game_time)
        {
            explose_rect = new Rectangle(current_frame * frame_width, 0, frame_width, frame_height);
            origin_position = new Vector2(explose_rect.Width / 4, explose_rect.Height / 4);

            if (current_frame != count_frames)
            {
                GunOneExplose(game_time);
            }
            else
            {
                is_visible = false;
            }
        }

        public void GunOneExplose(GameTime game_time) 
        {
            timer += game_time.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                current_frame++;
                timer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(explose_texture, position, explose_rect, Color.White, 0f, origin_position, 1.0f, SpriteEffects.None, 0);
        }


    }
}
