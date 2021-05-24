using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    class Client
    {
        public int clientsNumber;
        public int money;
        //public int arrivingTime;
        public int waitingTime;

        public Clerk clerk = null;

        public Client(int clientNum,Random rnd, bool normal,bool uniform,
            Tuple<int, int> arrivingRange, Tuple<int, int> incomeRange)
        {
            clientsNumber = clientNum;
            waitingTime = 0;

            if (normal)
            {
                //arrivingTime = ToNormalDistribution(rnd, (double)(arrivingRange.Item1 + arrivingRange.Item2) / 2,
                    //(double)(arrivingRange.Item2 - arrivingRange.Item1) / 2);
                money = ToNormalDistribution(rnd, (double)(incomeRange.Item1 + incomeRange.Item2) / 2,
                    (double)(incomeRange.Item2 - incomeRange.Item1) / 2);

            }

            if(uniform)
            {
                //arrivingTime = rnd.Next(arrivingRange.Item1,arrivingRange.Item2 + 1);
                money = rnd.Next(incomeRange.Item1, incomeRange.Item2 + 1);
            }
        }

        int ToNormalDistribution(Random rnd, double m, double sigma)
        {
            double u1 = 1.0 - rnd.NextDouble();
            double u2 = 1.0 - rnd.NextDouble();
            double randStdNorm = Math.Sqrt(-2.0 * Math.Log(u1)) *
                             Math.Sin(2.0 * Math.PI * u2);
            double randNormal = m + sigma * randStdNorm;
            return (int)Math.Truncate(randNormal);
        }


        public void Tick()
        {
            if(clerk == null)
            {
                waitingTime++;
            }
        }

    }
}
