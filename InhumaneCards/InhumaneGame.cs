using InhumaneCards.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using InhumaneCards.Classes.Networking;
using System.Collections.Generic;

namespace InhumaneCards {
	public class InhumaneGame {

		public const int TARGET_X = 1920, TARGET_Y = 1080;

		public static readonly Color CLEAR_COLOR = new Color(0.08f, 0.08f, 0.08f);

		//Values for the box that frames the entire application
		public const int BOX_OFFSET = 20;
		public const int BOX_WIDTH = 5;
		public static readonly Color BOX_COLOR = new Color(0.82f, 0.82f, 0.82f);

		private static readonly Vector2 LINE_1_POS = new Vector2(BOX_OFFSET, BOX_OFFSET);
		private static readonly Vector2 LINE_2_POS = new Vector2(BOX_OFFSET, TARGET_Y - BOX_OFFSET - BOX_WIDTH);
		private static readonly Vector2 LINE_3_POS = new Vector2(TARGET_X - BOX_OFFSET - BOX_WIDTH, BOX_OFFSET);
		private static readonly Vector2 HOR_LINE_SIZE = new Vector2(TARGET_X - 2 * BOX_OFFSET, BOX_WIDTH);
		private static readonly Vector2 VER_LINE_SIZE = new Vector2(BOX_WIDTH, TARGET_Y - 2 * BOX_OFFSET);

		private byte playerCount = 1;
		public bool networkingStarted = false;
		public bool hostsGame = false;
		public bool mainPartStarted = false;

		private byte winningPoints = 5;
		private byte blankCards = 2;

		private GameServer server;
		private GameClient client;
		private byte clientId = 0;
		public BaseGame baseGame { get; }
		private MainPartOfGame partOfGame;
		private Menu menu;

		private List<string> players = new List<string>();

		public string username = "Benutzername";

		public InhumaneGame(BaseGame baseGame) {
			this.baseGame = baseGame;
		}

		public void Init() {

			this.players.Add(username);
		}

		public void LoadContent() {

			Textures.LoadTextures(baseGame.GetContent());
			Fonts.LoadFonts(baseGame.GetContent());

			LoadCards();

			this.menu = new Menu(this);
		}

		public string[] blackCards = null;
		public bool[] blackCardTakesTwo = null;
		public string[] whiteCards = null;

		public void LoadCards() {


			Console.WriteLine("Loading Cards...");
			//This var counts the time passed
			int ms_before = Environment.TickCount;

			using (var stream = TitleContainer.OpenStream("Content/black_cards.txt")) {
				using (var reader = new StreamReader(stream)) {
					string blackCardsString = reader.ReadToEnd();
					blackCards = blackCardsString.Split('\n');
				}
			}

			blackCardTakesTwo = new bool[blackCards.Length];

			for (int i = 0; i < blackCards.Length; i++) {
				var currentCard = blackCards[i];
				var markPosition = currentCard.IndexOf('^');
				if (markPosition > 0) {
					blackCards[i] = currentCard.Substring(0, markPosition).Trim();
					blackCardTakesTwo[i] = true;
				}
			}

			using (var stream = TitleContainer.OpenStream("Content/white_cards.txt")) {
				using (var reader = new StreamReader(stream)) {
					string whiteCardsString = reader.ReadToEnd();
					whiteCards = whiteCardsString.Split('\n');
				}
			}

			//Prints the Time passed
			Console.WriteLine("Loading Cards done after " + (double)(Environment.TickCount - ms_before) / 1000.0 + " seconds");
		}

		public void OnClientConnected(byte clientId) {
			playerCount = ++clientId;

			while (players.Count < playerCount) {
				players.Add("");
			}
		}

		public byte GetPlayerCount() {
			return playerCount;
		}

		public byte GetPlayerId() {
			if (hostsGame) {
				return 0;
			} else {
				return clientId;
			}
		}

		public string GetPlayer(byte playerId) {
			return players[playerId];
		}

		public void HostGame() {
			this.server = new GameServer(this);
			server.StartServer();
			networkingStarted = true;
			hostsGame = true;

			server.SetDataReceiver(LobbyHostDataReceiver);

			players.Clear();
			players.Add(username);
		}

