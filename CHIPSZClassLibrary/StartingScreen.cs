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

        public bool playDemo1()
        {
            if (!endDemo && !firstStepDone)
            {
                UI.WindowBegin("  Clench fist to spawn EARTH ball   ", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   NEXT    -->   "))
                {
                    firstStepDone = true;                    
                }
                UI.WindowEnd();
            }
            return firstStepDone;
        }

        public void playDemo2()
        {
            if (!endDemo)
            {
                UI.WindowBegin("  Make palm to spawn FIRE ball   ", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   PLAY    GAME    -->   "))
                {
                    endDemo = true;
                }
                UI.WindowEnd();
            }
        }
    }
}