using StereoKit;
using System;
using Windows.UI.Xaml;

namespace CHIPSZ
{
    internal class StartingScreen
    {
        private Pose windowPose;
        private Vec3 winVec = new Vec3(-0.1f, 0.2f, -0.3f);
        private bool ifCloseStartGame = true;
        private bool ifCloseStartDemo = true;
        private bool endDemo = false;

        public StartingScreen()
        {
            this.windowPose = new Pose(winVec, Quat.LookDir(1, 0, 1));
        }

        public bool getIfStartGame()
        {
            return ifCloseStartGame;
        }

        public bool getIfStartDemo()
        {
            return ifCloseStartDemo;
        }

        public bool getIfEndDemo()
        {
            return endDemo;
        }

        public void setIfStartGame(bool set)
        {
            ifCloseStartGame = set;
        }

        public void setIfStartDemo(bool set)
        {
            ifCloseStartDemo = set;
        }

        public void Draw()
        {
            if (ifCloseStartGame != false && ifCloseStartDemo != false)
            {
                UI.WindowBegin("  Welcome to this Game!", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   START  GAME   -->   "))
                {
                    ifCloseStartGame = false;
                }
                else if (UI.Button("   START  DEMO   -->   "))
                {
                    ifCloseStartDemo = false;
                }
                UI.WindowEnd();
            }

        }

        public void playDemo()
        {
            if (!endDemo)
            {
                UI.WindowBegin("  Clench fist to spawn ball", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   PLAY   GAME   -->   "))
                {
                    endDemo = true;
                }
                UI.WindowEnd();
                //TODO: Add further instruction using a 'next instruction button'??
            }
        }
    }
}