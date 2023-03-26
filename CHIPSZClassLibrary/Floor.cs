using StereoKit;

namespace CHIPSZClassLibrary
{
    public class Floor
    {
        Solid collider;
        Material material;
        Matrix transform;

        public Floor()
        {
            collider = new Solid(new Vec3(0, -1.5f, 0), Quat.Identity, SolidType.Immovable);
            transform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            material = new Material(Shader.FromFile("floor.hlsl"));
            collider.AddBox(new Vec3(20, 0.01f, 20));
            material.Transparency = Transparency.Blend;
        }

        public Material GetMaterial()
        {
            return material;
        }

        public Matrix GetTransform()
        {
            return transform;
        }


    }
}
