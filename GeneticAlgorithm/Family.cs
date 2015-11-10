using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPigshot
{
    class Family
    {
        List<Cannon> cannons;
        private static Random rand = new Random();

        public Family()
        {
            this.cannons = new List<Cannon>();
        }

        public Family(List<Cannon> previousGen) : this()
        {
            for (int i = 0; i < previousGen.Count(); i+=2)
            {
                Cannon[] parents = new Cannon[] { previousGen[i], previousGen[i + 1] };

                Cannon child1 = this.createRandomChild(parents);
                Cannon child2 = this.createRandomChild(parents);
                this.cannons.Add(child1);
                this.cannons.Add(child2);
            }
        }

        private Cannon createRandomChild(Cannon[] parents)
        {
            int maxAngleParent = rand.Next(2);
            int maxForceParent = rand.Next(2);
            int minAngleParent = rand.Next(2);
            int minForceParent = rand.Next(2);
            Cannon child = new Cannon(parents[maxForceParent].maximumForce, parents[minForceParent].minimumForce, parents[maxAngleParent].maximumAngle, parents[minAngleParent].minimumAngle);
            return child;
        }

        public void AddRandomCannons(int count, double maximumForce)
        {
            for (int i = 0; i < count; i++)
            {
                this.cannons.Add(new Cannon(maximumForce, 1, Math.PI / 180 * 90, Math.PI / 180 * 1));
            }
        }

        public void fireAllCannons()
        {
            foreach (var item in cannons)
            {
                item.Fire(10);
            }
        }

        public List<Cannon> getFittest(int amount)
        {
            List<Cannon> fittest = cannons.OrderBy(x => x.fitness).Take(amount).ToList();
            return fittest;
        }
    }
}
