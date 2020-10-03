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

                List<Task> tasks = new List<Task>();

                // обновляем каждый нейрон
                for (int j = 0; j < layers[i].Count; j++)
                {
                    List<Neuron> neurons = new List<Neuron>();

                    foreach (var item in layers[i - 1])
                    {
                        neurons.Add(item);
                    }

                    Task task = new Task(() => layers[i][j].UpdateData(neurons));

                    tasks.Add(task);
                }
                // start tasks
                foreach (Task task in tasks)
                {
                    task.Start();
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        public void Learn(double[] LearnSet)
        {
            List<Task> tasks;

            // поиск ошибки методом обратного распространения ошибки
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    tasks = new List<Task>();
                    
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        Task task = new Task(() => layers[i][j].GetError(LearnSet[j]));
                        tasks.Add(task);
                    }
                    // start tasks
                    foreach (Task task in tasks)
                    {
                        task.Start();
                    }

                    Task.WaitAll(tasks.ToArray());
                }
                else
                {
                    tasks = new List<Task>();

                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        List<Neuron> neurons = new List<Neuron>();

                        foreach (var item in layers[i + 1])
                        {
                            neurons.Add(item);
                        }

                        Task task = new Task(() => layers[i][j].GetError(j, neurons));
                        
                        tasks.Add(task);
                    }
                    // start tasks
                    foreach (Task task in tasks)
                    {
                        task.Start();
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            }

            // корректировка весов согласно ошибкам
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    tasks = new List<Task>();

                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        Task task = new Task(() => layers[i][j].CorrectWeigts(null, learnSpeed));
                        tasks.Add(task);
                    }

                    // start tasks
                    foreach (Task task in tasks)
                    {
                        task.Start();
                    }

                    Task.WaitAll(tasks.ToArray());
                }
                else
                {
                    tasks = new List<Task>();

                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        List<Neuron> neurons = new List<Neuron>();

                        foreach (var item in layers[i - 1])
                        {
                            neurons.Add(item);
                        }

                        Task task = new Task(() => layers[i][j].CorrectWeigts(neurons, learnSpeed));
                        tasks.Add(task);
                    }

                    // start tasks
                    foreach (Task task in tasks)
                    {
                        task.Start();
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            }
        }
        
    }
}
