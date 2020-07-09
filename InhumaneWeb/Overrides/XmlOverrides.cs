using InhumaneCards.Classes.Networking;
using InhumaneWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml {

	public static class AttributeExtensions {
		public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector)
			where TAttribute : Attribute {
			var att = type.GetCustomAttributes(
				typeof(TAttribute), true
			).FirstOrDefault() as TAttribute;
			if (att != null) {
				return valueSelector(att);
			}
			return default(TValue);
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class XmlIncludeAttribute : Attribute {

		public XmlIncludeAttribute(Type type) {

		}

	}

	public class XmlRoot : Attribute {

		public XmlRoot(string a) {

		}

		public string Namespace;

	}

	public class XmlType : Attribute {
		public string xmlName;
		public XmlType(string xmlName) {
			this.xmlName = xmlName;
		}

	}

	public class XmlAttribute : Attribute {
		public XmlAttribute(string a) {

		}
	}

	public class XmlArray : Attribute {
		public XmlArray(string a) {

		}
	}

	public class XmlArrayItem : Attribute {
		public XmlArrayItem(string a) {

		}
	}
}

namespace System.Xml.Serialization {
	public class XmlSerializer {
		public XmlSerializer(Type a) {
			
		}

		public void Serialize(StringWriter writer, NetworkingData data, XmlSerializerNamespaces c) {

			var parser = new Retyped.dom.DOMParser();

			var xmlText = $"<Dat xmlns:p1=\"http://www.w3.org/2001/XMLSchema-instance\"></Dat>";

			var xmlDoc = parser.parseFromString(xmlText, "text/xml");
			var datElement = xmlDoc.getElementsByTagName("Dat")[0];

			if (data is JoinNetDat) {
				var dat = (JoinNetDat)data;
				datElement.setAttribute("p1:type", "A");
				datElement.setAttribute("a", dat.username);
			} else if (data is PlayersNetDat) {
				//Never send by client

				var dat = (PlayersNetDat)data;
				datElement.setAttribute("p1:type", "B");
			} else if (data is StartGameNetDat) {
				//Never send by client

				var dat = (StartGameNetDat)data;
				datElement.setAttribute("p1:type", "C");
			} else if (data is ClientReadyNetDat) {

				datElement.setAttribute("p1:type", "D");
			} else if (data is NewBlackCardNetDat) {
				//Never send by client
				
				datElement.setAttribute("p1:type", "E");
			} else if (data is StartSelectingNetDat) {
				//Never send by client
				
				datElement.setAttribute("p1:type", "F");
			} else if (data is RequestNewBlackCardNetDat) {

				datElement.setAttribute("p1:type", "G");
			} else if (data is ClientCardNetDat) {
				var dat = (ClientCardNetDat)data;
				datElement.setAttribute("p1:type", "H");
				datElement.setAttribute("a", dat.cardOne);
				datElement.setAttribute("b", dat.cardTwo);
			} else if (data is StartVotingPhaseNetDat) {
				//Never send by client
				//var dat = (StartVotingPhaseNetDat)data;
				datElement.setAttribute("p1:type", "I");
			} else if (data is ClientCardCancelNetDat) {

				datElement.setAttribute("p1:type", "J");
			} else if (data is RevealCardNetDat) {
				var dat = (RevealCardNetDat)data;
				datElement.setAttribute("p1:type", "K");
				datElement.setAttribute("a", dat.playerToReveal.ToString());
				datElement.setAttribute("b", dat.revealFlag.ToString());

			} else if (data is OpenPageNetDat) {
				var dat = (OpenPageNetDat)data;
				datElement.setAttribute("p1:type", "L");
				datElement.setAttribute("a", dat.page.ToString());

			} else if (data is RoundWinnerNetDat) {
				var dat = (RoundWinnerNetDat)data;
				datElement.setAttribute("p1:type", "M");

				datElement.setAttribute("a", dat.winnerId.ToString());

			} else if (data is GameWinnerNetDat) {
				//never send by Client
				datElement.setAttribute("p1:type", "N");

			}

			var tmp = xmlDoc.createElement("div");
			tmp.appendChild(datElement);

			writer.Write(tmp.innerHTML);
			writer.Close();

		}

		public object Deserialize(StringReader reader) {

			var parser = new Retyped.dom.DOMParser();

			var xmlText = reader.ReadToEnd();

			Console.WriteLine(xmlText);

			var xmlDoc = parser.parseFromString(xmlText, "text/xml");

			var datElement = xmlDoc.getElementsByTagName("Dat")[0];
			var type = datElement.getAttribute("p1:type");


			switch (type) {
				case "A": {
					var dat = new JoinNetDat();
					dat.username = datElement.getAttribute("a");
					return dat;
				}

				case "B": {
					var dat = new PlayersNetDat();
					dat.playerCount = byte.Parse(datElement.getAttribute("b"));
					dat.players = new string[dat.playerCount];
					var playerElements = datElement.getElementsByTagName("a")[0].getElementsByTagName("s");
					for (int i = 0; i < dat.playerCount; i++) {
						dat.players[i] = playerElements[i].textContent;
					}

					return dat;
				}

				case "C": {
					var dat = new StartGameNetDat();
					dat.playerCount = byte.Parse(datElement.getAttribute("b"));
					dat.players = new string[dat.playerCount];
					var playerElements = datElement.getElementsByTagName("a")[0].getElementsByTagName("s");
					for (int i = 0; i < dat.playerCount; i++) {
						dat.players[i] = playerElements[i].textContent;
					}

					dat.maxBlankCards = byte.Parse(datElement.getAttribute("c"));

					return dat;
				}

				case "D": {
					return new ClientReadyNetDat();
				}

				case "E": {
					var dat = new NewBlackCardNetDat();
					dat.cardText = datElement.getAttribute("a");
					dat.takesTwo = bool.Parse(datElement.getAttribute("b"));
					return dat;
				}

				case "F": {
					var dat = new StartSelectingNetDat();
					dat.cardText = datElement.getAttribute("a");
					dat.takesTwo = bool.Parse(datElement.getAttribute("b"));
					dat.czarId = byte.Parse(datElement.getAttribute("c"));
					return dat;
				}

				case "G": {
					return new RequestNewBlackCardNetDat();
				}

				case "H": {
					var dat = new ClientCardNetDat();
					dat.cardOne = datElement.getAttribute("a");
					dat.cardTwo = datElement.getAttribute("b");
					return dat;
				}
				case "I": {
					var dat = new StartVotingPhaseNetDat();
					dat.randSeed = int.Parse(datElement.getAttribute("b"));
					var playerElements = datElement.getElementsByTagName("a")[0].getElementsByTagName("p");
					dat.playerCards = new PlayerCards[WebApp.webGame.childGame.GetPlayerCount()];
					Console.WriteLine("Players: " + dat.playerCards.Length);
					for (int i = 0; i < dat.playerCards.Length; i++) {
						if (playerElements[i].getAttribute("p1:nil") == "true") {
							dat.playerCards[i] = null;
						} else {
							dat.playerCards[i] = new PlayerCards(playerElements[i].getAttribute("a"), playerElements[i].getAttribute("b"));
						}
					}
					return dat;
				}
				case "J": {
					return new ClientCardCancelNetDat();
				}
				case "K": {
					var dat = new RevealCardNetDat();
					dat.playerToReveal = byte.Parse(datElement.getAttribute("a"));
					dat.revealFlag = int.Parse(datElement.getAttribute("b"));
					return dat;
				}
				case "L": {
					var dat = new OpenPageNetDat();
					dat.page = byte.Parse(datElement.getAttribute("a"));
					return dat;
				}
				case "M": {
					var dat = new RoundWinnerNetDat();
					dat.winnerId = byte.Parse(datElement.getAttribute("a"));
					return dat;
				}

				case "N": {
					var dat = new GameWinnerNetDat();
					dat.winnerId = byte.Parse(datElement.getAttribute("a"));
					return dat;
				}
			}


			return new NetworkingData();
		}

	}

	public class XmlSerializerNamespaces {
		public XmlSerializerNamespaces(object[] a) {

		}
	}

	public class XmlQualifiedName {
		public static object Empty = null;
	}

	public class XmlWriterSettings {

		public bool Indent;
		public bool OmitXmlDeclaration;

		public XmlWriterSettings() {
			
		}
	}

	public class XmlWriter {
		public static StringWriter Create(StringWriter a, XmlWriterSettings b) {
			return a;
		}
	}
}
