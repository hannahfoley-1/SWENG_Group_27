using System;
using StereoKit;
using Windows.Devices.Enumeration;

namespace CHIPSZ
{
    /// <summary>
    /// Class that sets the game's time limit
    /// </summary>
    internal class Countdown : Model
    {
        private float duration;
        private Vec3 position;
        private bool isRunning;

        /// <summary>
        /// The constructor takes the desired duration of the game as a parameter
        /// </summary>
        /// <param name="duration"></param>
        public Countdown(float duration)
        {
            this.position = new Vec3(-2, 1, -2); // top left
            this.duration = duration;
            this.isRunning = true;
        }

        /// <summary>
        /// This method updates counter by decrementing the duration everytime that is called
        /// Since it is when the frame changes, I am subtracting the amount of time that takes
        /// to change from one frame to another.
        /// </summary>
        public void Update()
        {
            if (duration < 0)
            {
                isRunning = false;
                return;
            }

            duration -= Time.Elapsedf;
            Text.Add($"{MathF.Floor(duration)}", Matrix.TRS(position, Quat.FromAngles(0, 180, 0), 5)); // without rotation the text is inversed

        }

        public bool IsRunning()
        {
            return isRunning;
        }
    }
}
