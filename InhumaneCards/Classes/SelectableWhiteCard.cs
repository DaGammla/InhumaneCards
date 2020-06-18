using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards.Classes {
	public class SelectableWhiteCard : CrossableCard {

		private const int BUTTON_RIGHT_OFFSET = CROSS_BOX + 2 * CROSS_MARGIN;
		private const int BUTTON_LEFT_OFFSET = 0;
		private const int BUTTON_HEIGHT = CROSS_BOX + 2 * CROSS_MARGIN;
		private const int BUTTON_BOT_OFFSET = 0;

		private Action okayClicked = () => { }, cancelClicked = () => { };

		private Button okayButton, cancelButton;

		public bool okayActive = true;
		public bool buttonsActive = true;

		public SelectableWhiteCard(int posX, int posY, string text, BaseGame game) : base(WHITE, posX, posY, text, game) {
			okayButton = new Button("Spielen", new Rectangle(posX + BUTTON_LEFT_OFFSET, posY + CARD_HEIGHT - BUTTON_HEIGHT - BUTTON_BOT_OFFSET, CARD_WIDTH - BUTTON_RIGHT_OFFSET - BUTTON_LEFT_OFFSET, BUTTON_HEIGHT), game, OkayActionWrapper);
			cancelButton = new Button("Zurück", new Rectangle(posX + BUTTON_LEFT_OFFSET, posY + CARD_HEIGHT - BUTTON_HEIGHT - BUTTON_BOT_OFFSET, CARD_WIDTH - BUTTON_RIGHT_OFFSET - BUTTON_LEFT_OFFSET, BUTTON_HEIGHT), game, CancelActionWrapper);

			while (lines.Count > 6) {
				lines.RemoveAt(lines.Count - 1);
			}
		}

		public SelectableWhiteCard WithOkayListener(Action okayClicked) {

			this.okayClicked = okayClicked;

			return this;
		}

		public SelectableWhiteCard WithCancelListener(Action cancelClicked) {

			this.cancelClicked = cancelClicked;

			return this;
		}

		private void OkayActionWrapper() {
			okayActive = false;
			okayClicked();
		}

		private void CancelActionWrapper() {
			okayActive = true;
			cancelClicked();
		}

		public override void Draw() {
			base.Draw();
			if (buttonsActive) {
				if (okayActive) {
					okayButton.Draw();
				} else {
					cancelButton.Draw();
				}
			}
		}

		public override void Update() {
			base.Update();
			if (buttonsActive) {
				if (okayActive) {
					okayButton.Update();
				} else {
					cancelButton.Update();
				}
			}
		}
	}
}
