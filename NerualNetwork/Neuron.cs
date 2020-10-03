using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork
{
    enum NeuronType
    {
        Input,
        Output,
        Hidden,
        Bias
    }

    class Neuron
    {
        // внешние данные
        public double Output { get; private set; }

        public NeuronType NeruonType { get; }

        double error;

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

            return double.NaN;
        }

        public void GetError(int neuronIndex, List<Neuron> nextNeurons)
        {
            if (NeruonType == NeuronType.Hidden)
            {
                for (int i = 0; i < nextNeurons.Count; i++)
                {
                    if (nextNeurons[i].NeruonType == NeuronType.Bias)
                        continue;
                    error += nextNeurons[i].weights[neuronIndex] * nextNeurons[i].error;
                }

            }
        }
        
        public void GetError(double needOutput)
        {
            if (NeruonType == NeuronType.Output)
            {
                error = needOutput - Output;
            }
        }

        public void CorrectWeigts(List<Neuron> prewNeurons, double learnSpeed)
        {
            if (NeruonType == NeuronType.Hidden || NeruonType == NeuronType.Hidden)
            {
                double deltaError = error * function.DeltaFunc(Output);

                for (int i = 0; i < prewNeurons.Count; i++)
                {
                    double correct = learnSpeed * deltaError * prewNeurons[i].Output;
                    weights[i] += correct;
                }
            }
        }
    }
}
