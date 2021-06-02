using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Copyright (c) 2021 BonMAS14
namespace NerualNetwork.Neurons
{
    public class HiddenNeuron : Neuron
    {

        double[] weights;

        static Random random;

        static HiddenNeuron()
        {
            random = new Random();
        }

        public HiddenNeuron(IFunction function, int prewLayerNeruonCount) : base(function)
        {
            if (prewLayerNeruonCount == 0)
            {
                throw new ArgumentException("prewLayerNeruonCount равно нулю!");
            }

            weights = new double[prewLayerNeruonCount];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.NextDouble() - 0.5;
            }
        }

        public override void UpdateData(List<Neuron> prewNeurons)
        {
            double data = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                data += prewNeurons[i].Output * weights[i];
            }

            Output = function.ActivationFunc(data);

            base.UpdateData(prewNeurons);
        }

        public override void CorrectWeigts(List<Neuron> prewNeurons, double learnSpeed)
        {
            double deltaError = Error * function.DeltaFunc(Output);

            for (int i = 0; i < prewNeurons.Count; i++)
            {
                double correct = learnSpeed * deltaError * prewNeurons[i].Output;
                weights[i] += correct;
            }

            base.CorrectWeigts(prewNeurons, learnSpeed);
        }

        public override void GetError(int neuronIndex, List<Neuron> nextNeurons)
        {
            Error = 0;

            for (int i = 0; i < nextNeurons.Count - 1; i++)
            {
                HiddenNeuron neuron = (HiddenNeuron)nextNeurons[i];

                Error += neuron.weights[neuronIndex] * nextNeurons[i].Error;
            }
        }
    }
}
