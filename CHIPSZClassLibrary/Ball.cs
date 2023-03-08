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
    public enum Element
    {
        FIRE,
        EARTH,
    }

    public class Ball // creates an interactive ball with physics
    {
        private Pose currentPose;
        private Pose prevPose;
        public Solid solid;
        private Model ball;
        private float time;
        public Element element;

        public Ball(Vec3 position, float diameter, Element element)
        {
            this.element = element;
            if (element == Element.EARTH)
            {
                this.solid = new Solid(position, Quat.Identity);
                this.solid.AddSphere(diameter);
                this.solid.Enabled = true;
                this.currentPose = solid.GetPose();
                this.prevPose = this.currentPose;
                this.ball = Model.FromMesh(Mesh.GenerateSphere(diameter), Default.MaterialUI);
            }
            else
            {
                this.prevPose = new Pose(position, Quat.Identity);
                this.ball = Model.FromMesh(Mesh.GenerateSphere(diameter), Default.MaterialUI);
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

        public void SetPosition(Vec3 newPos) { 
            solid.Enabled = false; solid.Teleport(newPos, Quat.Identity); solid.Enabled = true; 
        }



        public void UpdatePosition()
        {
            if (element == Element.EARTH)
            {
                solid.GetPose(out currentPose);
            }
            else if (element == Element.FIRE)
            {
                currentPose = Linear(this.time);
                time += Time.Elapsedf;
            }
        }
        public void Draw()
        { 
            Renderer.Add(ball, currentPose.ToMatrix());
        }

        private Pose Linear(float time)
        {
            return new Pose(prevPose.position.x, prevPose.position.y + ((-2f * (time * time)) + (1.5f * time)), prevPose.position.z + (-9f * time), Quat.Identity);
        }


    }
}
