using System;

namespace NerualNetwork
{
    public class GANNetwork
    {
        public double LearnSpeed 
        {
            get
            {
                return firstNet.LearnSpeed;
            }
            set
            { 
                firstNet.LearnSpeed = value;
                secoundNet.LearnSpeed = value;
            }
        }

        // update
        // Net1 -> Net2 -> output
        
        // learn
        // Net1 <- Net2 <- data
        NNetwork firstNet;
        NNetwork secoundNet;

        public GANNetwork(IFunction function, double learnSpeed, int[] maket1, int[] maket2)
        {
            LearnSpeed = learnSpeed;

            if (maket1[maket1.Length - 1] != maket2[0])
            {
                throw new ArgumentException("Сети не могут соединиться. " +
                    "Так как имеют не равное количество нейронов в слоях соединения " +
                    $"фактические значения maket1 - {maket1[maket1.Length - 1]}; maket2 - {maket2[0]}");
            }

            firstNet   = new NNetwork(function, maket1);
            secoundNet = new NNetwork(function, maket2);
        }

        //public GANNetwork(NNetwork first, NNetwork secound)
        //{ 
        //    firstNet = first;
        //    secoundNet = secound;
        //}

        public void SetData(double[] data)
        {
            firstNet.SendData(data, true);
        }

        public void UpdateNet()
        {
            firstNet.UpdateData();

            var data = firstNet.GetOutput();

            secoundNet.SendData(data, false);

            secoundNet.UpdateData();
        }

        public void LearnAll(double[] needOutput)
        {
            var errors = secoundNet.GanUpdateErrorFirst(needOutput);
            firstNet.GanUpdateErrorSecound(errors);

            secoundNet.CorrectWeights();
            firstNet.CorrectWeights();
        }

        public void LearnGenerator(double[] needOutput)
        {
            var errors = secoundNet.GanUpdateErrorFirst(needOutput);
            firstNet.GanUpdateErrorSecound(errors);

            firstNet.CorrectWeights();
        }

        public double[] GetOutput()
        {
            return secoundNet.GetOutput();
        }

        /// <summary>
        /// Взять просмоторщика
        /// </summary>
        /// <returns>Сеть-просмоторщик</returns>
        public NNetwork GetNetworkViewer()
        {
            return secoundNet;
        }

        /// <summary>
        /// Взять генегатор
        /// </summary>
        /// <returns> Сеть-генератор чего-либо</returns>
        public NNetwork GetNetworkCreator()
        {
            return firstNet;
        }
        /// <summary>
        /// требуется для отдельного обучения просмоторщика
        /// </summary>
        /// <param name="network">обученный просмоторщик </param>
        public void SetNetworkViewer(NNetwork network)
        {
            secoundNet = network;
        }
    }
}
