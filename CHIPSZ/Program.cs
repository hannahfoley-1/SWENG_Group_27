using StereoKit;
using System;
using System.Collections;
using System.Timers;

namespace CHIPSZ
{
    internal class Program
    {
        private static Target target;
 
        private static ArrayList cubes = new ArrayList();
        private static ArrayList cubesPoses = new ArrayList();
        static void Main(string[] args)
        {
            // Initialize StereoKit
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "StereoKitProject1",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);

            target = new Target();
            target.setDefaultShape();
            target.setRandomPose();


            Solid floorCollider = new Solid(new Vec3(0, -1.5f, 0), Quat.Identity, SolidType.Immovable);
            floorCollider.AddBox(new Vec3(20, 0.01f, 20));

            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;
            //Ball ball = new Ball(new Vec3(0, 0.5f, -0.5f), 0.3f);
            ArrayList projectiles = new ArrayList();
            Hand hand = Input.Hand(Handed.Right);
            int projectileSize = projectiles.Count;
            Vec3 textPos = new Vec3(-1.0f, 0.5f, -1.5f);


            // Core application loop
            while (SK.Step(() =>
            {
                //Pose solidCurrentPose;              
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw(floorMaterial, floorTransform);
                if (Input.Key(Key.MouseRight).IsJustActive())
                {
                    projectiles.Add(new Ball(hand.palm.position, 0.3f));
                }
                Text.Add("Count :" + projectiles.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                for (int i = 0; i < projectiles.Count; i++)
                {
                    Ball currentBall = (Ball)projectiles[i];
                    currentBall.Draw(hand, i);
                }
                target.draw();
            })) ;
            SK.Shutdown();
        }

        
    }
}
