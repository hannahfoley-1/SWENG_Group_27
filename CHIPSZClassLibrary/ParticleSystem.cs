using StereoKit;
using System;

namespace CHIPSZClassLibrary
{
    internal class ParticleSystem
    {
        public Mesh mesh;
        private Random random;

        public ParticleSystem(float diameter = 0.5f, int subdivisions = 16, float delta = 0.1f)
        {
            random = new Random();
            mesh = GenerateParticleMesh(diameter, subdivisions, delta);
        }

        private Mesh GenerateParticleMesh(float diameter = 0.5f, int subdivisions = 2, float delta = 0.1f)
        {
            Mesh particleMesh = new Mesh();
            Mesh sphereMesh = Mesh.GenerateSphere(diameter, subdivisions);
            Vertex[] vertices = sphereMesh.GetVerts();

            for (int i = 0; i < vertices.Length; i++) {
                vertices[i].pos += RandomPointInUnitSphere() * delta;
            }

            particleMesh.SetVerts(vertices);
            particleMesh.SetInds(sphereMesh.GetInds());

            return particleMesh;
        }

        private Vec3 RandomPointInUnitSphere()
        {
            Vec3 randomPoint = RandomPointInCube(2f);

            // While point outside unit sphere, retry
            while (!IsPointWithinUnitSphere(randomPoint))
            {
                randomPoint = RandomPointInCube(2f);
            }

            return randomPoint;
        }

        private Vec3 RandomPointInCube(float sideLength)
        {
            Vec3 randomPoint = new Vec3();

            randomPoint.x = (float)(random.NextDouble() * sideLength) - (sideLength / 2f);
            randomPoint.y = (float)(random.NextDouble() * sideLength) - (sideLength / 2f);
            randomPoint.z = (float)(random.NextDouble() * sideLength) - (sideLength / 2f);

            return randomPoint;
        }

        static bool IsPointWithinUnitSphere(Vec3 point)
        {
            bool isPointWithinUnitSphere = true;

            if (point.x * point.x + point.y * point.y + point.z * point.z > 1)
            {
                isPointWithinUnitSphere = false;
            }

            return isPointWithinUnitSphere;
        }
    }
}
