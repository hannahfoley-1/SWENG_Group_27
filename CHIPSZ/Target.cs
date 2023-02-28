using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using StereoKit;
using System.Collections;

namespace CHIPSZ
{
    class Target
    {
        private Model shape;
        private Pose position;
        private Random randomNumberGenerator;
        private static Timer timer;
        public float size;
        public float distanceFromPlayer;
        private Boolean hideTarget;
        
        public Target()
        {
            shape = null;
            position = Pose.Identity;
            randomNumberGenerator = new Random();
            distanceFromPlayer = -2f;
            hideTarget = false;
            size = 0.5f;
        }

        public Model getModel()
        {
            return shape;
        }

        public Pose getPose()
        {
            return position;
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
            timer = new Timer();
            timer.Interval = 5000;
            timer.Elapsed += changeCubePoses;
            timer.AutoReset = true;
            timer.Enabled = true;
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
                    Mesh.GenerateRoundedCube(Vec3.One * size, 0.02f),
                    Default.MaterialUI);
            createTimer();
        }

        public void draw()
        {
            if (!hideTarget)
                shape.Draw(position.ToMatrix());
            
        }

        private void changeCubePoses(Object source, System.Timers.ElapsedEventArgs e)
        {
            setRandomPose();
        }

        public void checkHit(ArrayList projectiles)
        {
            foreach (Ball ball in projectiles)
            {
                if (shape.Bounds.Contains(ball.getPosition().position - position.position))
                {
                    hideTarget = true;
                }
            }
        }
    }
}
