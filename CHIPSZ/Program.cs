using StereoKit;
using System;
using System.Collections;
using System.Timers;

namespace CHIPSZ
{
    internal class Program
    {
        private static Target target;
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

            floor = new Floor();
            target = new Target();
            target.setDefaultShape();
            target.setPose(0.0f, 0.0f);
            ballGenerator = new BallGenerator();


            // Core application loop
            while (SK.Step(() =>
            {
                //Pose solidCurrentPose;
                Hand hand = Input.Hand(Handed.Right);
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw( floor.getMaterial(), floor.getTransform() );

                if( Input.Key( Key.MouseRight).IsJustActive())
                {
                    ballGenerator.add(hand);
                }
                ballGenerator.draw( hand );
                target.draw();
                target.checkHit(ballGenerator.getAllBalls()); ;
            })) ;
            SK.Shutdown();
        }
        
        
    }

}
