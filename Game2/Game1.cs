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

namespace Game2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public enum ScreenEnum
        {
            Menu,
            Settings,
            Game,
            GameOverWin,
            GameOverLose
        }

        private ScreenEnum current_screen = ScreenEnum.Menu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Gun gun;
        private Vector2 sight;
        //private Ship[] ships;
        private List<Ship> ships = new List<Ship>();
        private Score score;
        private Background background;
        private Explosion explose;
        private int count_ships = 5;
        private List<Fire> fires = new List<Fire>();
        private Texture2D fire_texture;
        private List<Explosion> exploses =  new List<Explosion>();
        private SoundEffect exp_sound;
        private SoundEffect shoot_sound;
        private Song music;
        private SpriteFont font;
        public static int health = 1000;
        private int level = 1;
        private int ultimate = 0;
        private Texture2D level_texture;
        private Rectangle level_rect;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gun = new Gun(Content.Load<Texture2D>("gun2"),graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            fire_texture = Content.Load<Texture2D>("bullet");
            CreateShips(5);
            score = new Score(Content.Load<Texture2D>("bar"),graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            exp_sound = Content.Load<SoundEffect>("dsbarexp");
            shoot_sound = Content.Load<SoundEffect>("galil-2");
            music = Content.Load<Song>("music");
            background = new Background(Content.Load<Texture2D>("bg"), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            MediaPlayer.Play(music);
            font = Content.Load<SpriteFont>("SpriteFont1");
            level_texture = Content.Load<Texture2D>("level_bar");
        }

        private void CreateShips(int count)
        {
            count_ships = count;
            Random rnd = new Random();
            for (int i = 0; i < count_ships; i++)
            {
                if (rnd.Next(0, 2) == 0)
                    ships.Add(new Ship(Content.Load<Texture2D>("ship2"), level, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                else
                    ships.Add(new Ship(Content.Load<Texture2D>("ship1"),level, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            }
        }

        private void DeleteShips() 
        {
            for (int i = 0; i < count_ships; i++)
            {
                ships.RemoveAt(i);
                count_ships--;
                i--;
            }
        }

        private void DeleteSprites()
        {
            for (int i = 0; i < exploses.Count; i++)
            {
                exploses.RemoveAt(i);
                i--;
            }
        }

        private void UseUltimate()
        {
            gun.points -= 5000;
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].Damage(100);
            }
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

            switch (current_screen)
            {
                
               case ScreenEnum.Menu:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        current_screen = ScreenEnum.Game;
                    break;
                case ScreenEnum.Game:
                    //MouseState mouse = Mouse.GetState();
                    //sight.X = mouse.X - gun.gun_rect.X;
                    //sight.Y = mouse.Y - gun.gun_rect.Y;
                    //gun.gun_rotation = (float)Math.Atan2(sight.X, sight.Y);
                    ultimate = gun.points / 5000;
                    UpdateShips();
                    if (count_ships == 0)
                        current_screen = ScreenEnum.GameOverWin;
                    if (health <= 0)
                        current_screen = ScreenEnum.GameOverLose;
                    UpdateFire();
                    foreach (Fire bullet in fires)
                    {
                        foreach (Ship ship in ships)
                        {
                            if (bullet.is_visible && ship.ship_rect.Intersects(new Rectangle((int)bullet.fire_position.X, (int)bullet.fire_position.Y, 200, 200)))
                            {
                                ship.Damage();
                                bullet.is_visible = false;
                                gun.points += 20;
                                exploses.Add(new Explosion(Content.Load<Texture2D>("explose"), 3, bullet.fire_position, 61, 63));
                                if (ship.is_visible == false)
                                {
                                    exploses.Add(new Explosion(Content.Load<Texture2D>("explose"), 11, new Vector2(bullet.fire_position.X, bullet.fire_position.Y - 25), 61, 63));
                                    exp_sound.Play();
                                    gun.points += 100;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < exploses.Count; i++)
                    {
                        if (exploses[i].is_visible == true)
                            exploses[i].Update(gameTime);
                        else
                        {
                            exploses.RemoveAt(i);
                            i--;
                        }
                    }

                        foreach (Ship s in ships)
                        {
                            s.Move();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Space) && gameTime.TotalGameTime.Milliseconds % 100 == 0 && fires.Count < 5+2*level)
                        {
                            //Shooting((level / 7)+1);
                            Shooting(1);
                            shoot_sound.Play((float)0.2, (float)0.7, (float)0.5);
                        }

                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                          UseUltimate();
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                        gun.MoveRight();
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                        gun.MoveLeft();
                    break;
                case ScreenEnum.GameOverWin:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        CreateShips(5);
                        DeleteSprites();
                        health += 20 * level;
                        level++;
                        gun.SpeedUp();
                        current_screen = ScreenEnum.Game;
                    }
                    break;
                case ScreenEnum.GameOverLose:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        DeleteShips();
                        DeleteSprites();
                        health = 1000;
                        gun.points = 0;
                        level = 1;
                        CreateShips(5);
                        current_screen = ScreenEnum.Game;
                    }
                    break;

            }
           base.Update(gameTime);
        }

        private void UpdateShips()
        {
            for (int i = 0; i < count_ships; i++)
            {
                if (ships[i].is_visible == false)
                {
                    ships.RemoveAt(i);
                    i--;
                    count_ships--;
                }
            }
        }

        public void UpdateFire()
        {
            foreach (Fire f in fires)
            {
                f.fire_position += f.fire_velocity;
                if (f.fire_position.Y < 200)
                    f.is_visible = false;
            }

            for (int i = 0; i < fires.Count; i++)
            {
                if (!fires[i].is_visible)
                {
                    fires.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Shooting(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Fire fire = new Fire(Content.Load<Texture2D>("bullet"));
                fire.fire_velocity = new Vector2(-1, -5);
                fire.fire_position = new Vector2(gun.gun_rect.X + gun.gun_texture.Width / 4 - 2*i, graphics.PreferredBackBufferHeight - 50 - gun.gun_texture.Height) + fire.fire_velocity * 5f;
                fire.is_visible = true;
                fires.Add(fire);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            switch (current_screen)
            {
                case ScreenEnum.GameOverLose:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch, Color.Red);
                    spriteBatch.DrawString(font, "You`re lose...\n Press 'Enter' to try again", new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 2), Color.White);
                    spriteBatch.End();
                    break;
                case ScreenEnum.GameOverWin:
                    GraphicsDevice.Clear(Color.Green);
                    spriteBatch.Begin();
                    background.Draw(spriteBatch, Color.Green);
                    spriteBatch.DrawString(font, "You`re WIN!!!\n Press 'Enter' to continue", new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 2), Color.Black);
                    spriteBatch.End();
                    break;
                case ScreenEnum.Menu:
                    GraphicsDevice.Clear(Color.DarkRed);
                    spriteBatch.Begin();
                    background.Draw(spriteBatch, Color.Blue);
                    spriteBatch.DrawString(font, "Press 'Enter' for start", new Vector2(graphics.PreferredBackBufferWidth/6,graphics.PreferredBackBufferHeight/2), Color.Black);
                    spriteBatch.End();
                    break;
                case ScreenEnum.Game:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch, Color.White);

                    foreach (Explosion exp in exploses)
                    {
                        if (exp.is_visible)
                            exp.Draw(spriteBatch);
                    }

                    foreach (Ship s in ships)
                    {
                        if(s.is_visible)
                            s.Draw(spriteBatch);
                    }
                    gun.Draw(spriteBatch);
                    score.Draw(spriteBatch);

                    foreach (Fire f in fires) 
                    {
                        f.Draw(spriteBatch);
                    }

                    spriteBatch.DrawString(font, gun.points.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 180, graphics.PreferredBackBufferHeight-50), Color.White);
                    spriteBatch.DrawString(font, health.ToString(), new Vector2(355, graphics.PreferredBackBufferHeight - 50), Color.White);

                    spriteBatch.Draw(level_texture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, 50), Color.White);
                    spriteBatch.DrawString(font, level.ToString(), new Vector2(300, -7), Color.White);
                    spriteBatch.DrawString(font, ultimate.ToString(), new Vector2(graphics.PreferredBackBufferWidth-80, -7), Color.White);

                    spriteBatch.End();
                    base.Draw(gameTime);
                    break;
            }
        }
    }
}
