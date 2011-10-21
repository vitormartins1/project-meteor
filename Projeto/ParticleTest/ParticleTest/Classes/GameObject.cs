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
    public class GameObject 
    {
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public Rectangle Rect;
        



        public GameObject(Texture2D loadedTexture)
        {
            

            sprite = loadedTexture;
            position = Vector2.Zero;
            rotation = 0.0f;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            Rect = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            alive = false;
        }
    }
}
