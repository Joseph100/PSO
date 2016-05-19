using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interferometer
{
    class Program
    {
        static void Main(string[] args)
        {
            //create an double slit interferometer with 50 micro meter separation between slits and 25 centmeter length
            var interferometer = new DoubleslitInterferometer(100E-6, 1000E-2);

            //get the expected intensity distribution for a given wavelength light
            double[] pattern1 = interferometer.getInterferencePattern(700E-9);
            double[] pattern2 = interferometer.getInterferencePattern(500E-9);
            double[] pattern = new double[pattern1.Length];
            for (int i = 0; i < pattern1.Length; i++)
            {
                pattern[i] = (0.5 * pattern1[i] + 0.5 * pattern2[i]);
            }

            //display the cummulative interference pattern
            foreach (double d in pattern)
            {
                printAsterix(d);
            }

            SpectrumAnalyzer spectrumAnalyzer = new SpectrumAnalyzer(interferometer);
            var dict = spectrumAnalyzer.getspectralComponents(pattern);

            Console.WriteLine("The given interference pattern is caused by the following spectral components:");
            foreach (KeyValuePair<double, double> t in dict)
            {
                Console.WriteLine(String.Format("{1}% of wavelength {0}%", t.Key, Math.Round(t.Value * 100, 2)));
            }
            Console.ReadKey();
        }
    
        //prints asterix based on amplitude at a given point
        private static void printAsterix(double asterixNumber)
        {
            //since the amplitudes are small I scalled them up by 1000 so the interference pattern is visible
            int i = (int)(asterixNumber * 1000);
            string asterixes = "";
            for (int j = 0; j < i; j++)
            {
                asterixes+="*";
            }
            Console.WriteLine(asterixes);
        }
    }
}
