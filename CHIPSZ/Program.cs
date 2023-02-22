using StereoKit;
using System;
using System.Collections;
using System.Timers;

namespace CHIPSZ
{
    internal class Program
    {
        private static Countdown countdown;
        private static BallGenerator ballGenerator;
        private static Floor floor;
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "StereoKitProject1",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);

            countdown = new Countdown(90); // sets the game duration to 90 seconds
            ArrayList targets = new ArrayList();
            floor = new Floor();

            for (int i = 0; i < 10; i++) {
                targets.Add(new Target());
                Target target = (Target)targets[i];
                target.setDefaultShape();
                target.setRandomPose();
            }
            ballGenerator = new BallGenerator();


            // Core application loop
            while (countdown.IsRunning() && SK.Step(() => // when the time runs out the app closes
            {
                //Pose solidCurrentPose;
                Hand hand = Input.Hand(Handed.Right);
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw( floor.getMaterial(), floor.getTransform() );

                if(Input.Key( Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                {
                    ballGenerator.add(hand);
                }
                ballGenerator.draw(hand);
                foreach (Target target in targets) {
                    target.draw();
                    target.checkHit(ballGenerator.getAllBalls());
                };

                countdown.Update();
            })) ;
            SK.Shutdown();
        }
        
        
    }

}
