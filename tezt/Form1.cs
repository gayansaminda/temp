using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tezt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] arr = File.ReadAllLines(@"E:\Devolopment\Jim\data.csv");

            string[] subArr = arr[0].Split(',');

            foreach (string str in subArr)
            {
                try
                {
                    frequencyList.Add(int.Parse(str));
                }
                catch 
                {
                                       
                }
            }

            for (int i = 1; i < arr.Length; i++)
            {
                subArr = arr[i].Split(',');
                try
                {
                    modulationList.Add(double.Parse(subArr[0]));
                   dataList.Add(double.Parse(subArr[1]));

                }
                catch 
                {
                   
                }


            }


            Thread th = new Thread(AutoCalibrate);
            th.Start();
        }


        List<double> frequencyList = new List<double>();
        List<double> modulationList = new List<double>();

        List<double> dataList = new List<double>();


        bool firstFrequencyOnly = true;
        int dontStopBefore = 50;

        private void AutoCalibrate()
        {
            Thread.Sleep(1000);
                  

            for (int i = 0; i < frequencyList.Count; i++)
            {
                float maxAvgPower = 0;
                List<float> angValueList = new List<float>();
                float prewAvaragePower = 0;

                for (int j = 0; j < modulationList.Count; j++)
                {
                    float averagePower = (float)GetAverageEnergyRefined(j);

                    maxAvgPower = Math.Max(averagePower, maxAvgPower);

                    bool beakable = true;
                    if ((!firstFrequencyOnly) || (firstFrequencyOnly && (i == 0)))
                    {
                        if (i < (modulationList.Count * dontStopBefore) / 100f)
                        {
                            beakable = false;
                        }
                    }

                    if (beakable)
                    {
                        if ((maxAvgPower > 0) && (averagePower < prewAvaragePower) && (averagePower < (maxAvgPower * 90 / 100)))
                        {
                            break;
                        }
                    }

                    //if ((averagePower < prewAvaragePower) || (averagePower > expectedPower))
                    //{
                    //    break;
                    //}

                    prewAvaragePower = averagePower;

                    angValueList.Add(averagePower);
                    
                }
              

            }            
        }

        private double GetAverageEnergyRefined(int j)
        {
            return dataList[j];
        }

           

    }
}
