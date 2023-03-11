using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StereoKit;
using StereoKit.Framework;
using Windows.Devices.PointOfService;

namespace CHIPSZClassLibrary

{
    public class Ball // creates an interactive ball with physics
    {
        private Pose currentPose;
        private Pose prevPose;
        private Solid solid;
        private Model ball;


        public Ball(Vec3 position, float diameter)
        {
            this.element = element;
            if (element == Element.EARTH)
            {
                this.solid = new Solid(position, Quat.Identity);
                this.solid.AddSphere(diameter);
                this.solid.Enabled = true;
                this.currentPose = solid.GetPose();
                this.prevPose = this.currentPose;
                Material mat = Default.Material.Copy();
                mat[MatParamName.ColorTint] = Color.HSV(0.33f,0.6f,0.80f);
                this.ball = Model.FromMesh(Mesh.GenerateSphere(diameter), mat);
                this.time = 0;
            }
            else
            {
                this.prevPose = new Pose(position, Quat.Identity);
                Material mat = Default.Material.Copy();
                mat[MatParamName.ColorTint] = Color.HSV(16f, 85f, 94f);
                this.ball = Model.FromMesh(Mesh.GenerateSphere(diameter), mat);
                this.time = 0;
            }
        }

        public Model GetModel()
        {
            return ball;
        }

        public Pose GetPosition()
        {
            return currentPose;
        }

        public Pose GetPrevPose()
        {
            return prevPose;
        }

        public float getTime() {
            return time;
        }

        public void SetPosition(Vec3 newPos) { solid.Enabled = false; solid.Teleport(newPos, Quat.Identity); solid.Enabled = true; }
        public void Draw(Hand hand, int id)
        {
            if (element == Element.EARTH)
            {
                solid.GetPose(out currentPose);
                time += Time.Elapsedf;
            }
            else if (element == Element.FIRE);
            prevPose = currentPose;
              
            if (UI.Handle(id.ToString(), ref this.currentPose, this.ball.Bounds))
            {
                hand.Solid = false;
                solid.Teleport(this.currentPose.position, Quat.Identity);
                solid.SetVelocity( GetVelocity(this.currentPose.position, this.prevPose.position));
            }
            solid.GetPose(out currentPose);
            Renderer.Add(ball, currentPose.ToMatrix());
        }

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = ((currentPos - prevPos) / Time.Elapsedf) * 1.5f; ;
            return result;
        }
    }
}
