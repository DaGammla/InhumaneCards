using InhumaneCards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using System.Threading.Tasks;

namespace InhumaneWeb{
    public class WebGame : BaseGame {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public InhumaneGame childGame;

        float screenSize = 1f;
        Vector2 screenOffset = Vector2.Zero;

        public WebGame() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.childGame = new InhumaneGame(this);
            this.IsMouseVisible = true;

            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);

        }

        public void SetScreenSize(uint width, uint height) {
            float sizeX = ((float)width) / ((float)InhumaneGame.TARGET_X);
            float sizeY = ((float)height) / ((float)InhumaneGame.TARGET_Y);

            if (sizeX < sizeY) {
                screenSize = sizeX;
            } else {
                screenSize = sizeY;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {

            SetScreenSize(WebApp.GAME_CANVAS_X, WebApp.GAME_CANVAS_Y);

            childGame.Init();

            Window.Title = "Inhumane Cards for Web";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            childGame.LoadContent();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {

        }


        private bool mouseLastTick = false;
        private bool mouseThisTick = false;
        private Point mousePosition = Point.Zero;

        protected override void Update(GameTime gameTime) {

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
        protected override void Draw(GameTime gameTime) {

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
            spriteBatch.Draw(texture, position * screenSize + screenOffset, null, color, 0f, Vector2.Zero, screenSize, SpriteEffects.None, 0);
        }

        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color) {
            spriteBatch.Draw(texture, position * screenSize + screenOffset, sourceRectangle, color, 0f, Vector2.Zero, screenSize, SpriteEffects.None, 0);
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

        private bool otherTextInputOpen = false;

        public override void PerformTextInput(string question, string defaultText, Action<string> onDoneAction) {
            if (!otherTextInputOpen) {
                otherTextInputOpen = true;

                WebApp.PerformStringInput(question, defaultText, (text) => {

                    if (text != null && text.Length > 0) {
                        onDoneAction(text);
                    }

                    otherTextInputOpen = false;
                });
                
            }
        }

		public override bool CanHostGames() {
            return false;
		}
	}
}
