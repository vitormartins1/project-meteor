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

//

namespace ParticleTest
{
    public class NaveFilho : GameObject
    {
        public int life;
        public int VarRandom;
        Random random;
        public double Navetimer;
        public double TimeMovimento;
        public Color[] Db;
        public Color currentColor;
        public bool boolVermelhidao;
        public float Velocidade;
        static public int direcao;


        public NaveFilho(Texture2D loadedTexture, int Px)
            : base(loadedTexture)
        {

            random = new Random();
            sprite = loadedTexture;
            position = new Vector2(Px, Game1.viewportRect.Height / 2);
            rotation = 0.0f;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            Db = new Color[sprite.Width * sprite.Height];
            sprite.GetData(Db);
            alive = true;
            life = 5;
            TimeMovimento = 0;
            VarRandom = random.Next(1, 4);
            currentColor = Color.White;
            boolVermelhidao = false;
            Velocidade = 5;
            direcao = 1;
        }

        public void Update(GameTime gameTime)
        {
            position.X += Velocidade * direcao;

            Rect = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);




            #region Movimentacao
            //position.X = MathHelper.Clamp((float)position.X, 0, viewportRect.Width - sprite.Width);


            if (position.X >= Game1.viewportRect.Width - sprite.Width)
            {
                position.X = Game1.viewportRect.Width - sprite.Width - 1;
                NaveFilho.direcao *= -1;
            }

            if (position.X <= 0)
            {
                position.X = +1;
                NaveFilho.direcao *= -1;
            }





            #endregion

            if (boolVermelhidao)
            {

                Vermelhidao(gameTime);
            }


        }




        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(sprite, position, null, currentColor,
                        rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);


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


    }
}
