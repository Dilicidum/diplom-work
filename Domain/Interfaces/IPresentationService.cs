using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPresentationService
    {
        public ModelResponse PrepareData(int bestAlternative, double[] Q_Proximity, double[] Q_Proximity1, double[] S_medium, double[] S_medium1, double[] R_max, double[] R_max1, double DQ, double minQ);
    }
}
