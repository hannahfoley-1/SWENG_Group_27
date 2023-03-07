using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StereoKit;
using System.Xml.Serialization;

namespace CHIPSZClassLibrary
{
    public class BallGenerator
    {
        private List<Ball> balls;
        private Vec3 textPos;
        private Vec3 scoreTextPos;
        public BallGenerator()
        {
            balls = new List<Ball>();
            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
        }

        public void Add(Hand hand, Element element)
        {
            balls.Add(new Ball(hand.palm.position, 0.3f,element) );
        }

        public void Draw(Hand hand, bool demo)
        {
            if (!demo)
            {
                Text.Add("Count :" + balls.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                Text.Add("Score :" + "", Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            }
            for (int i = 0; i < balls.Count; i++)
            {
                Ball currentBall = balls[i];
                currentBall.Draw((hand), i);
            }
        }

        public List<Ball> GetAllBalls()
        {
            return balls;
        }
    }
}
