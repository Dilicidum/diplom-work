﻿namespace Domain.Entities;

public class ModelResponse
{

    public ModelResponse() {
        BestAlternatives = new List<int>();
        AlternativesInOrder = new Dictionary<int, double>();
        AlternativesInOrder_S = new List<int>();
        AlternativesInOrder_R = new List<int>();
        AlternativesInOrder_R_Dictionary = new Dictionary<int, double>();
        AlternativesInOrder_S_Dictionary = new Dictionary<int, double>();
        QRS_Horizontal_Chart = new List<ChartDisplayModel>();
        Topsis_C_bestAlternative = new List<int>();
        Topsis_Vertical_Chart_Model = new List<Serie>();
        Topsis_Vertical_Chart_Model_NIS = new List<Serie>();
        PIS_NIS_Horizontal_Chart = new List<ChartDisplayModel>();
        Vikor_Pie_Grid = new List<Serie>();
        Topsis_Alternatives_In_Order = new List<int>();
        Topsis_Pie_Grid = new List<Serie>();
    }
    public List<int> BestAlternatives { get; set; }

    public Dictionary<int,double> AlternativesInOrder { get; set; }

    public List<int> AlternativesInOrder_S { get; set;}

    public List<int> AlternativesInOrder_R { get; set;}

    public Dictionary<int,double> AlternativesInOrder_S_Dictionary { get; set;}

    public Dictionary<int,double> AlternativesInOrder_R_Dictionary { get; set;}

    public List<double> Topsis_PIS { get;set;}

    public List<double> Topsis_NIS { get;set; }

    public List<double> Topsis_D_starNIS { get;set; }

    public List<double> Topsis_D_starPIS { get;set; }

    public List<double> Topsis_C_proximityPIS { get;set; }

    public string Topsis_C_spysokAlt { get;set; }

    public List<int> Topsis_C_bestAlternative { get;set; }

    public List<double> Topsis_C_proximityPIS1 { get;set; }

    public List<Serie> Topsis_Vertical_Chart_Model { get;set; }

    public List<Serie> Topsis_Vertical_Chart_Model_NIS { get;set; }

    public List<ChartDisplayModel> QRS_Horizontal_Chart { get; set; }

    public List<ChartDisplayModel> PIS_NIS_Horizontal_Chart { get; set; }

    public List<Serie> Vikor_Pie_Grid { get; set;}

    public List<Serie> Topsis_Pie_Grid { get; set;}

    public List<int> Topsis_Alternatives_In_Order { get; set; }

    public Dictionary<int, double> GetAlternativesInOrder(double[] Q_Proximity, double[] Q_Proximity1) {
        var result = new Dictionary<int, double>();
        for (int i = 0; i < Q_Proximity1.Length; i++)
        {
            int num = Array.IndexOf(Q_Proximity, Q_Proximity1[i]);
            var containsKey = result.ContainsKey(num);
            if (!containsKey) {
                result.Add(num, Q_Proximity1[i]);
                Q_Proximity1[i] = 0;
                Q_Proximity[num]= 0;
            }
        }

        return result;
    }
}