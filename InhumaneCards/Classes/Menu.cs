using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace InhumaneCards.Classes {
	public class Menu {

		InhumaneGame game;

		Button hostButton;
		Button joinButton;
		Button changeNameButton;
		Button startGameButton;

		DrawableText usernameText;
		DrawableText waitingForHostText;

		public Menu(InhumaneGame game) {
			this.game = game;

			SetupElements();
		}

		private void SetupElements() {
			hostButton = new Button("Spiel hosten", 480, 720, game.baseGame, () => { game.HostGame(); });
			joinButton = new Button("Spiel beitreten", 1440, 720, game.baseGame, () => {
				game.baseGame.PerformTextInput("Ip Adresse", "", (output) => {
					game.JoinGame(output);
				});
			});

			changeNameButton = new Button("Name ändern", 960, 520, game.baseGame, () => {
				game.baseGame.PerformTextInput("Neuer Name", game.username, (newName) => {
					game.ChangeUsername(newName);
					usernameText.text = newName;
					usernameText.MeasureOriginToCenter();
				});
			});

			startGameButton = new Button("Spiel starten", 960, 800, game.baseGame, () => {
				game.HostStartGame();
			});

			usernameText = new DrawableText(game.username, game.baseGame) {
				textSize = 0.8f,
				position = new Vector2(960, 600)
			}.MeasureOriginToCenter();

			waitingForHostText = new DrawableText("Warten auf Host", game.baseGame) {
				textSize = 0.8f,
				position = new Vector2(960, 800)
			}.MeasureOriginToCenter();
		}

		public void Update() {
			if (!game.networkingStarted) {
				hostButton.Update();
				joinButton.Update();
				changeNameButton.Update();
			} else {
				if (game.hostsGame) {
					startGameButton.Update();
				} else {
					
				}
			}
		}

		public void Draw() {
			if (!game.networkingStarted) {
				hostButton.Draw();
				joinButton.Draw();
				changeNameButton.Draw();
				usernameText.Draw();
			} else {
				if (game.hostsGame) {
					startGameButton.Draw();
				} else {
					waitingForHostText.Draw();
				}
				game.baseGame.DrawString(FontNum.DejaVuSans.F(), "Spieler: " + game.GetPlayerCount(), new Vector2(InhumaneGame.BOX_OFFSET + 10, InhumaneGame.BOX_OFFSET + 10), Color.White, 0f, Vector2.Zero, 0.8f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
				for (byte b = 0; b < game.GetPlayerCount(); b++) {
					game.baseGame.DrawString(FontNum.DejaVuSans.F(), "• " + game.GetPlayer(b), new Vector2(InhumaneGame.BOX_OFFSET + 10, InhumaneGame.BOX_OFFSET + 60 + b * 50), Color.White, 0f, Vector2.Zero, 0.8f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
				}
			}
		}
	}
}
