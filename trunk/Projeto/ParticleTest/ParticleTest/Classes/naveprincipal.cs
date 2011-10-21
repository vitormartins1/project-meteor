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
    public class naveprincipal
    {
        
        public Texture2D texturaNave;
        public Vector2 posicaoNave;
        //public Rectangle rectNave;
        public int vidaNave;
        //public Rectangle tela;
        public Rectangle retanguloDaNave;
        //public Game1 game1 = new Game1();
        public bool hitNave;

        public bool boolVermelhidao;

        public double Navetimer;

        public Color currentColor;
        

        public naveprincipal(Texture2D textura, Rectangle viewport)
        {
            texturaNave = textura;
            //rectNave = new Rectangle();
            vidaNave = 20;
            currentColor = Color.White;
            hitNave = false;
            boolVermelhidao = false;
            Navetimer = 0;
        }

        public void Update(GameTime gameTime)

        {
            //posicaoNave = new Vector2(game1.Window.ClientBounds.Width/2 - texturaNave.Width/2, game1.Window.ClientBounds.Height -140);

            posicaoNave = new Vector2(Game1.viewportRect.Width / 2 - texturaNave.Width / 2, Game1.viewportRect.Height - 140);
            
            retanguloDaNave = new Rectangle((int)posicaoNave.X,(int)posicaoNave.Y , texturaNave.Width, texturaNave.Height);
                        
            //vidaNave = (int)MathHelper.Clamp(vidaNave, 0, vidaNave);

            if (boolVermelhidao)
            {

                Vermelhidao(gameTime);
            }
        }

        public void Draw(SpriteBatch thisSpritebatch)
        {
            thisSpritebatch.Draw(texturaNave, posicaoNave, null, currentColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
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
