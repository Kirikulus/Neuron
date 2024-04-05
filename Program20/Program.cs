using System;
using System.IO;

namespace Program20
{
    internal class Program
    {
        public class Neuron
        {
            private const decimal ConversionRate = 0.621371m;
            private decimal weight = 0.5m;
            public decimal LastError { get; private set; }
            public decimal Smoothing { get; set; } = 0.00001m;

            public decimal ProcessInputData(decimal input)
            {
                return input * weight;
            }

            public decimal RestoreInputData(decimal output)
            {
                if (weight == 0)
                {
                    throw new InvalidOperationException("Attempted to divide by zero.");
                }
                return output / weight;
            }

            public void AdjustWeightBasedOnError(decimal input, decimal expectedResult)
            {
                var actualResult = ProcessInputData(input);
                LastError = expectedResult - actualResult;
                var correction = (LastError / actualResult) * Smoothing;
                weight += correction;
            }
        }

        private static void TrainNeuron(Neuron neuron, decimal km, decimal miles, StreamWriter writer)
        {
            int iteration = 0;
            do
            {
                iteration++;
                neuron.AdjustWeightBasedOnError(km, miles);
                if (iteration % 1000 == 0)
                {
                    writer.WriteLine($"Iteration: {iteration}\tError:\t{neuron.LastError}");
                }
            } while (neuron.LastError > neuron.Smoothing || neuron.LastError < -neuron.Smoothing);

            writer.WriteLine("Training complete!");
        }

        static void Main(string[] args)
        {
            decimal km = 100;
            decimal miles = 62.1371m;

            Neuron neuron = new Neuron();

            using (StreamWriter writer = new StreamWriter("neuron_output.txt"))
            {
                TrainNeuron(neuron, km, miles, writer);

                writer.WriteLine($"{neuron.ProcessInputData(100)} miles in 100 km");
                writer.WriteLine($"{neuron.ProcessInputData(541)} miles in 541 km");
                writer.WriteLine($"{neuron.ProcessInputData(221)} miles in 221 km");
                writer.WriteLine($"{neuron.RestoreInputData(22)} km in 22 miles");
            }

            Console.WriteLine("Output written to neuron_output.txt");
        }
    }
}
