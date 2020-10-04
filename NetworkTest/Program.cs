using System;
using NerualNetwork;

namespace NetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Sigmoid sigmoid = new Sigmoid();

            double[][] pack = new double[5][];

            pack[0] = new double[] { 0.5, 0 };
            pack[1] = new double[] { 0, 0.7 };
            pack[2] = new double[] { 0, 0 };
            pack[3] = new double[] { 0.2, 0 };
            pack[4] = new double[] { 1, 0.4 };
            
            double[][] learn = new double[5][];

            learn[0] = new double[] { 1, 0 };
            learn[1] = new double[] { 0, 1 };
            learn[2] = new double[] { 0, 0 };
            learn[3] = new double[] { 0, 0 };
            learn[4] = new double[] { 1, 0 };

            NNetwork network = new NNetwork(sigmoid, new int[] { 2, 4, 2 });

            for (int i = 0; i < 2000; i++)
            {
                for (int j = 0; j < pack.Length; j++)
                {
                    network.SendData(pack[j], true);
                    network.UpdateData();
                    network.Learn(learn[j]);
                }
            }

            network.SendData(new double[] { 0.5, 0 }, true);

            network.UpdateData();

            var data = network.GetOutput();


            foreach (var item in data)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
