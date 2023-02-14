using StereoKit;
using System;
using System.Collections;
using System.Timers;

namespace CHIPSZ
{
    internal class Program
    {
        private static Target target;
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

            Floor floor = new Floor();

            target = new Target();
            target.setDefaultShape();
            target.setRandomPose();

            
            ArrayList projectiles = new ArrayList();
            
            int projectileSize = projectiles.Count;
            Vec3 textPos = new Vec3(-1.0f, 0.5f, -2.0f);


            // Core application loop
            while (SK.Step(() =>
            {
                //Pose solidCurrentPose;
                Hand hand = Input.Hand(Handed.Right);
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw( floor.getMaterial(), floor.getTransform() );

                if (Input.Key(Key.MouseRight).IsJustActive())
                {
                    projectiles.Add(new Ball(hand.palm.position, 0.3f));
                }
                Text.Add("Count :" + projectiles.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                for (int i = 0; i < projectiles.Count; i++)
                {
                    Ball currentBall = (Ball)projectiles[i];
                    currentBall.Draw(( hand), i);
                }
                target.draw();
            })) ;
            SK.Shutdown();
        }

        
    }
}
