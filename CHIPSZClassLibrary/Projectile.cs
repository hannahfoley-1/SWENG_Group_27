using StereoKit;
using System.Diagnostics;

namespace CHIPSZClassLibrary
{
    public enum Element
    {
        FIRE,
        EARTH,
        WATER,
        AIR
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
            solid = new Solid(position, Quat.Identity);
            solid.AddSphere(diameter);
            Reset(position, diameter, element);
        }

        public void Reset(Vec3 position, float diameter, Element element)
        {
            Enable();
            Pose headPose = Input.Head; //save data for performance
            Hand hand = Input.Hand(Handed.Right);
            this.element = element;
            time = 0;
            currentPose = new Pose(position, Quat.Identity);
            prevPose = currentPose;

            switch (element)
            {
                case Element.EARTH:
                    solid.Enabled = true;
                    solid.Teleport(currentPose.position, currentPose.orientation);
                    Vec3 directedVector = headPose.position - hand.palm.position;
                    solid.SetVelocity(directedVector);
                    break;
                case Element.FIRE:
                    solid.Enabled = false;
                    FireProjectile fireProjectile = (FireProjectile)this;
                    // fireProjectile.velocity = new Vec3(0, 3, 0);
                    fireProjectile.direction = fireProjectile.GetDirection(headPose.position, hand.palm.position);
                    break;
                case Element.WATER: 
                    solid.Enabled = false;
                    WaterProjectile waterProjectile = (WaterProjectile)this;
                    // waterProjectile.velocity = new Vec3(0, 3, 0);
                    waterProjectile.direction = waterProjectile.GetDirection(headPose.position, hand.palm.position);
                    waterProjectile.ResetMesh(diameter);
                    break;
                case Element.AIR:
                    solid.Enabled = false;
                    AirProjectile airProjectile = (AirProjectile)this;
                    airProjectile.velocity = new Vec3(0, 3, 0);
                    airProjectile.direction = airProjectile.GetDirection(headPose.position, hand.palm.position);
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
