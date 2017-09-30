using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Components
{
    public class Number : IComponent
    {
        public decimal Value;
        public bool Negative;

        public Number(decimal value)
        {
            this.Value = value;
            this.Negative = Value < 0;
        }

        public bool EqualsTo(IComponent component)
        {
            if (component == null) return false;
            if (!(component is Number)) return false;
            return ((Number)component).Value.Equals(Value);
        }
    }
}
