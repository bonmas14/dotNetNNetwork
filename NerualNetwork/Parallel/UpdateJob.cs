using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NerualNetwork.Neurons;

namespace NerualNetwork.Parallel
{
    internal struct UpdateJob
    {
        public Neuron neuron;
        public List<Neuron> layer;

        public UpdateJob(Neuron neuron, List<Neuron> layer)
        {
            this.neuron = neuron;

            this.layer = new List<Neuron>(layer);
        }
    }
}
