using InhumaneCards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Text;

namespace InhumaneCardsAndroid {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class AndroidGame : BaseGame {

		Activity1 xamarinActivity;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		InhumaneGame childGame;

		float screenSize = 1f;

		public AndroidGame(Activity1 xamarinActivity) {

			this.xamarinActivity = xamarinActivity;

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			this.childGame = new InhumaneGame(this);

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = InhumaneGame.TARGET_X;
			graphics.PreferredBackBufferHeight = InhumaneGame.TARGET_Y;

			graphics.SupportedOrientations = DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft;
			graphics.ApplyChanges();
			this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			SetScreenSize();
			childGame.Init();

			base.Initialize();
		}

		private void SetScreenSize(){
			sizeX = ((float)GraphicsDevice.DisplayMode.Width) / ((float)InhumaneGame.TARGET_X);
			sizeY = ((float)GraphicsDevice.DisplayMode.Height) / ((float)InhumaneGame.TARGET_Y);

			if (sizeX < sizeY){
				screenSize = sizeX;
			} else {
				screenSize = sizeY;
			}
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
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// 

		private bool mouseLastTick = false;
		private bool mouseThisTick = false;
		private Point mousePosition = Point.Zero;

		long ticks = 0;

		protected override void Update(GameTime gameTime) {
			ticks++;

			if (ticks % 30 == 0) {
				xamarinActivity.FixFullscreen();
				SetScreenSize();
			}

			mousePosition = Point.Zero;

			var touchCol = TouchPanel.GetState();

			mouseThisTick = false;

			foreach (var touch in touchCol) {
				if (touch.State != TouchLocationState.Invalid) {
					if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved) {
						mouseThisTick = true;
					}
					mousePosition = (touch.Position / screenSize).ToPoint();
					break;
				}
			}

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
			spriteBatch.Draw(texture, destinationRectangle.Times(screenSize), color);
		}

		public override void Draw(Texture2D texture, Vector2 position, Color color) {
			spriteBatch.Draw(texture, position * screenSize, null, color, 0f, Vector2.Zero, screenSize, SpriteEffects.None, 0);
		}

		public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color) {
			spriteBatch.Draw(texture, position * screenSize, sourceRectangle, color, 0f, Vector2.Zero, screenSize, SpriteEffects.None, 0);
		}

		public override void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) {
			spriteBatch.Draw(texture, destinationRectangle.Times(screenSize), sourceRectangle, color, rotation, origin, effects, layerDepth);
		}

		public override void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color) {
			spriteBatch.Draw(texture, destinationRectangle.Times(screenSize), sourceRectangle, color);
		}

		public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
			spriteBatch.Draw(texture, position * screenSize, sourceRectangle, color, rotation, origin, scale * screenSize, effects, layerDepth);
		}

		public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
			spriteBatch.Draw(texture, position * screenSize, sourceRectangle, color, rotation, origin, scale * screenSize, effects, layerDepth);
		}

		public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
			spriteBatch.DrawString(spriteFont, text, position * screenSize, color, rotation, origin, scale * screenSize, effects, layerDepth);
		}
		public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color) {
			spriteBatch.DrawString(spriteFont, text, position * screenSize, color);
		}
		public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
			spriteBatch.DrawString(spriteFont, text, position * screenSize, color, rotation, origin, scale * screenSize, effects, layerDepth);
		}
		public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
			spriteBatch.DrawString(spriteFont, text, position * screenSize, color, rotation, origin, scale * screenSize, effects, layerDepth);
		}
		public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color) {
			spriteBatch.DrawString(spriteFont, text, position * screenSize, color);
		}
		public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
			spriteBatch.DrawString(spriteFont, text, position * screenSize, color, rotation, origin, scale * screenSize, effects, layerDepth);
		}

		public override void PerformTextInput(string question, string defaultText, Action<string> onDoneAction) {
			xamarinActivity.GetInputDialog(question, defaultText, onDoneAction);
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
	}
}
