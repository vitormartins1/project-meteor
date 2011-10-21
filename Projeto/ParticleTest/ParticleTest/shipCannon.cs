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
    class shipCannon : GameObject
    {

        KeyboardState previousKeyboradState;

        public shipCannon(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            previousKeyboradState = Keyboard.GetState();
            sprite = loadedTexture;
        }

        public void Update()
        {
            #region Inputs

            KeyboardState keyboradState = Keyboard.GetState();

            if (keyboradState.IsKeyDown(Keys.Left))
            {
                
                this.rotation -= 0.05f;
            }

            if (keyboradState.IsKeyDown(Keys.Right))
            {

                this.rotation += 0.05f;
            }

            this.rotation = MathHelper.Clamp(this.rotation, -MathHelper.PiOver2 + 0.3f, MathHelper.PiOver2 - 0.3f);

            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.position, null, Color.White, this.rotation, this.center, 1f, SpriteEffects.None, 0.5f);
        }
    }
}
