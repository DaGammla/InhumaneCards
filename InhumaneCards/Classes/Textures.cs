using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InhumaneCards.Classes{
    public class Textures{
        private static Texture2D[] textures;

        //Load all textures from @TexNum
        //@Content has to be taken from the Game class and is used to load the Textures
        public static void LoadTextures(ContentManager content) {
            Console.WriteLine("Loading Textures...");
            textures = new Texture2D[TexNum.Amount()];
            //This var counts the time passed
            int ms_before = Environment.TickCount;
            
            //Load every Texture from @TexNum into @textures
            for (int i = 0; i < TexNum.Amount(); i++) {
                Console.WriteLine("Loading \"" + TexNum.GetTexNum(i).GetPath() + "\"");
                textures[i] = content.Load<Texture2D>(TexNum.GetTexNum(i).GetPath());
            }

            //Prints the Time passed
            Console.WriteLine("Loading Textures done after " + (double)(Environment.TickCount - ms_before) / 1000.0 + " seconds");
        }

        //Returns the Loaded Texture with the index @i
        public static Texture2D GetTexture(int i) {
            return textures[i];
        }

        //Returns the Loaded Texture associated with the TexNum @num
        public static Texture2D GetTexture(TexNum num) {
            return textures[num.GetId()];
        }

    }
}