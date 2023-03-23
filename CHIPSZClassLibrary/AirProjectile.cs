using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZClassLibrary
{
    public class AirProjectile : Projectile {

        internal ParticleSystem particleSystem;
        internal float speed = 0.65f;
        internal float acceleration = 4f;

        internal Vec3 velocity;
        internal Vec3 direction;

        public AirProjectile(Vec3 position, float diameter = 0.5f, Element element = Element.AIR) : base(position, diameter, element)
        {
            particleSystem = new ParticleSystem(diameter, 4, diameter / 5);
            model = Model.FromMesh(particleSystem.mesh, CreateMaterial());
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

        internal Vec3 GetDirection(Vec3 headPos, Vec3 handPos)
        {
            Vec3 direction = handPos - headPos;
            direction.Normalize();
            direction.y = 0;

            return direction;
        }

        internal override void SetPosition(Vec3 newPos)
        {
            currentPose = new Pose(newPos, Quat.Identity);
        }


        internal override void UpdatePosition()
        {
            Vec3 floorVel = direction * speed;
            velocity += floorVel;
            velocity.y -= acceleration * Time.Elapsedf;
            currentPose.position += velocity * Time.Elapsedf;

            time += Time.Elapsedf;
        }
    }
}
