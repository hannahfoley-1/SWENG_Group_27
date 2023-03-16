using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StereoKit;

namespace CHIPSZClassLibrary
{
    class SinTarget : Target
    {
        private static readonly int POINTS = 10;
        int counter = 1;
        public SinTarget() : base( POINTS )
        {
        }

        public override void Move( float speed )
        {
            Pose coords = GetPose();
            coords.position.y += 0.5f * (float)Math.Sin(counter * Math.PI / 180) ;
            coords.position.z += speed;
            if (coords.position.z >= 1) this.SetHidden(true);
            else
            {
                this.SetPose(coords);
            }
        }
    }
}
