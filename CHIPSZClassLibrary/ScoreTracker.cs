using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZClassLibrary
{
    public  class ScoreTracker
    {
        private static List<Int32> scores = new List<Int32>();
        public  void AddScore(int score) => scores.Add(score);
        public List<Int32> GetScores() => scores;
    }
}
