using System;
using System.Collections.Generic;

// Copyright (c) 2020 BonMAS14
namespace NerualNetwork.Neurons
{
    public abstract class Neuron
    {
        public double Output;

        public double Error;

        public IFunction function;

        protected double _lastInput = 0;

        public Neuron(IFunction function)
        {
            this.function = function;
        }

        public virtual void UpdateData(List<Neuron> prewNeurons)
        {
        }
        
        public virtual void CorrectWeigts(List<Neuron> prewNeurons, double learnSpeed)
        {
        }

        public virtual void GetError(int neuronIndex, List<Neuron> nextNeurons)
        {
        }

    }
}
