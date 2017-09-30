using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaeParser.Components;
using static FormulaeParser.Flat.Patterns.MatchLibrary;

namespace FormulaeParser.Flat.Patterns
{
    public class UnifySum : FlatPattern
    {
        public override IEnumerable<IComponent> Flatten(IEnumerable<IComponent> list)
        {
            var c1 = list.ElementAtOrDefault(0);
            var c2 = list.ElementAtOrDefault(1);
            var c3 = list.ElementAtOrDefault(2);

            if(IsSum(c1) && IsBlock(c2) && (IsSum(c3) || IsSubtraction(c3) || IsNull(c3)))
            {
                var block = (Block)c2;
                return new IComponent[] { c1 }.Concat(block.Components).Concat(list.Skip(2));
            }

            return list;
        }
    }
}