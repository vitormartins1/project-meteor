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

namespace ParticleTest
{
   public  class NaveFilho : GameObject
    {
        public int life;
        public int VarRandom;
        Random random = new Random();
        public double Navetimer;
        public double TimeMovimento;
        public Color[] Db;
        public Color currentColor;
        public bool boolVermelhidao;

        public NaveFilho(Texture2D loadedTexture, int Px)
            : base(loadedTexture)
        {
            
            sprite = loadedTexture;
            position = new Vector2(Game1.viewportRect.Width / 2, Game1.viewportRect.Height / 2);
            rotation = 0.0f;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            Db = new Color[sprite.Width * sprite.Height];
            sprite.GetData(Db);            
            alive = true;
            life = 5;
            TimeMovimento = 0;
            VarRandom = random.Next(1, 8);
            currentColor = Color.White;
            boolVermelhidao = false;
        }

        public void Update(GameTime gameTime)
        {
            if (life <= 0)
                alive = false;

            if (alive == true)
            {
                #region Movimentacao
                position.X = MathHelper.Clamp((float)position.X, sprite.Width, Game1.viewportRect.Width - sprite.Width);

                if (VarRandom == 1 || VarRandom == 2 || VarRandom == 3)
                {
                    TimeMovimento += gameTime.ElapsedGameTime.TotalSeconds;

                    if (TimeMovimento < 0.5)
                        GoRight();

                    else
                    {
                        VarRandom = random.Next(1, 8);
                        TimeMovimento = 0;
                    }

                }




                if (VarRandom == 4 || VarRandom == 5 || VarRandom == 6)
                {
                    TimeMovimento += gameTime.ElapsedGameTime.TotalSeconds;

                    if (TimeMovimento < 0.5)
                        GoLeft();

                    else
                    {

                        VarRandom = random.Next(1, 8);
                        TimeMovimento = 0;
                    }
                }



                if (VarRandom == 7 || VarRandom == 8)
                {
                    TimeMovimento += gameTime.ElapsedGameTime.TotalSeconds;

                    if (TimeMovimento < 0.1)
                        Stop();

                    else
                    {

                        VarRandom = random.Next(1, 8);
                        TimeMovimento = 0;
                    }
                }


                #endregion

                if (boolVermelhidao)
                {

                    Vermelhidao(gameTime);
                }

                Rect = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            }
            
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive == true)
            {
                spriteBatch.Draw(sprite, position, null, currentColor,
                            rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);
            }

        }


        public void Vermelhidao(GameTime gameTime)
        {
            Navetimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (Navetimer < 0.13)
            {
                currentColor = Color.Red;

            }

            else
            {
                currentColor = Color.White;
                Navetimer = 0;
                boolVermelhidao = false;

            }


        }

        public void GoRight()
        {

            this.position.X += 5;

        }

        public void GoLeft()
        {

            this.position.X -= 5;

        }

        public void Stop()
        {

            this.position.X += 0;

        }



    }
}
