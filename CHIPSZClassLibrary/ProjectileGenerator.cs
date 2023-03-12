using System.Collections.Generic;
using StereoKit;
using System.Diagnostics;

namespace CHIPSZClassLibrary
{
    public class ProjectileGenerator
    {
        private static readonly Vec3 offScreenVec3 = Vec3.Forward * -1000f;

        private List<FireProjectile> fireProjectiles;
        private List<EarthProjectile> earthProjectiles;

        private Vec3 textPos;
        int playerScore;
        private Vec3 scoreTextPos;

        public ProjectileGenerator()
        {
            fireProjectiles = new List<FireProjectile>(10);
            earthProjectiles = new List<EarthProjectile>(10);

            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            playerScore = 0;
        }

        public void Add(Hand hand, Element element)
        {
            Debug.WriteLine("Adding new projectile of type: " + element);

            switch (element)
            {
                case Element.FIRE:
                    FireProjectile fireProjectile = new FireProjectile(hand.palm.position, 0.3f, element);
                    fireProjectiles.Add(fireProjectile);
                    break;
                case Element.EARTH:
                    EarthProjectile earthProjectile = new EarthProjectile(hand.palm.position, 0.3f, element);
                    earthProjectiles.Add(earthProjectile);
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
                Text.Add("Count :" + (fireProjectiles.Count + earthProjectiles.Count), Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                Text.Add("Score :" + playerScore, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            }

            // Draw fire projectiles
            for (int i = 0; i < fireProjectiles.Count; i++)
            {
                Projectile currentProjectile = fireProjectiles[i];
                currentProjectile.Draw();
            }

            // Draw earth projectiles
            for (int i = 0; i < earthProjectiles.Count; i++)
            {
                Projectile currentProjectile = earthProjectiles[i];
                currentProjectile.Draw();
            }
        }

        public void Update(Hand hand) {
            for (int i = 0; i < fireProjectiles.Count; i++)
            {
                Projectile projectile = fireProjectiles[i];

                if (projectile.enabled)
                {
                    if (projectile.GetTime() > 5.0f)
                    {
                        projectile.Disable();
                    }

                    projectile.UpdatePosition();
                }
            }

            for (int i = 0; i< earthProjectiles.Count;i++) 
            {
                Projectile projectile = earthProjectiles[i];

                if (projectile.enabled)
                {
                    if (projectile.GetTime() > 5.0f)
                    {
                        projectile.Disable();
                    } else
                    {
                        Pose prevProjectilePose = projectile.GetPrevPose();
                        Bounds projectileBounds = projectile.GetModel().Bounds;
                        Pose projectilePose = projectile.GetPosition();
                        prevProjectilePose = projectilePose;
                        if ( hand.gripActivation >= 0.7f && projectileBounds.Contains(hand.palm.position - projectilePose.position))
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
        }

        public List<Projectile> GetAllProjectiles()
        {
            List<Projectile> projectiles = new List<Projectile>();
            
            foreach (Projectile projectile in fireProjectiles)
            {
                projectiles.Add(projectile);
            }

            foreach (Projectile projectile in earthProjectiles)
            {
                projectiles.Add(projectile);
            }

            return projectiles;
        }

        public static Vec3 GetVelocity(Vec3 currentPos, Vec3 prevPos)
        {
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf; ;
            return result;
        }
    }
}
