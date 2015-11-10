using ActorModel.Distributers;
using ActorModel.Mapping;
using ActorModel.Reducers;
using WorkPigshot;
using MainProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPigShot
{
    class PigShot : IMapping, IReduce, IDistributer
    {

        public ReduceResult[] Distribute(object obj)
        {
            Console.WriteLine("distributing data................");
            double[] pigDataArray = (double[])obj;
            PigData pigData = new PigData() { distanceToPig = pigDataArray[0], generations = (int)pigDataArray[1], maximumforce = pigDataArray[2], wallheight = pigDataArray[3] };
            ReduceResult[] results = new ReduceResult[pigData.generations];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new ReduceResult("", obj);
            }
            Console.WriteLine("Broke apart the data.");
            return results;
        }

        public MappedAnswer[] Map(object data)
        {
            ReduceResult SomeData = (ReduceResult)data;
            double[] pigDataArray = (double[])SomeData.values;
            PigData pigData = new PigData() { distanceToPig = pigDataArray[0], generations = (int)pigDataArray[1], maximumforce = pigDataArray[2], wallheight = pigDataArray[2] };
            Family generation = new Family(pigData.wallheight);
            generation.AddRandomCannons(500, pigData.maximumforce);
            generation.fireAllCannons();
            Cannon[] answer = generation.getFittest(100).ToArray();
            List<double[]> cannonConversion = new List<double[]>();
            Console.WriteLine("Best is " + answer[0].distanceFired + " for " + pigData.distanceToPig);
            for (int i = 0; i < 100; i++)
            {
                cannonConversion.Add(new double[] { answer[i].Angle, answer[i].Force, answer[i].minimumForce, answer[i].maximumForce, answer[i].minimumAngle, answer[i].maximumAngle, answer[i].legthToPig, answer[i].distanceFired, answer[i].fitness, answer[i].wallHeight});
            }
            MappedAnswer[] ma = new MappedAnswer[1];
            ma[0] = new MappedAnswer("", cannonConversion);
            return ma;
        }

        public MappedAnswer[] Reduce(MappedAnswer[] mappedData)
        {
            Console.WriteLine("In the reduce");
            MappedAnswer[] ma = null;
            if(null != mappedData[0] && null != mappedData[1])
            {
                List<double[]> data1 = (List<double[]>)mappedData[0].Value;
                List<Cannon> family1 = new List<Cannon>();
                foreach (var item in data1)
                {
                    family1.Add(new Cannon() { Angle = item[0], Force = item[1], minimumForce = item[2], maximumForce = item[3], minimumAngle = item[4], maximumAngle = item[5], legthToPig = item[6], wallHeight = item[9]});
                }
                List<double[]> data2 = (List<double[]>)mappedData[1].Value;
                List<Cannon> family2 = new List<Cannon>();
                foreach (var item in data2)
                {
                    family2.Add(new Cannon() { Angle = item[0], Force = item[1], minimumForce = item[2], maximumForce = item[3], minimumAngle = item[4], maximumAngle = item[5], legthToPig = item[6], wallHeight = item[9] });
                }

                List<Cannon> combinedFamily = new List<Cannon>();

                for (int i = 0; i < family1.Count(); i++)
                {
                    combinedFamily.Add(family1[i]);
                    combinedFamily.Add(family2[i]);
                }

                Family newGeneration = new Family(combinedFamily, family1[0].wallHeight);
                newGeneration.AddRandomCannons(10, 2000);
                newGeneration.fireAllCannons();
                Cannon[] answer = newGeneration.getFittest(100).ToArray();
                List<double[]> cannonConversion = new List<double[]>();
                
                for (int i = 0; i < answer.Length; i++)
                {
                    cannonConversion.Add(new double[] { answer[i].Angle, answer[i].Force, answer[i].minimumForce, answer[i].maximumForce, answer[i].minimumAngle, answer[i].maximumAngle, answer[i].legthToPig, answer[i].distanceFired, answer[i].fitness, answer[i].wallHeight });
                }
                Console.WriteLine("Best is " + answer[0].distanceFired + " for " + answer[0].legthToPig);
                ma = new MappedAnswer[1];
                ma[0] = new MappedAnswer("", cannonConversion);
            }
            return ma;
        }
    }
}
