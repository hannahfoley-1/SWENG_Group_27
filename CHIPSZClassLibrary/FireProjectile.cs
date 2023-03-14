using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

namespace CHIPSZClassLibrary
{
    internal class FireProjectile : Projectile
    {
        internal float speed = 0.65f;

        public FireProjectile(Vec3 position, float diameter = 0.5f, Element element = Element.FIRE) : base(position, diameter, element)
        {
            particleSystem = new ParticleSystem(diameter, 4, diameter / 5);
        }

        internal override Color CreateColor()
        {
            return Color.Hex(0xFFA500FF); // Solid bright red
        }

        internal override Material CreateMaterial()
        {
            Shader shader = Shader.FromFile("FireProjectile.hlsl");
            Material fireMaterial = new Material(shader);
            fireMaterial["color"] = CreateColor();
            fireMaterial["color2"] = Color.Hex(0xFFFFFFFF); 
            fireMaterial["slope"] = 5.6f;
            fireMaterial["threshold"] = -0.24f;
            return fireMaterial;
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

        // Calculates a parabolic directory for the projectile
        internal Pose Linear(float time, float speed) 
        {
            float internalTime = time * speed;

            return new Pose(prevPose.position.x, prevPose.position.y + ((-2f * (internalTime * internalTime)) + (1.5f * internalTime)), prevPose.position.z + (-9f * internalTime), Quat.Identity);
        }
    }
}
