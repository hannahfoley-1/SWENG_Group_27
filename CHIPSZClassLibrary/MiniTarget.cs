using StereoKit;

namespace CHIPSZClassLibrary
{
    class MiniTarget : Target
    {
        private readonly int POINTS = 10;
        private readonly float size = 0.2f;
        public MiniTarget() : base( 10 )
        {

        }

        public override void SetDefaultShape()
        {
            Material mat = Default.Material.Copy();
            mat[MatParamName.ColorTint] = Color.HSV(0.0f, 0.4f, 1.0f);
            Model miniModel = Model.FromMesh(
                    Mesh.GenerateRoundedCube(Vec3.One * size, 0.02f),
                    mat);
            SetObject(miniModel);
        }

        public override void Move(float speed)
        {
            Pose coords = GetPose();
            coords.position.z += speed / 2;
            if (coords.position.z >= 1) this.SetHidden(true);
            else
            {
                this.SetPose(coords);
            }
        }
    }
}
