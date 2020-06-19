using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace InhumaneCards.Classes.Networking {
	[Serializable]
	[XmlInclude(typeof(JoinNetDat))]
	[XmlInclude(typeof(PlayersNetDat))]
	[XmlInclude(typeof(StartGameNetDat))]
	[XmlInclude(typeof(ClientReadyNetDat))]
	[XmlInclude(typeof(NewBlackCardNetDat))]
	[XmlInclude(typeof(StartSelectingNetDat))]
	[XmlInclude(typeof(RequestNewBlackCardNetDat))]
	[XmlInclude(typeof(ClientCardNetDat))]
	[XmlInclude(typeof(StartVotingPhaseNetDat))]
	[XmlInclude(typeof(ClientCardCancelNetDat))]
	[XmlInclude(typeof(RevealCardNetDat))]
	[XmlInclude(typeof(OpenPageNetDat))]
	[XmlInclude(typeof(RoundWinnerNetDat))]
	public class NetworkingData {
		public byte[] ToByteArray() {
			var serializer = new XmlSerializer(typeof(NetworkingData));
			using (var ms = new MemoryStream()) {
				serializer.Serialize(ms, this);
				return ms.ToArray();
			}
		}

		public static NetworkingData FromBytes(byte[] array) {
			using (var memStream = new MemoryStream()) {
				var serializer = new XmlSerializer(typeof(NetworkingData));
				memStream.Write(array, 0, array.Length);
				memStream.Seek(0, SeekOrigin.Begin);
				var obj = (NetworkingData) serializer.Deserialize(memStream);
				return obj;
			}
		}

		public NetworkingData() {
			
		}

		/*public E Cast<E>() where E : NetworkingData {
			return (E) this;
		}*/
	}

	[Serializable]
	public class JoinNetDat : NetworkingData {
		public string username;

		public JoinNetDat() {
			
		}

		public JoinNetDat(string username) {
			this.username = username;
		}
	}

	[Serializable]
	public class PlayersNetDat : NetworkingData {
		public string[] players;

		public byte playerCount;

		public PlayersNetDat(string[] players, byte playerCount) {
			this.players = players;
			this.playerCount = playerCount;
		}

		public PlayersNetDat() {
			
		}
	}

	[Serializable]
	public class StartGameNetDat : PlayersNetDat {
		public StartGameNetDat(string[] players, byte playerCount) : base(players, playerCount) {

		}

		public StartGameNetDat() : base() {

		}
	}

	[Serializable]
	public class ClientReadyNetDat : NetworkingData {
		public ClientReadyNetDat() {

		}
	}

	[Serializable]
	public class NewBlackCardNetDat : NetworkingData {

		public string cardText;

		public bool takesTwo;

		public NewBlackCardNetDat(string cardText, bool takesTwo) {
			this.cardText = cardText;
			this.takesTwo = takesTwo;
		}

		public NewBlackCardNetDat() {

		}
	}

	[Serializable]
	public class StartSelectingNetDat : NewBlackCardNetDat {
		public byte czarId;

		public StartSelectingNetDat(byte czarId, string cardText, bool takesTwo) : base(cardText, takesTwo) {
			this.czarId = czarId;
		}

		public StartSelectingNetDat() : base() {

		}
	}

	[Serializable]
	public class RequestNewBlackCardNetDat : NetworkingData {
		public RequestNewBlackCardNetDat() {

		}
	}

	[Serializable]
	public class ClientCardNetDat : NetworkingData {
		public string cardOne;
		public string cardTwo;
		public ClientCardNetDat() {

		}
		public ClientCardNetDat(string cardOne) {
			this.cardOne = cardOne;
		}
		public ClientCardNetDat(string cardOne, string cardTwo) {
			this.cardOne = cardOne;
			this.cardTwo = cardTwo;
		}
	}

	[Serializable]
	public class StartVotingPhaseNetDat : NetworkingData {
		public PlayerCards[] playerCards;
		public int randSeed;
		public StartVotingPhaseNetDat() {

		}
		public StartVotingPhaseNetDat(PlayerCards[] playerCards, int randSeed) {
			this.playerCards = playerCards;
			this.randSeed = randSeed;
		}
	}

	[Serializable]
	public class ClientCardCancelNetDat : NetworkingData {
		public ClientCardCancelNetDat() {

		}
	}

	[Serializable]
	public class RevealCardNetDat : NetworkingData {
		public byte playerToReveal;
		public int revealFlag;
		public RevealCardNetDat() {

		}

		public RevealCardNetDat(byte playerToReveal, int revealFlag) {
			this.playerToReveal = playerToReveal;
			this.revealFlag = revealFlag;
		}
	}

	[Serializable]
	public class OpenPageNetDat : NetworkingData {
		public byte page;
		public OpenPageNetDat() {

		}

		public OpenPageNetDat(byte page) {
			this.page = page;
		}
	}

	[Serializable]
	public class RoundWinnerNetDat : NetworkingData {
		public byte winnerId;
		public RoundWinnerNetDat() {

		}

		public RoundWinnerNetDat(byte winnerId) {
			this.winnerId = winnerId;
		}
	}


}
