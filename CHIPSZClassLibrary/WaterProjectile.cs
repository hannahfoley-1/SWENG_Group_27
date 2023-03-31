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

        internal Vec3 GetDirection(Hand hand)
        {
            Vec3 direction;

            // Normal style
            // direction = hand.palm.position - headPos;

            // Iron Man style
            direction = hand.palm.Forward;
            direction.Normalize();

            if (hand.handed == Handed.Right)
            {
                Matrix moveLeft = Matrix.R(0, -15, 0);
                direction = moveLeft.TransformNormal(direction);
            }
            else if (hand.handed == Handed.Left)
            {
                Matrix moveRight = Matrix.R(0, 15, 0);
                direction = moveRight.TransformNormal(direction);
            }

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