		public async void JoinGame(string address) {
			networkingStarted = true;
			hostsGame = false;

			this.client = new GameClient();
			this.clientId = await client.Connect(address);
			client.SetDataReceiver(LobbyClientDataReceiver);

			client.SendDataToServer(new JoinNetDat(username));
		}

		private void LobbyHostDataReceiver(NetworkingData dat, byte clientId) {
			if (dat is JoinNetDat) {
				var data = (JoinNetDat) dat;

				players[clientId] = data.username;

				server.MulticastData(new PlayersNetDat(players.ToArray(), playerCount));
			}
		}

		private void LobbyClientDataReceiver(NetworkingData dat) {
			if (dat is PlayersNetDat) {
				var data = (PlayersNetDat) dat;

				this.players = new List<string>(data.players);

				this.playerCount = data.playerCount;
			}
			if (dat is StartGameNetDat) {
				var data = (StartGameNetDat)dat;

				this.blankCards = data.maxBlankCards;

				ClientStartGame();
			}
		}

		public void HostStartGame() {
			partOfGame = new MainPartOfGame(this).AsHost(server);

			mainPartStarted = true;

			server.MulticastData(new StartGameNetDat(blankCards, players.ToArray(), playerCount));
		}

		private void ClientStartGame() {

			partOfGame = new MainPartOfGame(this).AsClient(client);

			mainPartStarted = true;
		}

		public void ChangeUsername(string newName) {
			if (!networkingStarted) {
				this.username = newName;
				this.players[0] = newName;
			}
		}

		public void MoreWinningPoints() {
			winningPoints++;
			if (winningPoints > 100) {
				winningPoints = 100;
			}
		}

		public void LessWinningPoints() {
			winningPoints--;
			if (winningPoints > 253 || winningPoints < 2) {
				winningPoints = 2;
			}
		}

		public void MoreBlankCards() {
			blankCards++;
			if (blankCards > 100) {
				blankCards = 100;
			}
		}

		public void LessBlankCards() {
			blankCards--;
			if (blankCards > 253 || blankCards < 0) {
				blankCards = 0;
			}
		}

		public byte GetBlankCardCount() {
			return blankCards;
		}

		public void BackToMainMenu() {
			this.menu = new Menu(this);
			this.partOfGame = null;

			this.players = new List<string>();

			this.players.Add(username);

			this.playerCount = 1;
			this.networkingStarted = false;
			this.hostsGame = false;
			this.mainPartStarted = false;
			if (server != null)
				this.server.StopServer();
			this.server = null;
			if (client != null)
				this.client.StopClient();
			this.client = null;
			this.clientId = 0;
		}

		public byte GetWinningPoints() {
			return winningPoints;
		}

		public void Update(GameTime gameTime) {

			if (mainPartStarted) {
				partOfGame.Update(gameTime);
			} else {
				menu.Update();
			}
		}

		public void Draw(GameTime gameTime) {
			baseGame.GetGraphicsDevice().Clear(CLEAR_COLOR);
			baseGame.Draw(TexNum.PIXEL.T(), LINE_1_POS, null, BOX_COLOR, 0f, Vector2.Zero, HOR_LINE_SIZE, SpriteEffects.None, 0);
			baseGame.Draw(TexNum.PIXEL.T(), LINE_2_POS, null, BOX_COLOR, 0f, Vector2.Zero, HOR_LINE_SIZE, SpriteEffects.None, 0);
			baseGame.Draw(TexNum.PIXEL.T(), LINE_1_POS, null, BOX_COLOR, 0f, Vector2.Zero, VER_LINE_SIZE, SpriteEffects.None, 0);
			baseGame.Draw(TexNum.PIXEL.T(), LINE_3_POS, null, BOX_COLOR, 0f, Vector2.Zero, VER_LINE_SIZE, SpriteEffects.None, 0);
			if (mainPartStarted) {
				partOfGame.Draw(gameTime);
			} else {
				menu.Draw();
			}
		}

	}
}