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
                appName = "CHIPSZ",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);

            target = new Target();
            target.setDefaultShape();
            target.setRandomPose();

            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;


            // Core application loop
            while (SK.Step(() =>
            {
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw(floorMaterial, floorTransform);
                target.draw();
            })) ;
            SK.Shutdown();
        }

        
    }
}
