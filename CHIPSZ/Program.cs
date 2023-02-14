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
            target.setPose(0.0f, 0.0f);

            
            ArrayList projectiles = new ArrayList();
            
            int projectileSize = projectiles.Count;
            Vec3 textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            Vec3 textPos2 = new Vec3(1.0f, 0.0f, -2.0f);



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
                    Text.Add( target.getModel().Bounds + "   " + ((Ball)projectiles[ 0 ]).getPosition().position.x, Matrix.TRS(textPos2, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                }
                target.draw();
                checkHit(projectiles); ;
            })) ;
            SK.Shutdown();
        }
        public static void checkHit( ArrayList projectiles )
        {
            foreach( Ball ball in projectiles )
            { 
                if(ball.getPosition().position.x >= target.getPose().position.x && ball.getPosition().position.x < (target.getPose().position.x + target.size) )
                {
                    if(ball.getPosition().position.y >= target.getPose().position.y && ball.getPosition().position.y < (target.getPose().position.y + target.size) )
                    {
                        if(ball.getPosition().position.z >= target.getPose().position.z && ball.getPosition().position.z < (target.getPose().position.z + target.size))
                        {
                            target.hideBall = true;
                        }
                    }
                }

            }
        }
        
    }

}
