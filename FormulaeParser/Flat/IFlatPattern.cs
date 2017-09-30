using FormulaeParser.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Flat
{
    public interface IFlatPattern
    {
        IEnumerable<IComponent> Flatten(IEnumerable<IComponent> list);
    }
}
