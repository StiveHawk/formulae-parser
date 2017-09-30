using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaeParser.Parser
{
    public class NumberPattern : IPattern
    {
        public PatternResult GetMatch(string content)
        {
            var match = Regex.Match(content, @"^\d+\.?\d*");

            if(match.Success)
                return new PatternResult(this, match.Value, match.Value);

            return null;
        }
    }
}
