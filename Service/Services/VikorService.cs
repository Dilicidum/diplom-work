using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions.DTO;
using System.Linq;

namespace Application.Services
{
    public class VikorService : IVikorService
    {
        int[] Cplusminus = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        public static List<double> PIS { get; set; }
        public static List<double> NIS { get; set; }
        public static List<double> Matr_W { get; set; }

        IPresentationService _presentationService { get; set; }

        public VikorService(IPresentationService presentationService) {
            _presentationService = presentationService;
            PIS = new List<double> { /* initial values */ };
            NIS = new List<double> { };
            Matr_W = new List<double> { 0.16, 0.07, 0.02, 0.2, 0.16, 0.1, 0.05, 0.07, 0.17 };
        }

        public ModelResponse GetBestAlternatives(double[,] Matrix, List<double> Criterias)
        {
            double vaga = 0.5; // Weight of the "majority of criteria" strategy
            Matr_W = Criterias;
            using (StreamWriter file1 = new StreamWriter("C:\\University\\Diplom work\\Prakt6.txt"))
        {
            
            file1.WriteLine("metod Vikor Matr_R");

            // Sum of weights and normalization
            double sum_W = Matr_W.Sum();
            Console.WriteLine($"sum w {Math.Round(sum_W, 3)}");
            for (int i = 0; i < Matr_W.Count; i++)
            {
                Matr_W[i] = Math.Round(Matr_W[i] / sum_W, 4); // Normalizing weights
            }
            Console.WriteLine($"Matr_W: {string.Join(", ", Matr_W)}");

            // Placeholder for PIS and NIS calculations - Implement these methods based on your logic
            PIS = PisMax(Matrix,Cplusminus); // Assuming this method is implemented elsewhere
            NIS = NisMin(Matrix,Cplusminus); // Assuming this method is implemented elsewhere

            Console.WriteLine("вектор найкращих значень для кожної критеріальної функції f*(j)");
            Console.WriteLine(string.Join(", ", PIS));

            Console.WriteLine("вектор найгірших значень для кожної критеріальної функції f-(j)");
            Console.WriteLine(string.Join(", ", NIS));

            var S_medium = SDistansMedium(Matrix,PIS,NIS,Matr_W);
            file1.WriteLine("вектор значень Sk – середньої відстані від «ідеального» розв’язку для кожної альтернативи");
            // Assuming writt_fil_vect is a method that writes a vector to the file, you need to define this method.
            WrittFilVect(S_medium, file1);

            // Assuming r_distans_max is a method that calculates the maximum distance and returns a vector, you need to define this method.
            var R_max = RDistansMax(Matrix);
            Console.WriteLine("R_max " + R_max);
            file1.WriteLine("вектор значень Rk – максимальної відстані від «ідеального» розв’язку");
            WrittFilVect(R_max.ToList(),file1);

            file1.WriteLine("вектор значень Qk для кожної альтернативи Ak");
            
            var Q_proximity = CalculateQProxStar(S_medium,R_max,vaga);
            WrittFilVect(Q_proximity.ToList(),file1);
            // Order alternatives by descending distance indicator
            double[] Q_proximity1 = Q_proximity.OrderBy(x => x).ToArray();
            Console.WriteLine("Q_proximity1: " + String.Join(", ", Q_proximity1));

            string spysokAlt = "";
            for (int i = 0; i < Q_proximity1.Length; i++)
            {
                int num = Array.IndexOf(Q_proximity, Q_proximity1[i]);
                spysokAlt += "A" + (num + 1) + " ";
            }
            Console.WriteLine("spysok_alt: " + spysokAlt);

            double[] R_max1 = R_max.OrderByDescending(x => x).ToArray();
            Console.WriteLine($"R_max1: {string.Join(", ", R_max1)}");
            string spysok_alt_R_max = "";
            for (int i = 0; i < R_max1.Length; i++)
            {
                int num = Array.IndexOf(R_max, R_max1[i]);
                spysok_alt_R_max += $"A{num + 1} ";
            }
            Console.WriteLine($"spysok_alt_R_max: {spysok_alt_R_max}");

            // Sorting S_medium in descending order and generating spysok_alt_S_medium
            double[] S_medium1 = S_medium.OrderByDescending(x => x).ToArray();
            Console.WriteLine($"S_medium1: {string.Join(", ", S_medium1)}");
            string spysok_alt_S_medium = "";
            for (int i = 0; i < S_medium1.Length; i++)
            {
                int num = Array.IndexOf(S_medium.ToArray(), S_medium1[i]);
                spysok_alt_S_medium += $"A{num + 1} ";
            }
            Console.WriteLine($"spysok_alt_S_medium: {spysok_alt_S_medium}");

            double DQ = Math.Round((1.0 / (Matr_W.Count - 1)), 3); // Coefficient for acceptable advantage
            Console.WriteLine("коефіцієнт для прийнятної переваги DQ: " + DQ);
            
            file1.WriteLine("ранжування альтернатив");
            file1.WriteLine(spysokAlt);
            var minQ = Q_proximity1.Min(x => x);                                                                                               
            //var result = new ModelResponse();
            
            var bestAlternative = Q_proximity.ToList().IndexOf(minQ);
            
            var result = _presentationService.PrepareData(bestAlternative,Q_proximity,Q_proximity1, S_medium.ToArray(), S_medium1, R_max, R_max1, DQ, minQ);            

            return result;
            }
            
        }

