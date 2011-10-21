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
    public class Meteor : GameObject
    {
               
        /// <summary>
        /// Lista das posiçoes do meteoro
        /// </summary>
        List<Vector2> meteorPositions;        
        /// <summary>
        /// Array de cores dos meteoros
        /// </summary>
        Color[] meteorTextureDB;
        /// <summary>
        /// Randomiza a propabilidade dos blocos cairem
        /// </summary>
        Random random;
        double time;
        /// <summary>
        /// Lista da vida de cada meteoro
        /// </summary>
        List<int> ListaVida;
        /// <summary>
        /// Velocidade de caida dos meteoros
        /// </summary>
        /// <summary>
        /// Vida do meteoro
        /// </summary>
        public int life;
        /// <summary>
        /// Velocidade dos meteoros
        /// </summary>
        static public int BlockFallSpeed;
        /// <summary>
        /// Probabilidade dos meteoros cairem
        /// </summary>
        static public float BlockSpawnProbability;
        /// <summary>
        /// Probabilidade do tempo entre a caida dos meteoros
        /// </summary>
        public float timeProbability;
       
        
        public Meteor(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            meteorPositions = new List<Vector2>();                     
            random = new Random();
            meteorTextureDB =
             new Color[sprite.Width * sprite.Height];
            
            sprite.GetData(meteorTextureDB);         
            BlockFallSpeed = 2;
            time = 0;         
            BlockSpawnProbability = 10f;
            timeProbability = 0.7f;
            this.life = 2;
            ListaVida = new List<int>();
        }

        #region Updatemeteor

        public void Updatmeteor(GameTime gameTime)
        {
            // Cria o objeto do tipo meteoro

            Console.WriteLine(ListaVida.Count);

            #region Gerador de posicoes meteoros

            time += gameTime.ElapsedGameTime.TotalSeconds;

            if (random.NextDouble() < BlockSpawnProbability)
            {
                float x = (float)random.NextDouble() *
                    (Game1.viewportRect.Width - sprite.Width);
                
                

                if (time > timeProbability)
                {
                    meteorPositions.Add(new Vector2(x, -sprite.Height));
                    ListaVida.Add(2);
                    time = 0;
                }
            }
            #endregion

            

            for (int i = 0; i < meteorPositions.Count; i++)
            {
                meteorPositions[i] = new Vector2(meteorPositions[i].X,
                                meteorPositions[i].Y + BlockFallSpeed);

                Game1.myEffect.Trigger(new Vector2(meteorPositions[i].X + this.sprite.Width / 2, meteorPositions[i].Y));

                //Cria o retangulo do meteoro
                this.Rect = new Rectangle((int)meteorPositions[i].X, (int)meteorPositions[i].Y,
                   sprite.Width, sprite.Height);
                
                
                this.alive = true;

                #region Remove os Meteoros da Tela
                //Verifica se o meteoro saiu dos limites da tela e caso isso aconteca ele destroi o meteoro
                if (meteorPositions[i].Y > Game1.viewportRect.Height + sprite.Height * 2)
                {
                    meteorPositions.RemoveAt(i);
                    i--;
                }
                #endregion


                if (Game1.IntersectPixels(Game1.NaveFilho_1.Rect, Game1.NaveFilho_1.Db, this.Rect, meteorTextureDB) && this.alive == true && Game1.NaveFilho_1.alive)
                {

                    try
                    {

                        Game1.NaveFilho_1.boolVermelhidao = true;
                        this.alive = false;
                        Game1.NaveFilho_1.life--;
                        ListaVida.RemoveAt(i);                       
                        meteorPositions.RemoveAt(i);
                        i--;
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }

                if (Game1.nivel == 2 && Game1.NaveFilho_2.Rect.Intersects(this.Rect) && this.alive == true && Game1.NaveFilho_1.alive)
                {
                    try
                    {
                        Game1.NaveFilho_2.boolVermelhidao = true;
                        this.alive = false;
                        ListaVida.RemoveAt(i);                        
                        Game1.NaveFilho_2.life--;
                        meteorPositions.RemoveAt(i);
                        i--;
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }

                //Verifica se a navemae colidiu com o meteoro
                if (Game1.IntersectPixels(Game1.naveMae.retanguloDaNave, Game1.naveTextureDB,
                                    this.Rect, meteorTextureDB))
                {
                    Game1.naveMae.boolVermelhidao = true;                              
                    //Remove o meteoro
                    meteorPositions.RemoveAt(i);
                    this.alive = false;
                    //Diminui os pontos de vida da nave mae
                    Game1.naveMae.vidaNave -= 1;
                    ListaVida.RemoveAt(i);        
                    i--;
                }


                for (int j = 0; j < Game1.cannonMissiles.Count; j++)
                {
                    //Verifica de os meteoros colidiram com os misseis do canhao 01
                    if (this.Rect.Intersects(Game1.cannonMissiles[j].Rect) && this.alive == true)
                    {
                        if(this.alive == true)
                        ListaVida[i] -= 1;

                       
                         
                         if (ListaVida[i] <= 0)
                        {
                            //Se colidiram, isso significa que um meteoro foi destruido e o jogador ganha pontos
                            Game1.Score += 1;
                            //Game1.mudarDificuldade++;

                            Game1.elementoBen.ElementoMeteor(new Vector2(this.Rect.X, this.Rect.Y));

                            //Remove o misseldo canhao 01 da tela
                             Game1.cannonMissiles.RemoveAt(j);

                            
                             

                            //Reproduz o efeito de particulas
                            for (int a = 0; a < 60; a++)
                            {
                                Game1.myEffect.Trigger(new Vector2(this.Rect.X, this.Rect.Y));
                            }

                            //Se nao estiver colidindo com a nave mae o meteoro e removido da lista,
                            //esta condicao e feita pois se voce atirar no meteoro e ele estiver 
                            //colidindo ao mesmo tempo com a nave dois comandos manda eles sair da lista, causando um erro
                            if (!Game1.IntersectPixels(Game1.naveMae.retanguloDaNave, Game1.naveTextureDB,
                                        this.Rect, meteorTextureDB))
                            {
                                try
                                {
                                    this.alive = false;
                                    ListaVida.RemoveAt(i);
                                    meteorPositions.RemoveAt(i);
                                    i--;
                                    break;
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    break;
                                }
                            }

                            
                        }

                         if (ListaVida[i] >= 1 && this.alive == true)
                         {
                             //Remove o missel do canhao 01 da tela
                             Game1.cannonMissiles.RemoveAt(j);
                         }
                       
                    }
                }
                for (int u = 0; u < Game1.cannonMissiles2.Count; u++)
                {
                    //Verifica se o missel do canhao 02 colidiu com o meteoro
                    if (Game1.cannonMissiles2[u].Rect.Intersects(this.Rect) && this.alive == true)
                    {

                        life--;

                        if (life <= 0)
                        {
                            //Se colidiram, isso significa que um meteoro foi destruido e o jogador ganha pontos
                            Game1.Score += 1;
                            Game1.mudarDificuldade++;

                            Game1.elementoBen.ElementoMeteor(new Vector2(this.Rect.X, this.Rect.Y));

                            for (int a = 0; a < 60; a++)
                            {
                                Game1.myEffect.Trigger(new Vector2(this.Rect.X, this.Rect.Y));
                            }

                            //Remove o missel da tela
                            Game1.cannonMissiles2.RemoveAt(u);
                        }

                        //Se nao estiver colidindo com a nave mae o meteoro e removido da lista,
                        //esta condicao e feita pois se voce atirar no meteoro e ele estiver 
                        //colidindo ao mesmo tempo com a nave dois comandos manda eles sair da lista, causando um erro
                        if (!Game1.IntersectPixels(Game1.naveMae.retanguloDaNave, Game1.naveTextureDB,
                                    this.Rect, meteorTextureDB) && this.alive == true)
                        {
                            try
                            {
                                ListaVida.RemoveAt(i);                               
                                meteorPositions.RemoveAt(i);
                                i--;
                                break;
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }//end UpdateMeteors
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            //Desenhando os meteoros
            foreach (Vector2 meteorPosition in meteorPositions)
            {
                spriteBatch.Draw(sprite, meteorPosition, null, Color.White, 0,
                    Vector2.Zero, 1.0f, SpriteEffects.None, 0.3f);
            }
        }
    }
}
