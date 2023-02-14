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
        private float distanceFromPlayer;
        public Boolean hideBall = false;

        public Target()
        {
            shape = null;
            position = Pose.Identity;
            randomNumberGenerator = new Random();
            distanceFromPlayer = -2f;
            hideBall = false;
        }

        public Model getModel()
        {
            return shape;
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
            float posX = (float)(randomNumberGenerator.Next(-30, 30) / 10.0);
            float posY = (float)(randomNumberGenerator.Next(-10, 10) / 10.0);
            setPose( posX, posY );
        }

        public void setPose( float posX, float posY)
        {
            position = new Pose(posX, posY, distanceFromPlayer, Quat.Identity);
        }

        public void setDefaultShape()
        {
            shape = Model.FromMesh(
                    Mesh.GenerateRoundedCube(Vec3.One * 0.5f, 0.02f),
                    Default.MaterialUI);
            createTimer();
        }

        public void draw()
        {
            if (hideBall)
                shape.Draw(position.ToMatrix());
            
        }

        private void changeCubePoses(Object source, System.Timers.ElapsedEventArgs e)
        {
            //setRandomPose();
        }
    }
}
