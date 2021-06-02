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
        NNetwork network;

        BinaryWriter binaryWriter;

        public NNetworkSaver(NNetwork network)
        {
            this.network = network;
        }

        public void SaveNetwork(string path)
        {
            binaryWriter = new BinaryWriter(new StreamWriter(path).BaseStream);

            SaveMaket(network.GetMaket());


            binaryWriter.Close();
        }

        private void SaveMaket(int[] maket)
        {
            WriteHeader("MAKt");

            binaryWriter.Write(maket.Length * 4);

            foreach (int layer in maket)
            {
                binaryWriter.Write(layer);
            }
        }

        private void SaveData()
        {
            WriteHeader("DATa");

            //binaryWriter.Write();
        }

        private void WriteHeader(string header)
        {
            var buffer = Encoding.UTF8.GetBytes(header);

            binaryWriter.Write(buffer);
        }
    }
}
