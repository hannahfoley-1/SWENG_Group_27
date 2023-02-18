using System;
using System.Timers;

namespace CHIPSZ
{
    internal class Countdown
    {
        private int remainingTime;
        private Timer timer;
        private bool isRunning;

        public Countdown(int remainingTime)
        {
            this.remainingTime = remainingTime;
            timer = new Timer(1000);
            timer.Elapsed += TimerElapsedHandler;
            isRunning = false;
        }

        public void Start()
        {
            timer.Start();
            this.isRunning = true;
        }

        private void TimerElapsedHandler (Object sender, EventArgs e)
        {
            remainingTime--;
            Console.WriteLine($"{remainingTime} seconds left");

            if (remainingTime == 0) timer.Stop();
        }

        public bool IsRunning()
        {
            return this.isRunning;
        }

    }
}
