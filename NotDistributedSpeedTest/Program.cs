using ActorModel.Distributers;
using ActorModel.Mapping;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NotDistributedSpeedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();


           // string FunctionFile = @"..\..\..\MainProgram\bin\Debug\TestText.txt";
            string workFile = "";
            string DLLFileLoc = @"..\..\..\WorkFile\bin\Debug\WorkFile.dll";

            byte[] DLL = File.ReadAllBytes(DLLFileLoc);

            //if (FunctionFile != null && File.Exists(FunctionFile))
            //{
            //    workFile = File.ReadAllText(FunctionFile);
            //}

            DateTime startTime = DateTime.Now;

            ReduceResult[] distributedWork;
            MappedAnswer[] mappedWork;
            MappedAnswer answer;

            Assembly assembly = Assembly.Load(DLL);
            var types = assembly.GetTypes();
            IDistributer instanceOfMyType = (IDistributer)Activator.CreateInstance(types[0]);
            distributedWork = instanceOfMyType.Distribute(workFile);

            Console.WriteLine(distributedWork.Length + " lines");

            List<MappedAnswer> mappedTotal = new List<MappedAnswer>();

            foreach (var item in distributedWork)
            {
                Assembly assemblyMap = Assembly.Load(DLL);
                var types2 = assemblyMap.GetTypes();
                IMapping instanceOfMyTypeMap = (IMapping)Activator.CreateInstance(types2[0]);
                mappedWork = instanceOfMyTypeMap.Map(item);
                foreach (var map in mappedWork)
                {
                    mappedTotal.Add(map);
                }
            }

            Assembly assemblyReduce = Assembly.Load(DLL);
            var types3 = assembly.GetTypes();
            IReduce instanceOfMyTypeReduce = (IReduce)Activator.CreateInstance(types3[0]);
            answer = instanceOfMyTypeReduce.Reduce(mappedTotal.ToArray()).FirstOrDefault();

            //string[] split = workFile.Split(' ');

            //Dictionary<string, int> coll = new Dictionary<string, int>();
            //foreach (var item in split)
            //{
            //    if(coll.ContainsKey(item))
            //    {
            //        coll[item]++;
            //    }
            //    else
            //    {
            //        coll.Add(item, 1);
            //    }
            //}

            //int count = 0;
            //foreach (var item in coll)
            //{
            //    count += item.Value;
            //}
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("This document had " + answer.Value + " words");
            Console.WriteLine("This processed it in " + elapsedMs + "ms");
            Console.ReadLine();
        }
    }
}
