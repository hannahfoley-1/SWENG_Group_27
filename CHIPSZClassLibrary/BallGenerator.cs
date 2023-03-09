using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StereoKit;
using Windows.Media.PlayTo;
using System.IO.Ports;
using SK = StereoKit;
using System.Numerics;

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
            balls.Add(new Ball(hand.palm.position, 0.1f));
        }
        public void updatePlayerScore(Hand hand, Ball ball)
        {
            int xPosition = (int)(hand.palm.position.x - ball.GetPosition().position.x);
            int yPosition = (int)(hand.palm.position.y - ball.GetPosition().position.y);

            int multiplier = xPosition > yPosition ? xPosition : yPosition;
            playerScore += 5 * (multiplier != 0 ? multiplier : 1);
        }

        public void Draw(Hand hand, bool demo)
        {
            if (!demo)
            {
                Pose windowPoseSeparator = new Pose(-1.0f, 1.05f, -2.003f, Quat.FromAngles(0, 180.0f, 0));
                UI.WindowBegin("Window Separator", ref windowPoseSeparator, new Vec2(135, 70) * U.cm, UIWin.Body);
                UI.WindowEnd();

                Text.Add("Count :" + balls.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                Text.Add("Score :" + playerScore, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            }

            for (int i = 0; i < balls.Count; i++)
            {
                Ball currentBall = (Ball)balls[i];
                currentBall.Draw(hand, i);
            }
        }

        public ArrayList GetAllBalls()
        {
            return balls;
        }
    }
}
