using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly Color earthColor = Color.HSV(0.33f,0.6f,0.80f);
        private static readonly Color fireColor = Color.HSV(16f, 85f, 94f);
        private readonly Material earthMaterial = CreateEarthMaterial();
        private readonly Material fireMaterial = CreateFireMaterial();

        private Pose currentPose;
        private Pose prevPose;
        public Solid solid;
        private Model ball;
        private float time;
        public Element element;
        private ParticleSystem particleSystem;

        private static Material CreateEarthMaterial()
        {
            Material earthMaterial = Material.Default.Copy();
            earthMaterial[MatParamName.ColorTint] = earthColor;
            return earthMaterial;
        }

        private static Material CreateFireMaterial()
        {
            Material fireMaterial = Material.Default.Copy();
            fireMaterial[MatParamName.ColorTint] = fireColor;
            return fireMaterial;
        }

        public Ball(Vec3 position, float diameter, Element element)
        {
            particleSystem = new ParticleSystem(diameter, 1, 0.05f);
            solid = new Solid(position, Quat.Identity);
            solid.AddSphere(diameter);
            Reset(position, element);
        }

        public void Reset(Vec3 position, Element element)
        {
            this.element = element;
            time = 0;
            currentPose = new Pose(position, Quat.Identity);

            switch (element)
            {
                case Element.EARTH:
                    solid.Enabled = true;
                    ball = Model.FromMesh(particleSystem.mesh, earthMaterial);
                    break;
                case Element.FIRE:
                    solid.Enabled = false;
                    ball = Model.FromMesh(particleSystem.mesh, fireMaterial);
                    break;  
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
