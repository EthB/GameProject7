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
    class laser : ISprite
    {
        public Rectangle bounds;
        Texture2D texture;
        Vector2 position;
        Player player;
        float xPos;

        public laser(Player player, Texture2D texture)
        {
            this.player = player;
            this.texture = texture;
            position = player.Position;
            position.X -= 20;
            position.Y += 5;
            bounds = new Rectangle((int)position.X, (int)position.Y, 32, 32);
        }

        public void Update(GameTime gameTime)
        {
            xPos += 6;
            position.X = player.Position.X + xPos;
            bounds.X = (int)position.X;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.Red);
        }
    }
}
