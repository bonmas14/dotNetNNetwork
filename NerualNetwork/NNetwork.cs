﻿using NerualNetwork.Neurons;
using System;
using System.Collections.Generic;

// Copyright (c) 2020 BonMAS14
namespace NerualNetwork
{
    public sealed class NNetwork
    {
        public double LearnSpeed { get; set; } = 0.1;

        private List<Neuron>[] _layers;

        private int[] _networkMaket;

        public NNetwork(IFunction function, int[] network)
        {
            _networkMaket = (int[])network.Clone();

            Creator creator = new Creator();

            var maket = creator.CreateMaket(network);

            _layers = creator.CreateNetwork(maket, function);
        }

        public double[] GetWeightsFromNeuron(int layer, int index)
        {
            if (_layers[layer][index].GetType() == typeof(HiddenNeuron))
            {
                var neuron = (HiddenNeuron)_layers[layer][index];

                return neuron.GetWeights();
            }
            else
            {
                return new double[0];
            }
        }

        public bool SetNeuronWeights(double[] weight, int layer, int index)
        {
            if (_layers[layer][index].GetType() == typeof(HiddenNeuron))
            {
                for (int i = 0; i < weight.Length; i++)
                {
                    var neuron = (HiddenNeuron)_layers[layer][index];

                    neuron.SetWeight(weight[i], i);
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        public int[] GetMaket()
        {
            return (int[])_networkMaket.Clone();
        }

        public void SetData(double[] inputData, bool sendToActivationFunc)
        {
            int inputLayer = 0;

            for (int i = 0; i < _layers[inputLayer].Count - 1; i++)
            {
                if (sendToActivationFunc)
                {
                    var function = _layers[inputLayer][i].function;

                    _layers[inputLayer][i].Output = function.ActivationFunc(inputData[i]);
                }
                else
                {
                    _layers[inputLayer][i].Output = inputData[i];
                }
            }
        }
        
        public void UpdateNeurons()
        {
            for (int i = 1; i < _layers.Length; i++)
            {
                for (int j = 0; j < _layers[i].Count; j++)
                {
                    _layers[i][j].UpdateData(_layers[i - 1]);
                }
            }
        }

        public double[] GetOutputData()
        {
            int lastLayer = _layers.Length - 1;

            double[] output = new double[_layers[lastLayer].Count];

            for (int i = 0; i < _layers[lastLayer].Count; i++)
            {
                output[i] = _layers[lastLayer][i].Output;
            }

            return output;
        }

        public void Learn(double[] LearnSet)
        {
            UpdateNeuronsError(LearnSet);

            CorrectWeights();
        }

        public double GetNetworkError()
        {
            double error = 0;
            int lastLayer = _layers.Length - 1;

            for (int neuron = 0; neuron < _layers[lastLayer].Count; neuron++)
            {
                error += Math.Pow(_layers[lastLayer][neuron].Error, 2);
            }

            return error;
        }

        private void UpdateNeuronsError(double[] LearnSet)
        {
            int lastLayer = _layers.Length - 1;

            for (int j = 0; j < _layers[lastLayer].Count; j++)
            {
                _layers[lastLayer][j].Error = LearnSet[j] - _layers[lastLayer][j].Output;
            }

            for (int i = _layers.Length - 2; i >= 0; i--)
            {
                for (int j = 0; j < _layers[i].Count; j++)
                {
                    _layers[i][j].UpdateNeuronError(j, _layers[i + 1]);
                }
            }
        }

        private void CorrectWeights()
        {
            for (int i = _layers.Length - 1; i >= 1; i--)
            {
                for (int j = 0; j < _layers[i].Count; j++)
                {
                    _layers[i][j].CorrectWeigts(_layers[i - 1], LearnSpeed);
                }
            }
        }
    }
}
