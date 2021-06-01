using System.Collections.Generic;
// Copyright (c) 2020 BonMAS14
namespace NerualNetwork
{
    public sealed class NNetwork
    {
        public double LearnSpeed { get; set; } = 0.1;

        private List<Neuron>[] layers;

        /// <summary>
        /// Констуктор сети
        /// </summary>
        /// <param name="function"> Используемая функция активации, стандартная - Sigmoid</param>
        /// <param name="network"> Макет сети, количество слоёв это длина массива и тд</param>
        public NNetwork(IFunction function, int[] network)
        {
            var maket = Maket.CreateMaket(network);

            layers = Maket.CreateNetwork(maket, function);
        }

        public void SendData(double[] inputData, bool sendToActivationFunc)
        {
            int inputLayer = 0;

            for (int i = 0; i < layers[inputLayer].Count; i++)
            {
                if (layers[inputLayer][i].NeruonType == NeuronType.Bias) continue;

                layers[inputLayer][i].LoadData(inputData[i], sendToActivationFunc);
            }
        }
        
        public void UpdateDataInNeurons()
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0) continue;

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
                        layers[i][j].GetError(LearnSet[j]);
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
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i != 0)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].CorrectWeigts(layers[i - 1], LearnSpeed);
                    }
                }
            }
        }
    }
}
