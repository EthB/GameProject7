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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        List<laser> lasers;
        List<Enemy> enemies;
        ParallaxLayer bulletLayer;
        ParallaxLayer enemyLayer;
        Texture2D bulletTexture;
        Texture2D enemyTexture;
        bool bulletFlag;
        public int score;
        public SpriteFont scoreFont;
        double timer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //add midground
            scoreFont = Content.Load<SpriteFont>("scoreFont");
            timer = 0;

            var midgroundTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("midground1"),
                Content.Load<Texture2D>("midground2")
            };
            var midgroundSprites = new StaticSprite[]
            {
                new StaticSprite(midgroundTextures[0]),
                new StaticSprite(midgroundTextures[1], new Vector2(3500, 0))
            };
            var midgroundLayer = new ParallaxLayer(this);
            midgroundLayer.Sprites.AddRange(midgroundSprites);
            midgroundLayer.DrawOrder = 1;
            //var midgroundScrollController = midgroundLayer.ScrollController as AutoScrollController;
            //midgroundScrollController.Speed = 40f;
            Components.Add(midgroundLayer);

            //add foreground
            var foregroundTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("foreground1"),
                Content.Load<Texture2D>("foreground2"),
                Content.Load<Texture2D>("foreground3"),
                Content.Load<Texture2D>("foreground4")
            };
            var foregroundSprites = new List<StaticSprite>();
            for (int i = 0; i < foregroundTextures.Count; i++)
            {
                var position = new Vector2(i * 3500, 0);
                var sprite = new StaticSprite(foregroundTextures[i], position);
                foregroundSprites.Add(sprite);
            }
            var foregroundLayer = new ParallaxLayer(this);
            foreach (var sprite in foregroundSprites)
            {
                foregroundLayer.Sprites.Add(sprite);
            }
            foregroundLayer.DrawOrder = 4;
            //var foregroundScrollController = foregroundLayer.ScrollController as AutoScrollController;
            //foregroundScrollController.Speed = 80f;
            Components.Add(foregroundLayer);

            //add background
            var backgroundTexture = Content.Load<Texture2D>("background");
            var backgroundSprite = new StaticSprite(backgroundTexture);
            var backgroundLayer = new ParallaxLayer(this);
            backgroundLayer.Sprites.Add(backgroundSprite);
            backgroundLayer.DrawOrder = 0;
            Components.Add(backgroundLayer);




            // TODO: use this.Content to load your game content here
            //loadPlayer
            var spritesheet = Content.Load<Texture2D>("player");
            player = new Player(spritesheet, this);
            var playerLayer = new ParallaxLayer(this);
            playerLayer.Sprites.Add(player);
            playerLayer.DrawOrder = 2;
            Components.Add(playerLayer);

            //var playerScrollController = playerLayer.ScrollController as AutoScrollController;
            //playerScrollController.Speed = 80f;
            bulletLayer = new ParallaxLayer(this);
            bulletLayer.DrawOrder = 2;
            bulletTexture = Content.Load<Texture2D>("bullet");
            Components.Add(bulletLayer);

            lasers = new List<laser>();
            enemies = new List<Enemy>();

            enemyLayer = new ParallaxLayer(this);
            enemyLayer.DrawOrder = 2;
            enemyTexture = Content.Load<Texture2D>("enemy");
            Components.Add(enemyLayer);


            backgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0.1f);
            midgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0.4f);
            playerLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
            foregroundLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
            bulletLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
            enemyLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Components.Remove(bulletLayer);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if(timer >= .5)
            {
                enemyLayer.Sprites.Add(new Enemy(player, enemyTexture));
                timer = 0;
            }
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyUp(Keys.Space))
            {
                bulletFlag = false;
                
            }
            if (bulletFlag == false)
            {
                if (keyboard.IsKeyDown(Keys.Space))
                {
                    
                    bulletFlag = true;
                    var laser = new laser(player, bulletTexture);
                    //lasers.Add(laser);
                    bulletLayer.Sprites.Add(laser);
                    //Components.Add(bulletLayer);
                }
            }
            //bulletLayer.Update(gameTime);
            /*foreach(laser l in bulletLayer.Sprites)
            {
                foreach(Enemy e in enemyLayer.Sprites)
                {
                    if (l.bounds.Intersects(e.bounds))
                   {
                        bulletLayer.Sprites.Remove(l);
                        enemyLayer.Sprites.Remove(e);
                        score += 20;
                    }
                }
            }*/

            if (bulletLayer.Sprites.Count >= 0 && enemyLayer.Sprites.Count() >= 0)
            {
                for (int i = 0; i < bulletLayer.Sprites.Count; i++)
                {
                    for (int j = 0; j < enemyLayer.Sprites.Count; j++)
                    {
                        laser tempL = (laser)bulletLayer.Sprites[i];
                        Enemy tempE = (Enemy)enemyLayer.Sprites[j];
                        if (tempL.bounds.Intersects(tempE.bounds))
                        {
                            bulletLayer.Sprites.Remove(tempL);

                            enemyLayer.Sprites.Remove(tempE);
                            if (i != 0) { i -= 1; }
                            if (j != 0) { j -= 1; }
                            
                            score += 20;

                        }
                        else
                        {
                            if (tempE.bounds.X <= player.Position.X - 200)
                            {
                                score -= 10;
                                enemyLayer.Sprites.Remove(tempE);
                                if (j != 0) { j -= 1; }
                            }
                            if (tempL.bounds.X >= player.Position.X + 800)
                            {
                                bulletLayer.Sprites.Remove(tempL);
                                if (i != 0) { i -= 1; }
                            }
                        }
                    }
                }
            }

            // TODO: Add your update logic here
            //player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //bulletLayer.Draw(gameTime);
            

            base.Draw(gameTime);
        }
    }
}
