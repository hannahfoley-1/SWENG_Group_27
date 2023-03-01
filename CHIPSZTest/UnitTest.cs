
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CHIPSZClassLibrary;
using StereoKit;

namespace CHIPSZTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Ball ball = new Ball(new Vec3(0.0f, 0.0f, 0.0f), 0.3f);
            Assert.IsNotNull(ball);
        }
    }
}
