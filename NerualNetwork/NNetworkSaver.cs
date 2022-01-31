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

        NNetwork _network;

        BinaryWriter _saver;

        public NNetworkSaver(NNetwork network)
        {
            this._network = network;
        }

        public void SaveNetwork(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            FileStream save = new FileStream(path, FileMode.Create);
            _saver = new BinaryWriter(save, Encoding.UTF8);

            WriteHeader(header);
            SaveMaket(_network.GetMaket());
            SaveData();

            _saver.Close();
            save.Close();
        }

        private void WriteHeader(string header)
        {
            var buffer = Encoding.UTF8.GetBytes(header);

            _saver.Write(buffer);
        }

        private void SaveMaket(int[] maket)
        {
            _saver.Write(maket.Length);

            foreach (int layer in maket)
            {
                _saver.Write(layer);
            }
        }

        private void SaveData()
        {
            var maket = _network.GetMaket();

            for (int layer = 1; layer < maket.Length; layer++)
            {
                for (int neuronInd = 0; neuronInd < maket[layer]; neuronInd++)
                {
                    var weights = _network.GetWeightsFromNeuron(layer, neuronInd);

                    for (int i = 0; i < weights.Length; i++)
                    {
                        _saver.Write(weights[i]);
                    }
                }
            }

        }
    }
}
