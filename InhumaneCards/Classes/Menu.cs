using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;

namespace InhumaneCards.Classes {
	public class Menu {

		InhumaneGame game;

		Button hostButton;
		Button joinButton;
		Button changeNameButton;
		Button startGameButton;
		DrawableText cantHostText;


		DrawableText pointsNeededText;
		Button morePointsButton;
		Button lessPointsButton;
		DrawableText blanksAvailableText;
		Button moreBlanksButton;
		Button lessBlanksButton;

		DrawableText usernameText;
		DrawableText waitingForHostText;

		public Menu(InhumaneGame game) {
			this.game = game;

			SetupElements();
		}

		private void SetupElements() {
			if (game.baseGame.CanHostGames()) {
				hostButton = new Button("Spiel hosten", 480, 720, game.baseGame, () => { game.HostGame(); });
				joinButton = new Button("Spiel beitreten", 1440, 720, game.baseGame, () => {
					game.baseGame.PerformTextInput("Ip Adresse", "", (output) => {
						game.JoinGame(output);
					});
				});
			} else {
				joinButton = new Button("Spiel beitreten", 960, 760, game.baseGame, () => {
					game.baseGame.PerformTextInput("Ip Adresse", "", (output) => {
						game.JoinGame(output);
					});
				});
				cantHostText = new DrawableText("Aus dem Browser kann nicht gehostet werden", game.baseGame) {
					textSize = 0.8f,
					position = new Vector2(960, 880) 
				}.MeasureOriginToCenter();
			}

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

			morePointsButton = new Button("   ++   ", 960, 196, game.baseGame, () => {
				game.MoreWinningPoints();
				pointsNeededText.text = "Punkte benötigt zum Sieg: " + game.GetWinningPoints();
				pointsNeededText.MeasureOriginToCenter();
			});

			pointsNeededText = new DrawableText("Punkte benötigt zum Sieg: " + game.GetWinningPoints(), game.baseGame) {
				textSize = 0.8f,
				position = new Vector2(960, 256)
			}.MeasureOriginToCenter();

			lessPointsButton = new Button("   --   ", 960, 316, game.baseGame, () => {
				game.LessWinningPoints();
				pointsNeededText.text = "Punkte benötigt zum Sieg: " + game.GetWinningPoints();
				pointsNeededText.MeasureOriginToCenter();
			});


			moreBlanksButton = new Button("   ++   ", 960, 496, game.baseGame, () => {
				game.MoreBlankCards();
				blanksAvailableText.text = "Anzahl beschreibbarer Karten: " + game.GetBlankCardCount();
				blanksAvailableText.MeasureOriginToCenter();
			});

			blanksAvailableText = new DrawableText("Anzahl beschreibbarer Karten: " + game.GetBlankCardCount(), game.baseGame) {
				textSize = 0.8f,
				position = new Vector2(960, 556)
			}.MeasureOriginToCenter();

			lessBlanksButton = new Button("   --   ", 960, 616, game.baseGame, () => {
				game.LessBlankCards();
				blanksAvailableText.text = "Anzahl beschreibbarer Karten: " + game.GetBlankCardCount();
				blanksAvailableText.MeasureOriginToCenter();
			});
		}

		public void Update() {
			if (!game.networkingStarted) {
				if (game.baseGame.CanHostGames()) {
					hostButton.Update();
				}
				joinButton.Update();
				changeNameButton.Update();
			} else {
				if (game.hostsGame) {
					startGameButton.Update();

					morePointsButton.Update();
					lessPointsButton.Update();

					moreBlanksButton.Update();
					lessBlanksButton.Update();
					
				} else {
					
				}
			}
		}

		public void Draw() {
			if (!game.networkingStarted) {
				if (game.baseGame.CanHostGames()) {
					hostButton.Draw();
				} else {
					cantHostText.Draw();
				}
				joinButton.Draw();
				changeNameButton.Draw();
				usernameText.Draw();
			} else {
				if (game.hostsGame) {
					startGameButton.Draw();

					morePointsButton.Draw();
					pointsNeededText.Draw();
					lessPointsButton.Draw();

					moreBlanksButton.Draw();
					blanksAvailableText.Draw();
					lessBlanksButton.Draw();
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
