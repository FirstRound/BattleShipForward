using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class Score
    {
        private Texture2D score_texture;
        private Rectangle score_rect;
        private int screen_height;
        private int screen_width;
        private int scorebar_height = 50;

        public Score(Texture2D score, int width, int height)
        {
            score_texture = score;
            screen_height = height;
            screen_width = width;
            score_rect = new Rectangle(0, screen_height - score_texture.Height + scorebar_height-10, screen_width, scorebar_height);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(score_texture, score_rect, Color.White);
        }
    }
}
