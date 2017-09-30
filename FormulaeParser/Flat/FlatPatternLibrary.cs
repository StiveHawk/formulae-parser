using FormulaeParser.Components;
using FormulaeParser.Flat.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaeParser.Flat
{
    public class FlatPatternLibrary
    {
        public List<IFlatPattern> Patterns;

        public FlatPatternLibrary()
        {
            Patterns = new List<IFlatPattern>();
        }

        public Block Flatten(Block block)
        {
            Block previousIteration = block;
            
            bool restart = true;
            while(restart)
            {
                restart = false;

                Block currentIteration = new Block();
                currentIteration.Components = previousIteration.Components;
                
                int offset = 0;
                
                while(!restart && offset < currentIteration.Components.Count)
                {
                    var offsetSample = currentIteration.Components.Take(offset);
                    var currentSample = currentIteration.Components.Skip(offset);

                    foreach(var pattern in Patterns)
                    {
                        currentIteration.Components = offsetSample.Concat(pattern.Flatten(currentSample)).ToList();

                        if(!previousIteration.IsMatch(currentIteration))
                        {
                            previousIteration = currentIteration;
                            restart = true;
                        }
                    }

                    offset++;
                }
            }

            return previousIteration;
        }

        public static FlatPatternLibrary Default()
        {
            return new FlatPatternLibrary()
            {
                Patterns = new List<IFlatPattern>()
                {
                    new UnifySum()
                }
            };
        }
    }
}