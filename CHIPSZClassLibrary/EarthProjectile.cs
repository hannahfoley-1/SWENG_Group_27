using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZClassLibrary
{
    internal class EarthProjectile : Projectile
    {
        public EarthProjectile(Vec3 position, float diameter = 0.5f, Element element = Element.EARTH) : base(position, diameter, element)
        {
            
        }

        internal override Color CreateColor()
        {
            return Color.HSV(0.33f, 0.6f, 0.80f);
        }

        internal override Material CreateMaterial()
        {
            Material earthMaterial = Material.Default.Copy();
            earthMaterial[MatParamName.ColorTint] = CreateColor();
            return earthMaterial;
        }

        internal override void SetPosition(Vec3 newPos)
        {
            solid.Enabled = false;
            solid.Teleport(newPos, Quat.Identity);
        }

        internal override void UpdatePosition()
        {
            solid.GetPose(out currentPose);
            time += Time.Elapsedf;
        }
    }
}
