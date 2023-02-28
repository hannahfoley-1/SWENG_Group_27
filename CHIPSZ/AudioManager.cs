using StereoKit;
using System.Diagnostics;
using System.IO;

namespace CHIPSZ
{
    internal class AudioManager
    {
        // TODO: Potentially change this into a Singleton?

        public struct GameSound
        {
            public string name;
            public Sound sound;
        }

        public GameSound[] gameSounds;

        public AudioManager()
        {
            string activeDirectory = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(Path.Combine(activeDirectory, "Assets/Sounds"));

            gameSounds = new GameSound[files.Length];

            for (int i = 0; i < gameSounds.Length; i++)
            {
                Debug.WriteLine(files[i]);

                gameSounds[i].name = StripSoundName(files[i]);
                gameSounds[i].sound = Sound.FromFile(files[i]);
                Debug.WriteLine(gameSounds[i].name);
            }
        }

        public bool Play(string name, Vec3 position, float volume)
        {
            bool success = false;

            for (int i = 0; i < gameSounds.Length; i++)
            {
                // There can't be duplicates because there can't be
                // two files with the same name
                if (gameSounds[i].name == name)
                {
                    success = true;
                    gameSounds[i].sound.Play(position, volume);
                    break;
                }
            }

            return success; // Only one return path for clarity
        }

        // For simpler use
        public bool Play(string name)
        {
            return Play(name, Vec3.Zero, 1);
        }

        public string StripSoundName(string unstrippedSoundName)
        {
            unstrippedSoundName = unstrippedSoundName.Remove(unstrippedSoundName.Length - 4); // removes .wav/.mp3 from name
            string[] splitSoundName = unstrippedSoundName.Split("\\");
            return splitSoundName[splitSoundName.Length - 1]; // removes path to sound file
        }
    }
}
