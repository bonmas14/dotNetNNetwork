using System;
using NerualNetwork.Neurons;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Copyright (c) 2021 BonMAS14
namespace NerualNetwork
{
    internal class Creator
    {
        List<Neuron> _layer;
        
        IFunction _function;
        
        public NeuronType[][] CreateMaket(int[] network)
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

        public List<Neuron>[] CreateNetwork(NeuronType[][] maket, IFunction function)
        {
            this._function = function;
            
            var layers = new List<Neuron>[maket.Length];

            for (int i = 0; i < maket.Length; i++)
            {
                layers[i] = new List<Neuron>(maket[i].Length);

                for (int j = 0; j < maket[i].Length; j++)
                {
                    _layer = layers[i];

                    if (i == 0)
                    {
                        AddNeuron(0, maket[i][j]); 
                    }
                    else
                    {
                        AddNeuron(layers[i - 1].Count, maket[i][j]); 
                    }
                }
            }

            return layers;
        }

        public void AddNeuron(int prewLayerCount, NeuronType type)
        {
            switch (type)
            {
                case NeuronType.Input:
                    _layer.Add(new InputNeuron(_function));
                    break;
                case NeuronType.Output:
                    _layer.Add(new HiddenNeuron(_function, prewLayerCount));
                    break;
                case NeuronType.Hidden:
                    _layer.Add(new HiddenNeuron(_function, prewLayerCount));
                    break;
                case NeuronType.Bias:
                    _layer.Add(new BiasNeuron(_function));
                    break;
            }
        }

    }


}
