﻿using System;
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
        
        public void updatePlayerScore(Hand hand, Ball ball)
        {
            int xPosition = (int)(hand.palm.position.x - ball.GetPosition().position.x);
            int yPosition = (int)(hand.palm.position.y - ball.GetPosition().position.y);

            int multiplier = xPosition > yPosition ? xPosition : yPosition;
            playerScore += 5 * (multiplier != 0 ? multiplier : 1 );
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

        public void Update(Hand hand) {
            for (int i = 0; i< balls.Count;i++) {
                Ball ball = balls[i];
                if (ball.GetTime() > 5.0f)
                {
                    balls.RemoveAt(i);
                }
                else 
                {
                    Pose prevBallPose = ball.GetPrevPose();
                    Bounds ballBounds = ball.GetModel().Bounds;
                    Pose ballPose = ball.GetPosition();
                    prevBallPose = ballPose;
                    if (ball.element == Element.EARTH && hand.gripActivation >= 0.7f && ballBounds.Contains(hand.palm.position - ballPose.position))
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
    }
}
