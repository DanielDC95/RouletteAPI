using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouletteAPI.Models;


namespace UnitTestRouletteAPI
{
    [TestClass]
    public class Roulette_UnitTest
    {
        [TestMethod]
        public void TestRouletteExist()
        {
            Roulette roulette = new Roulette(_id: 0);
            var exists = roulette.rouletteExists();
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void TestCreate()
        {
            Roulette roulette = new Roulette(_id: 0);
            var exists = roulette.create();
            Assert.IsTrue(exists);
        }
    }
}
