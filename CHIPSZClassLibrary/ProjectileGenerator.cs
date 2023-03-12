using System.Collections.Generic;
using StereoKit;
using System.Diagnostics;

namespace CHIPSZClassLibrary
{
    public class ProjectileGenerator
    {
        private static readonly Vec3 offScreenVec3 = Vec3.Forward * -1000f;

        private List<Projectile> projectiles;
        private Vec3 textPos;
        int playerScore;
        private Vec3 scoreTextPos;

        public ProjectileGenerator()
        {
            projectiles = new List<Projectile>();
            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            playerScore = 0;
        }

        public void Add(Hand hand, Element element)
        {
            Debug.WriteLine("Adding new projectile of type: " + element);

            Projectile projectile;

            switch (element)
            {
                case Element.FIRE:
                    projectile = new FireProjectile(hand.palm.position, 0.3f, element);
                    projectiles.Add(projectile);
                    break;
                case Element.EARTH:
                    projectile = new EarthProjectile(hand.palm.position, 0.3f, element);
                    projectiles.Add(projectile);
                    break;

            }
        }
        
        public void UpdatePlayerScore(Hand hand, Projectile projectile)
        {
            int xPosition = (int)(hand.palm.position.x - projectile.GetPosition().position.x);
            int yPosition = (int)(hand.palm.position.y - projectile.GetPosition().position.y);

            int multiplier = xPosition > yPosition ? xPosition : yPosition;
            playerScore += 5 * (multiplier != 0 ? multiplier : 1 );
        }

        public void Draw(bool demo)
        {
            if (!demo)
            {
                Text.Add("Count :" + projectiles.Count, Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                Text.Add("Score :" + playerScore, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            }
            for (int i = 0; i < projectiles.Count; i++)
            {
                Projectile currentProjectile = projectiles[i];
                currentProjectile.Draw();
            }
        }

        public void Update(Hand hand) {
            for (int i = 0; i< projectiles.Count;i++) {
                Projectile projectile = projectiles[i];
                if (projectile.GetTime() > 5.0f)
                {
                    projectiles.RemoveAt(i);
                }
                else 
                {
                    Pose prevProjectilePose = projectile.GetPrevPose();
                    Bounds projectileBounds = projectile.GetModel().Bounds;
                    Pose projectilePose = projectile.GetPosition();
                    prevProjectilePose = projectilePose;
                    if (projectile.element == Element.EARTH && hand.gripActivation >= 0.7f && projectileBounds.Contains(hand.palm.position - projectilePose.position))
                    {
                        projectilePose.position = hand.palm.position;
                        projectile.solid.Teleport(projectilePose.position, Quat.Identity);
                        projectile.solid.SetVelocity(GetVelocity(projectilePose.position, prevProjectilePose.position));
                    }
                    //updatePlayerScore(hand, model);
                    projectile.UpdatePosition();
                }
            }
        }

        public List<Projectile> GetAllProjectiles()
        {
            return projectiles;
        }

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf; ;
            return result;
        }
    }
}
