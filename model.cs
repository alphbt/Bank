using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank
{

    class Model
    {
        public Schedule schedule;

        public int currentTime = 0; // in min
        //public int clerksCount;
        //public int maxQueueLen;
        public int newClientTimer;
        public int modelingPeriod; // week -> hours
       // public int modelingStep; // in min
        public int clientNumber;
        public int secondArrivingParamTuple;
        public int currentDay;
        public int dayDuration;

        public int midQueue;
        //public int midQueueCounter;

        public Tuple<int, int> rangeClientsArriving;
        public Tuple<int, int> rangeIncome;
        //public Tuple<int, int> rangeService;

        bool isNormalDistribution;
        bool isUniformDistribution;
        Random rnd = new Random();

        public List<Client> clients = new List<Client>();
        public Bank modelBank;

        public Model(int clerksCount,int maxQueueLen,int modelingPeriod,
            bool isNormalDistribution, bool isUniformDistribution,
            Tuple<int,int> rangeClientsArriving,
            Tuple<int,int> rangeIncome, Tuple<int,int> rangeService,string schedulePath)
        {
            //this.clerksCount = clerksCount;
            //this.maxQueueLen = maxQueueLen;
            this.modelingPeriod = modelingPeriod;  
            //this.modelingStep = modelingStep;

            this.isNormalDistribution = isNormalDistribution;
            this.isUniformDistribution = isUniformDistribution;

            this.rangeClientsArriving = rangeClientsArriving;
            this.rangeIncome = rangeIncome;
            //this.rangeService = rangeService;

            clientNumber = 1;
            newClientTimer = 0;
            currentDay = 0;
            dayDuration = 0;

            midQueue = 0;
            //midQueueCounter = 1;

            secondArrivingParamTuple = rangeClientsArriving.Item2;

            schedule = new Schedule(schedulePath);
            int period = 0;
            for (int i = 0; i < modelingPeriod; i++)
            {
                period += (schedule.monday.Item2.Item1 - schedule.monday.Item1.Item1) * 60 +
                    schedule.monday.Item2.Item2 - schedule.monday.Item1.Item2;
                period += (schedule.tuesday.Item2.Item1 - schedule.tuesday.Item1.Item1) * 60 +
                    schedule.tuesday.Item2.Item2 - schedule.tuesday.Item1.Item2;
                period += (schedule.wednesday.Item2.Item1 - schedule.wednesday.Item1.Item1) * 60 +
                    schedule.wednesday.Item2.Item2 - schedule.wednesday.Item1.Item2;
                period += (schedule.thursday.Item2.Item1 - schedule.thursday.Item1.Item1) * 60 +
                    schedule.thursday.Item2.Item2 - schedule.thursday.Item1.Item2;
                period += (schedule.friday.Item2.Item1 - schedule.friday.Item1.Item1) * 60 +
                    schedule.friday.Item2.Item2 - schedule.friday.Item1.Item2;
                period += (schedule.saturday.Item2.Item1 - schedule.saturday.Item1.Item1) * 60 +
                    schedule.saturday.Item2.Item2 - schedule.saturday.Item1.Item2;

            }
            this.modelingPeriod = period;
            modelBank = new Bank(maxQueueLen, clerksCount, this.rnd,isNormalDistribution,isUniformDistribution,
                rangeClientsArriving,rangeIncome,rangeService);
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
            if (modelingPeriod > 0)
            {
                if (dayDuration > 0)
                {
                    if (newClientTimer <= 0)
                    {
                        clients.Add(new Client(clientNumber++, rnd, isNormalDistribution, isUniformDistribution,
                        rangeClientsArriving, rangeIncome));
                        modelBank.clients.Enqueue(clients.Last());
                        midQueue += modelBank.clients.Count();
                        //midQueueCounter++;

                        if (isNormalDistribution)
                        {
                            newClientTimer = ToNormalDistribution(rnd, (double)(rangeClientsArriving.Item1 + rangeClientsArriving.Item2) / 2,
                            (double)(rangeClientsArriving.Item2 - rangeClientsArriving.Item1) / 2);
                        }
                        if (isUniformDistribution)
                        {
                            newClientTimer = rnd.Next(rangeClientsArriving.Item1, rangeClientsArriving.Item2 + 1);
                        }
                    }
                    else
                    {
                        newClientTimer--;
                    }

                    if (currentTime >= 180 && currentTime <= 360 || currentDay % 7 == 6)
                    {
                        rangeClientsArriving = new Tuple<int, int>(rangeClientsArriving.Item1, Math.Min(rangeClientsArriving.Item1 + 1,
                            rangeClientsArriving.Item2));
                    }
                    else
                    {
                            rangeClientsArriving = new Tuple<int, int>(rangeClientsArriving.Item1, secondArrivingParamTuple);
                    }


                    modelBank.Tick();
                    currentTime++;
                    dayDuration--;
                    modelingPeriod--;

                    
                }
                else
                {
                    currentDay++;
                    currentTime = 0;
                    newClientTimer = 0;
                    midQueue = 0;
                    //midQueueCounter = 1;
                    clientNumber = 1;
                   //modelBank.lostclients = 0;
                    modelBank.income -= Math.Max(0,modelBank.clerks.Count() * 2);
                    modelBank.avaregeWorkTime = 0;
                    
                    while (modelBank.clients.Count() > 0)
                    {
                        Client client = modelBank.clients.Dequeue();
                        if (client.clerk == null)
                            modelBank.loss += client.money;
                        else
                            modelBank.income += client.money;
                    }
                    

                    switch (currentDay % 7)
                    {
                        case 1:
                            dayDuration = (schedule.monday.Item2.Item1 - schedule.monday.Item1.Item1) * 60 +
                                schedule.monday.Item2.Item2 - schedule.monday.Item1.Item2;
                            break;
                        case 2:
                            dayDuration = (schedule.tuesday.Item2.Item1 - schedule.tuesday.Item1.Item1) * 60 +
                                schedule.tuesday.Item2.Item2 - schedule.tuesday.Item1.Item2;
                            break;
                        case 3:
                            dayDuration = (schedule.wednesday.Item2.Item1 - schedule.wednesday.Item1.Item1) * 60 +
                                schedule.wednesday.Item2.Item2 - schedule.wednesday.Item1.Item2;
                            break;
                        case 4:
                            dayDuration = (schedule.thursday.Item2.Item1 - schedule.thursday.Item1.Item1) * 60 +
                                schedule.thursday.Item2.Item2 - schedule.thursday.Item1.Item2;
                            break;
                        case 5:
                            dayDuration = (schedule.friday.Item2.Item1 - schedule.friday.Item1.Item1) * 60 +
                                schedule.friday.Item2.Item2 - schedule.friday.Item1.Item2;
                            break;
                        case 6:
                            dayDuration = (schedule.saturday.Item2.Item1 - schedule.saturday.Item1.Item1) * 60 +
                                schedule.saturday.Item2.Item2 - schedule.saturday.Item1.Item2;
                            break;
                        default: break;
                    }
                }
            }
        }
    }
}
