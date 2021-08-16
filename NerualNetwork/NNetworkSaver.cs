using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork
{
    public sealed class NNetworkSaver
    {
        public const string header = "NETw";

        NNetwork network;

        BinaryWriter saver;

        public NNetworkSaver(NNetwork network)
        {
            this.network = network;
        }

        public void SaveNetwork(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            if (saver != null)
            {
                saver.Close();
            }
            
            FileStream fsStream = new FileStream(path, FileMode.Create);
            saver = new BinaryWriter(fsStream, Encoding.UTF8);

            SaveMaket(network.GetMaket());
            SaveData();

            saver.Close();
            fsStream.Close();
        }

        private void SaveMaket(int[] maket)
        {
            WriteHeader(header);

            saver.Write(maket.Length);

            foreach (int layer in maket)
            {
                saver.Write(layer);
            }
        }

        private void SaveData()
        {
            var maket = network.GetMaket();

            for (int layer = 1; layer < maket.Length; layer++)
            {
                for (int neuronInd = 0; neuronInd < maket[layer]; neuronInd++)
                {
                    var weights = network.GetWeightsFromNeuron(layer, neuronInd);

                    for (int i = 0; i < weights.Length; i++)
                    {
                        saver.Write(weights[i]);
                    }
                }
            }

        }

        private void WriteHeader(string header)
        {
            var buffer = Encoding.UTF8.GetBytes(header);

            saver.Write(buffer);
        }
    }
}
