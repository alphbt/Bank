using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    class Schedule
    {
        public Tuple<Tuple<int, int>, Tuple<int, int>> monday;
        public Tuple<Tuple<int, int>, Tuple<int, int>> tuesday;
        public Tuple<Tuple<int, int>, Tuple<int, int>> wednesday;
        public Tuple<Tuple<int, int>, Tuple<int, int>> thursday;
        public Tuple<Tuple<int, int>, Tuple<int, int>> friday;
        public Tuple<Tuple<int, int>, Tuple<int, int>> saturday;

        public Schedule(string filePath)
        {
            string[] daysInformation = System.IO.File.ReadAllLines(filePath);
            foreach(string str in daysInformation)
            {
                char[] separators = new char[] { ' ', '.', '-', ':' };
                string[] words = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                switch (words[0])
                {
                    case "Понедельник":
                         monday = new Tuple<Tuple<int, int>, Tuple<int, int>>
                            (new Tuple<int,int>(Int32.Parse(words[1]), Int32.Parse(words[2])),
                            (new Tuple<int,int>(Int32.Parse(words[3]), Int32.Parse(words[4]))));
                        break;
                    case "Вторник":
                        tuesday = new Tuple<Tuple<int, int>, Tuple<int, int>>
                            (new Tuple<int, int>(Int32.Parse(words[1]), Int32.Parse(words[2])),
                            (new Tuple<int, int>(Int32.Parse(words[3]), Int32.Parse(words[4]))));
                        break;
                    case "Среда":
                        wednesday = new Tuple<Tuple<int, int>, Tuple<int, int>>
                            (new Tuple<int, int>(Int32.Parse(words[1]), Int32.Parse(words[2])),
                            (new Tuple<int, int>(Int32.Parse(words[3]), Int32.Parse(words[4]))));
                        break;
                    case "Четверг":
                        thursday = new Tuple<Tuple<int, int>, Tuple<int, int>>
                            (new Tuple<int, int>(Int32.Parse(words[1]), Int32.Parse(words[2])),
                            (new Tuple<int, int>(Int32.Parse(words[3]), Int32.Parse(words[4]))));
                        break;
                    case "Пятница":
                        friday = new Tuple<Tuple<int, int>, Tuple<int, int>>
                            (new Tuple<int, int>(Int32.Parse(words[1]), Int32.Parse(words[2])),
                            (new Tuple<int, int>(Int32.Parse(words[3]), Int32.Parse(words[4]))));
                        break;
                    case "Суббота":
                        saturday = new Tuple<Tuple<int, int>, Tuple<int, int>>
                            (new Tuple<int, int>(Int32.Parse(words[1]), Int32.Parse(words[2])),
                            (new Tuple<int, int>(Int32.Parse(words[3]), Int32.Parse(words[4]))));
                        break;
                    default: break ;
                }
                
            }
        }
    }
}
