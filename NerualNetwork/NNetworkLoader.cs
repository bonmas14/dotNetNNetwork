using NerualNetwork.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork
{
    public sealed class NNetworkLoader
    {
        string _path;

        private BinaryReader _loader;
        private ILogger _logger;

        public NNetworkLoader(string path, ILogger logger)
        {
            _path = path;
            _logger = logger;
        }

        public NNetwork LoadNNetwork(IFunction function)
        {
            _loader = new BinaryReader(new StreamReader(_path).BaseStream);
            
            NNetwork network = null;

            try
            {
                var maket = GetMaket();
            
                network = new NNetwork(function, maket);

                GetWeights(network, maket);
            }
            catch (Exception e)
            {
                _logger.Log(e.Message);
            }
            finally
            {
                _loader.Close();
            }

            return network;
        }

        private int[] GetMaket()
        {
            var byteHeader = _loader.ReadBytes(4);

            string header = Encoding.UTF8.GetString(byteHeader);

            if (header != NNetworkSaver.header)
            {
                throw new ArgumentException("заголовок не совпал");
            }

            int length = _loader.ReadInt32();

            int[] maket = new int[length];

            for (int i = 0; i < length; i++)
            {
                maket[i] = _loader.ReadInt32();
            }

            return maket;
        }

        private void GetWeights(NNetwork network, int[] maket)
        {
            for (int layer = 1; layer < maket.Length; layer++)
            {
                for (int index = 0; index < maket[layer]; index++)
                {
                    double[] weights = new double[maket[layer - 1] + 1];

                    for (int weightInd = 0; weightInd < maket[layer - 1] + 1; weightInd++)
                    {
                        weights[weightInd] = _loader.ReadDouble();
                    }

                    network.SetNeuronWeights(weights, layer, index);
                }
            }
        }
    }
}
