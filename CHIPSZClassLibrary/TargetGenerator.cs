using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace CHIPSZClassLibrary
{
    public class TargetGenerator
    {
        private List<Target> pool;
        private int poolSize = 10;
        public int targetsHit = 0;
        public GameTimer timer;
        private float speed = 0.02f;

        public TargetGenerator() {
            timer = new GameTimer(2.0);
            pool = new List<Target>();
            for (int i = 0; i < poolSize; i++) { 
                pool.Add(new Target());
                Target current = pool[i];
                current.SetHidden(true);               
                current.SetDefaultShape();
            }
        }

        private void UpdatePosition(Target target) { 
            Pose currentTargetPose = target.GetPose();
            currentTargetPose.position.z += speed;
            if (currentTargetPose.position.z >= 1) target.SetHidden(true);
            else 
            {
                target.SetPose(currentTargetPose);
            }   
        }

        private void EnableAvailableTarget() {
            foreach (Target target in pool) {
                if (target.GetHidden()) {
                    target.SetRandomPose();
                    target.SetHidden(false);
                    return;
                }
            }
        }

        public void CheckHit(List<Projectile> projectiles, ProjectileGenerator ballGenerator, Hand hand) {
            foreach (Target target in pool) {
                if (!target.GetHidden()) targetsHit += target.CheckHit(projectiles, ballGenerator, hand);               
            }
        }

        public void Draw() {
            timer.Update();
            if (timer.elasped) { 
                EnableAvailableTarget();
                timer.Reset();
            }
            foreach (Target target in pool) {
                if (!target.GetHidden()) {
                    target.Draw();
                    UpdatePosition(target);                 
                }
            }
        }
    }
}
