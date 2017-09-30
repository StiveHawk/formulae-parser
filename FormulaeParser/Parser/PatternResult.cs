using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Parser
{
    public class PatternResult
    {
        public IPattern Pattern;
        public string InnerContent;
        public string FullContent;

        public PatternResult(IPattern pattern, string innerContent, string fullContent)
        {
            this.Pattern = pattern;
            this.InnerContent = innerContent;
            this.FullContent = fullContent;
        }
    }
}