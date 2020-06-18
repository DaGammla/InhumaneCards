using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards.Classes {
	public class CrossableCard : DrawableCard {

		Action crossClicked = () => { };

		Rectangle crossDestination;
		Vector2 crossPosition;
		float crossScale;

		protected const int CROSS_BOX = 50;
		protected const int CROSS_MARGIN = 5;

		private static readonly Texture2D CROSS_TEXTURE = TexNum.CROSS.T();
		private static readonly Vector2 CROSS_ORIGIN = new Vector2(CROSS_TEXTURE.Width / 2f, CROSS_TEXTURE.Height / 2f);

		private bool hovered = false;

		private static readonly Color CROSS_COLOR = Color.White * 0.2f;
		private static readonly Color CROSS_COLOR_HOVERED = Color.White * 0.25f;

		public CrossableCard(bool color, int posX, int posY, string text, BaseGame game): base(color, posX, posY, text, game) {
			crossDestination = new Rectangle(
				posX + destination.Width - CROSS_BOX - CROSS_MARGIN,
				posY + destination.Height - CROSS_BOX - CROSS_MARGIN,
				CROSS_BOX, CROSS_BOX);

			crossPosition = crossDestination.Center.ToVector2();
			crossScale = (float) CROSS_BOX / (float) CROSS_TEXTURE.Width;
		}

		public CrossableCard WithCrossListener(Action crossClicked) {

			this.crossClicked = crossClicked;

			return this;
		}

		public override void Update() {
			
			hovered = crossDestination.Contains(game.GetMousePosition());

			if (hovered && game.GotMouseReleased()) {
				crossClicked();
			}
		}

		public override void Draw() {
			base.Draw();

			//public abstract void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth);
			//game.Draw(TexNum.PIXEL.T(), crossDestination, Color.Red);
			game.Draw(TexNum.CROSS.T(), crossPosition, null, hovered ? CROSS_COLOR_HOVERED : CROSS_COLOR, Utils.FLOAT_PI * 0.25f, CROSS_ORIGIN, crossScale, SpriteEffects.None, 0f);
		}
	}
}
