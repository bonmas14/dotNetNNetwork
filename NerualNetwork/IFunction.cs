
namespace NerualNetwork
{
    public interface IFunction
    {
        double ActivationFunc(double x);
        
        double DeltaFunc(double x);
    }
}
