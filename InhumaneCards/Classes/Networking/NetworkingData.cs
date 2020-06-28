using System;
using System.IO;
using System.Text;
using System.Xml;
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
	[XmlInclude(typeof(GameWinnerNetDat))]
	[XmlRoot("Dat", Namespace = "")]
	public class NetworkingData {

		static XmlSerializer serializer = new XmlSerializer(typeof(NetworkingData));
		static XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
		static XmlWriterSettings settings = new XmlWriterSettings() {
			Indent = false,
			OmitXmlDeclaration = true
		};

		public byte[] ToByteArray() {
			using (var stringWriter = new StringWriter())
			using (var xmlWriter = XmlWriter.Create(stringWriter, settings)) {
				serializer.Serialize(xmlWriter, this, ns);
				var xml = stringWriter.ToString();
				//*
				xml = xml.Replace("xmlns:p1=\"http://www.w3.org/2001/XMLSchema-instance\"", "&&&");
				xml = xml.Replace("p1:type=", "&&");
				//*/

				return Encoding.UTF8.GetBytes(xml);
			}
		}

		public static NetworkingData FromBytes(byte[] array) {
			var xml = Encoding.UTF8.GetString(array);
			//*
			xml = xml.Replace("&&&", "xmlns:p1=\"http://www.w3.org/2001/XMLSchema-instance\"");
			xml = xml.Replace("&&", "p1:type=");
			//*/
			using (var stringReader = new StringReader(xml)) {
				var obj = (NetworkingData) serializer.Deserialize(stringReader);
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
	[XmlType("A")]
	public class JoinNetDat : NetworkingData {
		[XmlAttribute("a")]
		public string username;

		public JoinNetDat() {
			
		}

		public JoinNetDat(string username) {
			this.username = username;
		}
	}

	[Serializable]
	[XmlType("B")]
	public class PlayersNetDat : NetworkingData {
		[XmlArray("a")]
		[XmlArrayItem("s")]
		public string[] players;

		[XmlAttribute("b")]
		public byte playerCount;

		public PlayersNetDat(string[] players, byte playerCount) {
			this.players = players;
			this.playerCount = playerCount;
		}

		public PlayersNetDat() {
			
		}
	}

	[Serializable]
	[XmlType("C")]
	public class StartGameNetDat : PlayersNetDat {
		[XmlAttribute("c")]
		public byte maxBlankCards;
		public StartGameNetDat(byte maxBlankCards, string[] players, byte playerCount) : base(players, playerCount) {
			this.maxBlankCards = maxBlankCards;
		}

		public StartGameNetDat() : base() {

		}
	}

	[Serializable]
	[XmlType("D")]
	public class ClientReadyNetDat : NetworkingData {
		public ClientReadyNetDat() {

		}
	}

	[Serializable]
	[XmlType("E")]
	public class NewBlackCardNetDat : NetworkingData {

		[XmlAttribute("a")]
		public string cardText;

		[XmlAttribute("b")]
		public bool takesTwo;

		public NewBlackCardNetDat(string cardText, bool takesTwo) {
			this.cardText = cardText;
			this.takesTwo = takesTwo;
		}

		public NewBlackCardNetDat() {

		}
	}

	[Serializable]
	[XmlType("F")]
	public class StartSelectingNetDat : NewBlackCardNetDat {
		[XmlAttribute("c")]
		public byte czarId;

		public StartSelectingNetDat(byte czarId, string cardText, bool takesTwo) : base(cardText, takesTwo) {
			this.czarId = czarId;
		}

		public StartSelectingNetDat() : base() {

		}
	}

	[Serializable]
	[XmlType("G")]
	public class RequestNewBlackCardNetDat : NetworkingData {
		public RequestNewBlackCardNetDat() {

		}
	}

	[Serializable]
	[XmlType("H")]
	public class ClientCardNetDat : NetworkingData {

		[XmlAttribute("a")]
		public string cardOne;

		[XmlAttribute("b")]
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
	[XmlType("I")]
	public class StartVotingPhaseNetDat : NetworkingData {
		[XmlArray("a")]
		[XmlArrayItem("p")]
		public PlayerCards[] playerCards;

		[XmlAttribute("b")]
		public int randSeed;
		public StartVotingPhaseNetDat() {

		}
		public StartVotingPhaseNetDat(PlayerCards[] playerCards, int randSeed) {
			this.playerCards = playerCards;
			this.randSeed = randSeed;
		}
	}

	[Serializable]
	[XmlType("J")]
	public class ClientCardCancelNetDat : NetworkingData {
		public ClientCardCancelNetDat() {

		}
	}

	[Serializable]
	[XmlType("K")]
	public class RevealCardNetDat : NetworkingData {
		[XmlAttribute("a")]
		public byte playerToReveal;

		[XmlAttribute("b")]
		public int revealFlag;
		public RevealCardNetDat() {

		}

		public RevealCardNetDat(byte playerToReveal, int revealFlag) {
			this.playerToReveal = playerToReveal;
			this.revealFlag = revealFlag;
		}
	}

	[Serializable]
	[XmlType("L")]
	public class OpenPageNetDat : NetworkingData {
		[XmlAttribute("a")]
		public byte page;
		public OpenPageNetDat() {

		}

		public OpenPageNetDat(byte page) {
			this.page = page;
		}
	}

	[Serializable]
	[XmlType("M")]
	public class RoundWinnerNetDat : NetworkingData {
		[XmlAttribute("a")]
		public byte winnerId;
		public RoundWinnerNetDat() {

		}

		public RoundWinnerNetDat(byte winnerId) {
			this.winnerId = winnerId;
		}
	}

	[Serializable]
	[XmlType("N")]
	public class GameWinnerNetDat : NetworkingData {
		[XmlAttribute("a")]
		public byte winnerId;
		public GameWinnerNetDat() {

		}

		public GameWinnerNetDat(byte winnerId) {
			this.winnerId = winnerId;
		}
	}
}
