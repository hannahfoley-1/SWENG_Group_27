using StereoKit;
using System;

namespace CHIPSZ
{
    internal class Program
    {
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


            // Create assets used by the app
            Pose cubePose = new Pose(0, 0, -0.5f, Quat.Identity);
            Model cube = Model.FromMesh(
                Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
                Default.MaterialUI);

            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;


            // Core application loop
            while (SK.Step(() =>
            {
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw(floorMaterial, floorTransform);

<<<<<<< HEAD
                UI.Handle("Cube", ref cubePose, cube.Bounds);
                cube.Draw(cubePose.ToMatrix());
=======
                screen.Draw();
                closeForGame = screen.getIfStartGame();
                closeForDemo = screen.getIfStartDemo();

            //Pose solidCurrentPose;
            //GAME STATE:
                if (closeForGame == false)
                {
                    countdown.SetRunning(true);
                    Hand hand = Input.Hand(Handed.Right);
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.getMaterial(), floor.getTransform());

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.Add(hand);
                            audioManager.Play("cymbalCrash2Second");
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

                    screen.playDemo();

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.Add(hand);
                            audioManager.Play("cymbalCrash2Second");
                            spawnBallTimer.Reset();
                        }

                    }
                    ballGenerator.Draw(hand, true);

                    if (screen.getIfEndDemo())
                    {
                        screen.setIfStartDemo(true);
                        screen.setIfStartGame(false);
                    }
                }
                countdown.Update();
>>>>>>> parent of 8fff5d2 (Added new ball sound (whoosh) to replace old sound (cymbals))
            })) ;
            SK.Shutdown();
        }
    }
}
