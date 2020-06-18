using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InhumaneCards.Classes {
	class Fonts {
		private static SpriteFont[] fonts;

		//Load all fonts from @FontNum
		//@content has to be taken from the Game class and is used to load the Fonts
		public static void LoadFonts(ContentManager content) {
			Console.WriteLine("Loading Fonts...");
			fonts = new SpriteFont[FontNum.Amount()];
			//This var counts the time passed
			int ms_before = Environment.TickCount;

			//Load every Font from @FontNum into @fonts
			for (int i = 0; i < FontNum.Amount(); i++) {
				Console.WriteLine("Loading \"" + FontNum.GetFontNum(i).GetPath() + "\"");
				fonts[i] = content.Load<SpriteFont>(FontNum.GetFontNum(i).GetPath());
			}

			//Prints the Time passed
			Console.WriteLine("Loading Fonts done after " + (double)(Environment.TickCount - ms_before) / 1000.0 + " seconds");
		}

		//Returns the Loaded Font with the index @i
		public static SpriteFont GetFont(int i) {
			return fonts[i];
		}

		//Returns the Loaded Font associated with the FontNum @num
		public static SpriteFont GetFont(FontNum num) {
			return fonts[num.GetId()];
		}

		public static Vector2 MeasureString(FontNum num, String str) {
			return GetFont(num).MeasureString(str);
		}

		public static Vector2 MeasureString(int i, String str) {
			return GetFont(i).MeasureString(str);
		}
	}
}
