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
        public BallGenerator()
        {
            balls = new List<Ball>();
            textPos = new Vec3(-1.0f, 0.5f, -2.0f);          
        }

        public void Add(Hand hand, Element element)
        {
            balls.Add(new Ball(hand.palm.position, 0.3f,element) );
        }

        public void Draw(bool demo)
        {
            if (!demo)
            {
                Text.Add("Count :" + balls.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                
            }
            for (int i = 0; i < balls.Count; i++)
            {
                Ball currentBall = balls[i];
                currentBall.Draw();
            }
        }

        public void Update(Hand hand) {
            foreach (Ball ball in balls) {
                Pose prevBallPose = ball.GetPrevPose();
                Bounds ballBounds = ball.GetModel().Bounds;
                Pose ballPose = ball.GetPosition();
                prevBallPose = ballPose;
                if (ball.element == Element.EARTH && (ballBounds.Contains(hand.palm.position-ballPose.position)) && hand.IsGripped) {
                    ballPose.position = hand.palm.position;
                    ball.solid.Teleport(ballPose.position, Quat.Identity);
                    ball.solid.SetVelocity(GetVelocity(ballPose.position, prevBallPose.position));
                }
                ball.UpdatePosition();
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
