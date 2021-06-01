using System;
using NerualNetwork.Neurons;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork
{
    internal static class Creator
    {
        public static NeuronType[][] CreateMaket(int[] network)
        {
            NeuronType[][] maket = new NeuronType[network.Length][];

            for (int i = 0; i < network.Length; i++)
            {
                if (i != network.Length - 1)
                    maket[i] = new NeuronType[network[i] + 1];
                else
                    maket[i] = new NeuronType[network[i]];
            }

            for (int i = 0; i < maket.Length; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < maket[i].Length; j++)
                    {
                        if (j == maket[i].Length - 1)
                        {
                            maket[i][j] = NeuronType.Bias;
                        }
                        else
                        {
                            maket[i][j] = NeuronType.Input;
                        }
                    }
                    continue;
                }

                if (i == maket.Length - 1)
                {
                    for (int j = 0; j < maket[i].Length; j++)
                    {
                        maket[i][j] = NeuronType.Output;
                    }
                    continue;
                }

                for (int j = 0; j < maket[i].Length; j++)
                {
                    if (j == maket[i].Length - 1)
                    {
                        maket[i][j] = NeuronType.Bias;
                        continue;
                    }
                    else
                    {
                        maket[i][j] = NeuronType.Hidden;
                    }
                }
            }

            return maket;
        }

        public static List<Neuron>[] CreateNetwork(NeuronType[][] maket, IFunction function)
        {
            var layers = new List<Neuron>[maket.Length];

            for (int i = 0; i < maket.Length; i++)
            {
                layers[i] = new List<Neuron>(maket[i].Length);

                for (int j = 0; j < maket[i].Length; j++)
                {
                    if (i == 0)
                    {
                        AddNeuron(function, layers[i], 0, maket[i][j]); 
                    }
                    else
                    {
                        AddNeuron(function, layers[i], layers[i - 1].Count, maket[i][j]); 
                    }
                }
            }

            return layers;
        }

        public static void AddNeuron(IFunction func, List<Neuron> neurons, int prewLayerCount, NeuronType type)
        {
            switch (type)
            {
                case NeuronType.Input:
                    neurons.Add(new InputNeuron(func));
                    break;
                case NeuronType.Output:
                    neurons.Add(new HiddenNeuron(func, prewLayerCount));
                    break;
                case NeuronType.Hidden:
                    neurons.Add(new HiddenNeuron(func, prewLayerCount));
                    break;
                case NeuronType.Bias:
                    neurons.Add(new BiasNeuron(func));
                    break;
            }
        }

    }


}
