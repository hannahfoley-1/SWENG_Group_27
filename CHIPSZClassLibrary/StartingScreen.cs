using StereoKit;

namespace CHIPSZ
{
    public class StartingScreen
    {
        private Pose windowPose;
        private Vec3 winVec = new Vec3(-0.1f, 0.2f, -0.3f);
        private bool ifCloseStartGame = true;
        private bool ifCloseStartDemo = true;
        private bool endDemo = false;
        bool firstStepDone = false;


        public StartingScreen()
        {
            this.windowPose = new Pose(winVec, Quat.LookDir(1, 0, 1));
        }

        public bool GetIfStartGame()
        {
            return ifCloseStartGame;
        }

        public bool GetIfStartDemo()
        {
            return ifCloseStartDemo;
        }

        public bool GetIfEndDemo()
        {
            return endDemo;
        }

        public void SetIfStartGame(bool set)
        {
            ifCloseStartGame = set;
        }

        public void SetIfStartDemo(bool set)
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

        public bool PlayDemo1()
        {
            if (!endDemo && !firstStepDone)
            {
                UI.WindowBegin("  Clench fist to spawn EARTH model   ", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   NEXT    -->   "))
                {
                    firstStepDone = true;                    
                }
                UI.WindowEnd();
            }
            return firstStepDone;
        }

        public void PlayDemo2()
        {
            if (!endDemo)
            {
                UI.WindowBegin("  Make palm to spawn FIRE model   ", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   PLAY    GAME    -->   "))
                {
                    endDemo = true;
                }
                UI.WindowEnd();
            }
        }
    }
}