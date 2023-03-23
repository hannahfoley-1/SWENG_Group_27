using StereoKit;

namespace CHIPSZClassLibrary
{
    internal class WaterProjectile : Projectile
    {
        internal float speed = 8f;
        internal float acceleration = 3f;

        internal Vec3 velocity;
        internal Vec3 direction;

        public WaterProjectile(Vec3 position, float diameter, Element element) : base(position, diameter, element)
        {
            ResetMesh(diameter);
        }

        public void ResetMesh(float diameter)
        {
            model = Model.FromMesh(Mesh.GenerateSphere(diameter), CreateMaterial());
        }

        internal override Color CreateColor()
        {
            return Color.Hex(0xA5D5FFFF);
        }

        internal override Material CreateMaterial()
        {
            Shader shader = Shader.FromFile("WaterProjectile.hlsl");
            Material waterMaterial = new Material(shader);
            waterMaterial["color"] = CreateColor();
            waterMaterial["color2"] = Color.Hex(0xFFFFFFFF);
            waterMaterial["slope"] = 5.6f;
            waterMaterial["threshold"] = -0.24f;
            return waterMaterial;
        }

        internal override void SetPosition(Vec3 newPos)
        {
            currentPose = new Pose(newPos, Quat.Identity);
        }

        internal Vec3 GetDirection(Vec3 headPos, Vec3 handPos)
        {
            Vec3 direction = handPos - headPos;
            direction.Normalize();

            return direction;
        }

        internal override void UpdatePosition()
        {
            velocity.y -= acceleration * Time.Elapsedf;
            currentPose.position += velocity * Time.Elapsedf;

            time += Time.Elapsedf;
        }
    }
}
