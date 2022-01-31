using System;
// Copyright (c) 2020 BonMAS14
namespace NerualNetwork
{
    public class Sigmoid : IFunction
    {
        public double ActivationFunc(double x) => 1.0 / (1.0 + Math.Pow(Math.E, -x));

        public double DeltaFunc(double x)   => ActivationFunc(x) * (1.0 - ActivationFunc(x));
    }
}
