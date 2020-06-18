﻿using InhumaneCards.Classes;
using InhumaneCards.Classes.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InhumaneCards {
	class MainPartOfGame {

		private const int BLACK_CARD_X = InhumaneGame.BOX_OFFSET + 20 + BLACK_BOX_MARGIN;
		private const int BLACK_CARD_Y = InhumaneGame.BOX_OFFSET + 20 + BLACK_BOX_MARGIN;
		private const int BLACK_MARGIN = 10;
		private const int BLACK_BOX_MARGIN = 15;
		private static readonly Color BLACK_BOX_COLOR = new Color(0.36f, 0.36f, 0.36f);

		private static readonly Rectangle BLACK_CARD_BOX = new Rectangle(BLACK_CARD_X - BLACK_BOX_MARGIN, BLACK_CARD_Y - BLACK_BOX_MARGIN, DrawableCard.CARD_WIDTH * 2 + BLACK_MARGIN + 2 * BLACK_BOX_MARGIN, DrawableCard.CARD_HEIGHT + 2 * BLACK_BOX_MARGIN);



		private static readonly Rectangle BLANK_CARD_BOX = new Rectangle(BLACK_CARD_X + DrawableCard.CARD_WIDTH + BLACK_MARGIN, BLACK_CARD_Y, DrawableCard.CARD_WIDTH, DrawableCard.CARD_HEIGHT);

		private const int PLUS_SIZE = 90;
		private const int PLUS_X_OFF = (DrawableCard.CARD_WIDTH - PLUS_SIZE) / 2;
		private const int PLUS_Y_OFF = (DrawableCard.CARD_HEIGHT - PLUS_SIZE) / 2;
		private static readonly Rectangle PLUS_BOX = new Rectangle(BLANK_CARD_BOX.X + PLUS_X_OFF, BLANK_CARD_BOX.Y + PLUS_Y_OFF, PLUS_SIZE, PLUS_SIZE);
		private static readonly Color PLUS_COLOR = new Color(0.36f, 0.36f, 0.36f);
		private static readonly Color PLUS_COLOR_HOVERED = new Color(0.42f, 0.42f, 0.42f);

		private const int WHITE_START_X = InhumaneGame.BOX_OFFSET + 20;
		private const int WHITE_START_Y = InhumaneGame.TARGET_Y - 2 * DrawableCard.CARD_HEIGHT - WHITE_MARGIN - InhumaneGame.BOX_OFFSET - 20;
		private const int WHITE_MARGIN = 10;

		private static readonly Vector2 LINE_1_POS = new Vector2(InhumaneGame.BOX_OFFSET, WHITE_START_Y - 20);
		private static readonly Vector2 HOR_LINE_SIZE = new Vector2(WHITE_START_X + 5 * DrawableCard.CARD_WIDTH + 3 * WHITE_MARGIN + 20 - 2 * InhumaneGame.BOX_WIDTH, InhumaneGame.BOX_WIDTH);
		private static readonly Vector2 LINE_2_POS = new Vector2(LINE_1_POS.X + HOR_LINE_SIZE.X - InhumaneGame.BOX_WIDTH, LINE_1_POS.Y);
		private static readonly Vector2 VER_LINE_SIZE = new Vector2(InhumaneGame.BOX_WIDTH, InhumaneGame.TARGET_Y - InhumaneGame.BOX_OFFSET - LINE_2_POS.Y);

		private const int WHITE_CARD_COUNT = 10;


		private PhaseEnum phase;
		private long tickNextPhaseShouldStart = -1L;

		private string blackCardText;
		private bool blackCardTakesTwo = false;
		

		private InhumaneGame game;

		private DrawableCard drawableBlackCard;

		private Button blankCardButton;

		private DrawableText blankCardText;
		private DrawableText currentlyText;
		private DrawableText notAvailableText;
		private DrawableText waitingForOtherPlayers;

		private GameClient client;
		private GameServer server;
		private bool remote;

		private byte clientsReady = 0;

		private byte playerId { get; } = 0;
		private byte playerCount { get; } = 0;
		private byte czarId = 255;
		private byte[] score;
		private PlayerCards[] playerCards;
		private byte playersWhoPicked = 0;
		private byte[] randVotingMap;
		private int[] revealedCards;

		private string cardOne = null;
		private string cardTwo = null;


		private Random rng;

		private IList<string> whiteCards = new List<string>();

		private IList<DrawableCard> cardsToDraw = new List<DrawableCard>();

		public MainPartOfGame(InhumaneGame game) {
			this.game = game;
			this.rng = new Random();

			drawableBlackCard = new DrawableCard(DrawableCard.BLACK, BLACK_CARD_X, BLACK_CARD_Y, "Es wird auf alle Spieler gewartet", game.baseGame);

			blankCardButton = new Button("", BLANK_CARD_BOX, game.baseGame, () => {
				if (phase == PhaseEnum.SELECTING && !IsCzar() && whiteCards.Count < WHITE_CARD_COUNT) {
					game.baseGame.PerformTextInput("Eigene Karte", GetRandomWhiteCard(), (cardText) => {
						if (phase == PhaseEnum.SELECTING && !IsCzar() && whiteCards.Count < WHITE_CARD_COUNT) {
							if (cardText.Length > 90) {
								cardText = cardText.Substring(0, 90);

							}
							var cardBuilder = new StringBuilder();
							var characters = FontNum.DejaVuSans.F().Characters;
							foreach (char c in cardText) {
								if (characters.Contains(c)) {
									cardBuilder.Append(c);
								}
							}
							cardText = cardBuilder.ToString();

							whiteCards.Add(cardText);
							SetupWhiteCardsSelectingPhase();
						}
					});

				}
			});

			blankCardText = new DrawableText("Karte schreiben", game.baseGame) {
				position = BLANK_CARD_BOX.Center.ToVector2() - new Vector2(0, DrawableCard.CARD_HEIGHT / 3.25f),
				textSize = 0.75f
			}.MeasureOriginToCenter();


			currentlyText = new DrawableText("Momentan", game.baseGame) {
				position = BLANK_CARD_BOX.Center.ToVector2() + new Vector2(0, DrawableCard.CARD_HEIGHT / 3.5f),
				textSize = 0.75f
			}.MeasureOriginToCenter();

			notAvailableText = new DrawableText("nicht Möglich", game.baseGame) {
				position = currentlyText.position + new Vector2(0, 40),
				textSize = currentlyText.textSize
			}.MeasureOriginToCenter();

			waitingForOtherPlayers = new DrawableText("Auf andere Spieler warten", game.baseGame) {
				position = new Vector2(WHITE_START_X + 2.5f * DrawableCard.CARD_WIDTH + 2 * WHITE_MARGIN, WHITE_START_Y + DrawableCard.CARD_HEIGHT + WHITE_MARGIN / 2),
				textSize = 1.25f
			}.MeasureOriginToCenter();

			this.playerId = game.GetPlayerId();
			this.playerCount = game.GetPlayerCount();

			this.score = new byte[playerCount];
			this.playerCards = new PlayerCards[playerCount];
			this.revealedCards = new int[playerCount];

			this.phase = PhaseEnum.WAITING;

		}

		public MainPartOfGame AsClient(GameClient client) {
			this.client = client;
			client.SetDataReceiver(ClientDataReceiver);
			this.remote = true;

			return this;
		}

		public MainPartOfGame AsHost(GameServer server) {
			this.server = server;

			server.StopAcceptingClients();

			server.SetDataReceiver(HostDataReceiver);

			this.remote = false;

			if (playerCount == 1) {
				StartSelectingPhase();
			}

			return this;
		}

		private bool IsClient() {
			return remote;
		}

		private bool IsHost() {
			return !remote;
		}

		private void ClientDataReceiver(NetworkingData dat) {
			if (dat is NewBlackCardNetDat) {
				var data = (NewBlackCardNetDat)dat;
				this.blackCardText = data.cardText;
				this.blackCardTakesTwo = data.takesTwo;
				drawableBlackCard.UpdateText(blackCardText);

				cardOne = null;
				cardTwo = null;

				if (phase == PhaseEnum.SELECTING) {
					foreach (SelectableWhiteCard card in cardsToDraw) {
						card.buttonsActive = true;
						card.okayActive = true;
					}
				}
			}

			if (dat is StartSelectingNetDat) {
				var data = (StartSelectingNetDat)dat;
				this.czarId = data.czarId;
				StartSelectingPhase();
			}

			if (dat is StartVotingPhaseNetDat) {

				var data = (StartVotingPhaseNetDat) dat;

				this.playerCards = data.playerCards;

				SetupRandVotingMap(data.randSeed);

				StartVotingPhase();
			}
		}

		private void HostDataReceiver(NetworkingData dat, byte clientId) {
			if (dat is ClientReadyNetDat) {
				clientsReady++;
				if (clientsReady == playerCount - 1) {
					StartSelectingPhase();
				}
			}

			if (dat is RequestNewBlackCardNetDat) {
				if (clientId == czarId && phase == PhaseEnum.SELECTING) {
					ApplyRandomBlackCard();
					drawableBlackCard.UpdateText(blackCardText);
					server.MulticastData(new NewBlackCardNetDat(blackCardText, blackCardTakesTwo));

					cardOne = null;
					cardTwo = null;

					if (phase == PhaseEnum.SELECTING) {
						foreach (SelectableWhiteCard card in cardsToDraw) {
							card.buttonsActive = true;
							card.okayActive = true;
						}
					}
				}
			}

			if (dat is ClientCardNetDat) {
				var data = (ClientCardNetDat) dat;

				var playersCards = new PlayerCards(data.cardOne, data.cardTwo);
				playerCards[clientId] = playersCards;

				playersWhoPicked++;

				if (playersWhoPicked == playerCount - 1) {
					tickNextPhaseShouldStart = ticks + 30;
				}
			}

			if (dat is ClientCardCancelNetDat) {
				playersWhoPicked--;
			}
		}

		private bool IsCzar() {
			return playerId == czarId;
		}

		private void StartSelectingPhase() {
			if (IsHost()) {
				if (czarId == 255) {
					czarId = rng.Next(playerCount).B();
				} else {
					czarId++;
					if (czarId == playerCount) {
						czarId = 0;
					}
				}

				ApplyRandomBlackCard();

				server.MulticastData(new StartSelectingNetDat(czarId, blackCardText, blackCardTakesTwo));
			}

			if (IsCzar() && playerCount > 1) {

				this.drawableBlackCard = new CrossableCard(DrawableCard.BLACK, BLACK_CARD_X, BLACK_CARD_Y, blackCardText, game.baseGame).WithCrossListener(() => {
					if (IsHost()) {

						ApplyRandomBlackCard();
						drawableBlackCard.UpdateText(blackCardText);
						server.MulticastData(new NewBlackCardNetDat(blackCardText, blackCardTakesTwo));

					} else {
						client.SendDataToServer(new RequestNewBlackCardNetDat());
					}
				});

			} else {
				this.drawableBlackCard = new DrawableCard(DrawableCard.BLACK, BLACK_CARD_X, BLACK_CARD_Y, blackCardText, game.baseGame);
			}



			if (IsCzar() && playerCount > 1) {

			} else {
				while (whiteCards.Count < WHITE_CARD_COUNT) {
					whiteCards.Add(GetRandomWhiteCard());
				}

				SetupWhiteCardsSelectingPhase();
			}

			this.phase = PhaseEnum.SELECTING;
		}

		private void SetupWhiteCardsSelectingPhase() {
			cardsToDraw.Clear();

			for (int i = 0; i < whiteCards.Count; i++) {

				var cardText = whiteCards[i];

				var whiteCard = new SelectableWhiteCard(WHITE_START_X + i / 2 * (DrawableCard.CARD_WIDTH + WHITE_MARGIN), WHITE_START_Y + i % 2 * (DrawableCard.CARD_HEIGHT + WHITE_MARGIN), cardText, game.baseGame);
				whiteCard.WithCrossListener(() => {

					if (whiteCard.okayActive) {

						if (whiteCards.Count > 2) {
							whiteCards.Remove(cardText);

							SetupWhiteCardsSelectingPhase();
						}
					}

				});
				whiteCard.WithOkayListener(() => {

					if (blackCardTakesTwo) {
						if (cardOne == null) {
							cardOne = cardText;
						} else {
							cardTwo = cardText;
						}

						if (cardOne != null && cardTwo != null) {
							foreach (SelectableWhiteCard card in cardsToDraw) {
								if (card.okayActive) {
									card.buttonsActive = false;
								}
							}

							

							if (IsClient()) {
								client.SendDataToServer(new ClientCardNetDat(cardOne, cardTwo));
							} else {
								var playersCards = new PlayerCards(cardOne, cardTwo);
								playerCards[playerId] = playersCards;

								playersWhoPicked++;

								if (playersWhoPicked == playerCount - 1) {
									tickNextPhaseShouldStart = ticks + 30;
								}
							}
						}

					} else {
						cardOne = cardText;

						foreach (SelectableWhiteCard card in cardsToDraw) {
							if (card.okayActive) {
								card.buttonsActive = false;
							}
						}

						if (IsClient()) {
							client.SendDataToServer(new ClientCardNetDat(cardOne));
						} else {
							var playersCards = new PlayerCards(cardOne);
							playerCards[playerId] = playersCards;

							playersWhoPicked++;

							if (playersWhoPicked == playerCount - 1) {
								tickNextPhaseShouldStart = ticks + 30;
							}
						}
					}

				});
				whiteCard.WithCancelListener(() => {
					if (blackCardTakesTwo) {
						if (cardOne == cardText) {
							cardOne = null;

						} else {
							cardTwo = null;

						}

						foreach (SelectableWhiteCard card in cardsToDraw) {
							card.buttonsActive = true;
						}

						if (IsClient()) {
							client.SendDataToServer(new ClientCardCancelNetDat());
						} else {
							playersWhoPicked--;
						}

					} else {
						cardOne = null;

						foreach (SelectableWhiteCard card in cardsToDraw) {
							card.buttonsActive = true;
						}

						if (IsClient()) {
							client.SendDataToServer(new ClientCardCancelNetDat());
						} else {
							playersWhoPicked--;
						}
					}
				});
				cardsToDraw.Add(whiteCard);
			}
		}


		private void SetupRandVotingMap(int seed) {

			void Swap(int a, int b) {
				var hold = randVotingMap[a];
				randVotingMap[a] = randVotingMap[b];
				randVotingMap[b] = hold;
			}

			var rand = new Random(seed);

			byte Rand() {
				byte val = czarId;
				while (val == czarId) {
					val = rand.Next(playerCount).B();
				}
				return val;
			}

			randVotingMap = new byte[playerCount];
			for (byte b = 0; b < playerCount; b++) {
				randVotingMap[b] = b;
			}

			for (byte b = 0; b < playerCount; b++) {
				if (b != czarId)
					Swap(b, Rand());
			}

			Swap(czarId, playerCount - 1);
		}

		private void StartVotingPhase() {

			if (IsHost()) {

				int seed = rng.Next();

				SetupRandVotingMap(seed);

				server.MulticastData(new StartVotingPhaseNetDat(playerCards, seed));
			}

			this.drawableBlackCard = new DrawableCard(DrawableCard.BLACK, BLACK_CARD_X, BLACK_CARD_Y, blackCardText, game.baseGame);

			SetupWhiteCardsVotingPhase(0);

			this.phase = PhaseEnum.VOTING;
		}

		private int GetVotingPhasePages() {
			if (blackCardTakesTwo) {
				return 0;
			} else {
				return ((playerCount - 2) / 10) + 1;
			}
		}

		private void SetupWhiteCardsVotingPhase(byte page) {
			cardsToDraw.Clear();

			if (blackCardTakesTwo) {

			} else {

				int startPlayerId = 10 * page;

				int endId = startPlayerId + 9;
				if (endId >= playerCount - 1) {
					endId = playerCount - 2;
				}

				for (int i = startPlayerId; i <= endId; i++) {

					int j = i - startPlayerId;

					byte id = randVotingMap[i];

					var whiteCard = new RevealableWhiteCard(WHITE_START_X + j / 2 * (DrawableCard.CARD_WIDTH + WHITE_MARGIN), WHITE_START_Y + j % 2 * (DrawableCard.CARD_HEIGHT + WHITE_MARGIN), playerCards[id].cardOne, IsCzar(), game.baseGame);

					if ((revealedCards[id] & 1) != 0) {
						whiteCard.Reveal();
					}

					whiteCard.WithRevealListener(() => {
						if (IsCzar()) {
							if (IsHost()) {
								revealedCards[id] |= 1;
							} else {
								
							}
						}
					});

					cardsToDraw.Add(whiteCard);
				}

			}

		}

		private void ApplyRandomBlackCard() {

			int randId = rng.Next(game.blackCards.Length);

			this.blackCardText = game.blackCards[randId];
			this.blackCardTakesTwo = game.blackCardTakesTwo[randId];
		}

		private string GetRandomWhiteCard() {

			int randId = rng.Next(game.whiteCards.Length);

			return game.whiteCards[randId];
		}

		long ticks = 0L;
		public void Update(GameTime gameTime) {

			if (IsClient() && ticks == 5) {
				client.SendDataToServer(new ClientReadyNetDat());
			}


			if (IsHost() && tickNextPhaseShouldStart == ticks) {
				if (phase == PhaseEnum.SELECTING) {
					if (playersWhoPicked == playerCount - 1) {
						StartVotingPhase();
					}
				} else {
					
				}
			}

			ticks++;
			drawableBlackCard.Update();
			blankCardButton.Update();

			try {
				for (int i = 0; i < cardsToDraw.Count; i++) {
					cardsToDraw[i].Update();
				}
			} catch {

			}
		}

		public void Draw(GameTime gameTime) {

			game.baseGame.Draw(TexNum.PIXEL.T(), BLACK_CARD_BOX, BLACK_BOX_COLOR);

			drawableBlackCard.Draw();

			game.baseGame.Draw(TexNum.PIXEL.T(), BLANK_CARD_BOX, DrawableCard.BLACK_COLOR);

			bool plusHovered = BLANK_CARD_BOX.Contains(game.baseGame.GetMousePosition());

			game.baseGame.Draw(TexNum.CROSS.T(), PLUS_BOX, plusHovered ? PLUS_COLOR_HOVERED : PLUS_COLOR);

			blankCardText.Draw();

			DrawScoreboard();

			if (!(phase == PhaseEnum.SELECTING && playerId != czarId && whiteCards.Count < WHITE_CARD_COUNT)) {
				currentlyText.Draw();
				notAvailableText.Draw();
			}

			game.baseGame.Draw(TexNum.PIXEL.T(), LINE_1_POS, null, InhumaneGame.BOX_COLOR, 0f, Vector2.Zero, HOR_LINE_SIZE, SpriteEffects.None, 0);
			game.baseGame.Draw(TexNum.PIXEL.T(), LINE_2_POS, null, InhumaneGame.BOX_COLOR, 0f, Vector2.Zero, VER_LINE_SIZE, SpriteEffects.None, 0);

			try {

				for (int i = 0; i < cardsToDraw.Count; i++) {
					cardsToDraw[i].Draw();
				}
			} catch {

			}

			if (IsCzar() && phase == PhaseEnum.SELECTING) {
				waitingForOtherPlayers.Draw();
			}
		}

		private void DrawScoreboard() {

			for (byte b = 0; b < playerCount; b++) {

				string nameScoreString = game.GetPlayer(b) + ": " + score[b];

				var pos = new Vector2(BLACK_CARD_BOX.Location.X + BLACK_CARD_BOX.Width + 60 + b / 9 * 340, BLACK_CARD_BOX.Location.Y + b % 9 * 38);

				game.baseGame.DrawString(FontNum.DejaVuSans.F(), nameScoreString, pos, DrawableText.TEXT_COLOR, 0f, Vector2.Zero, 0.6125f, SpriteEffects.None, 0);

				if (b == czarId) {
					game.baseGame.Draw(TexNum.CZAR.T(), new Rectangle((pos - new Vector2(40, 0)).ToPoint(), new Point(30, 30)), Color.White);
				}
			}
		}
	}

	public class PlayerCards {
		public string cardOne;
		public string cardTwo;
		public PlayerCards(string cardOne, string cardTwo = null) {
			this.cardOne = cardOne;
			this.cardTwo = cardTwo;
		}
		public PlayerCards() {
			
		}
	}

	internal enum PhaseEnum {
		WAITING,
		SELECTING,
		VOTING,
		WINNER
	}
}