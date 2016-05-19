using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interferometer
{
    class DoubleslitInterferometer
    {
        //distance between the apertures
        double distancebetweenSlits;

        //distance from the apertures to the detection board
        double distancetomeasuringDevice;
        //double approxwidthofInterferencePattern=0.1;

        //probability amplitude as array
        double[] outputAsarray =new double[100];
        //points of detection(sampling points)
        double[] outputPoints = new double[100];
        //separation between sampling points
        double samplingSeparation = 1E-3;
        //wavelength of light
        double wavelength = 0;

        //constructor takes two parameter that determines the interferometer configuration
        public DoubleslitInterferometer(double distancebetweenSlits, double distancetomeasuringDevice)
        {
            this.distancebetweenSlits = distancebetweenSlits;
            this.distancetomeasuringDevice = distancetomeasuringDevice;
            //this.approxwidthofInterferencePattern = 700E-9 * distancetomeasuringDevice / distancebetweenSlits;
            initsamplingPoints();
        }

        //returns the probability density of interference for a light of certain wavelength in the configuration set in the constructor
        public double[] getInterferencePattern(double wavelength)
        {
            this.wavelength = wavelength;
            double[] distancefromSlitone=new double[outputAsarray.Length];
            double[] distancefromSlittwo= new double[outputAsarray.Length];
            for (int i = 0; i < outputPoints.Length; i++)
            {
                distancefromSlitone[i] = Math.Sqrt(Math.Pow(distancetomeasuringDevice, 2) + Math.Pow((distancebetweenSlits-outputPoints[i]),2));
            }

            for (int i = 0; i < outputPoints.Length; i++)
            {
                distancefromSlittwo[i] = Math.Sqrt(Math.Pow(distancetomeasuringDevice, 2) + Math.Pow((-distancebetweenSlits - outputPoints[i]), 2));
            }

            outputAsarray = getprobabilityAmplitudes(distancefromSlitone, distancefromSlittwo, wavelength);
            return outputAsarray;
        }

        //initializes the sampling points
        private void initsamplingPoints()
        {
            double maxwidth = (samplingSeparation*outputPoints.Length)/2;
            int points = outputAsarray.Length;
            for (int i = 0; i < points; i++)
            {
                outputPoints[i] = maxwidth - i * samplingSeparation;
            }
        }
        
        //returns interference pattern based on path deferences and wave length
        private double[ ] getprobabilityAmplitudes(double[] path1, double[] path2, double wavelength)
        {
            double[] phaseDifferences = new double[outputPoints.Length];
            double[] relativeProbabilities = new double[outputPoints.Length];
            for (int i = 0; i < outputPoints.Length; i++)
            {
                phaseDifferences[i] = 2*Math.PI*((Math.Abs(path1[i] - path2[i])/wavelength) %1);
            }
            for (int i = 0; i < outputPoints.Length; i++)
            {
                relativeProbabilities[i] = Math.Pow(Math.Cos(phaseDifferences[i]), 2);
            }
            relativeProbabilities = getNormalizedprobability(relativeProbabilities);
            return relativeProbabilities;
        }

        //normalizes a given probability distribution and manipulates amplitudes
        private double[] getNormalizedprobability(double[] relativeProbability)
        {
            double sum = 0;
            double intensity = 0;
            double arg = 0;
            double[] relativeProbabilities = new double[relativeProbability.Length];
            for (int i = 0; i < outputPoints.Length; i++)
            {
                arg = Math.PI * distancebetweenSlits * outputPoints[i] / (wavelength * distancetomeasuringDevice);
                if (arg == 0)
                    intensity = 1;
                else
                    intensity = Math.Pow((Math.Sin(arg)/ arg), 2);
                relativeProbabilities[i] = relativeProbability[i] * intensity;
            }

            foreach (double d in relativeProbabilities)
            {
                sum += d;
            }
            for (int i = 0; i < outputPoints.Length; i++)
            {
                relativeProbabilities[i] /= sum;
            }
            return relativeProbabilities;
        }
    }
}
