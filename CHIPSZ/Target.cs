using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using StereoKit;

namespace CHIPSZ
{
    class Target
    {
        private Model shape;
        private Pose position;
        private Random randomNumberGenerator;
        private static Timer targetTimer;

        public Target()
        {
            shape = null;
            position = Pose.Identity;
            randomNumberGenerator = new Random();
        }

        public Boolean setObject( Model shape )
        {
            if (shape == null)
            {
                return false;
            }
            this.shape = shape;

            return true;
        }

        private void createTimer()
        {
            targetTimer = new System.Timers.Timer();
            targetTimer.Interval = 5000;
            targetTimer.Elapsed += changeCubePoses;
            targetTimer.AutoReset = true;
            targetTimer.Enabled = true;
        }

        public Boolean setPose( Pose position )
        {
            if( position.Equals( Pose.Identity ) )
            {
                return false;
            }
            this.position = position;
            return true;
        }

        public void setRandomPose()
        {
            float targetPosX = (float)(randomNumberGenerator.Next(-30, 30) / 10.0);
            float targetPosY = (float)(randomNumberGenerator.Next(-10, 10) / 10.0);
            position = new Pose(targetPosX, targetPosY, -2f, Quat.Identity);
        }

        public void setDefaultShape()
        {
            shape = Model.FromMesh(
                    Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
                    Default.MaterialUI);
            createTimer();
        }

        public void draw()
        {
            shape.Draw(position.ToMatrix());
        }

        private void changeCubePoses(Object source, System.Timers.ElapsedEventArgs e)
        {
            setRandomPose();
        }
    }
}
