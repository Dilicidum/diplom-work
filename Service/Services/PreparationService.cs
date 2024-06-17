using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PresentationService : IPresentationService
    {
        public ModelResponse PrepareData(int bestAlternative, double[] Q_proximity, double[] Q_proximity1, double[] S_medium, double[] S_medium1, double[] R_max, double[] R_max1, double DQ, double minQ)
        {
            var result = new ModelResponse();
            var Q_Proximity_Replacement = Q_proximity.ToList();
            result.BestAlternatives.Add(bestAlternative + 1);
            result.AlternativesInOrder = result.GetAlternativesInOrder(Q_proximity,Q_proximity1);
            result.AlternativesInOrder_S_Dictionary = result.GetAlternativesInOrder(S_medium.ToArray(),S_medium1);
            result.AlternativesInOrder_S = result.AlternativesInOrder_S_Dictionary.Keys.ToList();
            result.AlternativesInOrder_R_Dictionary = result.GetAlternativesInOrder(R_max, R_max1);
            result.AlternativesInOrder_R = result.AlternativesInOrder_R_Dictionary.Keys.ToList();
            int ij = 0;
            foreach(var item in result.AlternativesInOrder)
                {
                    var xyt = item.Value;
                    
                    if ((xyt -  minQ <= DQ) && xyt != minQ)
                    {
                        result.BestAlternatives.Add(Q_Proximity_Replacement.ToList().IndexOf(xyt)+1);
                    }
                    else if(xyt != minQ)
                    {
                        break;
                    }
                    ij++;
                }
            var AlternativesInOrderSorted = result.AlternativesInOrder.OrderBy(x=>x.Key);
            var AlternativesInOrder_S_Sorted = result.AlternativesInOrder_S_Dictionary.OrderBy(x=>x.Key);
            var AlternativesInOrder_R_Sorted = result.AlternativesInOrder_R_Dictionary.OrderBy(x=>x.Key);
            var series = new List<Serie>();
            var series1 = new List<Serie>();
            var series2 = new List<Serie>();
            foreach(var item in AlternativesInOrderSorted){
                series.Add(new Serie(){
                    Name = (item.Key + 1).ToString(),
                    Value = item.Value
                });
            }
            foreach(var item in AlternativesInOrder_S_Sorted){
                series1.Add(new Serie(){
                    Name = (item.Key + 1).ToString(),
                    Value = item.Value
                });
            }
            foreach(var item in AlternativesInOrder_R_Sorted){
                series2.Add(new Serie(){
                    Name = (item.Key + 1).ToString(),
                    Value = item.Value
                });
            }
            
            result.QRS_Horizontal_Chart.Add(new ChartDisplayModel()
            {
                Name = "Q",
                Series = series
            });
            result.QRS_Horizontal_Chart.Add(new ChartDisplayModel()
            {
                Name = "S",
                Series = series1
            });
            result.QRS_Horizontal_Chart.Add(new ChartDisplayModel()
            {
                Name = "R",
                Series = series2
            });
            series.ForEach(item =>
            {
                item.Value = 1 - item.Value;
            });
            result.Vikor_Pie_Grid = series;
            return result;
        }
    }
}
