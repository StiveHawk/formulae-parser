using FormulaeParser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaeParser.Components
{
    public class Block : IComponent
    {
        public Block Parent;
        public List<IComponent> Components;
        public string Raw;

        public Block()
        {
            this.Components = new List<IComponent>();
        }

        public Block P(decimal decimalValue) { Components.Add(new Number(decimalValue)); return this; }
        public Block P(string symbol) { Components.Add(new Operation(symbol)); return this; }

        public bool IsMatch(Block block) { return IsMatch(block.Components); }
        public bool IsMatch(params IComponent[] components) { return IsMatch(components); }
        public bool IsMatch(IEnumerable<IComponent> components)
        {
            if (Components.Count != components.Count()) return false;

            for (int i = 0; i < Components.Count; i++)
                if (!Components[i].EqualsTo(components.ElementAt(i)))
                    return false;

            return true;
        }

        private Block AddChildBlock()
        {
            Block block = new Block();

            block.Parent = this;
            Components.Add(block);

            return block;
        }

        public Block Flatten()
        {
            if (Components.Count == 1)
                if (Components[0] is Block)
                    return ((Block)Components[0]).Flatten();

            return this;
        }

        public static Block Parse(string formulae)
        {
            return Parse(formulae, Parser.Parser.Default());
        }

        public static Block Parse(string formulae, Parser.Parser parser)
        {
            if (String.IsNullOrWhiteSpace(formulae)) return null;
            if (formulae.IndexOf(' ') >= 0)
            {
                formulae = Regex.Replace(formulae, " +", string.Empty, RegexOptions.Compiled);
                if (String.IsNullOrWhiteSpace(formulae)) return null;
            }

            string raw = formulae;

            List<IComponent> components = new List<IComponent>();

            PatternResult match = null;

            do
            {
                match = parser.NextPattern(formulae);

                if (match != null)
                {
                    if (match.Pattern is NumberPattern)
                    {
                        components.Add(new Number(Convert.ToDecimal(match.InnerContent)));
                    }
                    else if (match.Pattern is CharPattern)
                    {
                        components.Add(new Operation(match.InnerContent));
                    }
                    else if (match.Pattern is BlockPattern)
                    {
                        var parsedBlock = Block.Parse(match.InnerContent, parser);
                        if (parsedBlock != null) components.Add(parsedBlock);
                    }

                    formulae = formulae.Remove(0, match.FullContent.Length);
                }
            } while (match != null);

            Block returnBlock = null;
            if (components.Count > 0)
            {
                returnBlock = new Block()
                {
                    Components = components,
                    Raw = raw
                };
            }

            return returnBlock;
        }
        
        public bool EqualsTo(IComponent component)
        {
            if (component == null) return false;
            if (!(component is Block)) return false;

            var block = (Block)component;
            for(var i = 0; i < block.Components.Count; i++)
            {
                if (!block.Components[i].EqualsTo(Components[i]))
                    return false;
            }

            return true;
        }
    }
}
