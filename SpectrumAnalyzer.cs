using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interferometer
{
    class SpectrumAnalyzer
    {
        private DoubleslitInterferometer interferometer;
        private VisibleSpectrum spectrum;
        public SpectrumAnalyzer(DoubleslitInterferometer interferometer)
        {
            this.interferometer = interferometer;
        }

        public Dictionary<double, double> getspectralComponents(double[] interferencePattern)
        {
            this.spectrum = new VisibleSpectrum(interferometer, interferencePattern);
            Dictionary<double, double> spectralWeights = new Dictionary<double, double>();
            var listofSpectrum = spectrum.getFitpopulation();
            foreach (SpectralComponent component in listofSpectrum)
            {
                spectralWeights.Add(component.getWavelength(), component.getpercentComponent());
            }
            return spectralWeights;
        }
    }
}
