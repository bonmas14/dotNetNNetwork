using System;
using System.Collections.Generic;

// Copyright (c) 2021 BonMAS14

namespace NerualNetwork.Neurons
{
    public class HiddenNeuron : Neuron
    {
        double[] _weights;

        static Random _random;

        static HiddenNeuron()
        {
            _random = new Random();
        }

        public HiddenNeuron(IFunction function, int prewLayerNeruonCount) : base(function)
        {
            if (prewLayerNeruonCount == 0)
            {
                throw new ArgumentException("prewLayerNeruonCount равно нулю!");
            }

            _weights = new double[prewLayerNeruonCount];

            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = _random.NextDouble() - 0.5;
            }
        }

        public HiddenNeuron(IFunction function, double[] weights) : base(function)
        {
            this._weights = weights;
        }

        public override void UpdateData(List<Neuron> prewNeurons)
        {
            double data = 0;

            for (int i = 0; i < _weights.Length; i++)
            {
                data += prewNeurons[i].Output * _weights[i];
            }

            _lastInput = data;

            Output = function.ActivationFunc(data);

            base.UpdateData(prewNeurons);
        }

        public override void CorrectWeigts(List<Neuron> prewNeurons, double learnSpeed)
        {
            double deltaError = Error * function.DeltaFunc(_lastInput);

            for (int i = 0; i < prewNeurons.Count; i++)
            {
                double correct = learnSpeed * deltaError * prewNeurons[i].Output;
                _weights[i] += correct;
            }

            base.CorrectWeigts(prewNeurons, learnSpeed);
        }

        public override void GetError(int neuronIndex, List<Neuron> nextNeurons)
        {
            Error = 0;

            for (int i = 0; i < nextNeurons.Count - 1; i++)
            {
                HiddenNeuron neuron = (HiddenNeuron)nextNeurons[i];

                Error += neuron._weights[neuronIndex] * nextNeurons[i].Error;
            }
        }

        public double[] GetWeights()
        {
            return (double[])_weights.Clone();
        }

        public void SetWeight(double weight, int index)
        {
            _weights[index] = weight;
        }
    }
}
