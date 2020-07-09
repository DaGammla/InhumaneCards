using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InhumaneCards.Classes {


    //This class mimics a Java Enum
    public class TexNum {

        //This List contains all Nums for Looping through or index related stuff
        private static List<TexNum> tex_nums = new List<TexNum>();
        public static TexNum GetTexNum(int i) {
            return tex_nums.ElementAt(i);
        }
        public static int Amount() {
            return tex_nums.Count;
        }

        public static readonly TexNum PIXEL = a("pixel");
        public static readonly TexNum CROSS = a("textures/Cross");
        public static readonly TexNum CZAR = a("textures/Czar");

        //This var counts the number of Nums added for unique ids
        private static int count_nums = 0;
        //This method adds a new Num to the list and return it
        private static TexNum a(String path) {
            TexNum num = new TexNum(count_nums, path);
            count_nums++;
            tex_nums.Add(num);
            return num;
        }

        

        private int index;
        private String path;
        private TexNum(int index, String path) {
            this.index = index;
            this.path = path;
        }

        public int GetId() {
            return index;
        }

        public String GetPath() {
            return path;
        }

        public Texture2D T() {
            return Textures.GetTexture(index);
        }
        
    }
}