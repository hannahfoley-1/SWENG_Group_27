using System.Collections.Generic;
using StereoKit;
using System.Diagnostics;

namespace CHIPSZClassLibrary
{
    public class ProjectileGenerator
    {
        internal int startFireProjectileCount;
        internal int startEarthProjectileCount;
        internal int startWaterProjectileCount;
        internal int startAirProjectileCount;
        internal int playerScore;

        // Projectiles
        private List<FireProjectile> fireProjectiles;
        private List<EarthProjectile> earthProjectiles;
        private List<WaterProjectile> waterProjectiles;
        private List<AirProjectile> airProjectiles;

        // Positions
        private readonly Vec3 textPos;
        private readonly Vec3 scoreTextPos;

        private Model earthProjectileModel;

        public ProjectileGenerator(int startFireProjectileCount = 10, int startEarthProjectileCount = 10, int startWaterProjectileCount = 10)
        {
            this.startFireProjectileCount = startFireProjectileCount;
            this.startEarthProjectileCount = startEarthProjectileCount;
            this.startWaterProjectileCount = startWaterProjectileCount;
            this.startAirProjectileCount = startWaterProjectileCount;

            earthProjectileModel = Model.FromFile("EarthProjectile.obj");

            fireProjectiles = new List<FireProjectile>();
            InitialiseFireProjectilePool(startFireProjectileCount);

            earthProjectiles = new List<EarthProjectile>();
            InitialiseEarthProjectilePool(startEarthProjectileCount);

            waterProjectiles = new List<WaterProjectile>();
            InitialiseWaterProjectilePool(startWaterProjectileCount);

            airProjectiles = new List<AirProjectile>();
            InitialiseAirProjectilePool(startAirProjectileCount);

            textPos = new Vec3(-1.0f, 0.5f, -2.0f);
            scoreTextPos = new Vec3(-1.0f, 0.9f, -2.0f);
            playerScore = 0;

        }

        public void InitialiseFireProjectilePool(int startFireProjectileCount)
        {
            for (int i = 0; i < startFireProjectileCount; i++)
            {
                Projectile projectile = CreateNewFireProjectile(Vec3.Zero, 0.01f);
                projectile.Disable();
            }
        }

        public void InitialiseEarthProjectilePool(int startEarthProjectileCount)
        {
            for (int i = 0; i < startEarthProjectileCount; i++)
            {
                Projectile projectile = CreateNewEarthProjectile(Vec3.Zero, 0.01f);
                projectile.Disable();
            }
        }

        public void InitialiseAirProjectilePool(int startEarthProjectileCount)
        {
            for (int i = 0; i < startAirProjectileCount; i++)
            {
                Projectile projectile = CreateNewAirProjectile(Vec3.Zero, 0.01f);
                projectile.Disable();
            }
        }

        public void InitialiseWaterProjectilePool(int startWaterProjectileCount)
        {
            for (int i = 0; i < startWaterProjectileCount; i++)
            {
                Projectile projectile = CreateNewWaterProjectile(Vec3.Zero, 0.25f);
                projectile.Disable();
            }
        }

