using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards.Classes {
	public class DrawableText {

		public static readonly Color TEXT_COLOR = new Color(0.82f, 0.82f, 0.82f);

		public string text = "";

		private BaseGame game;
		public float textSize = 1f;
		public Vector2 origin = Vector2.Zero;
		public Vector2 position = Vector2.Zero;

		public DrawableText(BaseGame game) {
			this.game = game;
		}

		public DrawableText(string text, BaseGame game) : this(game) {
			this.text = text;
		}

		public void Draw() {
			game.DrawString(FontNum.DejaVuSans.F(), text, position, TEXT_COLOR, 0f, origin, textSize, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
		}

		public DrawableText MeasureOriginToCenter() {
			this.origin = FontNum.DejaVuSans.F().MeasureString(text) * 0.5f + new Vector2(0, -8 * textSize);
			return this;
		}
	}
}
