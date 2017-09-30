using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaeParser.Components;

namespace FormulaeParser.Flat
{
    public abstract class FlatPattern : IFlatPattern
    {
        public abstract IEnumerable<IComponent> Flatten(IEnumerable<IComponent> list);
        
        public bool Match(IEnumerable<IComponent> list, int index, params Func<IComponent, bool>[] matches)
        {
            return matches.Any(match => match(list.ElementAtOrDefault(index)));
        }
    }
}
