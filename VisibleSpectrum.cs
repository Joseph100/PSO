using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interferometer
{
    class VisibleSpectrum
    {
        DoubleslitInterferometer interferometer;
        double[] interferencePattern;
        List<SpectralComponent> allComponents;
        double spectralDisperse = 50E-9;

        public VisibleSpectrum(DoubleslitInterferometer interferometer, double[] interferencePattern)
        {
            this.interferometer = interferometer;
            this.interferencePattern = interferencePattern;
            allComponents= new List<SpectralComponent>();
            initSpectrum();
        }

        public void normalizeComponents()
        {
            double sum = 0;
            foreach (SpectralComponent component in allComponents)
            {
                sum += component.getpercentComponent();
            }

            foreach (SpectralComponent component in allComponents)
            {
                component.setpercentComponent((component.getpercentComponent()) /sum);
            }

        }
        private void initSpectrum()
        {
            for (double wavelength = 400E-9; wavelength <= 700E-9; wavelength += spectralDisperse)
            {
                allComponents.Add(new SpectralComponent(wavelength, this));
            }
            double initialComponent = allComponents.Count;
            initialComponent = 1 / initialComponent;
            foreach (SpectralComponent component in allComponents)
            {
                component.setpercentComponent(initialComponent);
            }
        }

        public List<SpectralComponent> getFitpopulation()
        {
            for (int i = 0; i < 100; i++)
            {
                evolve();
            }
            return allComponents;
        }

        public double getFitness()
        {
            double fitness = 0;
            double temp = 0;
            double[] output = new double[interferencePattern.Length];
            for (int i = 0; i < output.Length; i++)
            {
                foreach (SpectralComponent component in allComponents)
                {
                    temp += (component.getpercentComponent() * interferometer.getInterferencePattern(component.getWavelength())[i]);
                }
                output[i] = temp;
                temp = 0;
            }
            for (int i = 0; i < output.Length; i++)
            {
                fitness += (Math.Pow((interferencePattern[i] - output[i]), 2));
            }
            if (fitness == 0)
                return 10E10;
            fitness = 1 / fitness;
            return fitness;
        }

        private void evolve()
        {
            foreach (SpectralComponent component in allComponents)
            {
                component.tryOptimization();
            }
        }
    }
}
