using ActorModel.Distributers;
using ActorModel.Mapping;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkFile
{
    public class WorkPI : IMapping, IReduce, IDistributer
    {
        public MappedAnswer[] Map(object data)
        {
            Thread.Sleep(10);
            Random rand = new Random();
            double throws = 1000000000;
            double hits = 0;

            for (int i = 0; i < throws; i++)
            {
                double x = (rand.NextDouble() * 2.0d) - 1.0d;
                double y = (rand.NextDouble() * 2.0d) - 1.0d;

                if(IsInside(x, y))
                {
                    hits++;
                }
            }

            Console.WriteLine("hits / throws = " + (decimal)(hits/throws));
            double PI = hits / throws;
            double finalPI = 4d * PI;
            Console.WriteLine("the value for pi here is " + finalPI);
            return new MappedAnswer[] { new MappedAnswer() { Key = hits + "", Value = throws } };


            //ReduceResult rr = (ReduceResult)data;
            //string info = (string)rr.values;
            //string[] split = info.Split(' ');
            //List<MappedAnswer> answers = new List<MappedAnswer>();

            //foreach (var item in split)
            //{
            //    if(answers.Exists(x => x.Key == item))
            //    {
            //        MappedAnswer a = answers.Find(x => x.Key == item);
            //        int sub = (int)a.Value;
            //        a.Value = ++sub;
            //    }
            //    else
            //    {
            //        answers.Add(new MappedAnswer(item, 1));
            //    }
            //}
            //int wordCount = 0;
            //foreach (var item in answers)
            //{
            //    wordCount += (int)item.Value;
            //}
            //MappedAnswer ma = new MappedAnswer(null, wordCount);

            //return new MappedAnswer[] { ma };
        }

        private bool IsInside(double x, double y)
        {
            double distance = Math.Sqrt((x * x) + (y * y));

            return (distance < 1d);
        }

        public MappedAnswer[] Reduce(MappedAnswer[] mappedData)
        {
            Console.WriteLine("In the Reduce!");
            double totalHits = 0;
            double totalThrows = 0;
            for (int i = 0; i < mappedData.Length; i++)
            {
                if(mappedData[i] != null)
                {
                    double hits = double.Parse(mappedData[i].Key); 
                    totalHits += hits;
                    totalThrows += (double)mappedData[i].Value;
                }
            }

            Console.WriteLine("adding together hits " + totalHits + " with a total Throw of " + totalThrows);
           // double answer = total / count;
            return new MappedAnswer[] { new MappedAnswer() { Key = "" + totalHits, Value = totalThrows } };
            //Console.WriteLine("Reducing " + mappedData[0].Value);
            //int count = 0;
            //for (int i = 0; i < mappedData.Length; i++)
            //{
            //    if(mappedData[i] != null)
            //    {
            //        count += (int)mappedData[i].Value;
            //    }
            //}
            //return new MappedAnswer[] { new MappedAnswer(count.ToString(), count) };
        }

        public ReduceResult[] Distribute(object obj)
        {
            int amountToSplit = 5;
            ReduceResult[] arrayCount = new ReduceResult[amountToSplit];
            for (int i = 0; i < amountToSplit; i++)
			{
                arrayCount[i] = new ReduceResult();
			}
            return arrayCount;
            //string input = (string)obj;
            //string[] splitInput = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //ReduceResult[] splitWork = new ReduceResult[splitInput.Length];
            //for (int i = 0; i < splitWork.Length; i++)
            //{
            //    splitWork[i] = new ReduceResult(null, splitInput[i]);
            //}
            //return splitWork;
        }
    }
}
