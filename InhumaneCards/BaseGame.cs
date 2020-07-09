using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace InhumaneCards {
	public abstract class BaseGame : Game {

		public ContentManager GetContent() {
			return base.Content;
		}

		public GraphicsDevice GetGraphicsDevice() {
			return base.GraphicsDevice;
		}

		public abstract GraphicsDeviceManager GetGraphics();

		public abstract SpriteBatch GetSpriteBatch();

		private SpriteSortMode sortMode = SpriteSortMode.Deferred;
		private BlendState blendState = null;
		private SamplerState samplerState = null;
		private DepthStencilState depthStencilState = null;
		private RasterizerState rasterizerState = null;
		private Effect effect = null;
		private Matrix? transformMatrix = null;

		public void ChangeDrawSettings(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null) {
			this.sortMode = sortMode;
			this.blendState = blendState;
			this.samplerState = samplerState;
			this.depthStencilState = depthStencilState;
			this.rasterizerState = rasterizerState;
			this.effect = effect;
			this.transformMatrix = transformMatrix;
		}

		public void ResetDrawSettings() {
			ChangeDrawSettings();
		}

		protected void BatchBegin() {
			GetSpriteBatch().Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
		}

		protected void BatchEnd() {
			GetSpriteBatch().End();
		}

		protected override void OnExiting(Object sender, EventArgs args) {
			base.OnExiting(sender, args);

			Environment.Exit(0);
		}

		public abstract void PerformTextInput(string question, string defaultText, Action<string> onDoneAction);

		public abstract bool GotMouseReleased();
		public abstract bool GotMousePressed();
		public abstract bool IsMouseDown();
		public abstract Point GetMousePosition();

		public abstract bool CanHostGames();

		//Draw Methods
		public abstract void Draw(Texture2D texture, Rectangle destinationRectangle, Color color);
		public abstract void Draw(Texture2D texture, Vector2 position, Color color);
		public abstract void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color);
		public abstract void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth);
		public abstract void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color);
		public abstract void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
		public abstract void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

		//Draw String
		public abstract void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
		public abstract void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color);
		public abstract void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
		public abstract void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
		public abstract void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color);
		public abstract void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
	}
}
