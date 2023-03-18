using StereoKit;
using System;
using System.Collections.Generic;
using CHIPSZClassLibrary;
using System.Threading;
using System.Runtime.CompilerServices;
using Windows.Media.Core;
using Windows.UI.Xaml.Documents;

namespace CHIPSZ
{
    internal class Program
    {
        private static Countdown countdown;
        private static ProjectileGenerator ballGenerator;
        private static TargetGenerator targetGenerator;
        private static TargetGenerator demoTargets;
        private static Floor floor;
		private static StartingScreen screen;
        private static FinishScreen finishScreen;
        private static AudioManager audioManager;
        private static GameTimer spawnBallTimer;

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf; ;
            return result;
        }
        public static double Magnitude(Vec3 velocity)
        {
            return Math.Sqrt((velocity.x * velocity.x) + (velocity.y * velocity.y) + (velocity.z * velocity.z));
        }

        public static void Initialise()
        {
            audioManager = new AudioManager();
            countdown = new Countdown(5); // sets the game duration to 90 seconds
            countdown.SetRunning(false);
            floor = new Floor();
            screen = new StartingScreen();
            finishScreen = new FinishScreen();
            ballGenerator = new ProjectileGenerator();
            targetGenerator = new TargetGenerator();
            demoTargets = new TargetGenerator();
            spawnBallTimer = new GameTimer(0.5);
        }

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

            Initialise();
            // Core application loop
            //while (countdown.IsRunning() && SK.Step(() => // when the time runs out the app closes
            //booleans to switch between game and demo states
            bool closeForGame = screen.GetIfStartGame();
            bool closeForDemo = screen.GetIfStartDemo();

            bool tempFlipWaterFireSpawn = false;

            Hand hand = Input.Hand(Handed.Right);
            Vec3 handPreviousFrame;
            Vec3 scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            while (SK.Step(() => 
            {
                handPreviousFrame = hand.palm.position;
                hand = Input.Hand(Handed.Right);
                spawnBallTimer.Update();
                screen.Draw();
                closeForGame = screen.GetIfStartGame();
                closeForDemo = screen.GetIfStartDemo();
                
                //Pose solidCurrentPose;
                //GAME STATE:
                if (closeForGame == false)
                {
                    ballGenerator.ResetPlayerScore();
                    countdown.SetRunning(true);

                    hand.Solid = false;
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.GetMaterial(), floor.GetTransform());

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.SpawnProjectile(hand, Element.EARTH);
                            audioManager.Play("spawnBall", hand.palm.position, 1f);
                            spawnBallTimer.Reset();
                        }
                    }
                    
                    else if(Input.Key(Key.F).IsJustActive() || (GetVelocity(hand.palm.position,handPreviousFrame).z < -3f && hand.gripActivation == 0)) {

                       if (spawnBallTimer.elasped)
                       {
                            if (tempFlipWaterFireSpawn)
                            {
                                ballGenerator.SpawnProjectile(hand, Element.FIRE);
                                audioManager.Play("spawnBall", hand.palm.position, 1f);
                                spawnBallTimer.Reset();
                                tempFlipWaterFireSpawn = false;
                            }
                            
                            else
                            {
                                ballGenerator.SpawnProjectile(hand, Element.WATER);
                                audioManager.Play("spawnBall", hand.palm.position, 1f);
                                spawnBallTimer.Reset();
                                tempFlipWaterFireSpawn = true;
                            }
                            
                       }
                    }

                    //Text.Add("Score :" + targetGenerator.targetsHit, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                    ballGenerator.Update(hand);
                    ballGenerator.Draw(false);
                    targetGenerator.Draw();
                    targetGenerator.CheckHit(ballGenerator.GetAllProjectiles(), ballGenerator, hand);
                }
                //DEMO STATE:
                else if (closeForDemo == false)
                {
                    if (SK.System.displayType == Display.Opaque)
                        Default.MeshCube.Draw(floor.GetMaterial(), floor.GetTransform());

                    if(screen.PlayDemo1() == true)
                    {
                        if(screen.PlayDemo2() == true)
                        {
                            screen.PlayDemo3();
                            demoTargets.Draw();
                            demoTargets.CheckHit(ballGenerator.GetAllProjectiles(), ballGenerator, hand);
                        }
                    }

                    if (Input.Key(Key.MouseRight).IsJustActive() || hand.IsJustGripped)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            ballGenerator.SpawnProjectile(hand, Element.EARTH);
                            audioManager.Play("spawnBall", hand.palm.position, 1f);
                            spawnBallTimer.Reset();
                        }
                    }
                    else if (Input.Key(Key.F).IsJustActive() || GetVelocity(hand.palm.position, handPreviousFrame).z < -3f && hand.gripActivation == 0)
                    {
                        if (spawnBallTimer.elasped)
                        {
                            if (tempFlipWaterFireSpawn)
                            {
                                ballGenerator.SpawnProjectile(hand, Element.FIRE);
                                audioManager.Play("spawnBall", hand.palm.position, 1f);
                                spawnBallTimer.Reset();
                                tempFlipWaterFireSpawn = false;
                            }

                            else
                            {
                                ballGenerator.SpawnProjectile(hand, Element.WATER);
                                audioManager.Play("spawnBall", hand.palm.position, 1f);
                                spawnBallTimer.Reset();
                                tempFlipWaterFireSpawn = true;
                            }
                        }
                    }

                    ballGenerator.Update(hand);
                    ballGenerator.Draw(true);

                    if (screen.GetIfEndDemo())
                    {
                        screen.SetIfStartDemo(true);
                        screen.SetIfStartGame(false);
                    }
                }
                countdown.Update();
                if (countdown.GetDuration() <= 0)
                {
                    finishScreen.Update();
                    if(finishScreen.OptionSelected() && finishScreen.IsReset())
                    {
                        Initialise();
                    }
                    else if (finishScreen.OptionSelected() && finishScreen.IsExit())
                    {
                    }

                }
            })) ;
            SK.Shutdown();
        }

    }

}
