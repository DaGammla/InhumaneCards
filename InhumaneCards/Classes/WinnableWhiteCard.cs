using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards.Classes {
	public class WinnableWhiteCard : DrawableCard {

		private const int BUTTON_RIGHT_OFFSET = 0;
		private const int BUTTON_LEFT_OFFSET = 0;
		private const int BUTTON_HEIGHT = 60;
		private const int BUTTON_BOT_OFFSET = 0;

		private Action winningAction = () => { };
		private Button winnerButton;

		public WinnableWhiteCard(int posX, int posY, string text, BaseGame game) : base(WHITE, posX, posY, text, game) {
			

			winnerButton = new Button("Gewinner", new Rectangle(posX + BUTTON_LEFT_OFFSET, posY + CARD_HEIGHT - BUTTON_HEIGHT - BUTTON_BOT_OFFSET, CARD_WIDTH - BUTTON_RIGHT_OFFSET - BUTTON_LEFT_OFFSET, BUTTON_HEIGHT), game, WinningActionWrapper);
			

			while (lines.Count > 6) {
				lines.RemoveAt(lines.Count - 1);
			}

		}

		public WinnableWhiteCard WithWinningListener(Action winningAction) {

			this.winningAction = winningAction;

			return this;
		}

		private void WinningActionWrapper() {
			winningAction();
		}

		public override void Draw() {
			base.Draw();

			winnerButton.Draw();
		}

		public override void Update() {
			base.Update();

			winnerButton.Update();
			
		}
	}
}
