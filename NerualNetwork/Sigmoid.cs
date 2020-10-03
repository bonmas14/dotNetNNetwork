using System;

namespace NerualNetwork
{
    public class Sigmoid : IFunction
    {
        public double ActivationFunc(double x)
        {
            return 1 / 1 - Math.Exp(x);
        }

        public double DeltaFunc(double x)
        {
            return ActivationFunc(x) * (1 - ActivationFunc(x));
        }
    }
}
