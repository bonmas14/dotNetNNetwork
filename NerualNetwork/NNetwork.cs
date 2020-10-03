using System.Collections.Generic;

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

        NeuronType[][] maket;

        //---/ debug
        int layersCount;
        private List<Neuron>[] layers;

        // methods

        public NNetwork(IFunction function, int[] network)
        {
            layersCount = network.Length;

            CreateMaket(network);

            CreateNetwork(function);
        }

        private void CreateMaket(int[] network)
        {
            maket = new NeuronType[network.Length][];

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
                        // если последний то это нейрон смещения
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
                    // если последний то это нейрон смещения
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
        }

        private void CreateNetwork(IFunction function)
        {
            // создания сети
            layers = new List<Neuron>[maket.Length];

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
        }

        public void SendData(double[] inputData, bool activate)
        {
            // берём первый слой
            int inputLayer = 0;

            for (int i = 0; i < layers[inputLayer].Count; i++)
            {
                // пропускаем нейрон смещения
                if (layers[inputLayer][i].NeruonType == NeuronType.Bias) continue;

                // записываем данные в каждый нейрон
                layers[inputLayer][i].LoadData(inputData[i], activate);
            }
        }

        public void UpdateData()
        {
            for (int i = 0; i < layers.Length; i++)
            {
                // пропускаем первый слой
                if (i == 0) continue;

                // обновляем каждый нейрон
                for (int j = 0; j < layers[i].Count; j++)
                {
                    layers[i][j].UpdateData(layers[i - 1]);
                }
            }
        }

        public void Learn(double[] LearnSet)
        {
            // поиск ошибки методом обратного распространения ошибки
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].GetError(LearnSet[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].GetError(j, layers[i + 1]);
                    }
                }
            }

            // корректировка весов согласно ошибкам
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].CorrectWeigts(null, learnSpeed);
                    }
                }
                else
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].CorrectWeigts(layers[i - 1], learnSpeed);
                    }
                }
            }
        }
        
        public double[] GetOutput()
        {
            // берём последний слой
            int lastLayer = layers.Length - 1;

            // создаём массив размером с этот слой
            double[] output = new double[layers[lastLayer].Count];

            // читаем выходные значения каждого нейрона
            for (int i = 0; i < layers[lastLayer].Count; i++)
            {
                output[i] = layers[lastLayer][i].Output;
            }
            // возвращаем
            return output;
        }

    }
}