        private double[] RDistansMax(double[,] matrix)
        {
        // Assuming PIS, NIS, and Matr_W are accessible in the current context
        double[] distRMax = new double[matrix.GetLength(0)];
        double[] dist1 = new double[matrix.GetLength(1)];

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                dist1[j] = Math.Round(
                    Math.Abs((PIS[j] - matrix[i, j]) * Matr_W[j]) / Math.Abs(PIS[j] - NIS[j]),
                    3);
            }

            distRMax[i] = dist1.Max();
        }

        return distRMax;
        }


    private void WrittFilVect(List<double> Vect_p, StreamWriter file1)
    {
        string str1Row = "";

        foreach (var item in Vect_p)
        {
            string str1P = item.ToString(""); // Formats the float to a string with 2 decimal places
            // Ensure the string has at least 6 characters
            str1P = str1P.PadRight(6, '0');
            str1P += " ";
            str1Row += str1P;
        }

        file1.WriteLine(str1Row); // Writes the completed row to the file and appends a new line
    }

    private Dictionary<int, List<int>> CreateG1Matrix(int[,] Matr1_s)
    {
        Dictionary<int, List<int>> g1_p = new Dictionary<int, List<int>>();
        
        int len = Matr1_s.GetLength(0); // Assuming a square matrix

        // Initialize adjacency list
        for (int i = 0; i < len; i++)
        {
            g1_p[i] = new List<int>();
        }

        // Populate adjacency list based on Matr1_s
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                if (Matr1_s[i, j] == 1)
                {
                    g1_p[i].Add(j + 1); // Note: Adding 1 to match Python's 1-based indexing in the result
                }
            }
        }

        return g1_p;
    }

    private int[] SubtractRows(int[,] matrix, int row1Index, int row2Index)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // Validate row indices
        if (row1Index < 0 || row1Index >= rows || row2Index < 0 || row2Index >= rows)
        {
            throw new ArgumentException("Індекси рядків поза допустимим діапазоном");
        }

        // Initialize delta_s and sigma_s arrays
        int[] deltaS = new int[cols];
        int[] sigmaS = new int[cols];

        // Calculate deltaS (difference between the two rows)
        for (int i = 0; i < cols; i++)
        {
            deltaS[i] = matrix[row1Index, i] - matrix[row2Index, i];
        }

        // Map the differences to -1, 0, or 1
        for (int i = 0; i < deltaS.Length; i++)
        {
            if (deltaS[i] < 0)
            {
                sigmaS[i] = -1;
            }
            else if (deltaS[i] > 0)
            {
                sigmaS[i] = 1;
            }
            else
            {
                sigmaS[i] = 0;
            }
        }

        // Optionally, print sigmaS
        // Console.WriteLine("sigma_s: " + string.Join(", ", sigmaS));

        return sigmaS;
    }

    private double[] SubtractDelta(double[,] matrix, int row1Index, int row2Index)
    {
        int rowCount = matrix.GetLength(0); // Number of rows in the matrix
        int colCount = matrix.GetLength(1); // Number of columns in the matrix

        // Check if row indices are within the valid range
        if (row1Index < 0 || row1Index >= rowCount || row2Index < 0 || row2Index >= rowCount)
        {
            throw new ArgumentException("Індекси рядків поза допустимим діапазоном");
        }

        // Ensure the rows are of the same length
        // In a 2D array, this is inherently true, so this check is redundant in C# and can be omitted.
        // if (matrix[row1Index].Length != matrix[row2Index].Length)
        // {
        //     throw new ArgumentException("Рядки мають різну довжину");
        // }

        double[] deltaS = new double[colCount]; // Initialize the array to store differences

        // Calculate the differences between the two specified rows
        for (int i = 0; i < colCount; i++)
        {
            deltaS[i] = matrix[row1Index, i] - matrix[row2Index, i];
        }

        // Optionally, print deltaS
        // Console.WriteLine("delta_s: " + string.Join(", ", deltaS));

        return deltaS;
    }

    private List<double> PisMax(double[,] matrix, int[] Cplusminus)
    {
        var pisMax1 = new List<double>();
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int j = 0; j < cols; j++)
        {
            double maxElement = matrix[0, j];
            double minElement = matrix[0, j];
            for (int i = 0; i < rows; i++)
            {
                maxElement = Math.Max(maxElement, matrix[i, j]);
                minElement = Math.Min(minElement, matrix[i, j]);
            }
            pisMax1.Add(Cplusminus[j] == 1 ? maxElement : minElement);
        }

        return pisMax1;
    }

    private List<double> NisMin(double[,] matrix, int[] Cplusminus)
    {
        var nisMin1 = new List<double>();
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int j = 0; j < cols; j++)
        {
            double maxElement = matrix[0, j];
            double minElement = matrix[0, j];
            for (int i = 0; i < rows; i++)
            {
                maxElement = Math.Max(maxElement, matrix[i, j]);
                minElement = Math.Min(minElement, matrix[i, j]);
            }
            nisMin1.Add(Cplusminus[j] == 1 ? minElement : maxElement);
        }

        return nisMin1;
    }

    private List<double> SDistansMedium(double[,] matrix, List<double> PIS, List<double> NIS, List<double> Matr_W)
    {
        var distSMedium = new List<double>();
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        double[,] Matr_Distans = new double[rows, cols]; // Assuming this needs to be populated

        for (int i = 0; i < rows; i++)
        {
            double summS = 0;
            for (int j = 0; j < cols; j++)
            {
                double value = Math.Abs((PIS[j] - matrix[i, j]) * Matr_W[j]) / Math.Abs(PIS[j] - NIS[j]);
                double value1 = Math.Round(value, 5);
                Matr_Distans[i, j] = value1;
                summS += value1;
            }
            var res = Math.Round(summS, 4);
            distSMedium.Add(res);
        }

        return distSMedium;
    }

    private List<double> RDistansMax(double[,] matrix, List<double> PIS, List<double> NIS, double[] Matr_W)
    {
        var distRMax = new List<double>();
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            double[] dist1 = new double[cols];
            for (int j = 0; j < cols; j++)
            {
                dist1[j] = Math.Round(Math.Abs((PIS[j] - matrix[i, j]) * Matr_W[j]) / Math.Abs(PIS[j] - NIS[j]), 4);
            }
            distRMax.Add(dist1.Max());
        }

        return distRMax;
    }

    private double[] CalculateQProxStar(List<double> sMedium, double[] rMax, double v)
    {
        double[] qProxyStar = new double[sMedium.Count];
        double sStar = sMedium.Min();
        double sMin = sMedium.Max();
        double rStar = rMax.Min();
        double rMin = rMax.Max();

        Console.WriteLine($"S* = min(S_medium) = {sStar}");
        Console.WriteLine($"S- = max(S_medium) = {sMin}");
        Console.WriteLine($"R* = min(R_max) = {rStar}");
        Console.WriteLine($"R- = max(R_max) = {rMin}");

        for (int i = 0; i < sMedium.Count; i++)
        {
            qProxyStar[i] = Math.Round((v * (sMedium[i] - sStar) / (sMin - sStar) + (1 - v) * (rMax[i] - rStar) / (rMin - rStar)), 4);
            Console.WriteLine($"Q{i + 1} = {qProxyStar[i]}");
        }

        return qProxyStar;
    }

    private void WriteMatrixToFile(string filePath, double[,] matrix)
    {
        using (StreamWriter file1 = new StreamWriter(filePath))
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string str1Row = "";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    string str1P = matrix[i, j].ToString("F6") + " ";
                    str1Row += str1P;
                }
                file1.WriteLine(str1Row);
            }
        }
    }

    private void WriteVectorToFile(string filePath, double[] vector)
    {
        using (StreamWriter file1 = new StreamWriter(filePath))
        {
            string str1Row = "";
            foreach (double item in vector)
            {
                str1Row += item.ToString("F6") + " ";
            }
            file1.WriteLine(str1Row);
        }
    }

    private double SumRowElements(double[,] matrix, int i)
    {
        double sum = 0;
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            sum += matrix[i, j] * matrix[i, j];
        }
        return Math.Round(Math.Sqrt(sum), 4);
    }

    private double[] SumColumnElements(double[,] matrix)
    {
        double[] sumC = new double[matrix.GetLength(1)];

        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                sumC[j] += matrix[i, j] * matrix[i, j];
            }
            sumC[j] = Math.Round(Math.Sqrt(sumC[j]), 4);
        }

        return sumC;
    }
    }
}