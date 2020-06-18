using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InhumaneCards.Classes {
	public class DrawableCard {

		public const bool BLACK = false, WHITE = true;

		
		private const int MARGIN = 5;
		private const int LINE_HEIGHT = 40;

		public const int CARD_WIDTH = 310;
		public const int CARD_HEIGHT = 300;
		private const float TEXT_SIZE = 0.6125f;
		public static readonly Color BLACK_COLOR = new Color(0.19f, 0.19f, 0.19f);
		private static readonly Color WHITE_COLOR = new Color(0.36f, 0.36f, 0.36f);
		private static readonly Color TEXT_COLOR = new Color(0.82f, 0.82f, 0.82f);

		protected List<string> lines;

		private bool color;
		private int posX, posY;
		protected Rectangle destination;
		protected BaseGame game;

		public DrawableCard(bool color, int posX, int posY, string text, BaseGame game) {
			this.color = color;
			this.posX = posX;
			this.posY = posY;
			this.destination = new Rectangle(posX, posY, CARD_WIDTH, CARD_HEIGHT);

			this.game = game;

			UpdateText(text);
		}

		public void UpdateText(string text) {
			lines = new List<string>();

			text = Regex.Replace(text, @"\b-\b", "- µ");

			var wordSplit = text.Split(' ');
			

			SpriteFont font = FontNum.DejaVuSans.F();

			var correctedWords = new List<string>();
			for (int i = 0; i < wordSplit.Length; i++) {
				var word = wordSplit[i];
				if (font.MeasureString(word + " ").X * TEXT_SIZE < CARD_WIDTH - (2 * MARGIN)) {
					correctedWords.Add(word);
				} else {
					var secondHalf = "";
					while (font.MeasureString(word + " ").X * TEXT_SIZE >= CARD_WIDTH - (2 * MARGIN)) {
						secondHalf = word[word.Length - 1] + secondHalf;
						word = word.Substring(0, word.Length - 1);
					}
					correctedWords.Add(word);
					correctedWords.Add(secondHalf);
				}
			}

			var currentLine = "";

			for (int i = 0; i < correctedWords.Count; i++) {
				if (font.MeasureString(currentLine + " " + correctedWords[i]).X * TEXT_SIZE <= CARD_WIDTH - (2 * MARGIN)) {
					if (correctedWords[i][0] == 'µ') {
						currentLine += correctedWords[i].Remove(0, 1);
					} else {
						currentLine += " " + correctedWords[i];
					}
				} else {
					lines.Add(currentLine.Trim());
					currentLine = correctedWords[i];
					if (currentLine[0] == 'µ') {
						currentLine = currentLine.Remove(0, 1);
					}
				}

			}
			lines.Add(currentLine.Trim());
		}

		public virtual void Draw() {
			game.Draw(TexNum.PIXEL.T(), destination, (color == BLACK ? BLACK_COLOR : WHITE_COLOR));

			for (int i = 0; i < lines.Count; i++) {
				game.DrawString(FontNum.DejaVuSans.F(), lines[i], new Vector2(posX + MARGIN, posY + MARGIN + LINE_HEIGHT * i), TEXT_COLOR, 0f, Vector2.Zero, TEXT_SIZE, SpriteEffects.None, 0f);
			}
		}

		public virtual void Update() {
			
		}
	}
}
