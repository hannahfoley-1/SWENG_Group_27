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
            currentPose = Linear(time, speed);
            time += Time.Elapsedf;
        }

        // TEMP: Copied from FireProjectile
        // Calculates a parabolic directory for the projectile
        internal Pose Linear(float time, float speed)
        {
            float internalTime = time * speed;

            return new Pose(prevPose.position.x, prevPose.position.y + ((-2f * (internalTime * internalTime)) + (1.5f * internalTime)), prevPose.position.z + (-9f * internalTime), Quat.Identity);
        }
    }
}
