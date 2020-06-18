using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InhumaneCards.Classes {


    //This class mimics a Java Enum
    public class FontNum {

        //This List contains all Nums for Looping through or index related stuff
        private static List<FontNum> font_nums = new List<FontNum>();
        public static FontNum GetFontNum(int i) {
            return font_nums.ElementAt(i);
        }
        public static int Amount() {
            return font_nums.Count;
        }

        public static readonly FontNum DejaVuSans = a("font/DejaVuSans");


		//This var counts the number of Nums added for unique ids
		private static int count_nums = 0;
        //This method adds a new Num to the list and return it
        private static FontNum a(String path) {
			FontNum num = new FontNum(count_nums, path);
            count_nums++;
            font_nums.Add(num);
            return num;
        }

        

        private int index;
        private String path;
        private FontNum(int index, String path) {
            this.index = index;
            this.path = path;
        }

        public int GetId() {
            return index;
        }

        public String GetPath() {
            return path;
        }

        public SpriteFont F() {
            return Fonts.GetFont(index);
        }
    }
}