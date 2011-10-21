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

using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace ParticleTest
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variaveis

        #region Graficos
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #endregion

        #region int
        int mudarDificuldade;
        /// <summary>
        /// Nivel do jogo, modifica o numero de canhoes e a probabilidade dos meteoros cairem
        /// </summary>
        int nivel = 1;
        /// <summary>
        /// Marca os pontos do jogo
        /// </summary>
        int Score = 0;
        /// <summary>
        /// Velocidade de caida dos meteoros
        /// </summary>
        const int BlockFallSpeed = 5;
        #endregion

        #region float
        /// <summary>
        /// Rotacao do sprite do meteoro
        /// </summary>
        float girarMeteoro = 0f;
        /// <summary>
        /// Probabilidade dos meteoros cairem
        /// </summary>
        float BlockSpawnProbability = 0.9f;
        /// <summary>
        /// Probabilidade do tempo entre a caida dos meteoros
        /// </summary>
        float timeProbability = 1f;
        /// <summary>
        /// Velocidade da tela de fundo
        /// </summary>
        float velocidadeFundo = 1f;
        #endregion

        #region double
        /// <summary>
        /// Contabiliza os milisegundos do jogo
        /// </summary>
        double GameTime = 0;
        /// <summary>
        /// Guarda o tempo de jogo
        /// </summary>
        double time = 0;
        #endregion

        #region GameObject
        /// <summary>
        /// Misseis do canhao 01
        /// </summary>
        static public List<GameObject> cannonMissiles = new List<GameObject>();
        /// <summary>
        /// Misseis do canhao 02
        /// </summary>
        static public List<GameObject> cannonMissiles2 = new List<GameObject>();
        /// <summary>
        /// Lista de meteoros
        /// </summary>
        List<GameObject> Meteors = new List<GameObject>();
        /// <summary>
        /// Canhao 02
        /// </summary>
        GameObject shipCannon2;
        /// <summary>
        /// Canhao 01
        /// </summary>
        GameObject shipCannon;
        #endregion

        #region Vector2
        /// <summary>
        /// Lista das posicoes dos meteoros
        /// </summary>
        List<Vector2> meteorPositions = new List<Vector2>();
        /// <summary>
        /// Posicao de fundo da textura de fundo
        /// </summary>
        Vector2 fundopos;
        /// <summary>
        /// Tamanho da tela de fundo
        /// </summary>
        public Vector2 tamanhoTela;
        #endregion

        #region Rectangle
        /// <summary>
        /// Lista de Rects dos meteoros
        /// </summary>
        List<Rectangle> meteorRects = new List<Rectangle>();
        /// <summary>
        /// Projecoes da tela de jogo
        /// </summary>
        static public Rectangle viewportRect;
        #endregion

        #region bool
    
        #endregion

        #region Color
        /// <summary>
        /// Array de cores da nave
        /// </summary>
        Color[] naveTextureDB;
        /// <summary>
        /// Array de cores dos meteoros
        /// </summary>
        Color[] meteorTextureDB;
        #endregion

        #region Texture2D
        /// <summary>
        /// Textura de fundo
        /// </summary>
        Texture2D Fundo;
        /// <summary>
        /// Textura do meteoro
        /// </summary>
        Texture2D meteorTexture;
        #endregion

        #region BarraEnergia
        /// <summary>
        /// Barra de energia da nave
        /// </summary>
        BarraEnergia vidaNave;
        #endregion

        #region ElementoBenefico
        /// <summary>
        /// Elementos pontuativos do jogo
        /// </summary>
        elementoBenefico elementoBen;
        #endregion

        #region SpriteFont
        /// <summary>
        /// Debug de informacoes necessarias
        /// </summary>
        SpriteFont Debug;
        /// <summary>
        /// HUD do jogo
        /// </summary>
        SpriteFont HUD;
        #endregion

        #region Renderer and ParticleEffect
        /// <summary>
        /// Desenha particulas na tela
        /// </summary>
        Renderer myRenderer;
        /// <summary>
        /// Objeto que guarda informacoes da minha particula
        /// </summary>
        ParticleEffect myEffect;
        #endregion

        #region Random
        /// <summary>
        /// Randomiza a propabilidade dos blocos cairem
        /// </summary>
        Random random = new Random();
        #endregion

        #region navePrincipal
        /// <summary>
        /// Nave do nosso jogo
        /// </summary>
        naveprincipal naveMae;
        #endregion

        #region Imputs
        /// <summary>
        /// Tecla pressionada anteriormente
        /// </summary>
        KeyboardState previousKeyboradState = Keyboard.GetState();
        #endregion

        #endregion

        #region Construtor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Create new renderer and set its graphics devide to "this" device
            myRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };
            myEffect = new ParticleEffect();

            //graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }
        #endregion

        #region Initialize
        protected override void Initialize()
        {
            naveMae = new naveprincipal(Content.Load<Texture2D>("Sprites\\Mother_2"), viewportRect);

            naveTextureDB =
                new Color[naveMae.texturaNave.Width * naveMae.texturaNave.Height];
            naveMae.texturaNave.GetData(naveTextureDB);

            viewportRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            tamanhoTela = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            base.Initialize();
        }
        #endregion

        #region LoadContent
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load particle effects
            myEffect = Content.Load<ParticleEffect>(@"EffectLibrary\BasicExplosion");
            myEffect.LoadContent(this.Content);
            myEffect.Initialise();
            myRenderer.LoadContent(Content);

            shipCannon = new GameObject(Content.Load<Texture2D>("Sprites\\shipCannon"));

            HUD = Content.Load<SpriteFont>("Fonts\\HUD");

            meteorTexture = Content.Load<Texture2D>("Sprites\\meteoro_marron");
            //meteorTexture = Content.Load<Texture2D>("Sprites\\1");

            Debug = Content.Load<SpriteFont>("Fonts\\Score");

            Fundo = Content.Load<Texture2D>("Sprites\\background");

            shipCannon2 = new GameObject(Content.Load<Texture2D>("Sprites\\shipCannon"));

            elementoBen = new elementoBenefico(Content.Load<Texture2D>("Sprites\\insgrocha"), Content.Load<Texture2D>("Sprites\\insignacoracao"));

            shipCannon2.position = new Vector2(Window.ClientBounds.Width / 2 - shipCannon.sprite.Width / 2 + 110,
               Window.ClientBounds.Height - 100);

            shipCannon.position = new Vector2(Window.ClientBounds.Width / 2 - shipCannon.sprite.Width / 2 - 80,
                Window.ClientBounds.Height - 100);

            meteorTextureDB =
               new Color[meteorTexture.Width * meteorTexture.Height];
            meteorTexture.GetData(meteorTextureDB);

        }
        #endregion

        #region UnloadContent
        protected override void UnloadContent()
        {
        }
        #endregion

        #region Update
        protected override void Update(GameTime gameTime)
        {
            //Gira o meteoro ao redor de si
            //girarMeteoro += 0.04f;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            vidaNave = new BarraEnergia(naveMae.vidaNave, Content.Load<Texture2D>("Sprites\\HealthBar2"),
                new Vector2(5, 400));

            Rectangle Naveretangulo =
               new Rectangle((int)naveMae.posicaoNave.X, (int)naveMae.posicaoNave.Y,
               naveMae.texturaNave.Width, naveMae.texturaNave.Height);

            // Update each block
            naveMae.hitNave = false;



            #region Gerador de posicoes meteoros

            time += gameTime.ElapsedGameTime.TotalSeconds;

            if (random.NextDouble() < BlockSpawnProbability)
            {
                float x = (float)random.NextDouble() *
                    (Window.ClientBounds.Width - meteorTexture.Width);

                if (time > timeProbability)
                {
                    meteorPositions.Add(new Vector2(x, -meteorTexture.Height));

                    time = 0;
                }
            }
            #endregion

            KeyboardState keyboradState = Keyboard.GetState();

            GameTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboradState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            
            if (mudarDificuldade % 20 == 0 && mudarDificuldade > 0)
            {
                this.timeProbability -= 0.2f;
                mudarDificuldade = 0;
                nivel = 2;
            }

            /*if (Score > 20)
            {
                this.timeProbability = 0.8f;
                nivel = 2;
            }

            if (Score > 40)
            {
                this.timeProbability = 0.6f;
                nivel = 2;
            }

            if (Score > 60)
            {
                this.timeProbability = 0.4f;
                nivel = 2;
            }

            if (Score > 80)
            {
                this.timeProbability = 0.2f;
                nivel = 2;
            }*/

            #region MovimentoFundo
            if (fundopos.Y >= tamanhoTela.Y)
                fundopos.Y = 0;

            fundopos.Y += velocidadeFundo;

            #endregion

            if (naveMae.vidaNave <= 0)
                this.Exit();

            #region Inputs

            if (keyboradState.IsKeyDown(Keys.Left))
            {
                if (nivel == 2)
                    shipCannon2.rotation = shipCannon.rotation;

                shipCannon.rotation -= 0.05f;
            }

            if (keyboradState.IsKeyDown(Keys.Right))
            {
                if (nivel == 2)
                    shipCannon2.rotation = shipCannon.rotation;

                shipCannon.rotation += 0.05f;
            }

            shipCannon.rotation = MathHelper.Clamp(shipCannon.rotation, -MathHelper.PiOver2 + 0.3f, MathHelper.PiOver2 - 0.3f);

            if (keyboradState.IsKeyDown(Keys.Space) && previousKeyboradState.IsKeyUp(Keys.Space))
            {
                FireCannonMissiles();
            }
            previousKeyboradState = keyboradState;

            #endregion

            // Chamando a funcao que atualiza os misseis
            UpdadeCannonMissiles();

            // Chamando a funcao que atualiza os meteoros
            Updatmeteor();

            // Chamando o update da nave mae
            naveMae.Update(gameTime);

            elementoBen.Update2(gameTime);
            
            MouseState ms = Mouse.GetState();
            // Check if mouse left button was presed
            if (ms.LeftButton == ButtonState.Pressed)
            {
                // Add new particle effect to mouse coordinates
                myEffect.Trigger(new Vector2(ms.X, ms.Y));
            }
            // "Deltatime" ie, time since last update call
            float SecondsPassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myEffect.Update(SecondsPassed);

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            naveMae.Draw(spriteBatch);
            vidaNave.Draw(spriteBatch);
            elementoBen.Draw(spriteBatch);

            spriteBatch.DrawString(Debug, "Score : " + Score, new Vector2(0, 0), Color.White, 0, Vector2.Zero, 1f,
                SpriteEffects.None, 0.1f);

            spriteBatch.DrawString(HUD, "Nave : " + naveMae.vidaNave, new Vector2(0, 20), Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

            spriteBatch.DrawString(HUD, "Probabilidade de cair meteoros : " + this.BlockSpawnProbability + " Probabilidade de tempo " + this.timeProbability, new Vector2(0, 40), Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

            //Desenhando os meteoros
            foreach (Vector2 meteorPosition in meteorPositions)
            {
                spriteBatch.Draw(meteorTexture, meteorPosition, null, Color.White, 0f,
                    Vector2.Zero, 1.0f, SpriteEffects.None, 0.3f);
            }

            //Desenhando os misseis
            foreach (GameObject Shoot in cannonMissiles)
            {
                if (Shoot.alive)
                {
                    spriteBatch.Draw(Shoot.sprite, Shoot.position, null, Color.White,
                        Shoot.rotation, Shoot.center, 1.0f, SpriteEffects.None, 0.4f);
                }
            }

            foreach (GameObject Shoot2 in cannonMissiles2)
            {
                //spriteBatch.DrawString(Debug, "" + Shoot.position, new Vector2(0, Shoot.position.Y), Color.Black);
                if (Shoot2.alive)
                {
                    spriteBatch.Draw(Shoot2.sprite, Shoot2.position, null, Color.White,
                        Shoot2.rotation, Shoot2.center, 1.0f, SpriteEffects.None, 0.4f);
                }
            }

            spriteBatch.Draw(shipCannon.sprite, shipCannon.position, null, Color.White, shipCannon.rotation, shipCannon.center, 1f, SpriteEffects.None, 0.5f);

            if (nivel == 2)
            {
                spriteBatch.Draw(shipCannon.sprite, shipCannon2.position, null, Color.White, shipCannon2.rotation, shipCannon2.center, 1f, SpriteEffects.None, 0.5f);
            }

            #region Movimento do Fundo
            spriteBatch.Draw(Fundo, new Rectangle((int)fundopos.X, (int)fundopos.Y, (int)tamanhoTela.X, (int)tamanhoTela.Y), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(Fundo, new Rectangle((int)fundopos.X, (int)fundopos.Y - (int)tamanhoTela.Y, (int)tamanhoTela.X, (int)tamanhoTela.Y),
            null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
            #endregion

            spriteBatch.End();

            //Renderiza o efeito
            myRenderer.RenderEffect(myEffect);

            base.Draw(gameTime);
        }
        #endregion

        #region  UpdadeCannonMissiles

        static public void UpdadeCannonMissiles()
        {
            for (int i = 0; i < cannonMissiles.Count; i++)
            {
                if (cannonMissiles[i].alive)
                {
                    cannonMissiles[i].position += cannonMissiles[i].velocity;

                    cannonMissiles[i].Rect = new Rectangle((int)cannonMissiles[i].position.X,
                        (int)cannonMissiles[i].position.Y, cannonMissiles[i].sprite.Width, cannonMissiles[i].sprite.Height);
                }

                if (!viewportRect.Contains(new Point((int)cannonMissiles[i].position.X, (int)cannonMissiles[i].position.Y)))
                {
                    cannonMissiles.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < cannonMissiles2.Count; i++)
            {
                if (cannonMissiles2[i].alive)
                {
                    cannonMissiles2[i].position += cannonMissiles2[i].velocity;

                    cannonMissiles2[i].Rect = new Rectangle((int)cannonMissiles2[i].position.X,
                        (int)cannonMissiles2[i].position.Y, cannonMissiles2[i].sprite.Width, cannonMissiles2[i].sprite.Height);
                }

                if (!viewportRect.Contains(new Point((int)cannonMissiles2[i].position.X, (int)cannonMissiles2[i].position.Y)))
                {
                    cannonMissiles2.RemoveAt(i);
                    i--;
                }
            }
        }//end UpdateMissiles

        #endregion

        #region FireCannonMissiles

        public void FireCannonMissiles()
        {
            GameObject Shoot = new GameObject(Content.Load<Texture2D>("Sprites\\cannonMissile"));
            GameObject Shoot2 = new GameObject(Content.Load<Texture2D>("Sprites\\cannonMissile"));

            Shoot.alive = true;
            Shoot.position = shipCannon.position;
            Shoot.rotation = shipCannon.rotation;
            Shoot.velocity = new Vector2((float)Math.Sin(shipCannon.rotation),
                (float)-Math.Cos(shipCannon.rotation)) * 10.0f;

            cannonMissiles.Add(Shoot);

            if (nivel == 2)
            {
                Shoot2.alive = true;
                Shoot2.position = shipCannon2.position;
                Shoot2.rotation = shipCannon2.rotation;
                Shoot2.velocity = new Vector2((float)Math.Sin(shipCannon2.rotation),
                    (float)-Math.Cos(shipCannon2.rotation)) * 10.0f;

                cannonMissiles2.Add(Shoot2);
            }
            return;
        }//end firemissiles

        #endregion

        #region Updatemeteor
        
        public void Updatmeteor()
        {
            // Cria o objeto do tipo meteoro

            int numeroDoRandom = random.Next(0, 2);
            
            GameObject meteor = new GameObject(Content.Load<Texture2D>("Sprites\\meteoro_teste"));
                  
            

            for (int i = 0; i < meteorPositions.Count; i++)
            {
                meteorPositions[i] = new Vector2(meteorPositions[i].X,
                                meteorPositions[i].Y + BlockFallSpeed);

                //Cria o retangulo do meteoro
                meteor.Rect = new Rectangle((int)meteorPositions[i].X, (int)meteorPositions[i].Y,
                   meteorTexture.Width, meteorTexture.Height);

                meteor.alive = true;

                #region Remove os Meteoros da Tela
                //Verifica se o meteoro saiu dos limites da tela e caso isso aconteca ele destroi o meteoro
                if (meteorPositions[i].Y > Window.ClientBounds.Height + meteorTexture.Height * 2)
                {
                    meteorPositions.RemoveAt(i);
                    i--;
                }
                #endregion

                //Verifica se a navemae colidiu com o meteoro
                if (  IntersectPixels(naveMae.retanguloDaNave, naveTextureDB,
                                    meteor.Rect, meteorTextureDB))
                {
                    //Remove o meteoro
                    meteorPositions.RemoveAt(i);

                    //Faz o efeito de particula
                    for (int a = 0; a < 60; a++)
                    {
                        myEffect.Trigger(new Vector2(meteor.Rect.X/2, meteor.Rect.Y / 2));
                    }

                    //Diminui os pontos de vida da nave mae
                    naveMae.vidaNave -= 1;
                    i--;
                }

                for (int j = 0; j < cannonMissiles.Count; j++)
                {
                    //Verifica de os meteoros colidiram com os misseis do canhao 01
                    if (cannonMissiles[j].Rect.Intersects(meteor.Rect))
                    {
                        //Se colidiram, isso significa que um meteoro foi destruido e o jogador ganha pontos
                        Score += 1;
                        mudarDificuldade++;

                        //Reproduz o efeito de particulas
                        for (int a = 0; a < 60; a++)
                        {
                           myEffect.Trigger(new Vector2(meteor.Rect.X, meteor.Rect.Y));
                        }

                        //Remove o misseldo canhao 01 da tela
                        cannonMissiles.RemoveAt(j);

                        //Se nao estiver colidindo com a nave mae o meteoro e removido da lista,
                        //esta condicao e feita pois se voce atirar no meteoro e ele estiver 
                        //colidindo ao mesmo tempo com a nave dois comandos manda eles sair da lista, causando um erro
                        if (!IntersectPixels(naveMae.retanguloDaNave, naveTextureDB,
                                    meteor.Rect, meteorTextureDB))
                        {
                            try
                            {
                                meteor.alive = false;
                                meteorPositions.RemoveAt(i);
                                i--;
                                break;
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                break;
                            }
                            
                                                        
                        }
                    }                    
                }
                for (int u = 0; u < cannonMissiles2.Count; u++)
                {
                    //Verifica se o missel do canhao 02 colidiu com o meteoro
                    if (cannonMissiles2[u].Rect.Intersects(meteor.Rect) && meteor.alive == true)
                    {
                        //Se colidiram, isso significa que um meteoro foi destruido e o jogador ganha pontos
                        Score += 1;
                        mudarDificuldade++;

                        for (int a = 0; a < 60; a++)
                        {
                            myEffect.Trigger(new Vector2(meteor.Rect.X, meteor.Rect.Y));
                        }

                        //Remove o missel da tela
                        cannonMissiles2.RemoveAt(u);

                        //Se nao estiver colidindo com a nave mae o meteoro e removido da lista,
                        //esta condicao e feita pois se voce atirar no meteoro e ele estiver 
                        //colidindo ao mesmo tempo com a nave dois comandos manda eles sair da lista, causando um erro
                        if (!IntersectPixels(naveMae.retanguloDaNave, naveTextureDB,
                                    meteor.Rect, meteorTextureDB) && meteor.alive == true)
                        {
                            try
                            {
                                meteorPositions.RemoveAt(i);



                                i--;
                                break;
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }//end UpdateMeteors
        #endregion

        #region IntersectPixels
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
