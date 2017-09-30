using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaeParser.Parser;
using FormulaeParser.Components;

namespace FormulaeParser.Test
{
    [TestClass]
    public class BlockTest
    {
        [TestMethod]
        public void Operation_EqualToTrue()
        {
            var op1 = new Operation("+");
            var op2 = new Operation("+");

            Assert.AreEqual(op1.EqualsTo(op2), true);
        }

        [TestMethod]
        public void Operation_EqualToFalse()
        {
            var op1 = new Operation("+");
            var op2 = new Operation("-");

            Assert.AreEqual(op1.EqualsTo(op2), false);
        }

        [TestMethod]
        public void Block_SimpleEqualityBlocks()
        {
            var block1 = new Block().P(1).P("+").P(1);
            var block2 = new Block().P(1).P("+").P(1);

            var match = block1.IsMatch(block2);
            Assert.AreEqual(match, true);
        }

        [TestMethod]
        public void Block_SimpleInequalityBlocks1()
        {
            var block1 = new Block().P(1).P("+").P(1);
            var block2 = new Block().P(1).P("-").P(1);

            var match = block1.IsMatch(block2);
            Assert.AreEqual(match, false);
        }

        [TestMethod]
        public void Block_SimpleInequalityBlocks2()
        {
            var block1 = new Block().P(1).P("+").P(1);
            var block2 = new Block().P(1).P("+").P(2);

            var match = block1.IsMatch(block2);
            Assert.AreEqual(match, false);
        }

        [TestMethod]
        public void BlockParse_Blank()
        {
            var b = Block.Parse("");

            Assert.AreEqual(b == null, true);
        }

        [TestMethod]
        public void BlockParse_Single_Empty()
        {
            var b = Block.Parse("()");

            Assert.AreEqual(b == null, true);
        }

        [TestMethod]
        public void BlockParse_Number()
        {
            var b = Block.Parse("11");

            Assert.AreEqual(b != null, true);
            Assert.AreEqual(b.Components.Count == 1, true);
            Assert.AreEqual(b.Components[0] is Number, true);
        }

        [TestMethod]
        public void BlockParse_Single_Filled()
        {
            var b = Block.Parse("(11)");

            Assert.AreEqual(b != null, true);
            Assert.AreEqual(b.Components.Count == 1, true);
            Assert.AreEqual(b.Components[0] is Block, true);

            var childBlock = (Block)b.Components[0];

            Assert.AreEqual(childBlock.Components.Count == 1, true);
            Assert.AreEqual(childBlock.Components[0] is Number, true);
        }

        [TestMethod]
        public void BlockParse_Single_OneChild_Empty()
        {
            var b = Block.Parse("(())");

            Assert.AreEqual(b == null, true);
        }

        [TestMethod]
        public void BlockParse_Single_OneChild_Filled()
        {
            var b = Block.Parse("((11))");

            Assert.AreEqual(b != null, true);
            Assert.AreEqual(b.Components.Count == 1, true);
            Assert.AreEqual(b.Components[0] is Block, true);

            var childBlock = (Block)b.Components[0];
            Assert.AreEqual(childBlock.Components.Count == 1, true);
            Assert.AreEqual(childBlock.Components[0] is Block, true);

            childBlock = (Block)childBlock.Components[0];
            Assert.AreEqual(childBlock.Components.Count == 1, true);
            Assert.AreEqual(childBlock.Components[0] is Number, true);

            var childValue = (Number)childBlock.Components[0];
            Assert.AreEqual(childValue.Value == 11, true);
        }

        [TestMethod]
        public void BlockParse_Single_TwoChild_Empty()
        {
            var b = Block.Parse("(()())");

            Assert.AreEqual(b == null, true);
        }

        [TestMethod]
        public void BlockParse_Single_TwoChild_Empty_Filled()
        {
            var b = Block.Parse("((11)(22))");

            Assert.AreEqual(b != null, true);
            Assert.AreEqual(b.Components.Count == 1, true);
            Assert.AreEqual(b.Components[0] is Block, true);

            var childBlock = (Block)b.Components[0];
            Assert.AreEqual(childBlock.Components.Count == 2, true);
            
            Assert.AreEqual(childBlock.Components[0] is Block, true);
            Assert.AreEqual(((Block)childBlock.Components[0]).Components.Count == 1, true);

            Assert.AreEqual(childBlock.Components[1] is Block, true);
            Assert.AreEqual(((Block)childBlock.Components[1]).Components.Count == 1, true);
        }

        [TestMethod]
        public void BlockParse_Double_Empty()
        {
            var b = Block.Parse("()()");

            Assert.AreEqual(b == null, true);
        }

        [TestMethod]
        public void BlockParse_Double_Filled()
        {
            var b = Block.Parse("(11)(22)");

            Assert.AreEqual(b != null, true);
            Assert.AreEqual(b.Components.Count == 2, true);

            Assert.AreEqual(b.Components[0] is Block, true);
            Assert.AreEqual(((Block)b.Components[0]).Components.Count == 1, true);

            Assert.AreEqual(b.Components[1] is Block, true);
            Assert.AreEqual(((Block)b.Components[1]).Components.Count == 1, true);
        }

        [TestMethod]
        public void BlockFlatten_Deepness0()
        {
            var b = Block.Parse("11").Flatten(null);

            Assert.AreEqual(b.Components[0] is Number, true);
        }

        [TestMethod]
        public void BlockFlatten_Deepness1()
        {
            var b = Block.Parse("(11)").Flatten(null);

            Assert.AreEqual(b.Components[0] is Number, true);
        }

        [TestMethod]
        public void BlockFlatten_Deepness2()
        {
            var b = Block.Parse("((11))").Flatten(null);

            Assert.AreEqual(b.Components[0] is Number, true);
        }

        [TestMethod]
        public void BlockFlatten_Formulae()
        {
            var b = Block.Parse("(((1+1)/(2+2)))").Flatten(null);

            Assert.AreEqual(b.Components[0] is Block, true);
            Assert.AreEqual(b.Components[1] is Operation, true);
            Assert.AreEqual(b.Components[2] is Block, true);
            Assert.AreEqual(b.Raw == "(1+1)/(2+2)", true);

        }
    }
}