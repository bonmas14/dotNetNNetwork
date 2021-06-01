using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Copyright (c) 2021 BonMAS14
namespace NerualNetwork.Neurons
{
    public class BiasNeuron : Neuron
    {
        public BiasNeuron(IFunction function) : base(function)
        {
            Output = 1.0;
        }
    }
}
