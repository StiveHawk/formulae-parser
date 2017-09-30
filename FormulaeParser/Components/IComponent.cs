using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Components
{
    public interface IComponent
    {
        string ToString();
        bool EqualsTo(IComponent component);
    }
}
