using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork
{
    public static class Maket
    {
        public static NeuronType[][] CreateMaket(int[] network)
        {
            NeuronType[][] maket = new NeuronType[network.Length][];

            // create array
            for (int i = 0; i < network.Length; i++)
            {
                if (i != network.Length - 1)
                    maket[i] = new NeuronType[network[i] + 1];
                else
                    maket[i] = new NeuronType[network[i]];
            }

            // полная иницаиализация макета
            for (int i = 0; i < maket.Length; i++)
            {
                // первый слой
                if (i == 0)
                {
                    // инициализация нейронов
                    for (int j = 0; j < maket[i].Length; j++)
                    {
                        // если последний, то это нейрон смещения
                        // иначе это вход
                        if (j == maket[i].Length - 1)
                        {
                            maket[i][j] = NeuronType.Bias;
                        }
                        else
                        {
                            maket[i][j] = NeuronType.Input;
                        }
                    }
                    continue;
                }

                // последний слой
                if (i == maket.Length - 1)
                {
                    // все нейроны тут выходные
                    for (int j = 0; j < maket[i].Length; j++)
                    {
                        maket[i][j] = NeuronType.Output;
                    }
                    continue;
                }

                // все остальные слои
                for (int j = 0; j < maket[i].Length; j++)
                {
                    // если последний, то это нейрон смещения
                    // иначе это внутренний нейрон
                    if (j == maket[i].Length - 1)
                    {
                        maket[i][j] = NeuronType.Bias;
                        continue;
                    }
                    else
                    {
                        maket[i][j] = NeuronType.Hidden;
                    }
                }
            }

            return maket;
        }

        public static List<Neuron>[] CreateNetwork(NeuronType[][] maket, IFunction function)
        {
            // создания сети
            var layers = new List<Neuron>[maket.Length];

            // инициализация сети
            for (int i = 0; i < maket.Length; i++)
            {
                // создание слоя
                layers[i] = new List<Neuron>(maket[i].Length);

                // инициализация слоя
                for (int j = 0; j < maket[i].Length; j++)
                {
                    if (i == 0)
                        layers[i].Add(new Neuron(function, maket[i][j], 0));
                    else
                        layers[i].Add(new Neuron(function, maket[i][j], layers[i - 1].Count));
                }
            }

            return layers;
        }

    }


}
