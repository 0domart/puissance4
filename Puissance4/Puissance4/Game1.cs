using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Puissance4
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private ObjetPuissance4 cadre;
        private ObjetPuissance4 pionjaune;
        private ObjetPuissance4 pionrouge;
        private ObjetPuissance4 background;
        private Byte[,] map;
        private int tourJoueur;
        private int dernierGagnant;
        private int partieEnCours;
        MouseState previousMouseState;
        private SpriteFont spriteFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            map = new byte[6, 7]{
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0}
            };
            tourJoueur = 0;
            partieEnCours = 0;
            dernierGagnant = 0;
            previousMouseState = Mouse.GetState();

            base.Initialize();
        }

        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 660;
            graphics.ApplyChanges();
            // on charge un objet mur 
            cadre = new ObjetPuissance4(Content.Load<Texture2D>("images\\cadre"), new Vector2(0f, 0f), new Vector2(100f, 100f));
            pionjaune = new ObjetPuissance4(Content.Load<Texture2D>("images\\pionjaune"), new Vector2(0f, 0f), new Vector2(100f, 100f));
            pionrouge = new ObjetPuissance4(Content.Load<Texture2D>("images\\pionrouge"), new Vector2(0f, 0f), new Vector2(100f, 100f));
            background = new ObjetPuissance4(Content.Load<Texture2D>("images\\ocean"), new Vector2(0f, 0f), new Vector2(1024f, 660));
            SoundEffect sound;
            sound = Content.Load<SoundEffect>("sound\\ocean");
            sound.Play();
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            switch (partieEnCours)
            {
                case 0:
                    Console.WriteLine("Menu");
                    UpdateMenu(gameTime);
                    break;
                case 1:
                    Console.WriteLine("Jeu Puissance 4");
                    UpdateGame(gameTime);
                    break;
                case 2:
                    Console.WriteLine("Resultat partie");
                    UpdateResult(gameTime);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            };
        }

        protected void UpdateMenu(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Released
            && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Initialize();
                partieEnCours = 1;
            }
            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        protected void UpdateGame(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Released
            && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                int x = FindX(Mouse.GetState().X);
                int y = FindY(Mouse.GetState().Y);

                if (x == 7 || y == 8)
                {

                }
                else
                {
                    if (isAccesible(y, x))
                    {
                        if (tourJoueur % 2 == 0)
                        {
                            map[y, x] = 1;
                        }
                        else
                        {
                            map[y, x] = 2;
                        }
                        verifierGagnant();
                        tourJoueur++;
                    }
                }
            }

            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        protected void UpdateResult(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Released
            && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Initialize();
                partieEnCours = 0;
            }
            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        private void verifierGagnant()
        {
            for (int y = 0; y < 6; y++)
            {
                for(int x=0; x < 7; x++)
                {
                    if(map[y,x] != 0)
                    {
                        int v = verifierColonnes(y, x);
                        if(v != 0)
                        {
                            Console.WriteLine("Victoire : " + v);
                            dernierGagnant = v;
                            partieEnCours = 2;
                        }
                        v = verifierLignes(y, x);
                        if (v != 0)
                        {
                            Console.WriteLine("Victoire : " + v);
                            dernierGagnant = v;
                            partieEnCours = 2;
                        }
                        v = verifierDiagonales(y, x);
                        if (v != 0)
                        {
                            Console.WriteLine("Victoire : " + v);
                            dernierGagnant = v;
                            partieEnCours = 2;
                        }
                    }
                }
            }
        }

        private int verifierColonnes(int y, int x)
        {
            int cpt = 0;
            int pionsIdentiques = 0;
            while (y + cpt < map.GetLength(0))
            {
                if (map[y , x] == map[y + cpt, x])
                {
                    pionsIdentiques++;
                }
                else
                {
                    break;
                }
                if (pionsIdentiques == 4)
                {
                    return map[y, x];
                }
                cpt++;
            }
            return 0;
        }

        private int verifierLignes(int y, int x)
        {
            int cpt = 0;
            int pionsIdentiques = 0;
            while(x + cpt < map.GetLength(1))
            {
                if (map[y, x] == map[y, x + cpt])
                {
                    pionsIdentiques++;
                }
                else
                {
                    break;
                }
                if(pionsIdentiques == 4)
                {
                    return map[y, x];
                }
                cpt++;
            }
            return 0;
        }

        private int verifierDiagonales(int y, int x)
        {
            int cpt = 0;
            int pionsIdentiques = 0;
            while (x + cpt < map.GetLength(1) && y + cpt < map.GetLength(0))
            {
                if (map[y, x] == map[y + cpt, x + cpt])
                {
                    pionsIdentiques++;
                }
                else
                {
                    break;
                }
                if (pionsIdentiques == 4)
                {
                    return map[y, x];
                }
                cpt++;
            }

            cpt = 0;
            pionsIdentiques = 0;
            while (x - cpt > 0 && y + cpt < map.GetLength(0))
            {
                if (map[y, x] == map[y + cpt, x - cpt])
                {
                    pionsIdentiques++;
                }
                else
                {
                    break;
                }
                if (pionsIdentiques == 4)
                {
                    return map[y, x];
                }
                cpt++;
            }


            return 0;
        }

        private Boolean isAccesible(int Y, int X)
        {
            if(map[Y,X] != 0)
            {
                return false;
            }
            else if(Y == 5 && map[Y,X] == 0)
            {
                return true;
            }
            else if (Y < 5 && map[Y+1, X] != 0)
            {
                return true;
            }
            else return false;
        }

        private int FindX(int X)
        {
            if (X >= 140 && X < 240)
            {
                return 0;
            }
            else if (X >= 240 && X < 340)
            {
                return 1;
            }
            else if (X >= 340 && X < 440)
            {
                return 2;
            }
            else if (X >= 440 && X < 540)
            {
                return 3;
            }
            else if (X >= 540 && X < 640)
            {
                return 4;
            }
            else if (X >= 640 && X < 740)
            {
                return 5;
            }
            else if (X >= 740 && X < 840)
            {
                return 6;
            }
            return 7;
        }

        private int FindY(int Y)
        {
            if (Y >= 40 && Y < 140)
            {
                return 0;
            }
            else if (Y >= 140 && Y < 240)
            {
                return 1;
            }
            else if (Y >= 240 && Y < 340)
            {
                return 2;
            }
            else if (Y >= 340 && Y < 440)
            {
                return 3;
            }
            else if (Y >= 440 && Y < 540)
            {
                return 4;
            }
            else if (Y >= 540 && Y < 640)
            {
                return 5;
            }
            else if (Y >= 640 && Y < 740)
            {
                return 6;
            }
            else if (Y >= 740 && Y < 840)
            {
                return 7;
            }
            return 8;
        }

        protected void DrawMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            int xpos, ypos;
            xpos = 0;
            ypos = 0;
            Vector2 pos = new Vector2(ypos, xpos);
            spriteBatch.Draw(background.Texture, pos, Color.White);

            SpriteFont Line1 = Content.Load<SpriteFont>("font\\Arial3");
            
            String s = "Puissance 4 par Julien G et Julie H";
            spriteBatch.DrawString(Line1, s, new Vector2(150, 50), Color.Black);

            s = "Lancez vous dans un combat contre notre robot Victor";
            spriteBatch.DrawString(Line1, s, new Vector2(100, 300), Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void DrawGame(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(background.Texture, new Vector2(0, 0), Color.White);

            int offsetX = 40;
            int offsetY = 140;
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (map[x, y] == 0)
                    {
                        int xpos, ypos;
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(cadre.Texture, pos, Color.White);
                    }
                    else if (map[x, y] == 1)
                    {
                        int xpos, ypos;
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionjaune.Texture, pos, Color.White);
                    }
                    else if (map[x, y] == 2)
                    {
                        int xpos, ypos;
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionrouge.Texture, pos, Color.White);
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        protected void DrawResult(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            int xpos, ypos;
            xpos = 0;
            ypos = 0;
            Vector2 pos = new Vector2(ypos, xpos);
            spriteBatch.Draw(background.Texture, pos, Color.White * 0.3f);
            int offsetX = 40;
            int offsetY = 140;
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (map[x, y] == 0)
                    {
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(cadre.Texture, pos, Color.White * 0.3f);
                    }
                    else if (map[x, y] == 1)
                    {
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionjaune.Texture, pos,Color.White * 0.3f);
                    }
                    else if (map[x, y] == 2)
                    {
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionrouge.Texture, pos, Color.White * 0.3f);
                    }
                }
            }

            SpriteFont Line2;
            String c;
            if(dernierGagnant == 1)
            {
                Line2 = Content.Load<SpriteFont>("font\\Arial");

                c = "La victoire est votre, felicitations";
            }
            else
            {
                Line2 = Content.Load<SpriteFont>("font\\Arial");
                c = "La victoire vous echappe cette fois ci...";
            }
            spriteBatch.DrawString(Line2, c, new Vector2(160, 320), Color.White);


            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (partieEnCours)
            {
                case 0:
                    DrawMenu(gameTime);
                    break;
                case 1:
                    DrawGame(gameTime);
                    break;
                case 2:
                    DrawResult(gameTime);
                    break;
                default:
                    break;
            };
        }
    }
}
