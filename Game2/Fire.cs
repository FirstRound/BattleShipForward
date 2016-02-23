using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class Fire
    {
        public Texture2D fire_texture;
        public Vector2 fire_position;
        public Vector2 fire_velocity;
        public Vector2 fire_origin;

        public bool is_visible;

        public Fire(Texture2D fire)
        {
            fire_texture = fire;
            is_visible = false;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(fire_texture, fire_position, null, Color.White, 0f, fire_origin, 1f, SpriteEffects.None, 0);
        }
    }
}
