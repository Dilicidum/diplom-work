import { ChartDisplayModel, Serie } from './chartDisplayModel';

export class AssesmentResponse {
  bestAlternatives: number[] = [];
  alternativesInOrder: Map<string, number>;
  alternativesInOrder_S: number[];
  alternativesInOrder_S_Dictionary: Map<string, number>;
  alternativesInOrder_R: number[];
  alternativesInOrder_R_Dictionary: Map<string, number>;
  qRS_Horizontal_Chart: ChartDisplayModel[];
  topsis_C_bestAlternative: number[] = [];
  topsis_PIS: number[] = [];
  topsis_NIS: number[] = [];
  vikor_Pie_Grid: Serie[] = [];
  topsis_Pie_Grid: Serie[] = [];
}
