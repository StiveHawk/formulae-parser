using FormulaeParser.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Flat.Patterns
{
    public class MatchLibrary
    {
        public static bool IsSum(IComponent comp) { return comp != null && comp is Operation && ((Operation)comp).Symbol.Equals("+"); }
        public static bool IsSubtraction(IComponent comp) { return comp != null && comp is Operation && ((Operation)comp).Symbol.Equals("-"); }
        public static bool IsMultiplication(IComponent comp) { return comp != null && comp is Operation && ((Operation)comp).Symbol.Equals("*"); }
        public static bool IsDivision(IComponent comp) { return comp != null && comp is Operation && ((Operation)comp).Symbol.Equals("/"); }
        public static bool IsNull(IComponent comp) { return comp == null; }

        public static bool IsBlock(IComponent comp) { return comp != null && comp is Block; }
    }
}