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
    public class elementoBenefico
    {
        Texture2D elementoTex1;
        Texture2D elementoTex2;

        Vector2 PosicaoMeteoro;
                
        static private ElementoScene currentScene;
        //public List<Vector2> blockPositions = new List<Vector2>();
        public List<Vector2> elementoRandom = new List<Vector2>();
        public List<Vector2> elementoBem = new List<Vector2>();
        public bool isAlive;

        int elementoSpeed = 2;
        public double time = 0;
        float elementoProbability = 0.1f;
        public float posicaoRect = 0f;
        public bool isPlaying; 
        public int VarRandom;
        Random random = new Random();	

        

        public elementoBenefico(Texture2D texturaElemento1, Texture2D texturaElemento2)
        {
            elementoTex1 = texturaElemento1;
            elementoTex2 = texturaElemento2;

            currentScene = ElementoScene.SCN_NORMAL;

            isAlive = false;

            
            isPlaying = true;

        }

        public void ElementoMeteor(Vector2 posicaoMeteoro)
        {
            PosicaoMeteoro.X = posicaoMeteoro.X;

            PosicaoMeteoro.Y = posicaoMeteoro.Y;



            if (random.NextDouble() < elementoProbability && currentScene == ElementoScene.SCN_NORMAL)
            {
                VarRandom = random.Next(1, 3);

                switch (VarRandom)
                {
                    case 1 :
                        elementoRandom.Add(new Vector2(PosicaoMeteoro.X, PosicaoMeteoro.Y));

                        break;

                    case 2 :
                        elementoBem.Add(new Vector2(PosicaoMeteoro.X, PosicaoMeteoro.Y));
                        break;
                }

               
            }
        }

        public void Update2(GameTime gameTime)
        {
            #region EFEITOS


            if (currentScene != ElementoScene.SCN_NORMAL)
            {
                
                time += gameTime.ElapsedGameTime.TotalSeconds;

                if (time > 5)
                {
                    currentScene = ElementoScene.SCN_NORMAL;

                    time = 0;
                }

            }

            switch (currentScene)
            {

                case ElementoScene.SCN_VELOCIDADE:
                    Meteor.BlockFallSpeed = 20;
                    //isPlaying = true;
                    break;

                case ElementoScene.SCN_NORMAL:
                    time = 0;
                    //isPlaying = false;
                    Meteor.BlockFallSpeed = 5;
                    break;

                case ElementoScene.SCN_LIFE:

                    if (Game1.naveMae.vidaNave < 20)
                    Game1.naveMae.vidaNave = 20;
                    //isPlaying = true;

                    currentScene = ElementoScene.SCN_NORMAL;
                    break;
            }

            
         


            // Update each block
            for (int i = 0; i < elementoRandom.Count; i++)
            {
                // Animate this block falling
                elementoRandom[i] = new Vector2(elementoRandom[i].X + elementoSpeed, elementoRandom[i].Y);
               
                Rectangle Rect = new Rectangle((int)elementoRandom[i].X, (int)elementoRandom[i].Y, elementoTex1.Width,
                        elementoTex1.Height);
                

                                                 


                if (elementoRandom[i].X > Game1.viewportRect.Width)
                {

                    elementoRandom.RemoveAt(i);
                    i--;
                }

                for (int j = 0; j < Game1.cannonMissiles.Count; j++)
                {
                        if (Game1.cannonMissiles[j].Rect.Intersects(Rect))
                        {
                        
                          
                            try
                            {
                                currentScene = ElementoScene.SCN_VELOCIDADE;
                                isAlive = false;
                                elementoRandom.RemoveAt(i);
                                i--;

                                break;
                            }

                            catch(ArgumentOutOfRangeException)
                            {
                                break;
                            }


                        }


                        posicaoRect = Rect.X; 
                }

                for (int j = 0; j < Game1.cannonMissiles2.Count; j++)
                {
                    if (Game1.cannonMissiles2[j].Rect.Intersects(Rect))
                    {

                                                               

                        try
                        {
                            currentScene = ElementoScene.SCN_VELOCIDADE;
                            isAlive = false;
                            elementoRandom.RemoveAt(i);
                            i--;

                            break;
                        }

                        catch (ArgumentOutOfRangeException)
                        {
                            break;
                        }


                    }


                    posicaoRect = Rect.X;
                }
   

            }

                  
        
            #endregion

            for (int i = 0; i < elementoBem.Count; i++)
            {
                // Animate this block falling
                elementoBem[i] = new Vector2(elementoBem[i].X + elementoSpeed, elementoBem[i].Y);

                Rectangle Rect = new Rectangle((int)elementoBem[i].X, (int)elementoBem[i].Y, elementoTex1.Width,
                        elementoTex1.Height);





                if (elementoBem[i].X > Game1.viewportRect.Width)
                {

                    elementoBem.RemoveAt(i);
                    i--;
                }

                for (int j = 0; j < Game1.cannonMissiles.Count; j++)
                {
                    if (Game1.cannonMissiles[j].Rect.Intersects(Rect))
                    {


                        try
                        {
                            currentScene = ElementoScene.SCN_LIFE;
                            isAlive = false;
                            elementoBem.RemoveAt(i);
                            

                            break;
                        }

                        catch (ArgumentOutOfRangeException)
                        {
                            break;
                        }


                    }


                    posicaoRect = Rect.X;
                }

                for (int j = 0; j < Game1.cannonMissiles2.Count; j++)
                {
                    if (Game1.cannonMissiles2[j].Rect.Intersects(Rect))
                    {



                        try
                        {
                            currentScene = ElementoScene.SCN_LIFE;
                            isAlive = false;
                            elementoBem.RemoveAt(i);
                            i--;

                            break;
                        }

                        catch (ArgumentOutOfRangeException)
                        {
                            break;
                        }


                    }


                    posicaoRect = Rect.X;
                }


            }


        }



            
        
        
        public void Draw(SpriteBatch thisSpritebatch)
        {


            foreach (Vector2 PosicaoElemento in elementoRandom)
            {
                
                        thisSpritebatch.Draw(elementoTex1, PosicaoElemento, null, Color.White, 0,
                            Vector2.Zero, 1.0f, SpriteEffects.None, 0.3f);
               
            }

            foreach (Vector2 PosicaoElemento in elementoBem)
            {

                thisSpritebatch.Draw(elementoTex2, PosicaoElemento, null, Color.White, 0,
                    Vector2.Zero, 1.0f, SpriteEffects.None, 0.3f);

            }
                    
        


                 
                   
            
        }

        public enum ElementoScene
        {
            SCN_VELOCIDADE, SCN_LIFE, SCN_NORMAL 
        }

        


    }
}
