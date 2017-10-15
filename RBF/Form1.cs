using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace RBF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int TrainingDataAmount = 21;
        int TestingDataAmount;
        Stack pub = new Stack();
        Stack pub_w = new Stack();
        decimal LearningRate = (decimal)0.1;
        double x;
        Random crandom = new Random();
        decimal desirerror;

        private void button1_Click(object sender, EventArgs e)
        {
            decimal[,] training_set = new decimal[2, TrainingDataAmount];
            for (decimal i = 0; i <= 4; i += (decimal)0.2)
            {
                training_set[0, (int)(i * 5)] = i;
                training_set[1, (int)(i * 5)] = (decimal)Math.Exp(-(double)i) * (decimal)Math.Sin((double)i * (double)3);

            }
            pub.Push(training_set);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            decimal[,] TrainingSet = (decimal[,])pub.Pop();
            decimal[] C = new decimal[TrainingDataAmount];
            decimal[] Phi_C = new decimal[TrainingDataAmount];
            decimal[] W = new decimal[TrainingDataAmount];
            decimal y;
            decimal[] sigma = new decimal[TrainingDataAmount];
            decimal error;
            decimal J_error;
            decimal[] delta_w = new decimal[TrainingDataAmount]; 
            decimal[] delta_c = new decimal[TrainingDataAmount];
            decimal[] delta_sigma = new decimal[TrainingDataAmount];
            decimal SumSquareError = 100;
            int epochs = 0;
            desirerror = Convert.ToDecimal( textBox2.Text);


            for (int i = 0; i < TrainingDataAmount; i++) {
                sigma[i] = 4 / (decimal)Math.Sqrt((double)TrainingDataAmount);
            }

            for (int i = 0; i < TrainingDataAmount; i++) {
                C[i] = TrainingSet[0, i];
                W[i] = (decimal)crandom.NextDouble() * 2 - 1;
            }
            int times = 10000;
            //for(int s = 0; s < times; s++) {
            while (SumSquareError > desirerror) {
                SumSquareError = 0;
                for (int i = 0; i < TrainingDataAmount; i++)
                {
                    for (int j = 0; j < TrainingDataAmount; j++)
                    {
                        Phi_C[j] = (decimal)(Math.Pow((double)(TrainingSet[0, i] - C[j]), 2) / Math.Pow((double)sigma[j], 2));
                        Phi_C[j] = (decimal)Math.Exp(-(double)Phi_C[j]);
                    }

                    y = 0;
                    for(int j = 0; j < TrainingDataAmount; j++)
                        y += Phi_C[j] * W[j];

                    error = TrainingSet[1, i] - y;
                    SumSquareError += error * error;
                    J_error = (decimal)Math.Pow((double)error, 2) / 2;

                    for (int j = 0; j < TrainingDataAmount; j++) {
                        delta_w[j] = LearningRate * error * Phi_C[j];
                        delta_c[j] = LearningRate * (error * W[j] / (decimal)Math.Pow((double)sigma[j], 2)) * Phi_C[j] * (TrainingSet[0, i] - C[j]);
                        delta_sigma[j] = LearningRate * (error * W[j] / (decimal)Math.Pow((double)sigma[j], 3)) * Phi_C[j] * (decimal)(Math.Pow((double)(TrainingSet[0, i] - C[j]), 2));
                        W[j] = W[j] + delta_w[j];
                        C[j] = C[j] + delta_c[j];
                        sigma[j] = sigma[j] + delta_sigma[j];
                    }
                }
                epochs++;
            }
            label1.Text = "TRAINING SUCCESS!";
            textBox1.Text = "epochs:" + Convert.ToString(epochs) + "\n\r" +
                            "error:" + Convert.ToString(SumSquareError);
            pub_w.Push(W);
            pub_w.Push(C);
            pub_w.Push(sigma);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            decimal[] C = new decimal[TrainingDataAmount];
            decimal[] Phi_C = new decimal[TrainingDataAmount];
            decimal[] W = new decimal[TrainingDataAmount];
            decimal y;
            decimal[] sigma = new decimal[TrainingDataAmount];
            sigma = (decimal[])pub_w.Pop();
            C = (decimal[])pub_w.Pop();
            W = (decimal[])pub_w.Pop();

            TestingDataAmount = (int)(4 / 0.01) + 1;
            decimal[] TestingSet = new decimal[TestingDataAmount];
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\ATai\Desktop\RBF.txt");
            string lines;
            for (decimal i = 0; i <= 4; i += 0.01M)
                TestingSet[(int)(i * 100)] = i;

            for (int i = 0; i < TestingDataAmount; i++)
            {
                for (int j = 0; j < TrainingDataAmount; j++)
                {
                    Phi_C[j] = (decimal)(Math.Pow((double)(TestingSet[i] - C[j]), 2) / Math.Pow((double)sigma[j], 2));
                    Phi_C[j] = (decimal)Math.Exp(-(double)Phi_C[j]);
                }

                y = 0;
                for (int j = 0; j < TrainingDataAmount; j++)
                    y += Phi_C[j] * W[j];

                lines = Convert.ToString(y);
                file.WriteLine(lines);
            }
            file.Close();
            label1.Text = "OUTPUT SUCCESS!";

        }
    }
}
