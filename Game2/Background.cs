using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class Background
    {
        private Texture2D background_texture;
        private Rectangle background_rect;
        private int screen_height;
        private int screen_width;

        public Background(Texture2D score, int width, int height)
        {
            background_texture = score;
            screen_width = width;
            screen_height = height;
            background_rect = new Rectangle(0, 0, width, height);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(background_texture, background_rect, color);
        }
    }
}
