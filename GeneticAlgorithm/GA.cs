using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkPigshot
{
    class GA
    {
        List<Cannon> fittest = new List<Cannon>();

        static void Main(string[] args)
        {
            List<Cannon> fittest = runGenerations(100);
           
            foreach (var item in fittest)
            {
                Console.WriteLine(item.distanceFired);
            }

            

            //double XDistance = (2 * Math.Pow(fittest[0].ForceAdjust * pig, 2) * Math.Sin(fittest[0].AngleAdjust * pig) * Math.Cos(fittest[0].AngleAdjust * pig)) / 9.8;

            Console.ReadLine();
        }

        static List<Cannon> runGenerations(int generationCount)
        {
            int fittestCount = 100;
            List<Cannon> fittestParents = new List<Cannon>();

            Family firstGen = new Family();
            firstGen.AddRandomCannons(1000, 1000);
            firstGen.fireAllCannons();
            fittestParents = firstGen.getFittest(fittestCount);

            for (int i = 0; i < generationCount - 1; i++)
            {
                Family Gen = new Family(fittestParents);
                Gen.AddRandomCannons(10, 1000);
                Gen.fireAllCannons();
                fittestParents = Gen.getFittest(fittestCount);
            }
            return fittestParents;
        }
    }
}
