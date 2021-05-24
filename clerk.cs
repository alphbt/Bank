using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace bank
{
    class Clerk
    {
        public int clerkNumber;
        public int timeWork;
        public int breakTime;
        //public bool isUniform;
        //public bool isNormal;
        public int idleTime;
        public int totalWorkTime;
        //public PictureBox picture = null;

        public Client client = null;

        public Clerk(int clerkNumber)
        {
            timeWork = 0;
            breakTime = 0;
            idleTime = 0;
            totalWorkTime = 0;
            this.clerkNumber = clerkNumber;
            //isNormal = normal;
            //isUniform = uniform;
        }

        public void Service(Client client, Tuple<int, int> rangeService, Random rnd,
            bool normal, bool uniform)
        {
            this.client = client;
            client.clerk = this;

            if(normal)
            {
                timeWork = ToNormalDistribution(rnd, (double)(rangeService.Item1 + rangeService.Item2) / 2,
                    (double)(rangeService.Item2 - rangeService.Item1) / 2);
            }

            if(uniform)
            {
                timeWork = rnd.Next(rangeService.Item1, rangeService.Item2 + 1);
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

        public void Break(Tuple<int, int> rangeService, Random rnd,
            bool normal, bool uniform)
        {
            this.client = null;

            if (normal)
            {
                breakTime = ToNormalDistribution(rnd, (double)(rangeService.Item1 + rangeService.Item2) / 2,
                    (double)(rangeService.Item2 - rangeService.Item1) / 2);
            }

            if (uniform)
            {
                breakTime = rnd.Next(rangeService.Item1, rangeService.Item2 + 1);
            }

        }

        public void Tick() // 1 min it is min of time
        {
            if (timeWork > 0)
            {
                timeWork--;
                totalWorkTime++;
            }
            else
            {
                if (breakTime > 0)
                {
                    breakTime--;
                    idleTime++;
                }
            }
        }

    }
}
