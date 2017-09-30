using FormulaeParser.Components;
using FormulaeParser.Flat;
using FormulaeParser.Flat.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Test
{
    [TestClass]
    public class FlattenerTest
    {
        #region General

        private FlatPatternLibrary GetSumLibrary()
        {
            return new FlatPatternLibrary()
            {
                Patterns = new List<IFlatPattern>() { new UnifySum() }
            };
        }

        [TestMethod]
        public void Flattener_EqualOutput()
        {
            var block = new Block() { Components = new List<IComponent>() { new Number(1), new Operation("+"), new Number(2) } };
            var flatten = block.Flatten(GetSumLibrary());

            Assert.IsTrue(block.IsMatch(flatten));
        }

        [TestMethod]
        public void Flattener_DifferentOutput()
        {
            var block = new Block()
            {
                Components = new List<IComponent>()
                {
                    new Number(1),
                    new Operation("+"),
                    new Block()
                    {
                        Components = new List<IComponent>()
                        {
                            new Number(2),
                            new Operation("*"),
                            new Number(3)
                        }
                    }
                }
            };

            var flatten = block.Flatten(GetSumLibrary());

            Assert.IsFalse(block.IsMatch(flatten));

            var expectedOutput = new Block()
            {
                Components = new List<IComponent>()
                {
                    new Number(1),
                    new Operation("+"),
                    new Number(2),
                    new Operation("*"),
                    new Number(3)
                }
            };

            Assert.IsTrue(flatten.IsMatch(expectedOutput));
        }

        #endregion

        #region UnifySum
        [TestMethod]
        public void Flattener_UnifySum_1()
        {
            var components = new IComponent[] { new Number(1), new Operation("+"), new Number(2)};
            var newBlock = new UnifySum().Flatten(components).ToArray();
            
            Assert.AreEqual((newBlock[0] as Number).Value, 1);
            Assert.AreEqual((newBlock[1] as Operation).Symbol, "+");
            Assert.AreEqual((newBlock[2] as Number).Value, 2);
        }

        [TestMethod]
        public void Flattener_UnifySum_2()
        {
            var components = new IComponent[]
            {
                new Operation("+"),
                new Block()
                {
                    Components = new List<IComponent>() { new Number(2) }
                }
            };

            var newBlock = new UnifySum().Flatten(components).ToArray();
            
            Assert.AreEqual((newBlock[0] as Operation).Symbol, "+");
            Assert.AreEqual((newBlock[1] as Number).Value, 2);
        }

        [TestMethod]
        public void Flattener_UnifySum_3()
        {
            var components = new IComponent[]
            {
                new Operation("+"),
                new Block()
                {
                    Components = new List<IComponent>()
                    {
                        new Number(2),
                        new Operation("*"),
                        new Number(3)
                    }
                }
            };

            var newBlock = new UnifySum().Flatten(components).ToArray();
            
            Assert.AreEqual((newBlock[0] as Operation).Symbol, "+");
            Assert.AreEqual((newBlock[1] as Number).Value, 2);
            Assert.AreEqual((newBlock[2] as Operation).Symbol, "*");
            Assert.AreEqual((newBlock[3] as Number).Value, 3);
        }

        [TestMethod]
        public void Flattener_UnifySum_4()
        {
            var components = new IComponent[]
            {
                new Operation("+"),
                new Block()
                {
                    Components = new List<IComponent>()
                    {
                        new Number(2),
                        new Operation("*"),
                        new Number(3)
                    }
                },
                new Operation("*"),
                new Number(4)
            };

            var newBlock = new UnifySum().Flatten(components).ToArray();
            
            Assert.AreEqual((newBlock[0] as Operation).Symbol, "+");
            Assert.IsTrue(newBlock[1] is Block);
            Assert.AreEqual((newBlock[2] as Operation).Symbol, "*");
        }

        [TestMethod]
        public void Flattener_UnifySum_5()
        {
            var components = new IComponent[]
            {
                new Operation("+"),
                new Block()
                {
                    Components = new List<IComponent>()
                    {
                        new Number(2),
                        new Operation("*"),
                        new Number(3)
                    }
                },
                new Operation("-"),
                new Number(4)
            };

            var newBlock = new UnifySum().Flatten(components).ToArray();

            Assert.AreEqual((newBlock[0] as Operation).Symbol, "+");
            Assert.AreEqual((newBlock[1] as Number).Value, 2);
            Assert.AreEqual((newBlock[2] as Operation).Symbol, "*");
            Assert.AreEqual((newBlock[3] as Number).Value, 3);
            Assert.AreEqual((newBlock[4] as Operation).Symbol, "-");
            Assert.AreEqual((newBlock[5] as Number).Value, 4);
        }

        #endregion
    }
}
