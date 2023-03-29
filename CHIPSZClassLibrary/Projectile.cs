using StereoKit;
using System.Diagnostics;
using Windows.ApplicationModel.UserDataTasks.DataProvider;
using Windows.Media.Devices.Core;

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
        public float radius;
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
            radius = diameter / 2;
        }

        public void Reset(Vec3 position, float diameter, Element element)
        {
            Enable();
            Hand hand = Input.Hand(Handed.Right);
            this.element = element;
            time = 0;
            currentPose = new Pose(position, Quat.Identity);
            prevPose = currentPose;

            switch (element)
            {
                case Element.EARTH:
                    solid.Enabled = false;
                    EarthProjectile earthProjectile = (EarthProjectile)this;

                    earthProjectile.direction = earthProjectile.GetDirection(hand);

                    earthProjectile.velocity = earthProjectile.direction * earthProjectile.speed;
                    break;
                case Element.FIRE:
                    solid.Enabled = false;
                    FireProjectile fireProjectile = (FireProjectile)this;

                    fireProjectile.direction = fireProjectile.GetDirection(hand);

                    fireProjectile.velocity = fireProjectile.direction * fireProjectile.speed;
                    break;
                case Element.WATER:
                    solid.Enabled = false;
                    WaterProjectile waterProjectile = (WaterProjectile)this;

                    waterProjectile.direction = waterProjectile.GetDirection(hand);

                    waterProjectile.velocity = waterProjectile.direction * waterProjectile.speed;
                    waterProjectile.ResetMesh(diameter);
                    break;
                case Element.AIR:
                    solid.Enabled = false;
                    AirProjectile airProjectile = (AirProjectile)this;
                    airProjectile.velocity = new Vec3(0, 3, 0);
                    airProjectile.direction = airProjectile.GetDirection(hand);
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

        public float GetTime()
        {
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