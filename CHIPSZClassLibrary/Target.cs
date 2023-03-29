using System;
using System.Collections.Generic;
using StereoKit;

namespace CHIPSZClassLibrary
{
    public class Target
    {
        private Model shape;
        private Pose position;
        private Random randomNumberGenerator;
        public float size;
        public float distanceFromPlayer;
        private bool hideTarget;
        private int points;
        private bool stopTarget;
        private GameTimer timer;

        public Target()
        {
            shape = null;
            position = Pose.Identity;
            randomNumberGenerator = new Random();
            distanceFromPlayer = -20f;
            hideTarget = false;
            size = 0.5f;
            points = 5;
            stopTarget = false;
        }

        public Target(int points)
        {
            shape = null;
            position = Pose.Identity;
            randomNumberGenerator = new Random();
            distanceFromPlayer = -20f;
            hideTarget = false;
            size = 0.5f;
            this.points = points;
            stopTarget = false;
            timer = new GameTimer(3.0d);
        }
        public bool getStopped()
        {
            return stopTarget;
        }

        public void SetStopped(bool b)
        {
            stopTarget = b;
        }

        public void UpdateTimer()
        {
            timer.Update();
        }

        public GameTimer GetTimer()
        {
            return timer;
        }

        public Model GetModel()
        {
            return shape;
        }

        public Pose GetPose()
        {
            return position;
        }


        public bool SetObject(Model shape)
        {
            if (shape == null)
            {
                return false;
            }
            this.shape = shape;

            return true;
        }

        public bool SetPose(Pose position)
        {
            if (position.Equals(Pose.Identity))
            {
                return false;
            }
            this.position = position;
            return true;
        }

        public void SetRandomPose()
        {
            float posX = (float)(randomNumberGenerator.Next(-30, 30) / 10.0);
            float posY = (float)(randomNumberGenerator.Next(-10, 10) / 10.0);
            SetPose(posX, posY);
        }

        public void SetPose(float posX, float posY)
        {
            position = new Pose(posX, posY, distanceFromPlayer, Quat.Identity);
        }

        public virtual void SetDefaultShape()
        {
            Material mat = Default.Material.Copy();
            mat[MatParamName.ColorTint] = Color.HSV(0.3f, 0.4f, 1.0f);
            shape = Model.FromMesh(
                    Mesh.GenerateRoundedCube(Vec3.One * size, 0.02f),
                    mat);
        }

        public void Draw()
        {
            if (!hideTarget)
                shape.Draw(position.ToMatrix());

        }

        private void ChangeCubePoses(object source, System.Timers.ElapsedEventArgs e)
        {
            SetRandomPose();
        }

        public void SetHidden(bool value)
        {
            this.hideTarget = value;
        }

        public bool GetHidden()
        {
            return this.hideTarget;
        }

        public int CheckHit(List<Projectile> projectiles, ProjectileGenerator ballGenerator, Hand hand)
        {
            int targetsHit = 0;
            foreach (Projectile projectile in projectiles)
            {
                if (SphereToSphereCollision(projectile) && projectile.enabled)
                {
                    switch (projectile.element)
                    {
                        case Element.FIRE:
                            AudioManager.Instance.Play("FireHit", Vec3.Zero, 1f);
                            hideTarget = true;
                            break;
                        case Element.EARTH:
                            AudioManager.Instance.Play("StoneHit", projectile.currentPose.position, 1f);
                            hideTarget = true;
                            break;
                        case Element.WATER:
                            AudioManager.Instance.Play("WaterHit", projectile.currentPose.position, 1f);
                            hideTarget = true;
                            break;
                        case Element.AIR:
                            AudioManager.Instance.Play("WindHit", projectile.currentPose.position, 1f);
                            stopTarget = true;
                            projectile.Disable(); //remove the projectile when it hits
                            break;
                    }
                    ballGenerator.UpdatePlayerScore(hand, projectile, points);
                    targetsHit++;
                }
            }
            return targetsHit;
        }

        public virtual void Move(float speed)
        {
            Pose coords = GetPose();
            coords.position.z += speed;
            if (coords.position.z >= 1) this.SetHidden(true);
            else
            {
                this.SetPose(coords);
            }
        }

        internal bool SphereToSphereCollision(Projectile projectile) {
            Vec3 diff = this.position.position - projectile.GetPosition().position;
            double distance = Math.Sqrt((diff.x * diff.x) + (diff.y * diff.y) + (diff.z * diff.z));
            double sumOfRadii = (this.size-0.1f) + projectile.radius;
            return (distance < sumOfRadii);
        }
    }
}