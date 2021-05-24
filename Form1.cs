using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace bank
{
    public partial class mainForm : Form
    {
        Model experiment;
        bool isBegin = false;
        int modelingperiod;
       
        List<PictureBox> clients = new List<PictureBox>();
        List<PictureBox> clerks = new List<PictureBox>();

        int chartX = 0;
        double max1 = 0;
        double max2 = 0;
        public mainForm()
        {
            InitializeComponent();
            string[] properties = new string[]
                {
                    "День недели",
                    "Текущее время",
                    "Обслужено клиентов",
                    "Потеряно клиентов",
                    "Средняя занятость клерков",
                    "Средняя длина очереди",
                    "Среднее время ожидания клиента в очереди",
                    "Полученная прибыль",
                    "Потери банка"
                };

            foreach (string str in properties)
            {
                ListViewItem lvi = propertiesLV.Items.Add(str);
                lvi.Name = str;
                lvi.SubItems.Add("");
            }

            

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if(!isBegin)
            {
                if (cashNUD1.Value > cashNUD2.Value)
                {
                    MessageBox.Show("Неправильное значение прибыли клиента. Значение" +
                        " первой ячейки должно быть не больше значения второй ячейки",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    cashNUD1.Focus();
                    //isBegin = false;
                }
                else if (arrivingNUD1.Value > arrivingNUD2.Value)
                {
                    MessageBox.Show("Неправильное значение диапозона прихода клиента. " +
                        "Значение первой ячейки должно быть не больше значения второй ячейки",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    arrivingNUD1.Focus();
                    //isBegin = false;
                }
                else if (serviceNUD1.Value > serviceNUD2.Value)
                {
                    MessageBox.Show("Неправильное значение диапозона обслуживания клиента. " +
                        "Значение первой ячейки должно быть не больше значения второй ячейки",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    serviceNUD1.Focus();
                    //isBegin = false;
                }
                else
                {

                    informtionLV.BeginUpdate();
                    foreach (ListViewItem item in informtionLV.Items)
                    {
                        item.SubItems[1].Text = "";
                    }
                    informtionLV.Items.Clear();
                    informtionLV.EndUpdate();

                    propertiesLV.BeginUpdate();
                    propertiesLV.Items["День недели"].SubItems[1].Text = "";
                    propertiesLV.Items["Текущее время"].SubItems[1].Text = "";
                    propertiesLV.Items["Обслужено клиентов"].SubItems[1].Text = "";
                    propertiesLV.Items["Потеряно клиентов"].SubItems[1].Text = "";
                    propertiesLV.Items["Средняя занятость клерков"].SubItems[1].Text = "";
                    propertiesLV.Items["Средняя длина очереди"].SubItems[1].Text = "";
                    propertiesLV.Items["Среднее время ожидания клиента в очереди"].SubItems[1].Text = "";
                    propertiesLV.Items["Полученная прибыль"].SubItems[1].Text = "";
                    propertiesLV.Items["Потери банка"].SubItems[1].Text = "";
                    propertiesLV.EndUpdate();

                    timer.Interval = 100;
                    for (int i = 1; i <= clerksCountNUD.Value; i++)
                    {
                        ListViewItem lvi = informtionLV.Items.Add(i.ToString());
                        lvi.Name = i.ToString();
                        lvi.SubItems.Add("");
                    }

                    int x = 46;
                    int y = 22;
                    for (int i = 0; i < clerksCountNUD.Value; i++)
                    {
                        PictureBox picture = new PictureBox
                        {
                            //Image image = Image.FromFile("../../cb.jpg"),
                            Name = "cashBox" + i.ToString(),
                            Size = new Size(51, 51),
                            Location = new Point(x, y),
                            Image = Image.FromFile(@"../../cb.png")
                        };
                        panel1.Controls.Add(picture);
                        clerks.Add(picture);
                        x += 70;
                    };



                    /*while (chart1.Series["Прибыль"].Points.Count > 0)
                    {
                        chart1.Series["Прибыль"].Points.RemoveAt(0);
                    }*/

                    chart1.Series["Прибыль"].Points.Clear();


                    /*while (chart1.Series["Потери"].Points.Count > 0)
                    {
                        chart1.Series["Потери"].Points.RemoveAt(0);
                    }*/

                    chart1.Series["Потери"].Points.Clear();

                    while (chart2.Series["Клиентов в момент времени"].Points.Count > 0)
                    {
                        chart2.Series["Клиентов в момент времени"].Points.RemoveAt(0);
                    }

                    while (chart3.Series["Простой клерка"].Points.Count > 0)
                    {
                        chart3.Series["Простой клерка"].Points.RemoveAt(0);
                    }

                    while (chart3.Series["Занятость клерка"].Points.Count > 0)
                    {
                        chart3.Series["Занятость клерка"].Points.RemoveAt(0);
                    }

                    chartX = 0;
                    max1 = 0;
                    max2 = 0;

                    maxQueueLenNUD.Enabled = false;
                    clerksCountNUD.Enabled = false;
                    cashNUD1.Enabled = false;
                    cashNUD2.Enabled = false;
                    arrivingNUD1.Enabled = false;
                    arrivingNUD2.Enabled = false;
                    serviceNUD1.Enabled = false;
                    serviceNUD2.Enabled = false;
                    normalDistrRB.Enabled = false;
                    uniformDistrRB.Enabled = false;
                    modelingStepNUD.Enabled = false;
                    periodNUD.Enabled = false;
                    //startButton.Enabled = false;

                    isBegin = true;
                    timer.Enabled = true;
                    //modelingperiod = (int)periodNUD.Value * 6 * 24 * 60;
                    experiment = new Model((int)clerksCountNUD.Value, (int)maxQueueLenNUD.Value,
                        (int)periodNUD.Value,
                        normalDistrRB.Checked, uniformDistrRB.Checked,
                        new Tuple<int, int>((int)arrivingNUD1.Value, (int)arrivingNUD2.Value),
                        new Tuple<int, int>((int)cashNUD1.Value, (int)cashNUD2.Value),
                        new Tuple<int, int>((int)serviceNUD1.Value, (int)serviceNUD2.Value),
                        @"../../schedule.txt");
                    modelingperiod = experiment.modelingPeriod;

                    System.Windows.Forms.DataVisualization.Charting.Series series3;
                    System.Windows.Forms.DataVisualization.Charting.Series series4;
                    series3 = chart3.Series["Простой клерка"];
                    series4 = chart3.Series["Занятость клерка"];
                    foreach (Clerk clerk in experiment.modelBank.clerks)
                    {
                        series3.Points.AddXY(clerk.clerkNumber, 0);
                        series4.Points.AddXY(clerk.clerkNumber, 0);
                    }
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (modelingperiod > 0 && isBegin)
            {
                for (int i = 0; i < modelingStepNUD.Value; i++)
                    experiment.Tick();

                if (experiment.clients.Count() > 0)
                    showStatistcs();
                experiment.midQueue = 0;

                foreach (Clerk clerk in experiment.modelBank.clerks)
                {
                    if (clerk.client != null)
                    {
                        PictureBox picture = new PictureBox
                        {
                            //Image image = Image.FromFile("../../cb.jpg"),
                            Name = "client" + clerk.clerkNumber.ToString(),
                            Size = panel1.Controls["cashBox" + (clerk.clerkNumber - 1).ToString()].Size,
                            Location = new Point(panel1.Controls["cashBox" + (clerk.clerkNumber - 1).ToString()].Location.X,
                            panel1.Controls["cashBox" + (clerk.clerkNumber - 1).ToString()].Location.Y + 50),
                            Image = Image.FromFile(@"../../human.png")
                        };
                        panel1.Controls.Add(picture);
                        //clerk.picture = picture;
                        clients.Add(picture);
                    }
                    else
                    {
                        PictureBox cl = new PictureBox();
                        foreach (PictureBox client in clients)
                        {
                            if (client.Name.Contains(clerk.clerkNumber.ToString()))
                            {
                                panel1.Controls.Remove(client);
                                cl = client;
                            }
                        }
                        clients.Remove(cl);
                        //panel1.Controls.Remove(clerk.picture);
                        //clerk.picture = null;
                    }
                }

                clientCounerTB.Text = experiment.modelBank.clients.Count().ToString();

                if (experiment.dayDuration == 0)
                {
                    foreach (PictureBox picture in clients)
                    {
                        panel1.Controls.Remove(picture);
                    }
                }
                modelingperiod -= (int)modelingStepNUD.Value;
            }
            else
            {
                isBegin = false;
                timer.Enabled = false;

                maxQueueLenNUD.Enabled = true;
                clerksCountNUD.Enabled = true;
                cashNUD1.Enabled = true;
                cashNUD2.Enabled = true;
                arrivingNUD1.Enabled = true;
                arrivingNUD2.Enabled = true;
                serviceNUD1.Enabled = true;
                serviceNUD2.Enabled = true;
                normalDistrRB.Enabled = true;
                uniformDistrRB.Enabled = true;
                modelingStepNUD.Enabled = true;
                periodNUD.Enabled = true;
                timer.Enabled = false;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            isBegin = false;
            timer.Enabled = false;

            maxQueueLenNUD.Enabled = true;
            clerksCountNUD.Enabled = true;
            cashNUD1.Enabled = true;
            cashNUD2.Enabled = true;
            arrivingNUD1.Enabled = true;
            arrivingNUD2.Enabled = true;
            serviceNUD1.Enabled = true;
            serviceNUD2.Enabled = true;
            normalDistrRB.Enabled = true;
            uniformDistrRB.Enabled = true;
            modelingStepNUD.Enabled = true;
            periodNUD.Enabled = true;
            //startButton.Enabled = true;

            informtionLV.BeginUpdate();
            foreach(ListViewItem item in informtionLV.Items)
            {
                item.SubItems[1].Text = "";
            }
            informtionLV.Items.Clear();
            informtionLV.EndUpdate();

            propertiesLV.BeginUpdate();
            propertiesLV.Items["День недели"].SubItems[1].Text = "";
            propertiesLV.Items["Текущее время"].SubItems[1].Text = "";
            propertiesLV.Items["Обслужено клиентов"].SubItems[1].Text = "";
            propertiesLV.Items["Потеряно клиентов"].SubItems[1].Text = "";
            propertiesLV.Items["Средняя занятость клерков"].SubItems[1].Text = "";
            propertiesLV.Items["Средняя длина очереди"].SubItems[1].Text = "";
            propertiesLV.Items["Среднее время ожидания клиента в очереди"].SubItems[1].Text = "";
            propertiesLV.Items["Полученная прибыль"].SubItems[1].Text = "";
            propertiesLV.Items["Потери банка"].SubItems[1].Text = "";
            propertiesLV.EndUpdate();

            foreach (PictureBox picture in clients)
            {
                panel1.Controls.Remove(picture);
            }

            foreach(PictureBox picture in clerks)
            {
                panel1.Controls.Remove(picture);
            }

            while(chart1.Series["Прибыль"].Points.Count > 0)
            {
                chart1.Series["Прибыль"].Points.RemoveAt(0);
            }

            while (chart1.Series["Потери"].Points.Count > 0)
            {
                chart1.Series["Потери"].Points.RemoveAt(0);
            }

            while (chart2.Series["Клиентов в момент времени"].Points.Count > 0)
            {
                chart2.Series["Клиентов в момент времени"].Points.RemoveAt(0);
            }

            while (chart3.Series["Простой клерка"].Points.Count > 0)
            {
                chart3.Series["Простой клерка"].Points.RemoveAt(0);
            }

            while (chart3.Series["Занятость клерка"].Points.Count > 0)
            {
                chart3.Series["Занятость клерка"].Points.RemoveAt(0);
            }


        }                    

 
        void showStatistcs()
        {
            Tuple<Tuple<int, int>, Tuple<int, int>> workTime = new Tuple<Tuple<int,int>,Tuple<int,int>>
                (new Tuple<int,int>(0,0),new Tuple<int,int>(0,0));
            propertiesLV.BeginUpdate();

            switch(experiment.currentDay % 7)
            {
                case 1:
                    propertiesLV.Items["День недели"].SubItems[1].Text = "Понедельник";
                    workTime = experiment.schedule.monday;
                    break;
                case 2:
                    propertiesLV.Items["День недели"].SubItems[1].Text = "Вторник";
                    workTime = experiment.schedule.tuesday;
                    break;
                case 3:
                    propertiesLV.Items["День недели"].SubItems[1].Text = "Среда";
                    workTime = experiment.schedule.wednesday;
                    break;
                case 4:
                    propertiesLV.Items["День недели"].SubItems[1].Text = "Четверг";
                    workTime = experiment.schedule.thursday;
                    break;
                case 5:
                    propertiesLV.Items["День недели"].SubItems[1].Text = "Пятница";
                    workTime = experiment.schedule.friday;
                    break;
                case 6:
                    propertiesLV.Items["День недели"].SubItems[1].Text = "Суббота";
                    workTime = experiment.schedule.saturday;
                    break;
                default: break;
            }
            
            propertiesLV.Items["Текущее время"].SubItems[1].Text = (workTime.Item1.Item1 + 
                experiment.currentTime/60).ToString() + ":" + (((workTime.Item1.Item2 + experiment.currentTime % 60) / 10 == 0)
                ? "0" : "") +
                (workTime.Item1.Item2 + experiment.currentTime % 60).ToString();

            int clientCount = 0;
            int lostClients = 0;
            int avaregeWaitingTime = 0;
            foreach (Client client in experiment.clients)
            {
                if (client.clerk != null)
                {
                    clientCount++;
                    avaregeWaitingTime += client.waitingTime;
                }
                else
                    lostClients++;
            }

       

            propertiesLV.Items["Обслужено клиентов"].SubItems[1].Text = clientCount.ToString();
            propertiesLV.Items["Потеряно клиентов"].SubItems[1].Text = (lostClients).ToString();


            propertiesLV.Items["Средняя занятость клерков"].SubItems[1].Text = (Math.Round((double)experiment.modelBank.avaregeWorkTime
                 / (int)clerksCountNUD.Value, 2)).ToString();
            experiment.modelBank.avaregeWorkTime = 0;
            propertiesLV.Items["Средняя длина очереди"].SubItems[1].Text = (experiment.midQueue / modelingStepNUD.Value).ToString();
            propertiesLV.Items["Среднее время ожидания клиента в очереди"].SubItems[1].Text = (avaregeWaitingTime /Math.Max(1, clientCount)).ToString();
            propertiesLV.Items["Полученная прибыль"].SubItems[1].Text = (experiment.modelBank.income ).ToString() + " тыс.";
            propertiesLV.Items["Потери банка"].SubItems[1].Text = (experiment.modelBank.loss).ToString() + " тыс.";
            propertiesLV.EndUpdate();

            informtionLV.BeginUpdate();
            foreach(Clerk clerk in experiment.modelBank.clerks)
            {
                if(clerk.client != null)
                {
                    informtionLV.Items[clerk.clerkNumber.ToString()].SubItems[1].Text = clerk.client.clientsNumber.ToString();
                }
                else
                {
                    informtionLV.Items[clerk.clerkNumber.ToString()].SubItems[1].Text = "";
                }
            }
            informtionLV.EndUpdate();


            System.Windows.Forms.DataVisualization.Charting.Series series1;
            System.Windows.Forms.DataVisualization.Charting.Series series2;
            System.Windows.Forms.DataVisualization.Charting.Series series3;
            System.Windows.Forms.DataVisualization.Charting.Series series4;

            series1 = chart1.Series["Прибыль"];
            while (series1.Points.Count > 10)
            {
                series1.Points.RemoveAt(0);
            }
            series1.Points.AddXY(chartX, experiment.modelBank.income);

            series2 = chart2.Series["Клиентов в момент времени"];
            /*if(series2.Points.Count > 10)
            {
                series2.Points.Clear();                
            }*/
            series2.Points.AddXY((workTime.Item1.Item1 +
                experiment.currentTime / 60).ToString() + ":" + (((workTime.Item1.Item2 + experiment.currentTime % 60) / 10 == 0)
                ? "0" : "") +
                (workTime.Item1.Item2 + experiment.currentTime % 60 ).ToString() + " " + propertiesLV.Items["День недели"].SubItems[1].Text, 
                experiment.modelBank.clients.Count());


            series1 = chart1.Series["Потери"];
            while (series1.Points.Count > 10)
            {
                series1.Points.RemoveAt(0);
            }
            series1.Points.AddXY(chartX, experiment.modelBank.loss);

            series3 = chart3.Series["Простой клерка"];
            series4 = chart3.Series["Занятость клерка"];
            foreach (Clerk clerk in experiment.modelBank.clerks)
            {
                series3.Points.AddXY(clerk.clerkNumber,clerk.idleTime);
                series4.Points.AddXY(clerk.clerkNumber, clerk.totalWorkTime);
            }


            chart1.ChartAreas[0].AxisX.Minimum = series1.Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = chartX;
            max1 = Math.Max(max1, experiment.modelBank.income);
            max1 = Math.Max(max1, experiment.modelBank.loss);
            chart1.ChartAreas[0].AxisY.Maximum = max1;
          

            chart1.ChartAreas[0].AxisY.Maximum = max1;
            chart2.ChartAreas[0].AxisX.Minimum = series2.Points[0].XValue;
            chart2.ChartAreas[0].AxisX.Maximum = chartX;
            max2 = Math.Max(max2, experiment.modelBank.clients.Count());
            chart2.ChartAreas[0].AxisY.Maximum = max2;
          
            chartX++;
        }

        private void exitBut_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toTheEndButton_Click(object sender, EventArgs e)
        {
            if (isBegin)
            {
                timer.Enabled = false;
                while (modelingperiod-- > 0)
                {
                    experiment.Tick();
                }
                showStatistcs();
                maxQueueLenNUD.Enabled = true;
                clerksCountNUD.Enabled = true;
                cashNUD1.Enabled = true;
                cashNUD2.Enabled = true;
                arrivingNUD1.Enabled = true;
                arrivingNUD2.Enabled = true;
                serviceNUD1.Enabled = true;
                serviceNUD2.Enabled = true;
                normalDistrRB.Enabled = true;
                uniformDistrRB.Enabled = true;
                modelingStepNUD.Enabled = true;
                periodNUD.Enabled = true;
                isBegin = false;
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            this.Size = SystemInformation.PrimaryMonitorSize;

        }

        private void butStop_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(isBegin)
                timer.Enabled = true;
        }
    }
}

                    