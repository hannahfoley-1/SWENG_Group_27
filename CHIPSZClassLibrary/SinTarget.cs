using System;
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

        public override void SetDefaultShape()
        {
            Material mat = Default.Material.Copy();
            mat[MatParamName.ColorTint] = Color.HSV(0.667f, 0.4f, 1.0f);
            Model miniModel = Model.FromMesh(
                    Mesh.GenerateRoundedCube(Vec3.One * size, 0.02f),
                    mat);
            SetObject(miniModel);
        }

        public override void Move( float speed )
        {
            speed = speed / 2;
            Pose coords = GetPose();
            float xOffset = 0.05f * (float)Math.Cos((counter * Math.PI) / 180);
            //if (xOffset < 0) xOffset *= 2;
            coords.position.x += xOffset;
            counter++;
            coords.position.z += speed;
            if (coords.position.z >= 1) this.SetHidden(true);
            else
            {
                this.SetPose(coords);
            }
        }
    }
}
