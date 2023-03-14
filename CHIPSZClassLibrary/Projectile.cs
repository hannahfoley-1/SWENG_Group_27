using StereoKit;
using System.Diagnostics;
using Windows.Foundation.Diagnostics;

namespace CHIPSZClassLibrary
{
    public enum Element
    {
        FIRE,
        EARTH,
    }

    public class Projectile // creates an interactive projectile with physics
    {
        internal bool enabled;
        internal Pose currentPose;
        internal Pose prevPose;
        public Solid solid;
        internal Model model;
        internal float time;
        public Element element;
        internal ParticleSystem particleSystem;

        internal virtual Material CreateMaterial()
        {
            return Material.Default.Copy();
        }

        internal virtual Color CreateColor()
        {
            return Color.White;
        }

        public Projectile(Vec3 position, float diameter, Element element)
        {
            particleSystem = new ParticleSystem(diameter, 16, 0.005f);
            solid = new Solid(position, Quat.Identity);
            solid.AddSphere(diameter);
            Reset(position, diameter, element);
        }

        public void Reset(Vec3 position, float diameter, Element element)
        {
            particleSystem = new ParticleSystem(diameter, 8, diameter / 5);
            Enable();
            this.element = element;
            time = 0;
            currentPose = new Pose(position, Quat.Identity);
            prevPose = currentPose;

            switch (element)
            {
                case Element.EARTH:
                    solid.Enabled = true;
                    solid.Teleport(currentPose.position, currentPose.orientation);
                    model = Model.FromMesh(particleSystem.mesh, CreateMaterial());
                    break;
                case Element.FIRE:
                    solid.Enabled = false;
                    model = Model.FromMesh(particleSystem.mesh, CreateMaterial());
                    break;  
            }
        }

        internal bool GetEnabled()
        {
            return enabled;
        }

        internal void Enable()
        {
            // Debug.WriteLine("Enabling projectile");
            enabled = true;
        }

        internal void Disable()
        {
            // Debug.WriteLine("Disabling projectile");
            solid.Enabled = false;
            enabled = false;
        }

        public Model GetModel()
        {
            return model;
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

        internal virtual void SetPosition(Vec3 newPos) 
        { 
            currentPose = new Pose(newPos, Quat.Identity);
        }

        internal virtual void UpdatePosition()
        {
            Debug.WriteLine("Projectile without class in use, this should be fixed");
            currentPose = Pose.Identity;
            time += Time.Elapsedf;
        }

        public void Draw()
        {
            Renderer.Add(model, currentPose.ToMatrix());
        }

        
    }
}
