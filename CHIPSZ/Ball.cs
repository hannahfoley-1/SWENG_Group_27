using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StereoKit;
using StereoKit.Framework;
using Windows.Devices.PointOfService;

namespace CHIPSZ
{
    internal class Ball // creates an interactive ball with physics
    {
        private Pose currentPose;
        private Pose prevPose;
        private Solid solid;
        private Model ball;


        public Ball(Vec3 position, float diameter)
        {
	    // Creates a default 1kg ball with default model
            this.solid = new Solid(position, Quat.Identity);
            this.solid.AddSphere(diameter);
            this.solid.Enabled = true;
            this.currentPose = solid.GetPose();
            this.prevPose = this.currentPose;
            this.ball = Model.FromMesh(Mesh.GenerateSphere(diameter), Default.MaterialUI);
        }

        public Model GetModel()
        {
            return ball;
        }

        public Pose GetPosition()
        {
            return currentPose;
        }

        public void SetPosition(Vec3 newPos) { solid.Enabled = false; solid.Teleport(newPos, Quat.Identity); solid.Enabled = true; }
        public void Draw(Hand hand, int id)
        {
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
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf;;
            return result;
        }
    }
}
