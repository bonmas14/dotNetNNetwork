using System;
using System.Collections.Generic;
// Copyright (c) 2020 BonMAS14
namespace NerualNetwork
{
    public enum NeuronType
    {
        Input,
        Output,
        Hidden,
        Bias
    }

    public class Neuron
    {
        // внешние данные
        public double Output { get; private set; }

        public NeuronType NeruonType { get; }
        public double Error { get; private set; }

        IFunction function;
        
        double[] weights;

        public Neuron(IFunction function, NeuronType type, int prewLayerNeruonCount)
        {
            this.function = function;
            NeruonType     = type;

            if (type == NeuronType.Bias)
            {
                Output = 1;
            }
            else if (type == NeuronType.Hidden || type == NeuronType.Output)
            {
                if (prewLayerNeruonCount == 0)
                {
                    throw new ArgumentException("prewLayerNeruonCount равно нулю!");
                }

                weights = new double[prewLayerNeruonCount];

                Random random = new Random();

                for (int i = 0; i < weights.Length; i++)
                {
                    weights[i] = random.NextDouble() - 0.5;
                }
            }
        }

        public void LoadData(double data, bool sendToFunc)
        {
            if (NeruonType == NeuronType.Input)
            {
                if (sendToFunc)
                {
                    Output = function.ActivationFunc(data);
                }
                else
                {
                    Output = data;
                }
            }
        }

        public void UpdateData(List<Neuron> prewNeurons)
        {
            if (NeruonType == NeuronType.Hidden || NeruonType == NeuronType.Output)
            {
                double data = 0;

                for (int i = 0; i < weights.Length; i++)
                {
                    data += prewNeurons[i].Output * weights[i];
                }

                Output = function.ActivationFunc(data);
            }
        }

        public double GetData()
        {
            if (NeruonType == NeuronType.Output)
            {
                return Output;
            }
            return double.PositiveInfinity;
        }

        public void GetError(int neuronIndex, List<Neuron> nextNeurons)
        {
            if (NeruonType == NeuronType.Hidden)
            {
                Error = 0;

                for (int i = 0; i < nextNeurons.Count; i++)
                {
                    if (nextNeurons[i].NeruonType == NeuronType.Bias)
                        continue;
                    Error += nextNeurons[i].weights[neuronIndex] * nextNeurons[i].Error;  
                }
            }
        }
        
        public void GetError(double needOutput)
        {
            if (NeruonType == NeuronType.Output)
            {
                Error = 0;

                Error = needOutput - Output;
            }
        }

        public void SetError(double error)
        {
            if (NeruonType == NeuronType.Output)
            {
                this.Error = error;
            }
        }

        public void CorrectWeigts(List<Neuron> prewNeurons, double learnSpeed)
        {
            if (NeruonType == NeuronType.Output || NeruonType == NeuronType.Hidden)
            {
                double deltaError = Error * function.DeltaFunc(Output);

                for (int i = 0; i < prewNeurons.Count; i++)
                {
                    double correct = learnSpeed * deltaError * prewNeurons[i].Output;
                    weights[i] += correct;
                }
            }
        }
    }
}
