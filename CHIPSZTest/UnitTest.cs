
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
            Ball ball = new Ball(Vec3.Zero, 0.3f, Element.FIRE);
            Assert.IsNotNull(ball);
        }
    }
}
