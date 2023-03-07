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
    public enum Element { 
        FIRE,
        EARTH,
    }

    public class Ball // creates an interactive ball with physics
    {
        private Pose currentPose;
        private Pose prevPose;
        private Solid solid;
        private Model ball;
        private float time;
        Element element;

        public Ball(Vec3 position, float diameter,Element element)
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
            else { 
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

        public void SetPosition(Vec3 newPos) { solid.Enabled = false; solid.Teleport(newPos, Quat.Identity); solid.Enabled = true; }
        public void Draw(Hand hand, int id)
        {
            if (this.element == Element.EARTH)
            {

                if (UI.Handle(id.ToString(), ref this.currentPose, this.ball.Bounds))
                {
                    hand.Solid = false;
                    solid.Teleport(this.currentPose.position, Quat.Identity);
                    solid.SetVelocity(GetVelocity(this.currentPose.position, this.prevPose.position));
                }
                solid.GetPose(out currentPose);
                Renderer.Add(ball, currentPose.ToMatrix());
                prevPose = currentPose;
            }
            else {
                currentPose = Linear(this.time);
                ball.Draw(currentPose.ToMatrix());
                time += Time.Elapsedf; 
            }
        }

        private Pose Linear(float time) 
        {       
            return new Pose(prevPose.position.x, prevPose.position.y +((-2f * (time*time)) + (1.5f*time)), prevPose.position.z + (-9f * time),Quat.Identity);
        }

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf;;
            return result;
        }

        public static double Magnitude(Vec3 velocity) {
            return Math.Sqrt((velocity.x * velocity.x) + (velocity.y * velocity.y) + (velocity.z * velocity.z));
        }
    }
}
