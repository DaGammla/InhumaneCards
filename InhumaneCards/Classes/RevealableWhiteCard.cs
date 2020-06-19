using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards.Classes {
	public class RevealableWhiteCard : DrawableCard {

		private const int BUTTON_RIGHT_OFFSET = 0;
		private const int BUTTON_LEFT_OFFSET = 0;
		private const int BUTTON_HEIGHT = 60;
		private const int BUTTON_BOT_OFFSET = 0;

		private Action revealClicked = () => { };
		private bool revealed = false;
		private bool revealableByPlayer = false;
		private Button revealButton;

		private string realLines;


		public RevealableWhiteCard(int posX, int posY, string text, bool revealableByPlayer, BaseGame game) : base(WHITE, posX, posY, "???", game) {

			this.revealableByPlayer = revealableByPlayer;
			
			if (revealableByPlayer) {
				revealButton = new Button("Enthüllen", new Rectangle(posX + BUTTON_LEFT_OFFSET, posY + CARD_HEIGHT - BUTTON_HEIGHT - BUTTON_BOT_OFFSET, CARD_WIDTH - BUTTON_RIGHT_OFFSET - BUTTON_LEFT_OFFSET, BUTTON_HEIGHT), game, RevealActionWrapper);
			}

			while (lines.Count > 6) {
				lines.RemoveAt(lines.Count - 1);
			}

			realLines = text;

		}

		public RevealableWhiteCard WithRevealListener(Action revealClicked) {

			this.revealClicked = revealClicked;

			return this;
		}

		public void Reveal() {
			this.UpdateText(realLines);

			this.revealed = true;

			realLines = null;
		}

		private void RevealActionWrapper() {
			Reveal();
			revealClicked();
		}

		public override void Draw() {
			base.Draw();
			if (!revealed && revealableByPlayer) {
				revealButton.Draw();
			}
		}

		public override void Update() {
			base.Update();
			if (!revealed && revealableByPlayer) {
				revealButton.Update();
			}
		}
	}
}
