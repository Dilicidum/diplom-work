using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
     public interface IVikorService
     {
        public ModelResponse GetBestAlternatives(double[,] Matrix,List<double> Criterias);
     }
}
