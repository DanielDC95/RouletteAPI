using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouletteAPI.DataBase;

namespace UnitTestRouletteAPI
{
    [TestClass]
    public class DataBase_UnitTest
    {
        [TestMethod]
        public void TestConnect()
        {
            var connectResult = DataBase.connect();
            Assert.IsTrue(connectResult);
        }

        [TestMethod]
        public void TestDesonnect()
        {
            var connectResult = DataBase.desconnect();
            Assert.IsTrue(connectResult);
        }
    }
}
