using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Components
{
    public class Operation : IComponent
    {
        public string Symbol;
        
        public Operation(string symbol)
        {
            this.Symbol = symbol;
        }

        public bool EqualsTo(IComponent component)
        {
            if (component == null) return false;
            if (!(component is Operation)) return false;
            return ((Operation)component).Symbol.Equals(Symbol);
        }
    }
}