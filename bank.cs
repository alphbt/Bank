using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    class Bank
    {
        public int income;
        public int loss;
        public int maxQueueLen;
        public int avaregeWorkTime;
        //public int lostclients = 0;


        public Queue<Client> clients = new Queue<Client>();
        public List<Clerk> clerks = new List<Clerk>();

        public Random rnd;
        public bool isNormalDist;
        public bool isUnformDist;
        public Tuple<int, int> serviceRange;

        public Bank(int maxQueueLen, int clerksCount, Random rnd,bool normal, bool uniform,
            Tuple<int,int> arrivingRange, Tuple<int,int> incomeRange, Tuple<int,int> serviceRange)
        {
            income = 0;
            loss = 0;
            avaregeWorkTime = 0;

            this.maxQueueLen = maxQueueLen;
            this.serviceRange = serviceRange;
            this.rnd = rnd;

            isNormalDist = normal;
            isUnformDist = uniform;

            for(int i = 1; i <= clerksCount; i++)
            {
                AddClerk(new Clerk(i));
            }
        }

        public void AddClerk(Clerk clerk)
        {
            clerks.Add(clerk);
        }

        public void Tick()
        {
            if(clients.Count > maxQueueLen)
            {
                while (clients.Count > maxQueueLen)
                {
                    loss += clients.Last().money;
                    loss = Math.Max(0, loss);
                    //lostclients++;
                    clients.Dequeue();
                }
            }

            foreach(Clerk clerk in clerks)
            {
                if(clerk.client == null && clerk.breakTime <= 0 && clients.Count() > 0)
                {
                    clients.Peek().clerk = clerk;
                    clerk.client = clients.Dequeue();
                    clerk.Service(clerk.client, serviceRange,rnd,isNormalDist,isUnformDist); 
                    avaregeWorkTime += clerk.timeWork;
                    income += clerk.client.money;
                }
                else
                {
                    if(clerk.timeWork <= 0 && clerk.breakTime <= 0)
                    {
                        clerk.Break(new Tuple<int, int>(serviceRange.Item1, Math.Min(serviceRange.Item1 + 1, serviceRange.Item2)), rnd,
                            isNormalDist,isUnformDist);
                    }
                }
                clerk.Tick();
            }

            foreach(Client client in clients)
            {
                client.Tick();
            }
        }

    }
}
