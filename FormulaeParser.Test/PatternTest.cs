using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaeParser.Parser;

namespace FormulaeParser.Test
{
    [TestClass]
    public class PatternTest
    {
        [TestMethod]
        public void Parse_Number_Integer()
        {
            NumberPattern p = new NumberPattern();
            string value = "123";
            var match = p.GetMatch(value);

            Assert.AreEqual(match != null, true);
            Assert.AreEqual(match.InnerContent == "123", true);
            Assert.AreEqual(match.FullContent == "123", true);
        }

        [TestMethod]
        public void Parse_Number_Decimal1()
        {
            NumberPattern p = new NumberPattern();
            string value = "123.";
            var match = p.GetMatch(value);

            Assert.AreEqual(match != null, true);
            Assert.AreEqual(match.InnerContent == "123.", true);
            Assert.AreEqual(match.FullContent == "123.", true);
        }

        [TestMethod]
        public void Parse_Number_Decimal2()
        {
            NumberPattern p = new NumberPattern();
            string value = "123.45";
            var match = p.GetMatch(value);

            Assert.AreEqual(match != null, true);
            Assert.AreEqual(match.InnerContent == "123.45", true);
            Assert.AreEqual(match.FullContent == "123.45", true);
        }

        [TestMethod]
        public void Parse_Number_DecimalFail()
        {
            NumberPattern p = new NumberPattern();
            string value = "123.45a";
            var match = p.GetMatch(value);

            Assert.AreEqual(match != null, true);
            Assert.AreEqual(match.InnerContent == "123.45", true);
            Assert.AreEqual(match.FullContent == "123.45", true);
        }

        [TestMethod]
        public void Parse_Char_Simple_Success()
        {
            CharPattern p = new CharPattern("+");
            string value = "+";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Char_Simple_Fail()
        {
            CharPattern p = new CharPattern("+");
            string value = "-";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Char_Complex_Success()
        {
            CharPattern p = new CharPattern("Log(10)");
            string value = "Log(10)";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Char_Complex_Fail()
        {
            CharPattern p = new CharPattern("Log(10)");
            string value = "log(10)";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_Empty()
        {
            BlockPattern p = new BlockPattern();
            string value = "()";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_Simple()
        {
            BlockPattern p = new BlockPattern();
            string value = "(11)";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_Stack_One()
        {
            BlockPattern p = new BlockPattern();
            string value = "(())";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_Stack_Two()
        {
            BlockPattern p = new BlockPattern();
            string value = "(()())";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_DoubleStack_One()
        {
            BlockPattern p = new BlockPattern();
            string value = "((()))";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_Incomplete_Fail()
        {
            BlockPattern p = new BlockPattern();
            string value = "(()";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_WithStart()
        {
            BlockPattern p = new BlockPattern() { StartPattern = "Sqrt".ToCharArray() };
            string value = "Sqrt()";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_WithStart_Fail()
        {
            BlockPattern p = new BlockPattern() { StartPattern = "Sqrt".ToCharArray() };
            string value = "sqrt()";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_WithStart_Fail3()
        {
            BlockPattern p = new BlockPattern() { StartPattern = "Sqrt".ToCharArray() };
            string value = "Sqr()";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_WithStart_Fail4()
        {
            BlockPattern p = new BlockPattern() { StartPattern = "Sqrt".ToCharArray() };
            string value = "qrt()";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_WithStart_WithEnd_Sucess()
        {
            BlockPattern p = new BlockPattern() { StartPattern = "Sqrt".ToCharArray() };
            string value = "Sqrt()*2";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_WithEnd()
        {
            BlockPattern p = new BlockPattern() { EndPattern = "Aa".ToCharArray() };
            string value = "()Aa";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_WithEnd_Fail()
        {
            BlockPattern p = new BlockPattern() { EndPattern = "Aa".ToCharArray() };
            string value = "()aa";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_WithEnd_Fail2()
        {
            BlockPattern p = new BlockPattern() { EndPattern = "Aa".ToCharArray() };
            string value = "a()Aa";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_WithEnd_Fail3()
        {
            BlockPattern p = new BlockPattern() { EndPattern = "Aa".ToCharArray() };
            string value = "()A";

            Assert.AreEqual(p.GetMatch(value) != null, false);
        }

        [TestMethod]
        public void Parse_Block_WithContent()
        {
            BlockPattern p = new BlockPattern();
            string value = "(aa)";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        [TestMethod]
        public void Parse_Block_WithContent2()
        {
            BlockPattern p = new BlockPattern();
            string value = "(2*(1-2))";

            Assert.AreEqual(p.GetMatch(value) != null, true);
        }

        // Parser
        [TestMethod]
        public void Parser_GetNumber()
        {
            Parser.Parser p = Parser.Parser.Default();
            var match = p.NextPattern("12+34");

            Assert.AreEqual(match?.FullContent == "12", true);
        }

        [TestMethod]
        public void Parser_GetSqrt()
        {
            Parser.Parser p = Parser.Parser.Default();
            var match = p.NextPattern("Sqrt(5)*10");

            Assert.AreEqual(match?.FullContent == "Sqrt(5)", true);
        }

        [TestMethod]
        public void Parser_GetOperation()
        {
            Parser.Parser p = Parser.Parser.Default();
            var match = p.NextPattern("-12");

            Assert.AreEqual(match?.FullContent == "-", true);
        }

        [TestMethod]
        public void Parser_GetBlock()
        {
            Parser.Parser p = Parser.Parser.Default();
            var match = p.NextPattern("(12)-(1+2)");

            Assert.AreEqual(match?.FullContent == "(12)", true);
        }
    }
}