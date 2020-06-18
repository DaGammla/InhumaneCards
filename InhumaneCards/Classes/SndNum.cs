using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InhumaneCards.Classes {


    //This class mimics a Java Enum
    public class SndNum {

        //This List contains all Nums for Looping through or index related stuff
        private static List<SndNum> snd_nums = new List<SndNum>();
        public static SndNum GetSndNum(int i) {
            return snd_nums.ElementAt(i);
        }
        public static int Amount() {
            return snd_nums.Count;
        }

        public static readonly SndNum LASER0 = a("sounds/laser");
        public static readonly SndNum LASER1 = a("sounds/laser1");
		public static readonly SndNum MUSIC = a("sounds/8-bit-mayhem");
		public static readonly SndNum MENU_MUSIC = a("sounds/arcade-puzzler");
		public static readonly SndNum ENEMY_HIT = a("sounds/enemyhit");
		public static readonly SndNum PLAYER_HIT = a("sounds/playerhit");
		public static readonly SndNum METEOR_HIT = a("sounds/meteorhit");
		public static readonly SndNum DESTROY = a("sounds/destroy");
		public static readonly SndNum ITEM_PICKED = a("sounds/itemPicked");
		public static readonly SndNum MATERIAL_PICKED = a("sounds/materialsPickup");



		//This var counts the number of Nums added for unique ids
		private static int count_nums = 0;
        //This method adds a new Num to the list and return it
        private static SndNum a(String path) {
            SndNum num = new SndNum(count_nums, path);
            count_nums++;
            snd_nums.Add(num);
            return num;
        }

        

        private int index;
        private String path;
        private SndNum(int index, String path) {
            this.index = index;
            this.path = path;
        }

        public int GetId() {
            return index;
        }

        public String GetPath() {
            return path;
        }

        
        
    }
}