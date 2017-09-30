using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaeParser.Components;

namespace FormulaeParser.Test
{
    [TestClass]
    public class NumberTest
    {
        [TestMethod]
        public void Number_Negative()
        {
            var n1 = new Number(-1);
            Assert.AreEqual(n1.Negative, true);
        }

        [TestMethod]
        public void Number_Positive()
        {
            var n1 = new Number(1);
            Assert.AreEqual(n1.Negative, false);
        }

        [TestMethod]
        public void Number_Zero()
        {
            var n1 = new Number(0);
            Assert.AreEqual(n1.Negative, false);
        }
    }
}
