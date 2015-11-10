using ActorModel.Client;
using ActorModel.Reducers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorModel.Mapping;
using System.Threading;
using System.Diagnostics;
using WorkPigshot;

namespace MainProgram
{
    public class ClientTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to Start Client");
            Console.ReadLine();
            var watch = Stopwatch.StartNew();

            PigData pigData = new PigData();
            pigData.distanceToPig = 100;
            pigData.generations = 10;
            pigData.maximumforce = 1000;
            pigData.wallheight = 30;

            //DistributedProcess distributedProcess = new DistributedProcess("localhost", 56000, 55559);
            //distributedProcess.AddFunctionFile(@"..\..\..\WorkPigShot\bin\Release\WorkPigShot.dll")
            //    .addWork((object)pigData);


            DistributedProcess distributedProcess = new DistributedProcess("localhost", 56000, 55559);
            distributedProcess.AddFunctionFile(@"..\..\..\WorkPigShot\bin\Debug\WorkPigShot.dll")
                .addWork((object)new double[] { 100, 10, 1000, 30 });

            //DistributedProcess distributedProcess = new DistributedProcess("localhost", 56000, 55559);
            //distributedProcess.AddFunctionFile(@"..\..\..\WorkFile\bin\Release\WorkFile.dll")
            //    .addWork((string)@".\TestText.txt");

            long IDNumber = distributedProcess.Start();
            Console.WriteLine("Task ID number = " + IDNumber);
            var answer = distributedProcess.GetFinishedWork(IDNumber);

            List<double[]> answerValue = (List<double[]>)answer.values;
            List<Cannon> cannons = new List<Cannon>();
            foreach (var item in answerValue)
            {
                cannons.Add(new Cannon() { Angle = item[0], Force = item[1], minimumForce = item[2], maximumForce = item[3], minimumAngle = item[4], maximumAngle = item[5], legthToPig = item[6], distanceFired = item[7], fitness = item[8], wallHeight = item[9] });
            }

            Cannon bestCannon = cannons.OrderBy(x => x.fitness).FirstOrDefault();


            double angle = bestCannon.Angle;
            double force = bestCannon.Force;

            //double totalHits = double.Parse(answer.Key.ToString());
            //double totalThrows = (double)answer.values;
            //double result = (double)((totalHits / totalThrows) * 4d);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("The force for the shot is " + force);
            Console.WriteLine("The angle for the shot is " + angle * 180 / Math.PI);
            Console.WriteLine("The distance travelled is " + bestCannon.distanceFired);

            //Console.WriteLine("The answer to the mapReduce is for pi is : " + result);
            //Console.WriteLine("Total time elapsed = " + elapsedMs + "ms");
            Console.ReadLine();
        }
    }
}
