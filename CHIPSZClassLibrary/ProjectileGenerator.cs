using System.Collections.Generic;
using StereoKit;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace CHIPSZClassLibrary
{
    public class ProjectileGenerator
    {
        internal int startFireProjectileCount;
        internal int startEarthProjectileCount;

        private List<FireProjectile> fireProjectiles;
        private List<EarthProjectile> earthProjectiles;

        private Vec3 textPos;
        int playerScore;
        private Vec3 scoreTextPos;

        Model earthProjectileModel;

        public ProjectileGenerator(int startFireProjectileCount = 10, int startEarthProjectileCount = 10)
        {
            earthProjectileModel = Model.FromFile("EarthProjectile.obj");

            fireProjectiles = new List<FireProjectile>();

            for (int i = 0; i < startFireProjectileCount; i++)
            {
                Projectile projectile = CreateNewFireProjectile(Vec3.Zero, 0.01f);
                projectile.enabled = false;
            }

            earthProjectiles = new List<EarthProjectile>();

            for (int i = 0; i < startEarthProjectileCount; i++)
            {
                Projectile projectile = CreateNewEarthProjectile(Vec3.Zero, 0.01f);
                projectile.enabled = false;
            }

            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            playerScore = 0;
            this.startFireProjectileCount = startFireProjectileCount;
            this.startEarthProjectileCount = startEarthProjectileCount;
        }

        internal Projectile SpawnProjectile(Hand hand, Element element)
        {
            Projectile projectile = null;

            switch (element)
            {
                case Element.FIRE:
                    projectile = GetFireProjectile(hand.palm.position, 0.1f);
                    break;
                case Element.EARTH:
                    projectile = GetEarthProjectile(hand.palm.position, 0.5f);
                    break;
            }

            if (projectile == null)
            {
                Debug.WriteLine("No projectile available (this should never happen)");
            }

            return projectile;
        }

        internal FireProjectile GetFireProjectile(Vec3 position, float diameter)
        {
            for (int i = 0; i <  fireProjectiles.Count; i++)
            {
                if (!fireProjectiles[i].enabled)
                {
                    Debug.WriteLine("Reusing fire projectile");
                    fireProjectiles[i].Reset(position, diameter, Element.FIRE);
                    return fireProjectiles[i];
                }
            }

            Debug.WriteLine("No available fire projectiles, adding one");
            return CreateNewFireProjectile(position, diameter);
        }

        internal FireProjectile CreateNewFireProjectile(Vec3 position, float diameter = 0.5f)
        {
            FireProjectile newProjectile = new FireProjectile(position);
            newProjectile.Reset(position, diameter, Element.FIRE);
            fireProjectiles.Add(newProjectile);
            return newProjectile;
        }

        internal EarthProjectile GetEarthProjectile(Vec3 position, float diameter)
        {
            for (int i = 0; i < earthProjectiles.Count; i++)
            {
                if (!earthProjectiles[i].enabled)
                {
                    Debug.WriteLine("Reusing earth projectile");
                    earthProjectiles[i].Reset(position, diameter, Element.EARTH);
                    return earthProjectiles[i];
                }
            }

            Debug.WriteLine("No available earth projectiles, adding one");
         
            return CreateNewEarthProjectile(position, diameter);
        }

        internal EarthProjectile CreateNewEarthProjectile(Vec3 position, float diameter = 0.5f)
        {
            EarthProjectile newProjectile = new EarthProjectile(earthProjectileModel, position);
            newProjectile.Reset(position, diameter, Element.EARTH);
            earthProjectiles.Add(newProjectile);
            return newProjectile;
        }

        public void Add(Hand hand, Element element)
        {
            switch (element)
            {
                case Element.FIRE:
                    SpawnProjectile(hand, element);
                    break;
                case Element.EARTH:
                    SpawnProjectile(hand, element);
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

        public void ResetPlayerScore()
        {
            playerScore = 0;
        }

        public void Draw(bool demo)
        {
            if (!demo)
            {
                Pose windowPoseSeparator = new Pose(-1.0f, 1.05f, -2.003f, Quat.FromAngles(0, 180.0f, 0));
                UI.WindowBegin("Window Separator", ref windowPoseSeparator, new Vec2(135, 70) * U.cm, UIWin.Body);
                UI.WindowEnd();

                Text.Add("Count :" + (fireProjectiles.Count + earthProjectiles.Count), Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
                Text.Add("Score :" + playerScore, Matrix.TRS(scoreTextPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
            }


            // Draw fire projectiles
            for (int i = 0; i < fireProjectiles.Count; i++)
            {
                Projectile currentProjectile = fireProjectiles[i];

                if (currentProjectile.enabled)
                {
                    currentProjectile.Draw();
                }
            }

            // Draw earth projectiles
            for (int i = 0; i < earthProjectiles.Count; i++)
            {
                Projectile currentProjectile = earthProjectiles[i];

                if (currentProjectile.enabled)
                {
                    currentProjectile.Draw();
                }
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
                    if (projectile.GetTime() > 10.0f)
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
            Vec3 result = (currentPos - prevPos) / Time.Elapsedf;
            return result;
        }
    }
}
