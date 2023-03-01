using StereoKit;

namespace CHIPSZClassLibrary
{   
    public class GameTimer
    {
        public double start;
        public double remaining;
        public bool elasped = false;

        public GameTimer(double start)
        {
            this.start = start;
            this.remaining = start;
        }

        public void Update()
        {
            remaining -= Time.Elapsedf;

            if (remaining <= 0)
            {
                elasped = true;
            } else
            {
                elasped = false;
            }
        }

        public void Reset()
        {
            remaining = start;
            elasped = false;
        }
    }
}
