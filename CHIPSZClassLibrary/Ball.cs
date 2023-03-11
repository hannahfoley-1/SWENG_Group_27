using System;
using System.Collections.Generic;
using StereoKit;
using StereoKit.Framework;
using Windows.Devices.Bluetooth.Advertisement;
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

        public float GetTime() {
            return time;
        }

        public void SetPosition(Vec3 newPos) { solid.Enabled = false; solid.Teleport(newPos, Quat.Identity); solid.Enabled = true; }

        public void UpdatePosition()
        {
            if (element == Element.EARTH)
            {
                solid.GetPose(out currentPose);
                time += Time.Elapsedf;
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

            /*
            public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
            {
                Vec3 result = ((currentPos - prevPos) / Time.Elapsedf) * 1.5f; ;
                return result;
            }
            */

        }
    }
}
