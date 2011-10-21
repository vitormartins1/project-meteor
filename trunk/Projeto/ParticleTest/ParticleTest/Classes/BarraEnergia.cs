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
    class BarraEnergia
    {
        public Texture2D mHealthBar;
        public int mCurrentHealth;
        Vector2 position;
        public int tamanhoDaBarra;

        public BarraEnergia(int life, Texture2D textura, Vector2 HUDposition)
        {
            mCurrentHealth = life;
            mHealthBar = textura;
            position = HUDposition;
            tamanhoDaBarra = 11;
            
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            //Desenha o fundo da barra de vida
            theSpriteBatch.Draw(mHealthBar, new Rectangle((int)position.X,
               (int)position.Y, mHealthBar.Width - 158, tamanhoDaBarra), new Rectangle(0, 45, mHealthBar.Width, 44), Color.LightGray, 0.0f, Vector2.Zero, SpriteEffects.None, 0.10f);

            //Desenha a barra de vida
            theSpriteBatch.Draw(mHealthBar, new Rectangle((int)position.X, (int)position.Y,
                (int)((mHealthBar.Width - 158) * ((double)mCurrentHealth / 20)), tamanhoDaBarra),
                 new Rectangle(0, 45, mHealthBar.Width, 44), Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 0.11f);

        }
    }
}
