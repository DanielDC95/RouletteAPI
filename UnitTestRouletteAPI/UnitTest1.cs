using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouletteAPI.DataBase;

namespace UnitTestRouletteAPI
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var connectResult = DataBase.connect();
            Assert.AreEqual(true, connectResult);
        }
    }
}
