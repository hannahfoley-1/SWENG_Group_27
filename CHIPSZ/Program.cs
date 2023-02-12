using StereoKit;
using System;
using System.Collections;
using System.Timers;

namespace CHIPSZ
{
    internal class Program
    {
        private static Timer targetTimer;
        private static ArrayList cubes = new ArrayList();
        private static ArrayList cubesPoses = new ArrayList();
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "CHIPSZ",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);

            targetTimer = new System.Timers.Timer();
            targetTimer.Interval = 5000;
            targetTimer.Elapsed += changeCubePoses;
            targetTimer.AutoReset = true;
            targetTimer.Enabled = true;


            Random randomNumberGenerator = new Random();

            for( int i = 0; i < 5; i++)
            {
                float targetPosX = (float)( randomNumberGenerator.Next( -30, 30 ) / 10.0 );
                float targetPosY = (float)( randomNumberGenerator.Next( -10, 10 ) / 10.0 );

                //targetPosX = 

                // Create assets used by the app
                Pose cubePose = new Pose(targetPosX, targetPosY, -2f, Quat.Identity);
                Model cube = Model.FromMesh(
                    Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
                    Default.MaterialUI);
                cubes.Add(cube);
                cubesPoses.Add(cubePose);
            }
            

            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;


            // Core application loop
            while (SK.Step(() =>
            {
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw(floorMaterial, floorTransform);

                //UI.Handle("Cube", ref cubePose, cube.Bounds);
                for( int i = 0; i < 5; i++)
                {
                    Model cube = (Model)cubes[i];
                    Pose cubePose = (Pose)cubesPoses[i];
                    cube.Draw(cubePose.ToMatrix());
                }
            })) ;
            SK.Shutdown();
        }

        private static void changeCubePoses(Object source, System.Timers.ElapsedEventArgs e)
        {
            Random randomNumberGenerator = new Random();
            for (int i = 0; i < 5; i++)
            {
                float targetPosX = (float)(randomNumberGenerator.Next(-30, 30) / 10.0);
                float targetPosY = (float)(randomNumberGenerator.Next(-10, 10) / 10.0);

                //targetPosX = 

                // Create assets used by the app
                Pose cubePose = new Pose(targetPosX, targetPosY, -2f, Quat.Identity);
                cubesPoses[i] = cubePose;
            }
        }
    }
}
