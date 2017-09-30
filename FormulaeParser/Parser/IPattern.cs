using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Parser
{
    public interface IPattern
    {
        PatternResult GetMatch(string content);
    }
}
