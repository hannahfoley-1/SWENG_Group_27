using StereoKit;
using System;
using System.Collections.Generic;
using CHIPSZClassLibrary;

namespace CHIPSZ
{
    internal class Program
    {
        private static Countdown countdown;
        private static BallGenerator ballGenerator;
        private static TargetGenerator targetGenerator;
        private static Floor floor;
		private static StartingScreen screen;

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf; ;
            return result;
        }
        public static double Magnitude(Vec3 velocity)
        {
            return Math.Sqrt((velocity.x * velocity.x) + (velocity.y * velocity.y) + (velocity.z * velocity.z));
        }

        static void Main(string[] args)
        {
            AudioManager audioManager = new AudioManager();

            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "StereoKitProject1",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);

            countdown = new Countdown(90); // sets the game duration to 90 seconds
            countdown.SetRunning(false);
            floor = new Floor();
			screen = new StartingScreen();

            ballGenerator = new BallGenerator();
            targetGenerator = new TargetGenerator();

            GameTimer spawnBallTimer = new GameTimer(0.5);           

            // Core application loop
            //while (countdown.IsRunning() && SK.Step(() => // when the time runs out the app closes
            //booleans to switch between game and demo states
            bool closeForGame = screen.getIfStartGame();
            bool closeForDemo = screen.getIfStartDemo();
            Hand hand = Input.Hand(Handed.Right);
            Vec3 handPreviousFrame;
            Vec3 scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            while (countdown.GetDuration() > 0.0 && SK.Step(() => // when the time runs out the app closes
            {
                spawnBallTimer.Update();

                screen.Draw();
                closeForGame = screen.getIfStartGame();
                closeForDemo = screen.getIfStartDemo();

            //Pose solidCurrentPose;
            //GAME STATE:
                if (closeForGame == false)
                {
                    countdown.SetRunning(true);                                    
                    handPreviousFrame = hand.palm.position;
                    hand = Input.Hand(Handed.Right);
                    hand.Solid = false;
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.getMaterial(), floor.getTransform());

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.Add(hand, Element.EARTH);
                            audioManager.Play("cymbalCrash2Second");
                            spawnBallTimer.Reset();
                        }
                    }
                    
                    else if(Input.Key(Key.F).IsJustActive() || GetVelocity(hand.palm.position,handPreviousFrame).z < -3f) {

                       if (spawnBallTimer.elasped)
                       {
                            ballGenerator.Add(hand, Element.FIRE);
                            audioManager.Play("cymbalCrash2Second");
                            spawnBallTimer.Reset();
                       }
                    }
                    //Text.Add("Score :" + targetGenerator.targetsHit, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                    ballGenerator.Update(hand);
                    ballGenerator.Draw(false);
                    targetGenerator.Draw();
                    targetGenerator.CheckHit(ballGenerator.GetAllBalls(), ballGenerator, hand);
                }
                //DEMO STATE:
                else if (closeForDemo == false)
                {
                    hand = Input.Hand(Handed.Right);
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.getMaterial(), floor.getTransform());

                    screen.playDemo();

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.Add(hand, Element.EARTH);
                            audioManager.Play("cymbalCrash2Second");
                            spawnBallTimer.Reset();
                        }
                    }
                    ballGenerator.Update(hand);
                    ballGenerator.Draw(true);

                    if (screen.getIfEndDemo())
                    {
                        screen.setIfStartDemo(true);
                        screen.setIfStartGame(false);
                    }
                }
                countdown.Update();
            })) ;
            SK.Shutdown();
        }
        
        
    }

}
