using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards.Classes {
	public class Button {

		private static readonly Color BACK_COLOR_HOVERED = new Color(0.24f, 0.24f, 0.24f);
		private static readonly Color BACK_COLOR = new Color(0.19f, 0.19f, 0.19f);
		private static readonly Color TEXT_COLOR = new Color(0.82f, 0.82f, 0.82f);
		private const int MARGIN = 5;
		private const float TEXT_SIZE = 0.8f;
		private const int BOX_Y_OFFSET = (int)(-8 * TEXT_SIZE);

		private bool hovered = false;

		string text;

		Vector2 stringPos;
		Vector2 stringOrigin;

		Rectangle boxDestination;

		BaseGame game;

		Action onClicked;

		public Button(string text, int posX, int posY, BaseGame game, Action onClicked) {
			this.text = text;
			this.game = game;
			this.stringPos = new Vector2(posX, posY - BOX_Y_OFFSET);
			var stringMeasured = FontNum.DejaVuSans.F().MeasureString(text);
			stringOrigin = stringMeasured * 0.5f;
			//stringMeasured *= TEXT_SIZE;

			this.boxDestination = new Rectangle((int)(posX - stringOrigin.X * TEXT_SIZE - MARGIN * TEXT_SIZE * 1.5f) - MARGIN, (int)(posY - stringOrigin.Y * TEXT_SIZE) - MARGIN, (int)(stringMeasured.X * TEXT_SIZE + 2 * MARGIN + 2 * MARGIN * TEXT_SIZE * 1.5f), (int)(stringMeasured.Y * TEXT_SIZE + 2 * MARGIN));

			this.onClicked = onClicked;
		}

		public Button(string text, Rectangle boxDestination, BaseGame game, Action onClicked) {
			this.text = text;
			this.game = game;
			this.stringPos = boxDestination.Center.ToVector2() - new Vector2(0, BOX_Y_OFFSET);
			var stringMeasured = FontNum.DejaVuSans.F().MeasureString(text);
			stringOrigin = stringMeasured * 0.5f;
			//stringMeasured *= TEXT_SIZE;

			this.boxDestination = boxDestination;

			this.onClicked = onClicked;
		}

		public void Update() {
			hovered = boxDestination.Contains(game.GetMousePosition());

			if (hovered && game.GotMouseReleased()) {
				onClicked();
			}
		}

		public void Draw() {
			game.Draw(TexNum.PIXEL.T(), boxDestination, hovered ? BACK_COLOR_HOVERED : BACK_COLOR);
			game.DrawString(FontNum.DejaVuSans.F(), text, stringPos, TEXT_COLOR, 0f, stringOrigin, TEXT_SIZE, SpriteEffects.None, 0);


		}

	}
}
