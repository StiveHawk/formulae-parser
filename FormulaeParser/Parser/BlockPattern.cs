using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Parser
{
    public class BlockPattern : IPattern
    {
        public char[] StartPattern;
        public char[] EndPattern;

        public PatternResult GetMatch(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return null;

            int depth = 0;
            int i = 0;
            int bodyContentLength = 0;
            
            // Matches start pattern
            if(StartPattern != null)
            {
                while(i < StartPattern.Length)
                {
                    if (content[i] != StartPattern[i])
                        return null;

                    i++;
                }
            }

            if (content[i] != '(') return null;
            i++;
            depth++;

            // Read internal content until the block ends
            while (i < content.Length)
            {
                if (content[i] == '(') depth++;
                else if (content[i] == ')') depth--;
                
                i++;

                if (depth == 0) break;
                else bodyContentLength++;
            }

            if (depth != 0) return null;

            // Matches the end pattern
            if(EndPattern != null)
            {
                if (i + EndPattern.Length > content.Length) return null;

                for (var j = 0; j < EndPattern.Length; j++)
                {
                    if (EndPattern[j] != content[i + j])
                        return null;
                }
            }

            // Get only the content inside the block
            int contentStartIndex = 1;
            if (StartPattern != null) contentStartIndex = StartPattern.Length + 1;

            int fullContentLenght = (StartPattern?.Length ?? 0) + bodyContentLength + (EndPattern?.Length ?? 0);
            fullContentLenght += 2; // Includes "(" and ")"
            return new PatternResult
            (
                this,
                content.Substring(contentStartIndex, bodyContentLength),
                content.Substring(0, fullContentLenght)
            );
        }
    }
}
