// Copyright (c) 2020 BonMAS14
namespace NerualNetwork
{
    public interface IFunction
    {
        double ActivationFunc(double x);
        
        double DeltaFunc(double x);
    }
}
