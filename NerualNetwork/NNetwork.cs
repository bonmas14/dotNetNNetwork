using NerualNetwork.Neurons;
using System;
using System.Collections.Generic;

// Copyright (c) 2020 BonMAS14
namespace NerualNetwork
{
    public sealed class NNetwork
    {
        public double LearnSpeed { get; set; } = 0.1;

        private List<Neuron>[] layers;

        private int[] networkMaket;

        /// <summary>
        /// Констуктор сети
        /// </summary>
        /// <param name="function"> Используемая функция активации, стандартная - Sigmoid</param>
        /// <param name="network"> Макет сети, количество слоёв это длина массива и тд</param>
        public NNetwork(IFunction function, int[] network)
        {
            networkMaket = (int[])network.Clone();

            Creator creator = new Creator();

            var maket = creator.CreateMaket(network);

            layers = creator.CreateNetwork(maket, function);
        }

        public NNetwork(IFunction function, string path)
        {

        }

        public double[] GetWeightsFromNeuron(int layer, int index)
        {
            if (layers[layer][index].GetType() == typeof(HiddenNeuron))
            {
                var neuron = (HiddenNeuron)layers[layer][index];
            }
            else
            {
                return new double[0];
            }
        }

        public int[] GetMaket()
        {
            return (int[])networkMaket.Clone();
        }

        public void SetData(double[] inputData, bool sendToActivationFunc)
        {
            int inputLayer = 0;

            for (int i = 0; i < layers[inputLayer].Count - 1; i++)
            {
                if (sendToActivationFunc)
                {
                    var function = layers[inputLayer][i].function;

                    layers[inputLayer][i].Output = function.ActivationFunc(inputData[i]);
                }
                else
                {
                    layers[inputLayer][i].Output = inputData[i];
                }
            }
        }
        
        public void UpdateNeurons()
        {
            for (int i = 1; i < layers.Length; i++)
            {
                for (int j = 0; j < layers[i].Count; j++)
                {
                    layers[i][j].UpdateData(layers[i - 1]);
                }
            }
        }

        public double[] GetOutputData()
        {
            int lastLayer = layers.Length - 1;

            double[] output = new double[layers[lastLayer].Count];

            for (int i = 0; i < layers[lastLayer].Count; i++)
            {
                output[i] = layers[lastLayer][i].Output;
            }

            return output;
        }

        public void Learn(double[] LearnSet)
        {
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].Error = LearnSet[j] - layers[i][j].Output;
                    }
                }
                else
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].GetError(j, layers[i + 1]);
                    }
                }
            }

            CorrectWeights();
        }

        private void CorrectWeights()
        {
            for (int i = layers.Length - 1; i >= 1; i--)
            {
                for (int j = 0; j < layers[i].Count; j++)
                {
                    layers[i][j].CorrectWeigts(layers[i - 1], LearnSpeed);
                }
            }
        }
    }
}
