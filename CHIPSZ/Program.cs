using StereoKit;
using System;
using System.Collections;
using CHIPSZClassLibrary;

namespace CHIPSZ
{
    internal class Program
    {
        private static Countdown countdown;
        private static BallGenerator ballGenerator;
        private static Floor floor;
		private static StartingScreen screen;
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
            ArrayList targets = new ArrayList();
            floor = new Floor();
			screen = new StartingScreen();


            for (int i = 0; i < 10; i++) {
                targets.Add(new Target());
                Target target = (Target)targets[i];
                target.SetDefaultShape();
                target.SetRandomPose();
            }
            ballGenerator = new BallGenerator();

            GameTimer spawnBallTimer = new GameTimer(0.5);

            // Core application loop
            //while (countdown.IsRunning() && SK.Step(() => // when the time runs out the app closes
            //booleans to switch between game and demo states
            bool closeForGame = screen.getIfStartGame();
            bool closeForDemo = screen.getIfStartDemo();
            while (countdown.GetDuration() > 0.0 && SK.Step(() => // when the time runs out the app closes
            {
                handPreviousFrame = hand.palm.position;
                hand = Input.Hand(Handed.Right);
                spawnBallTimer.Update();
                screen.Draw();
                closeForGame = screen.getIfStartGame();
                closeForDemo = screen.getIfStartDemo();
                
                //Pose solidCurrentPose;
                //GAME STATE:
                if (closeForGame == false)
                {
                    countdown.SetRunning(true);
                    
                    hand.Solid = false;
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.getMaterial(), floor.getTransform());

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.Add(hand, Element.EARTH);
                            audioManager.Play("spawnBall");
                            spawnBallTimer.Reset();
                        }
                    }
                    
                    else if(Input.Key(Key.F).IsJustActive() || (GetVelocity(hand.palm.position,handPreviousFrame).z < -3f && hand.gripActivation == 0)) {

                       if (spawnBallTimer.elasped)
                       {
                            ballGenerator.Add(hand, Element.FIRE);
                            audioManager.Play("spawnBall");
                            spawnBallTimer.Reset();
                       }
                    }
                    ballGenerator.Draw(hand, false);
                    foreach (Target target in targets) {
                        target.Draw();
                        target.CheckHit(ballGenerator, hand);
                    };
                }
                //DEMO STATE:
                else if (closeForDemo == false)
                {
                    Hand hand = Input.Hand(Handed.Right);
                    
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.getMaterial(), floor.getTransform());

                    if(screen.playDemo1() == true)
                    {
                        screen.playDemo2();
                    }

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.Add(hand);
                            audioManager.Play("cymbalCrash2Second");
                            spawnBallTimer.Reset();
                        }

                    }
                    
                    else if (Input.Key(Key.F).IsJustActive() || GetVelocity(hand.palm.position, handPreviousFrame).z < -3f && hand.gripActivation == 0)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.Add(hand, Element.FIRE);
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
