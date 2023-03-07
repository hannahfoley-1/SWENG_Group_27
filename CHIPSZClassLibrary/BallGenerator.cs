using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StereoKit;
using Windows.Media.PlayTo;

namespace CHIPSZClassLibrary
{
    public class BallGenerator
    {
        private ArrayList balls;
        private Vec3 textPos;
        private Vec3 scoreTextPos;
        private int playerScore;
        public BallGenerator()
        {
            balls = new ArrayList();
            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            playerScore = 0;
        }
        
        public void Add(Hand hand)
        {
            balls.Add( new Ball(hand.palm.position, 0.1f) );
        }
        public void updatePlayerScore()
        {
            playerScore += 25;
        }

        public void Draw(Hand hand, bool demo)
        {
            if (!demo)
            {
                Text.Add("Count :" + balls.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                Text.Add("Score :" + playerScore, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            }
            for (int i = 0; i < balls.Count; i++)
            {
                Ball currentBall = (Ball)balls[i];
                currentBall.Draw((hand), i);
            }
        }

        public ArrayList GetAllBalls()
        {
            return balls;
        }
    }
}
