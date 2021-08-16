﻿using System;
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

            FileStream fsStream = new FileStream(path, FileMode.Create);
            _saver = new BinaryWriter(fsStream, Encoding.UTF8);

            SaveMaket(_network.GetMaket());
            SaveData();

            _saver.Close();
            fsStream.Close();
        }

        private void SaveMaket(int[] maket)
        {
            WriteHeader(header);

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

        private void WriteHeader(string header)
        {
            var buffer = Encoding.UTF8.GetBytes(header);

            _saver.Write(buffer);
        }
    }
}
