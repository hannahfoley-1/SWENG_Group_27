﻿using StereoKit;
using System.Collections.Generic;
using Windows.Storage.Pickers.Provider;

namespace CHIPSZClassLibrary
{
    public class TargetGenerator
    {
        private List<Target> pool;
        private int poolSize = 10;
        public int targetsHit = 0;
        public GameTimer timer;
        private float speed = 0.05f;

        public TargetGenerator()
        {
            timer = new GameTimer(2.0);
            pool = new List<Target>();
            for (int i = 0; i < poolSize; i++)
            {
                if (i % 3 == 0)
                    pool.Add(new MiniTarget());
                if (i % 4 == 0)
                    pool.Add(new Target());
                else
                    pool.Add(new SinTarget());
                Target current = pool[i];
                current.SetHidden(true);
                current.SetDefaultShape();
            }
        }

        private void UpdatePosition(Target target)
        {
            target.Move(speed);
        }

        private void EnableAvailableTarget()
        {
            foreach (Target target in pool)
            {
                if (target.GetHidden())
                {
                    target.SetRandomPose();
                    target.SetHidden(false);
                    return;
                }
            }
        }

        public void CheckHit(List<Projectile> projectiles, ProjectileGenerator ballGenerator, Hand hand, Countdown countdown)
        {
            foreach (Target target in pool)
            {
                if (!target.GetHidden()) targetsHit += target.CheckHit(projectiles, ballGenerator, hand, countdown);
            }
        }

        public void Draw()
        {
            timer.Update();
            if (timer.elasped)
            {
                EnableAvailableTarget();
                timer.Reset();
            }
            foreach (Target target in pool)
            {
                if (!target.GetHidden())
                {
                    target.Draw();
                    if (!target.getStopped()) UpdatePosition(target);
                    else
                    {
                        GameTimer targetTimer = target.GetTimer();
                        targetTimer.Update();
                        if (targetTimer.elasped) target.SetStopped(false);
                    }
                }
            }
        }
    }
}
