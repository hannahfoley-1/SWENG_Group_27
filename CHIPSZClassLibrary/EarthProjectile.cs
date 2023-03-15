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
        [Obsolete] // The SetMaterial() method is to be removed in v0.4 but that's not an issue for us
        public EarthProjectile(Model model, Vec3 position, float diameter = 0.5f, Element element = Element.EARTH) : base(position, diameter, element)
        {
            // Potential optimisation: store scaled mesh
            this.model = new Model(GetScaledMesh(model.GetMesh(0), 0.005f), CreateMaterial());
        }

        internal static Mesh GetScaledMesh(Mesh mesh, float scaleModifier)
        {
            Mesh oldMesh = mesh;
            Mesh newMesh = new Mesh();
            uint[] oldMeshIndices = oldMesh.GetInds();
            Vertex[] oldMeshVertices = oldMesh.GetVerts();

            newMesh.SetInds(oldMeshIndices);

            Vertex[] newMeshVertices = oldMeshVertices;

            for (int i = 0; i < newMeshVertices.Length; i++)
            {
                newMeshVertices[i].pos *= scaleModifier;
            }

            newMesh.SetVerts(newMeshVertices);
            newMesh.Bounds.Scale(scaleModifier);

            return newMesh;
        }

        internal override Color CreateColor()
        {
            // Option 1
            // return Color.Hex(0x99BB99FF);

            // Option 2
            // return Color.Hex(0xF7D48FFF);

            // Option 3
            // return Color.Hex(0xF7ED8FFF);

            // Option 4
            // return Color.Hex(0xF8E2CEFF);

            // Option 5
            // return Color.Hex(0xD2920FFF);

            // Option 6
            return Color.Hex(0x999999FF);
        }

        internal override Material CreateMaterial()
        {
            Shader shader = Shader.FromFile("EarthProjectile.hlsl");
            Material earthMaterial = new Material(shader);
            earthMaterial["color"] = CreateColor();
            earthMaterial["color2"] = Color.Hex(0xFFFFFFFF);
            earthMaterial["slope"] = 5.6f;
            earthMaterial["threshold"] = -0.24f;
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
