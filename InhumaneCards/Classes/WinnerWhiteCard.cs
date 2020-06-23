using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards.Classes {
	public class WinnerWhiteCard : DrawableCard {

		private const int TEXT_BOTTOM_OFFSET = 30;

		private const float WINNER_TEXT_SIZE = 0.8f;
		private const int WINNER_TEXT_OFFSET = (int)(8 * WINNER_TEXT_SIZE);
		private DrawableText winnerText;

		public WinnerWhiteCard(int posX, int posY, string text, BaseGame game) : base(WHITE, posX, posY, text, game) {

			winnerText = new DrawableText("Gewinner", game) {
				position = new Vector2(posX + CARD_WIDTH / 2, posY + CARD_HEIGHT - TEXT_BOTTOM_OFFSET + WINNER_TEXT_OFFSET),
				textSize = WINNER_TEXT_SIZE
			}.MeasureOriginToCenter();
			

			while (lines.Count > 6) {
				lines.RemoveAt(lines.Count - 1);
			}

		}

		public override void Draw() {
			base.Draw();

			winnerText.Draw();
		}
	}
}
