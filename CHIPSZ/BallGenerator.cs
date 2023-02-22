using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StereoKit;

namespace CHIPSZ
{
    class BallGenerator
    {
        private ArrayList balls;
        Vec3 textPos;
        Vec3 scoreTextPos;
        public BallGenerator()
        {
            balls = new ArrayList();
            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
        }
        
        public void add( Hand hand )
        {
            balls.Add( new Ball(hand.palm.position, 0.3f) );
        }

        public void draw( Hand hand )
        {
            Text.Add("Count :" + balls.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            Text.Add("Score :" + "", Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            for (int i = 0; i < balls.Count; i++)
            {
                Ball currentBall = (Ball)balls[i];
                currentBall.Draw((hand), i);
            }
        }

        public ArrayList getAllBalls()
        {
            return balls;
        }
    }
}
