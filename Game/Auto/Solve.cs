using System.Collections.Generic;
using System.Linq;

namespace Game.Auto
{
    public class Solve
    {
        public List<Step> Run(AutoBoard board)
        {
            var waits = new List<AutoBoard>() { board };
            var states = new List<string>() { board.ToString() };
            while (waits.Any())
            {
                var newWaits = new List<AutoBoard>();
                foreach (var wait in waits)
                {
                    var steps = wait.GetNextSteps();
                    foreach (var step in steps)
                    {
                        var nb = wait.Copy();
                        var flag = nb.TryMove(step);
                        if (!flag)
                        {
                            continue;
                        }

                        var state = nb.ToString();
                        if (states.Contains(state))
                        {
                            continue;
                        }

                        if (nb.IsSuccess())
                        {
                            return nb.GetSteps();
                        }
                        states.Add(state);
                        newWaits.Add(nb);
                    }
                }

                waits = newWaits;
            }

            return null;
        }
    }
}