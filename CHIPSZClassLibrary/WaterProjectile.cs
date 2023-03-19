using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZClassLibrary
{
    internal class WaterProjectile : Projectile
    {
        internal float speed = 0.5f;
        internal float acceleration = 4f;

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

        internal override void UpdatePosition()
        {
            Vec3 floorVel = direction * speed;
            velocity += floorVel;
            velocity.y -= acceleration * Time.Elapsedf;
            currentPose.position += velocity * Time.Elapsedf;
           
            time += Time.Elapsedf;
        }
}
