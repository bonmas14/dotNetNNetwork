using System;

namespace NerualNetwork
{
    public class Sigmoid : IFunction
    {
        public double ActivationFunc(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        } 

        public double DeltaFunc(double data)
        {
            return data * (1 - data);
        }
    }
}
