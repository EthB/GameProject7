using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ParallaxStarter
{
    class Enemy : ISprite
    {
        public Rectangle bounds;
        Texture2D texture;
        Vector2 position;
        Random random = new Random();
        Player player;
        float xPos;

        public Enemy(Player player, Texture2D texture)
        {
            this.player = player;
            this.texture = texture;
            xPos = 600;
            position = new Vector2(player.Position.X + xPos, random.Next(50, 350));
            bounds = new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }

        public void Update(GameTime gameTime)
        {
            xPos -= 2;
            //Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * direction;
            position.X = player.Position.X + xPos;
            bounds.X = (int)position.X;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.Red);
        }
    }
}
