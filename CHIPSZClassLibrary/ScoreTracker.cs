using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZClassLibrary
{
    public abstract class ScoreTracker
    {
        private static List<Int32> scores = new List<Int32>();

        public static bool updated = false;

        public static void AddScore(int score)
        {
            updated = true;
            scores.Add(score);
        }
    }
}
