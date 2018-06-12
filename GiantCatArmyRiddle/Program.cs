using System;
using System.Collections.Generic;
using System.Linq;

namespace GiantCatArmyRiddle
{
    enum Operation { Plus5, Plus7, Sqrt }
    class Display
    {
        static int[] squares_under_60 = { 4, 9, 16, 25, 36, 49 };

        int val = 0;
        bool dead = false;
        List<int> seq = new List<int>();

        public void Apply(Operation op)
        {
            switch (op)
            {
                case Operation.Plus5:
                    int newVal = val + 5;
                    if (seq.Contains(newVal))
                    {
                        dead = true;
                    }
                    else
                    {
                        seq.Add(newVal);
                        val = newVal;
                    }
                    break;
                case Operation.Plus7:
                    newVal = val + 7;
                    if (seq.Contains(newVal))
                    {
                        dead = true;
                    }
                    else
                    {
                        seq.Add(newVal);
                        val = newVal;
                    }
                    break;
                case Operation.Sqrt:
                    if (!squares_under_60.Contains(val))
                    {
                        dead = true;
                    }
                    else
                    {
                        newVal = (int)Math.Sqrt(val);
                        seq.Add(newVal);
                        val = newVal;
                    }
                    break;
            }
            if (val > 60)
            {
                dead = true;
            }
        }

        public List<int> GetSolutionOrNull()
        {
            List<int> find = new List<int>() { 2, 10, 14 };
            foreach (int c in seq)
            {
                if (find[0] == c)
                {
                    find.RemoveAt(0);
                    if (find.Count == 0)
                    {
                        return seq;
                    }
                }
            }
            return null;
        }

        public Display Clone()
        {
            Display cloned = new Display();
            cloned.val = val;
            cloned.dead = dead;
            cloned.seq = new List<int>(seq);
            return cloned;
        }

        public List<Display> GoDeeper()
        {
            List<Display> result = new List<Display>();
            foreach (Operation op in Enum.GetValues(typeof(Operation)))
            {
                Display d = this.Clone();
                d.Apply(op);
                if (!d.dead)
                {
                    result.Add(d);
                }
            }
            return result;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Display d = new Display();
            IEnumerable<Display> latest = new List<Display>() { d };
            while (true)
            {
                var nextLayer = latest.Select(x => x.GoDeeper());
                latest = nextLayer.SelectMany(x => x);
                var possibleSolutions = latest.Select(x => x.GetSolutionOrNull()).Where(x => x != null);
                var solution = possibleSolutions.FirstOrDefault();
                if (solution != null)
                {
                    Console.WriteLine(string.Join(" ", solution));
                    break;
                }
            }
        }
    }
}
