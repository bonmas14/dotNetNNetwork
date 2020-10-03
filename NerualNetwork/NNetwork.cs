using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork
{
    public class NNetwork
    {
        // public
        public double learnSpeed = 0.1;

        //---/ debug
        public int LayersCount 
        {
            get
            {
                return layersCount;
            }
        }

        // private

        //---/ debug
        int layersCount;

        // methods

        public NNetwork(IFunction function, params int[] network)
        {
            layersCount = network.Length;


            CreateNetwork(function);
        }

        private void CreateNetwork(IFunction function)
        {
            // send to new element interface
        }
    }
}
