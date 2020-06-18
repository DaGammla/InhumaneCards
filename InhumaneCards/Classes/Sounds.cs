using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;

namespace InhumaneCards.Classes {
    public class Sounds {
        private static SoundEffect[] sounds;

        //Load all sounds from @SndNum
        //@Content has to be taken from the Game class and is used to load the Textures
        public static void LoadSounds(ContentManager Content) {
            Console.WriteLine("Loading Sounds...");
            sounds = new SoundEffect[SndNum.Amount()];
            //This var counts the time passed
            int ms_before = Environment.TickCount;

            //Load every Sounds from @SndNum into @sounds
            for (int i = 0; i < SndNum.Amount(); i++) {
                Console.WriteLine("Loading \"" + SndNum.GetSndNum(i).GetPath() + "\"");
                sounds[i] = Content.Load<SoundEffect>(SndNum.GetSndNum(i).GetPath());
            }

            //Prints the Time passed
            Console.WriteLine("Loading Sounds done after " + (double)(Environment.TickCount - ms_before) / 1000.0 + " seconds");
        }

        //Returns the Loaded Sound with the index @i
        public static SoundEffect GetSound(int i) {
            return sounds[i];
        }

        //Returns the Loaded Sound associated with the SndNum @num
        public static SoundEffect GetSound(SndNum num) {
            return sounds[num.GetId()];
        }

    }
}