        public Projectile SpawnProjectile(Hand hand, Element element)
        {
            Projectile projectile = null;

            switch (element)
            {
                case Element.FIRE:
                    projectile = GetFireProjectile(hand.palm.position, 0.1f);
                    break;
                case Element.EARTH:
                    projectile = GetEarthProjectile(hand.palm.position, 0.1f);
                    break;
                case Element.WATER:
                    projectile = GetWaterProjectile(hand.palm.position, 0.1f);
                    break;
                case Element.AIR:
                    projectile = GetAirProjectile(hand.palm.position, 0.1f);
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
            for (int i = 0; i < fireProjectiles.Count; i++)
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


        internal AirProjectile GetAirProjectile(Vec3 position, float diameter)
        {
            for (int i = 0; i < airProjectiles.Count; i++)
            {
                if (!airProjectiles[i].enabled)
                {
                    Debug.WriteLine("Reusing Air projectile");
                    airProjectiles[i].Reset(position, diameter, Element.AIR);
                    return airProjectiles[i];
                }
            }

            Debug.WriteLine("No available Air projectiles, adding one");
            return CreateNewAirProjectile(position, diameter);
        }
        internal FireProjectile CreateNewFireProjectile(Vec3 position, float diameter = 0.5f)
        {
            FireProjectile newProjectile = new FireProjectile(position);
            newProjectile.Reset(position, diameter, Element.FIRE);
            fireProjectiles.Add(newProjectile);
            return newProjectile;
        }
        internal AirProjectile CreateNewAirProjectile(Vec3 position, float diameter = 0.5f)
        {
            AirProjectile newProjectile = new AirProjectile(position);
            newProjectile.Reset(position, diameter, Element.AIR);
            airProjectiles.Add(newProjectile);
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

        internal WaterProjectile GetWaterProjectile(Vec3 position, float diameter)
        {
            for (int i = 0; i < waterProjectiles.Count; i++)
            {
                if (!waterProjectiles[i].enabled)
                {
                    Debug.WriteLine("Reusing water projectile");
                    waterProjectiles[i].Reset(position, diameter, Element.WATER);
                    return waterProjectiles[i];
                }
            }

            Debug.WriteLine("No available water projectiles, adding one");

            return CreateNewWaterProjectile(position, diameter);
        }

        internal WaterProjectile CreateNewWaterProjectile(Vec3 position, float diameter)
        {
            WaterProjectile newProjectile = new WaterProjectile(position, diameter, Element.WATER);
            newProjectile.Reset(position, diameter, Element.WATER);
            waterProjectiles.Add(newProjectile);
            return newProjectile;
        }

        public void UpdatePlayerScore(Hand hand, Projectile projectile, int targetPoints)
        {
            playerScore += targetPoints;
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

                Text.Add("Count :" + (fireProjectiles.Count + earthProjectiles.Count + waterProjectiles.Count), Matrix.TRS(textPos, Quat.FromAngles(0, 180.0f, 0), 10.0f));
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

            // Draw water projectiles
            for (int i = 0; i < waterProjectiles.Count; i++)
            {
                Projectile currentProjectile = waterProjectiles[i];

                if (currentProjectile.enabled)
                {
                    currentProjectile.Draw();
                }
            }

            for (int i = 0; i < airProjectiles.Count; i++)
            {
                Projectile currentProjectile = airProjectiles[i];

                if (currentProjectile.enabled)
                {
                    currentProjectile.Draw();
                }
            }
        }

        public void Update(Hand hand)
        {
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

            for (int i = 0; i < airProjectiles.Count; i++)
            {
                Projectile projectile = airProjectiles[i];

                if (projectile.enabled)
                {
                    if (projectile.GetTime() > 5.0f)
                    {
                        projectile.Disable();
                    }

                    projectile.UpdatePosition();
                }
            }

            for (int i = 0; i < earthProjectiles.Count; i++)
            {
                Projectile projectile = earthProjectiles[i];

                if (projectile.enabled)
                {
                    if (projectile.GetTime() > 10.0f)
                    {
                        projectile.Disable();
                    }
                    else
                    {
                        //updatePlayerScore(hand, model);
                        projectile.UpdatePosition();
                    }
                }
            }

            // Draw water projectiles
            for (int i = 0; i < waterProjectiles.Count; i++)
            {
                Projectile projectile = waterProjectiles[i];

                if (projectile.enabled)
                {
                    if (projectile.GetTime() > 10.0f)
                    {
                        projectile.Disable();
                    }

                    projectile.UpdatePosition();
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

            foreach (Projectile projectile in waterProjectiles)
            {
                projectiles.Add(projectile);
            }
            foreach (Projectile projectile in airProjectiles)
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
        public int GetPlayerScore() => playerScore;
    }
}
