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
        static public int mudarDificuldade;
        /// <summary>
        /// Nivel do jogo, modifica o numero de canhoes e a probabilidade dos meteoros cairem
        /// </summary>
        static public int nivel = 1;
        /// <summary>
        /// Marca os pontos do jogo
        /// </summary>
        static public int Score = 0;        
        #endregion

        Camera2d Camera;

        static public NaveFilho NaveFilho_1;

        static public NaveFilho NaveFilho_2;

        #region float        
               
        /// <summary>
        /// Velocidade da tela de fundo
        /// </summary>
        float velocidadeFundo = 5f;
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
        /// Canhao 02
        /// </summary>
        shipCannon shipCannon2;
        /// <summary>
        /// Canhao 01
        /// </summary>
        shipCannon shipCannon;
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
        /// Projecoes da tela de jogo
        /// </summary>
        static public Rectangle viewportRect;
        #endregion

        


        #region bool

        bool mudarNivel = true;
      

        #endregion

        #region Color
        /// <summary>
        /// Array de cores da nave
        /// </summary>
        static public Color[] naveTextureDB;       
        #endregion

        #region Meteoros

        Meteor meteoros;

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
        static public elementoBenefico elementoBen;
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
        static public ParticleEffect myEffect;
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
        static public naveprincipal naveMae;
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
            graphics.PreferredBackBufferHeight = 800;
            
        }
        #endregion
        
        #region Initialize
        protected override void Initialize()
        {
            naveMae = new naveprincipal(Content.Load<Texture2D>("Sprites\\Mother_3"), viewportRect);

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
            myEffect = Content.Load<ParticleEffect>(@"EffectLibrary\CampFire");
            myEffect.LoadContent(this.Content);
            myEffect.Initialise();
            myRenderer.LoadContent(Content);

            Camera = new Camera2d();
            Camera.Pos = new Vector2(400.0f, 400.0f);

            meteoros = new Meteor(Content.Load<Texture2D>("Sprites\\meteoro_teste"));

            NaveFilho_1 = new NaveFilho(Content.Load<Texture2D>("Sprites\\StarShip_Filho"), viewportRect.Width / 2);
            NaveFilho_2 = new NaveFilho(Content.Load<Texture2D>("Sprites\\StarShip_Filho"), viewportRect.Width/2 - 100);

            shipCannon = new shipCannon(Content.Load<Texture2D>("Sprites\\shipCannon"));
            shipCannon2 = new shipCannon(Content.Load<Texture2D>("Sprites\\shipCannon"));

            HUD = Content.Load<SpriteFont>("Fonts\\HUD");

            meteorTexture = Content.Load<Texture2D>("Sprites\\meteoro_teste");
            //meteorTexture = Content.Load<Texture2D>("Sprites\\1");

            Debug = Content.Load<SpriteFont>("Fonts\\Score");

            Fundo = Content.Load<Texture2D>("Sprites\\background");           

            elementoBen = new elementoBenefico(Content.Load<Texture2D>("Sprites\\insgrocha"), Content.Load<Texture2D>("Sprites\\insignacoracao"));

            shipCannon2.position = new Vector2(Window.ClientBounds.Width / 2 - shipCannon.sprite.Width / 2 + 110,
               Window.ClientBounds.Height - 100);

            shipCannon.position = new Vector2(Window.ClientBounds.Width / 2 - shipCannon.sprite.Width / 2 - 65,
                Window.ClientBounds.Height - 120);

            shipCannon.center = new Vector2(shipCannon.sprite.Width / 2, shipCannon.sprite.Height / 2 - 30);

          
        }
        #endregion
       
        #region Update
        protected override void Update(GameTime gameTime)
        { 
            vidaNave = new BarraEnergia(naveMae.vidaNave, Content.Load<Texture2D>("Sprites\\HealthBar2"),
                new Vector2(255, 785));

            Rectangle Naveretangulo =
               new Rectangle((int)naveMae.posicaoNave.X, (int)naveMae.posicaoNave.Y,
               naveMae.texturaNave.Width, naveMae.texturaNave.Height);

            // Update each block
            naveMae.hitNave = false;
            
            KeyboardState keyboradState = Keyboard.GetState();

            GameTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboradState.IsKeyDown(Keys.W))
                Camera.Zoom += 0.1f;

            if (keyboradState.IsKeyDown(Keys.S))
                Camera.Zoom -= 0.1f;

            if (keyboradState.IsKeyDown(Keys.D))
                Camera.Move(new Vector2(10, 0));

            if (keyboradState.IsKeyDown(Keys.A))
                Camera.Move(new Vector2(-10, 0));



            if (keyboradState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (Score == 20 && mudarNivel == true)
            {
                meteoros.timeProbability -= 0.05f;                
                nivel = 2;
                mudarNivel = false;
            }

            if (keyboradState.IsKeyDown(Keys.Space) && previousKeyboradState.IsKeyUp(Keys.Space))
            {
                FireCannonMissiles();
            }
            previousKeyboradState = keyboradState;

            #region MovimentoFundo
            if (fundopos.Y >= tamanhoTela.Y)
                fundopos.Y = 0;

            fundopos.Y += velocidadeFundo;

            #endregion

            if (naveMae.vidaNave <= 0)
            {
                this.Exit();
            }

            if (nivel >= 2)
            {
                shipCannon2.Update();
            }

            shipCannon.Update();
       
            // Chamando a funcao que atualiza os misseis
            UpdadeCannonMissiles();

            // Chamando a funcao que atualiza os meteoros
            meteoros.Updatmeteor(gameTime);
           
            // Chamando o update da nave mae
            naveMae.Update(gameTime);
           
            NaveFilho_1.Update(gameTime);

            if (nivel == 2 && NaveFilho_1.alive)
            {
                NaveFilho_2.Update(gameTime);
            }

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
            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                                     BlendState.AlphaBlend,
                                     null,
                                     null,
                                     null,
                                     null,
                                     Camera.get_transformation(GraphicsDevice));

            // Desenahando nossa Nave mar
            naveMae.Draw(spriteBatch);
            // Desenhando nossa barra de vida da Nave mae
            vidaNave.Draw(spriteBatch);
            // Desenhando nosso elemento benefico
            elementoBen.Draw(spriteBatch);
            // Desenhando nossa Nave filho
            NaveFilho_1.Draw(spriteBatch);               
            // Desenhando nossos meteoros
            meteoros.Draw(spriteBatch);
            // Desenhando nosso primeiro canhao
            shipCannon.Draw(spriteBatch);
            // Desenhando nosso segundo canhao
            if (nivel == 2)
            {
                shipCannon2.Draw(spriteBatch);
            }
            // Desenhando a nave filho
            if (nivel == 2)
                NaveFilho_2.Draw(spriteBatch);

            spriteBatch.DrawString(Debug, "Score : " + Score, new Vector2(0, 0), Color.White, 0, Vector2.Zero, 1f,
                SpriteEffects.None, 0.1f);

            spriteBatch.DrawString(HUD, "Nave : " + naveMae.vidaNave, new Vector2(0, 20), Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

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
                if (Shoot2.alive)
                {
                    spriteBatch.Draw(Shoot2.sprite, Shoot2.position, null, Color.White,
                        Shoot2.rotation, Shoot2.center, 1.0f, SpriteEffects.None, 0.4f);
                }
            }
            // Desenhando nosso primeiro canhao
            shipCannon.Draw(spriteBatch);
            // Desenhando nosso segundo canhao
            if (nivel == 2)
            {
                //shipCannon2.Draw(spriteBatch);
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
            GameObject Shoot = new GameObject(Content.Load<Texture2D>("Sprites\\Tiro"));
            GameObject Shoot2 = new GameObject(Content.Load<Texture2D>("Sprites\\Tiro"));

            Shoot.alive = true;
            Shoot.position.X = shipCannon.position.X;
            Shoot.position.Y = shipCannon.position.Y;
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
                
        #region IntersectPixels
        static public bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
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
