using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork
{
    enum NeruonType
    {
        Input,
        Output,
        Hidden,
        Bias
    }

    class Neuron
    {
        // внешние данные
        public double Output 
        {
            get 
            {
                return output;
            }
        }

        public NeruonType NeruonType { get { return type; } }
        
        // Внутренние переменные
        NeruonType type;

        double output;
        double error;

        IFunction function;
        
        double[] weights;

        public Neuron(IFunction function, NeruonType type, int prewLayerNeruonCount)
        {
            this.function = function;
            this.type     = type;

            if (type == NeruonType.Bias)
            {
                output = 1;
            }
            else if (type == NeruonType.Hidden || type == NeruonType.Output)
            {
                if (prewLayerNeruonCount == 0)
                {
                    throw new ArgumentException("prewLayerNeruonCount равно нулю!");
                }

                weights = new double[prewLayerNeruonCount];

                Random random = new Random();

                for (int i = 0; i < weights.Length; i++)
                {
                    weights[i] = random.NextDouble() - 0.5;
                }
            }
        }

        public void LoadData(double data, bool sendToFunc)
        {
            if (type == NeruonType.Input)
            {
                if (sendToFunc)
                {
                    output = function.ActivationFunc(data);
                }
                else
                {
                    output = data;
                }
            }
        }

        public void UpdateData(List<Neuron> neurons)
        {
            if (type == NeruonType.Hidden || type == NeruonType.Output)
            {
                double data = 0;

                for (int i = 0; i < weights.Length; i++)
                {
                    data += neurons[i].Output * weights[i];
                }

                output = function.ActivationFunc(data);
            }
        }

        public double GetData()
        {
            if (type == NeruonType.Output)
            {
                return output;
            }

            return double.NaN;
        }

        public void GetError()
        {
            if (type == NeruonType.Hidden)
            {
                
            }
        }
        public void GetError(double needOutput)
        {
            if (type == NeruonType.Output)
            {
                error =
            }
        }
    }
}
