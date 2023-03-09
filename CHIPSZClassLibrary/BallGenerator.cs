using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StereoKit;
using System.Xml.Serialization;
using Windows.Media.PlayTo;

namespace CHIPSZClassLibrary
{
    public class BallGenerator
    {
        private List<Ball> balls;
        private Vec3 textPos;
        int playerScore;
        private Vec3 scoreTextPos;

        public BallGenerator()
        {
            balls = new List<Ball>();
            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            playerScore = 0;
        }

        public void Add(Hand hand, Element element)
        {

            balls.Add(new Ball(hand.palm.position, 0.3f,element) );
        }
        
        public void updatePlayerScore(Hand hand, Ball ball)
        {
            int xPosition = (int)(hand.palm.position.x - ball.GetPosition().position.x);
            int yPosition = (int)(hand.palm.position.y - ball.GetPosition().position.y);

            int multiplier = xPosition > yPosition ? xPosition : yPosition;
            playerScore += 5 * (multiplier != 0 ? multiplier : 1 );
        }

        public void Draw(bool demo)
        {
            if (!demo)
            {
                Text.Add("Count :" + balls.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                Text.Add("Score :" + playerScore, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            }
            for (int i = 0; i < balls.Count; i++)
            {
                Ball currentBall = balls[i];
                currentBall.Draw();
            }
        }

        public void Update(Hand hand) {
            for (int i = 0; i< balls.Count;i++) {
                Ball ball = balls[i];
                if (ball.getTime() > 5.0f)
                {
                    balls.RemoveAt(i);
                }
                else 
                {
                    Pose prevBallPose = ball.GetPrevPose();
                    Bounds ballBounds = ball.GetModel().Bounds;
                    Pose ballPose = ball.GetPosition();
                    prevBallPose = ballPose;
                    if (ball.element == Element.EARTH && hand.IsGripped && ballBounds.Contains(hand.palm.position - ballPose.position))
                    {
                        ballPose.position = hand.palm.position;
                        ball.solid.Teleport(ballPose.position, Quat.Identity);
                        ball.solid.SetVelocity(GetVelocity(ballPose.position, prevBallPose.position));
                    }
                    //updatePlayerScore(hand, ball);
                    ball.UpdatePosition();
                }
            }
        }

        public List<Ball> GetAllBalls()
        {
            return balls;
        }

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf; ;
            return result;
        }
    }
}
