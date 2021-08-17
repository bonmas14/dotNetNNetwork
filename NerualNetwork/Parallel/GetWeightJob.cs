using NerualNetwork.Neurons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork.Parallel
{
    internal struct GetWeightJob
    {
        public Neuron neuron;
        public List<Neuron> layer;
        public int weightCoord;
        public GetWeightJob(Neuron neuron, List<Neuron> layer, int coord)
        {
            this.neuron = neuron;

            this.layer = new List<Neuron>(layer);

            weightCoord = coord;
        }
    }
}
