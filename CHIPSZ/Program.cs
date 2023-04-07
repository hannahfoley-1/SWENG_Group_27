using StereoKit;
using System;
using System.Collections.Generic;
using CHIPSZClassLibrary;
using System.Threading;
using StereoKit.Framework;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.Media.Core;
using Windows.UI.Xaml.Documents;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Numerics;

namespace CHIPSZ
{
    internal class Program
    {

        // score tracker
        private static List<int> scores = new List<int>();

        // countdown
        private static Countdown countdown;

        // projectiles
        private static ProjectileGenerator ballGenerator;

        // targets
        private static TargetGenerator targetGenerator;
        private static TargetGenerator demoTargets;

        private static Floor floor;

        // screens
        private static StartingScreen screen;
        private static FinishScreen finishScreen;
        private static AudioManager audioManager;
        private static GameTimer spawnBallTimer;
        private static PauseMenu pauseMenu;


        private static bool paused;
        private static Element stance;

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf;
            return result;
        }
        public static double Magnitude(Vec3 velocity)
        {
            return Math.Sqrt((velocity.x * velocity.x) + (velocity.y * velocity.y) + (velocity.z * velocity.z));
        }

        public static void Initialise()
        {
            audioManager = new AudioManager();
            countdown = new Countdown(90); // sets the game duration to 90 seconds
            countdown.SetRunning(false);
            floor = new Floor();
            screen = new StartingScreen();
            //finishScreen = new screen();
            ballGenerator = new ProjectileGenerator();
            targetGenerator = new TargetGenerator();
            demoTargets = new TargetGenerator();
            spawnBallTimer = new GameTimer(0.5);

            // pause menu
            pauseMenu = new PauseMenu();
            paused = false;
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

            stance = Element.FIRE;
            HandMenuRadial handMenu = SK.AddStepper(new HandMenuRadial(
                new HandRadialLayer("Root", new HandMenuItem("Fire", null, () => stance = Element.FIRE),
                new HandMenuItem("Earth", null, () => stance = Element.EARTH), new HandMenuItem("Water", null, () => stance = Element.WATER), new HandMenuItem("Air", null, () => stance = Element.AIR))));
            Initialise();


            // Core application loop
            //while (countdown.IsRunning() && SK.Step(() => // when the time runs out the app closes
            //booleans to switch between game and demo states
            bool closeForGame = screen.GetIfStartGame();
            bool closeForDemo = screen.GetIfStartDemo();
          
            
            Vec3 handPreviousFrame = Vec3.Zero;
            Vec3 scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            while (!screen.IsExit() && SK.Step(() => // when the time runs out the app closes
            {
                Hand hand = Input.Hand(Handed.Right);
                pauseMenu.Draw();
                paused = pauseMenu.GetPaused();

                if (paused)
                {
                    countdown.SetRunning(false);
                    Pose pauseScreenPose = new Pose(0, 0, -1f, Quat.LookDir(new Vec3(0, 0, 5)));
                    UI.WindowBegin("", ref pauseScreenPose, new Vec2(30, 20) * U.cm, UIWin.Body);
                    UI.Text("\n\n\n\n\nPAUSED", TextAlign.Center);
                    UI.WindowEnd();
                }
                else
                {
                    // Debug stance toggle
                    if (Input.Key(Key.M).IsJustActive())
                    {
                        if (stance != Element.AIR) stance++;
                        else { stance = Element.FIRE; }
                    }

                    hand = Input.Hand(Handed.Right);
                    spawnBallTimer.Update();
                    screen.Draw(scores);
                    closeForGame = screen.GetIfStartGame();
                    closeForDemo = screen.GetIfStartDemo();
                    Lines.Add(hand.palm.position, hand.palm.position + (hand.palm.Forward.Normalized + new Vec3(0, 0.00f, 0)), Color.Hex(0xFF0000FF), 0.01f);
                    Lines.Add(hand.palm.position, hand.palm.position + (hand.palm.Forward.Normalized + new Vec3(0, 1.00f, 0)), Color.Hex(0x0000FFFF), 0.01f);

                    //Pose solidCurrentPose;
                    //GAME STATE:
                    if (closeForGame == false)
                    {
                        //ballGenerator.ResetPlayerScore();
                        countdown.SetRunning(true);
                        if (countdown.GetDuration() == 0.0)
                            ballGenerator.ResetPlayerScore();

                        hand.Solid = false;
                        if (SK.System.displayType == Display.Opaque)
                            Default.MeshCube.Draw(floor.GetMaterial(), floor.GetTransform());

                        if (Input.Key(Key.F).IsJustActive() || (Magnitude(GetVelocity(hand.palm.position, handPreviousFrame)) > 10f && hand.gripActivation == 0))
                        {
                            if (spawnBallTimer.elasped)
                            {
                                switch (stance)
                                {
                                    case Element.FIRE:
                                        ballGenerator.SpawnProjectile(hand, Element.FIRE);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("FireCast-Modified", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
                                    case Element.EARTH:
                                        ballGenerator.SpawnProjectile(hand, Element.EARTH);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("StoneCast-Modified", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
                                    case Element.WATER:
                                        ballGenerator.SpawnProjectile(hand, Element.WATER);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("WaterCast-Modified", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
                                    case Element.AIR:
                                        ballGenerator.SpawnProjectile(hand, Element.AIR);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("WindCast", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
                                }
                            }
                        }

                        ballGenerator.Update(hand);
                        ballGenerator.Draw(false);
                        targetGenerator.Draw();
                        targetGenerator.CheckHit(ballGenerator.GetAllProjectiles(), ballGenerator, hand, countdown);
                        if (ballGenerator.getDisplayScoreAnimation() && (ballGenerator.getAnimationStartTime() - (int)countdown.GetDuration() == 2))
                        {
                            ballGenerator.resetScoreAnimation();
                        }
                    }
                    //DEMO STATE:
                    else if (closeForDemo == false)
                    {
                        if (SK.System.displayType == Display.Opaque)
                            Default.MeshCube.Draw(floor.GetMaterial(), floor.GetTransform());

                        if (screen.PlayDemo1() == true)
                        {
                            if (screen.PlayDemo2() == true)
                            {
                                screen.PlayDemo3();
                                demoTargets.Draw();
                                demoTargets.CheckHit(ballGenerator.GetAllProjectiles(), ballGenerator, hand, countdown);
                            }
                        }

                        if (Input.Key(Key.F).IsJustActive() || (GetVelocity(hand.palm.position, handPreviousFrame).z < -3f && hand.gripActivation == 0))
                        {
                            if (spawnBallTimer.elasped)
                            {
                                switch (stance)
                                {
                                    case Element.FIRE:
                                        ballGenerator.SpawnProjectile(hand, Element.FIRE);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("FireCast-Modified", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
                                    case Element.EARTH:
                                        ballGenerator.SpawnProjectile(hand, Element.EARTH);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("StoneCast-Modified", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
                                    case Element.WATER:
                                        ballGenerator.SpawnProjectile(hand, Element.WATER);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("WaterCast-Modified", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
                                    case Element.AIR:
                                        ballGenerator.SpawnProjectile(hand, Element.AIR);
                                        AudioManager.Instance.Play("spawnBall", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        AudioManager.Instance.Play("FireCast-Modified", hand.palm.position, 1f);
                                        spawnBallTimer.Reset();
                                        break;
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
                    handPreviousFrame = hand.palm.position;
                    countdown.Update();
                }

                if (countdown.GetDuration() <= 0)
                {
                    scores.Add(ballGenerator.GetPlayerScore());
                    Initialise();
                }
            }));
            SK.RemoveStepper(handMenu);
            SK.Shutdown();
        }
    }
}
