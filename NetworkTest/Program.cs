using System;
using System.Threading.Tasks;
using NerualNetwork;
using NerualNetwork.Logging;
// Copyright (c) 2020 BonMAS14
namespace NetworkTest
{
    class Program
    {

        static NNetwork network;
        static void Main(string[] args)
        {
            // создание экземпляра функции активации
            Sigmoid sigmoid = new Sigmoid();

            network = new NNetwork(sigmoid, new int[] { 2, 4, 2 });

            TrainNetwork();

            TestNetwork();

            Console.WriteLine();

            Console.WriteLine("Save Load Test");

            NNetworkSaver saver = new NNetworkSaver(network);

            saver.SaveNetwork("network.nwk");

            NNetworkLoader loader = new NNetworkLoader("network.nwk", new ConsoleLogger());

            network = loader.LoadNNetwork(sigmoid);

            TestNetwork();

            Console.ReadLine();
        }

        private static void TrainNetwork()
        {
            // создание набора данных
            double[][] pack = new double[5][];

            pack[0] = new double[] { 0.5, 0.4 };
            pack[1] = new double[] { 0.1, 0.7 };
            pack[2] = new double[] { 0, 0 };
            pack[3] = new double[] { 0.2, 0 };
            pack[4] = new double[] { 1, 0.4 };

            // создание набора для обучения
            double[][] learn = new double[5][];

            learn[0] = new double[] { 0.9, 0.1 };
            learn[1] = new double[] { 0.1, 0.9 };
            learn[2] = new double[] { 0.1, 0.1 };
            learn[3] = new double[] { 0.1, 0.1 };
            learn[4] = new double[] { 0.9, 0.1 };

            double error = double.MaxValue;

            int k = 0;

            while (error > 0.1)
            {
                error = 0;

                for (int j = 0; j < pack.Length; j++)
                {
                    network.SetData(pack[j], true);

                    network.UpdateNeurons();

                    network.Learn(learn[j]);

                    error += network.GetNetworkError();

                }

                error /= pack.Length;

                Console.WriteLine($"{k++} - {error}");
            }
        }

        static void TestNetwork()
        {
            network.SetData(new double[] { 1, 0 }, true);

            network.UpdateNeurons();

            var data = network.GetOutputData();

            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        }
    }
}
