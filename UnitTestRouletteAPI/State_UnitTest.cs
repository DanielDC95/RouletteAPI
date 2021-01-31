using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouletteAPI.Models;

namespace UnitTestRouletteAPI
{
    [TestClass]
    public class State_UnitTest
    {

        [TestMethod]
        public void TestGetState_close()
        {
            var state = State.getState("2");
            Assert.AreEqual("close", state);
        }

        [TestMethod]
        public void TestGetState_open()
        {
            var state = State.getState("1");
            Assert.AreEqual("open", state);
        }
    }
}
