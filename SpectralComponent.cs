using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interferometer
{
    class SpectralComponent
    {
        double wavelength;
        double percentComponent;
        double optimizationSteps = 0.01;
        VisibleSpectrum spectrum;
        public SpectralComponent(double wavelength, VisibleSpectrum spectrum)
        {
            this.spectrum = spectrum;
            this.wavelength = wavelength;
        }

        public void setpercentComponent(double percentComponent)
        {
            this.percentComponent = percentComponent;
        }
        public double getpercentComponent()
        {
            return this.percentComponent;
        }
        public double getWavelength()
        {
            return wavelength;
        }
        public void tryOptimization()
        {
            double initialpercentComponent = this.percentComponent;
            double initialFitness = spectrum.getFitness();
            this.percentComponent += optimizationSteps;
            spectrum.normalizeComponents();
            double newFitness = spectrum.getFitness();
            if (newFitness > initialFitness)
                return;
            this.percentComponent =Math.Abs(percentComponent-2 * optimizationSteps);
            spectrum.normalizeComponents();
            newFitness = spectrum.getFitness();
            if (newFitness > initialFitness)
                return;
            this.percentComponent = initialpercentComponent;
            spectrum.normalizeComponents();
        }
    }
}
