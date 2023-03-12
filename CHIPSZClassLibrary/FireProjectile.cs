using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZClassLibrary
{
    internal class FireProjectile : Projectile
    {
        public FireProjectile(Vec3 position, float diameter = 0.5f, Element element = Element.FIRE) : base(position, diameter, element)
        {

        }

        internal override Color CreateColor()
        {
            return Color.HSV(16f, 85f, 94f);
        }

        internal override Material CreateMaterial()
        {
            Material fireMaterial = Material.Default.Copy();
            fireMaterial[MatParamName.ColorTint] = CreateColor();
            return fireMaterial;
        }

        internal override void SetPosition(Vec3 newPos)
        {
            currentPose = new Pose(newPos, Quat.Identity);
        }

        internal override void UpdatePosition()
        {
            currentPose = Linear(this.time);
            time += Time.Elapsedf;
        }

        // Calculates a parabolic directory for the projectile
        internal Pose Linear(float time)
        {
            return new Pose(prevPose.position.x, prevPose.position.y + ((-2f * (time * time)) + (1.5f * time)), prevPose.position.z + (-9f * time), Quat.Identity);
        }
    }
}
