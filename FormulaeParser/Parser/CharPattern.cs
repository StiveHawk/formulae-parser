using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Parser
{
    public class CharPattern : IPattern
    {
        public char[] Sequence;

        public CharPattern(char[] sequence)
        {
            Sequence = sequence;
        }

        public CharPattern(string sequence)
        {
            Sequence = sequence.ToArray();
        }

        public PatternResult GetMatch(string content)
        {
            if (content.Length < Sequence.Length) return null;

            for (var i = 0; i < Sequence.Length; i++)
                if (content[i] != Sequence[i]) return null;

            var result = content.Substring(0, Sequence.Length);
            return new PatternResult(this, result, result);
        }
    }
}
