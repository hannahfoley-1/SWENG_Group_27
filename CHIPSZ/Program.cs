using StereoKit;
using System;
using System.Collections;

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

            /*Widget widget = new Widget();
            widget.setSlider(0.5f);

            Widget highScores = new Widget();
            highScores.setWindowName("highScoreWindow");
            highScores.setPosition(new Pose(.4f, 0, .4f, Quat.LookDir(-1, 0, 1)));

            Widget welcome = new Widget();
            welcome.setWindowName("Welcome");
            welcome.setPosition(new Pose(-.4f, 0, .4f, Quat.LookDir(1, 0, 1)));
            welcome.addButton("Start Game");
            welcome.addButton("Start Demo");*/


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
            while (countdown.GetDuration() > 0.0 && SK.Step(() => // when the time runs out the app closes
            {
                spawnBallTimer.Update();

                // Draw Basic Widget
                /*widget.draw();
                highScores.drawHighScores();
                welcome.draw();*/

                screen.Draw();
                //Pose solidCurrentPose;
                bool close = screen.GetIfClose();
                if (close == false) {
                    countdown.SetRunning(true);
                    Hand hand = Input.Hand(Handed.Right);
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.getMaterial(), floor.getTransform());

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {

                        ballGenerator.Add(hand);
                        audioManager.Play("cymbalCrash2Second");

                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.add(hand);
                            audioManager.Play("cymbalCrash2Second");
                            spawnBallTimer.Reset();
                        }

                    }
                    ballGenerator.Draw(hand);
                    foreach (Target target in targets) {
                        target.Draw();
                        target.CheckHit(ballGenerator.GetAllBalls());
                    };
                }
                countdown.Update();
            })) ;
            SK.Shutdown();
        }
        
        
    }

}
