using System;

namespace NerualNetwork
{
    public class Sigmoid : IFunction
    {
        public double ActivationFunc(double x) => 1 / (1 + Math.Pow(Math.E, -x));

        public double DeltaFunc(double data)   => data * (1 - data);
    }
}
