using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using StereoKit;
using System.Collections;

namespace CHIPSZClassLibrary
{
    public class Target
    {
        private Model shape;
        private Pose position;
        private Random randomNumberGenerator;
        private static Timer timer;
        public float size;
        public float distanceFromPlayer;
        private bool hideTarget;
        
        public Target()
        {
            shape = null;
            position = Pose.Identity;
            randomNumberGenerator = new Random();
            distanceFromPlayer = -2f;
            hideTarget = false;
            size = 0.5f;
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

        private void CreateTimer()
        {
            timer = new Timer();
            timer.Interval = 5000;
            timer.Elapsed += ChangeCubePoses;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public bool SetPose( Pose position )
        {
            if( position.Equals( Pose.Identity ) )
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

        public void SetPose( float posX, float posY)
        {
            position = new Pose(posX, posY, distanceFromPlayer, Quat.Identity);
        }

        public void SetDefaultShape()
        {
            shape = Model.FromMesh(
                    Mesh.GenerateRoundedCube(Vec3.One * size, 0.02f),
                    Default.MaterialUI);
            CreateTimer();
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

        public void CheckHit(BallGenerator ballGenerator, Hand hand)
        {
            foreach (Ball ball in ballGenerator.GetAllBalls())
            {
                if (shape.Bounds.Contains(ball.GetPosition().position - position.position))
                {
                    if (!hideTarget)
                        ballGenerator.updatePlayerScore(hand, ball);
                    hideTarget = true;
                }
            }
        }
    }
}
