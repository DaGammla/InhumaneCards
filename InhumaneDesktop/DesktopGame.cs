using InhumaneCards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using System.Threading.Tasks;

namespace InhumaneCardsDesktop {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DesktopGame : BaseGame {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InhumaneGame childGame;

        float screenSize = 1f;
        Vector2 screenOffset = Vector2.Zero;
        bool justResized = false;

        private const int MULTISAMPLING_COUNT = 8;

        public DesktopGame(){
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.childGame = new InhumaneGame(this);
            this.IsMouseVisible = true;

            ResizeScreen(1280, 720, true);

            justResized = false;

            //Allow User Resizing and when the user does call @ResizeScreen
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowClientSizeChanged);
            void WindowClientSizeChanged(object sender, EventArgs e) {
                if (!justResized) {    
                    ResizeScreen(Window.ClientBounds.Width, Window.ClientBounds.Height);
                }
                justResized = false;
            }

            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(PreparingDevice);
            void PreparingDevice(object sender, PreparingDeviceSettingsEventArgs e) {
                graphics.PreferMultiSampling = true;
                graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = MULTISAMPLING_COUNT;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = MULTISAMPLING_COUNT;
            }

            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);

        }

        public void ResizeScreen(int x, int y, bool applyBackBuffer = false) {
            float scaleX = (float)x / (float)InhumaneGame.TARGET_X;
            float scaleY = (float)y / (float)InhumaneGame.TARGET_Y;

            //Takes the scale thats smaller and sets it as screen_scale
            screenSize = scaleX < scaleY ? scaleX : scaleY;

            if (applyBackBuffer) {
                graphics.PreferredBackBufferWidth = x;
                graphics.PreferredBackBufferHeight = y;
            }

            if (scaleX > scaleY) {
                screenOffset = new Vector2(x - InhumaneGame.TARGET_X * screenSize, 0) * 0.5f;
            } else {
                screenOffset = new Vector2(0, y - InhumaneGame.TARGET_Y * screenSize) * 0.5f;
            }

            justResized = true;
            graphics.ApplyChanges();
        }

        /*public void ResizeScreen(int x, int y) {
            float scaleX = (float)x / (float)InhumaneGame.TARGET_X;
            float scaleY = (float)y / (float)InhumaneGame.TARGET_Y;

            //Takes the dimension that changed the most and sets it as screen_scale
            screenSize = (Math.Abs(screenSize - scaleX) > Math.Abs(screenSize - scaleY) ? scaleX : scaleY);

            //Apply the newly calculated screen_scale
            graphics.PreferredBackBufferWidth = (int)(InhumaneGame.TARGET_X * screenSize);
            graphics.PreferredBackBufferHeight = (int)(InhumaneGame.TARGET_Y * screenSize);

            justResized = true;
            graphics.ApplyChanges();
        }*/

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize(){
            childGame.Init();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent(){

        }


        private bool mouseLastTick = false;
        private bool mouseThisTick = false;
        private Point mousePosition = Point.Zero;

        protected override void Update(GameTime gameTime){

            mouseThisTick = Mouse.GetState().LeftButton == ButtonState.Pressed;
            mousePosition = Mouse.GetState().Position.Times(1f / screenSize);

            childGame.Update(gameTime);

            base.Update(gameTime);

            mouseLastTick = mouseThisTick;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            BatchBegin();
            childGame.Draw(gameTime);
            BatchEnd();

            base.Draw(gameTime);
        }

        public override GraphicsDeviceManager GetGraphics() {
            return graphics;
        }

        public override SpriteBatch GetSpriteBatch() {
            return spriteBatch;
        }

        public override void Draw(Texture2D texture, Rectangle destinationRectangle, Color color) {
            spriteBatch.Draw(texture, destinationRectangle.Times(screenSize).SelfOffset(screenOffset), color);
        }

        public override void Draw(Texture2D texture, Vector2 position, Color color) {
            spriteBatch.Draw(texture, position * screenSize + screenOffset, color);
        }

        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color) {
            spriteBatch.Draw(texture, position * screenSize + screenOffset, sourceRectangle, color);
        }

        public override void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) {
            spriteBatch.Draw(texture, destinationRectangle.Times(screenSize).SelfOffset(screenOffset), sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public override void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color) {
            spriteBatch.Draw(texture, destinationRectangle.Times(screenSize).SelfOffset(screenOffset), sourceRectangle, color);
        }

        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            spriteBatch.Draw(texture, position * screenSize + screenOffset, sourceRectangle, color, rotation, origin, scale * screenSize, effects, layerDepth);
        }

        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            spriteBatch.Draw(texture, position * screenSize + screenOffset, sourceRectangle, color, rotation, origin, scale * screenSize, effects, layerDepth);
        }

        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            spriteBatch.DrawString(spriteFont, text, position * screenSize + screenOffset, color, rotation, origin, scale * screenSize, effects, layerDepth);
        }
        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color) {
            spriteBatch.DrawString(spriteFont, text, position * screenSize + screenOffset, color);
        }
        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            spriteBatch.DrawString(spriteFont, text, position * screenSize + screenOffset, color, rotation, origin, scale * screenSize, effects, layerDepth);
        }
        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            spriteBatch.DrawString(spriteFont, text, position * screenSize + screenOffset, color, rotation, origin, scale * screenSize, effects, layerDepth);
        }
        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color) {
            spriteBatch.DrawString(spriteFont, text, position * screenSize + screenOffset, color);
        }
        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            spriteBatch.DrawString(spriteFont, text, position * screenSize + screenOffset, color, rotation, origin, scale * screenSize, effects, layerDepth);
        }

        public override bool GotMouseReleased() {
            return mouseLastTick && !mouseThisTick;
        }

        public override bool GotMousePressed() {
            return !mouseLastTick && mouseThisTick;
        }

        public override bool IsMouseDown() {
            return mouseThisTick;
        }

        public override Point GetMousePosition() {
            return mousePosition;
        }

        public override void PerformTextInput(string question, string defaultText, Action<string> onDoneAction) {
            var pos = Window.Position;
            new Task(() => {
                string result = Microsoft.VisualBasic.Interaction.InputBox(question, "Title", defaultText, pos.X + (int)(InhumaneGame.TARGET_X * 0.5 * screenSize) - 240, pos.Y + (int)(InhumaneGame.TARGET_Y * 0.5 * screenSize) - 150);
                if (result != null && result.Length > 0) {
                    onDoneAction(result);
                }
            }).Start();
        }
    }
}
