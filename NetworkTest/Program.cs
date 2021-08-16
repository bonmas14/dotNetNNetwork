﻿using System;
using NerualNetwork;
// Copyright (c) 2020 BonMAS14
namespace NetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // создание экземпляра функции активации
            Sigmoid sigmoid = new Sigmoid();

            // создание набора данных
            double[][] pack = new double[5][];

            pack[0] = new double[] { 0.5, 0 };
            pack[1] = new double[] { 0, 0.7 };
            pack[2] = new double[] { 0, 0   };
            pack[3] = new double[] { 0.2, 0 };
            pack[4] = new double[] { 1, 0.4 };
            
            // создание набора для обучения
            double[][] learn = new double[5][];

            learn[0] = new double[] { 1, 0 };
            learn[1] = new double[] { 0, 1 };
            learn[2] = new double[] { 0, 0 };
            learn[3] = new double[] { 0, 0 };
            learn[4] = new double[] { 1, 0 };

            // создание сети
            NNetwork network = new NNetwork(sigmoid, new int[] { 2, 4, 2 });

            for (int i = 0; i < 4000; i++)
            {
                for (int j = 0; j < pack.Length; j++)
                {
                    network.SetData(pack[j], true);
                    
                    network.UpdateNeurons();
                    
                    network.Learn(learn[j]);
                }
            }
            network.SetData(new double[] { 0.5, 0 }, true);

            network.UpdateNeurons();

            var data = network.GetOutputData();

            foreach (var item in data)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();

            Console.WriteLine("Save Load Test");

            NNetworkSaver saver = new NNetworkSaver(network);

            saver.SaveNetwork("network.nwk");

            NNetworkLoader loader = new NNetworkLoader("network.nwk");

            network = null;

            network = loader.LoadNNetwork(sigmoid);

            network.SetData(new double[] { 0.5, 0.0 }, true);

            network.UpdateNeurons();

            data = network.GetOutputData();

            foreach (var item in data)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
