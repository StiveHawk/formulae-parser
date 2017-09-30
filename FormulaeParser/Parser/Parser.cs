using FormulaeParser.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaeParser.Parser
{
    public class Parser
    {
        public List<IPattern> Patterns;

        public Parser()
        {
            this.Patterns = new List<IPattern>();
        }

        public Block Parse(string content)
        {
            List<IComponent> components = new List<IComponent>();
            return Block.Parse(content);
        }

        public PatternResult NextPattern(string mainContent)
        {
            var numberMatch = Regex.Match(mainContent, @"^\d+\.?\d*");

            if (numberMatch.Success)
                return new PatternResult(new NumberPattern(), numberMatch.Value, numberMatch.Value);

            List<PatternResult> results = new List<PatternResult>();
            foreach(var pattern in Patterns)
            {
                var result = pattern.GetMatch(mainContent);

                if (result != null)
                    results.Add(result);
            }

            if (results.Count == 0) return null;
            if (results.Count == 1)
                return results[0];

            return null;
        }

        public static Parser Default()
        {
            return new Parser()
            {
                Patterns = new List<IPattern>()
                {
                    new BlockPattern(),

                    new BlockPattern() { StartPattern = "Sqrt".ToCharArray() },

                    new CharPattern("+"),
                    new CharPattern("-"),
                    new CharPattern("*"),
                    new CharPattern("/")
                }
            };
        }
    }
}