using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkPigshot
{
    [Serializable]
    class Cannon
    {
        public static Random rand = new Random();
        public double Angle;
        public double Force;
        public double minimumForce;
        public double maximumForce;
        public double minimumAngle;
        public double maximumAngle;
        public double legthToPig = 100;
        public double distanceFired = 0;
        public double fitness = 0;
        public double wallHeight = 0;

        public Cannon()
        {
            this.minimumAngle = 1 * Math.PI / 180;
            this.maximumAngle = 90 * Math.PI / 180;
            this.minimumForce = 10;
            this.maximumForce = 100;
            this.Force = (rand.NextDouble() * (maximumForce - minimumForce)) + minimumForce;

            double angle = (rand.NextDouble() * (maximumAngle - minimumAngle)) + minimumAngle;
            this.Angle = angle;
        }

        public Cannon(double maximumForce, double minimumForce, double maximumAngle, double minimumAngle, double wallHeight)
        {
            this.wallHeight = wallHeight;
            this.minimumAngle = minimumAngle;
            this.maximumAngle = maximumAngle;
            this.minimumForce = minimumForce;
            this.maximumForce = maximumForce;
            this.Force = (rand.NextDouble() * (maximumForce - minimumForce)) + minimumForce;
           
            double angle = (rand.NextDouble() * (maximumAngle - minimumAngle)) + minimumAngle;
            this.Angle = angle;
        }

        public void Fire()
        {
            double gravity = 9.8;
            double XDistance = (2 * Math.Pow(Force, 2) * Math.Sin(Angle) * Math.Cos(Angle)) / gravity;
            this.distanceFired = XDistance;
            if(XDistance >= legthToPig)
            {
                this.maximumAngle = Angle;
                this.maximumForce = Force;
            }
            else
            {
                this.minimumAngle = Angle;
                this.minimumForce = Force;
            }
            this.getFitness();
        }

        public double getFitness()
        {
            double baseValue = distanceFired < legthToPig ? distanceFired / legthToPig : legthToPig / distanceFired;
            double answer = Math.Abs(1 - baseValue);
            this.fitness = answer;
            return answer;
        }
    }
}
