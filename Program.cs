using StereoKit;
using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "HelloWorld",
                assetsFolder = "Assets",
                displayPreference = DisplayMode.Flatscreen
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);


            // Create assets used by the app
            Pose cubePose = new Pose(0, 0, -0.5f, Quat.Identity);
            Model cube = Model.FromMesh(
                Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
                Default.MaterialUI);

            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

            // for the chair
            Model chair = Model.FromFile("chair.glb");
            Pose chairPose = new Pose(0.5f, -0.2f, 0.2f, Quat.FromAngles(0, 0, 0));
            ////for the window
            //Pose windowPose = new Pose(-.4f, 0, 0, Quat.LookDir(1, 0, 1));

            //bool showHeader = true;
            //float slider = 0.5f;

            //Sprite powerSprite = Sprite.FromFile("power.png", SpriteType.Single);


            // Core application loop
            while (SK.Step(() =>
            {
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw(floorMaterial, floorTransform);

                UI.Handle("Cube", ref cubePose, cube.Bounds);
                UI.Handle("Chair", ref chairPose, chair.Bounds);
                cube.Draw(cubePose.ToMatrix());
                chair.Draw(chairPose.ToMatrix());
                Text.Add("hello there", Matrix.T(0, 0.5f, -0.5f));
            })) ;
            SK.Shutdown();

        }

    }
}


