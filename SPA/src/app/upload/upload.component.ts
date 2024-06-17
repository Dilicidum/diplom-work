import { Component, OnInit } from '@angular/core';
import { KeyValue } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { PopupFormComponent } from '../popup-form/popup-form.component';
import { AssesmentResponse } from '../models/assesmentResponse';
import { ActivatedRoute } from '@angular/router';
import { Criteria } from '../models/criteria';
@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css'],
})
export class UploadComponent implements OnInit {
  single: any[] = [];
  single_S: any[] = [];
  single_R: any[] = [];
  single1_topsis: any[] = [];
  vikor_Pie_Grid: any[] = [];
  QRS_CHART: any[] = [
    {
      name: 'Germany',
      series: [
        {
          name: '1990',
          value: 62000000,
        },
        {
          name: '2010',
          value: 73000000,
        },
        {
          name: '2011',
          value: 89400000,
        },
      ],
    },

    {
      name: 'USA',
      series: [
        {
          name: '1990',
          value: 250000000,
        },
        {
          name: '2010',
          value: 309000000,
        },
        {
          name: '2011',
          value: 311000000,
        },
      ],
    },

    {
      name: 'France',
      series: [
        {
          name: '1990',
          value: 58000000,
        },
        {
          name: '2010',
          value: 50000020,
        },
        {
          name: '2011',
          value: 58000000,
        },
      ],
    },
    {
      name: 'UK',
      series: [
        {
          name: '1990',
          value: 57000000,
        },
        {
          name: '2010',
          value: 62000000,
        },
      ],
    },
  ];
  single1: any = [
    {
      name: 'Germany',
      value: 8940000,
    },
    {
      name: 'USA',
      value: 5000000,
    },
    {
      name: 'France',
      value: 7200000,
    },
    {
      name: 'UK',
      value: 6200000,
    },
    {
      name: 'Italy',
      value: 4200000,
    },
    {
      name: 'Spain',
      value: 8200000,
    },
  ];

  PIS_NIS_CHART: any[] = [];
  view: [number, number] = [700, 400];
  combinedData_PIS_NIS$: Observable<any[]>;
  combinedData_starPIS_starNIS$: Observable<any[]>;
  data: any[] = [];
  data_S: any[] = [];
  data_R: any[] = [];
  data_topsis_pis_graphs: any[] = [];
  data_topsis_nis_graphs: any[] = [];
  // options
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel_Q = 'Candidate Id';
  showYAxisLabel = true;
  yAxisLabel_Q = 'Candidates scores';
  xAxisLabel_S = 'Candidate Id';
  yAxisLabel_S = 'Коефіцієнт загального задоволення (S)';
  yAxisLabel_R = 'Коефіцієнт максимальної відстані (R)';
  xAxisLabel_R = 'Candidate Id';
  colorScheme = 'vivid';
  colorSchemeHorizontal = 'solar';
  xAxis: boolean = true;
  yAxis: boolean = true;
  xAxisLabel: string = 'Candidate Id';
  yAxisLabel: string = 'Q,R,S показники';
  onSelect(event) {
    console.log(event);
  }
  options: string[] = ['Option 1', 'Option 2', 'Option 3', 'Option 4']; // Dropdown options
  selectedOptions = new FormControl(); // Form control for selected options
  vacancyId: any = ''; // Vacancy ID
  uploadForm: FormGroup;
  AssesmentData$: Observable<any>;
  mergedData$: Observable<any>;
  responseData: any;
  constructor(
    private formBuilder: FormBuilder,
    private httpClient: HttpClient,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe((params) => {
      this.vacancyId = params.get('id');
      console.log('vaca = ', this.vacancyId);
      //if (this.userId == null) this.userId = localStorage.getItem('userId');
    });
    this.uploadForm = this.formBuilder.group({
      file: [null],
      methodName: ['Vikor', Validators.required],
      criterias: this.fb.array([]),
    });
    this.addCriterias();
  }

  get criterias() {
    return this.uploadForm.get('criterias') as FormArray;
  }

  private defaultWeights = [0.16, 0.07, 0.02, 0.2, 0.16, 0.1, 0.05, 0.07, 0.17];
  private defaultCriterias = ['', '', '', '', '', '', '', '', ''];
  criteriasForVacancy: Criteria[] = [];

  addCriterias() {
    this.httpClient
      .get<Criteria[]>('http://localhost:5292/api/criterias/' + this.vacancyId)
      .subscribe((data) => {
        this.criteriasForVacancy = data;
        console.log('criterias = ', data);
        let i = 0;
        this.defaultWeights = this.criteriasForVacancy.map((criteria) => {
          return criteria.vacancyWeight;
        });
        this.defaultCriterias = this.criteriasForVacancy.map((criteria) => {
          return criteria.name;
        });
        console.log('this.defaultCriterias = ', this.defaultCriterias);
        console.log('this.defaultWeights = ', this.defaultWeights);
        this.defaultCriterias.forEach((criteriaValue) => {
          this.criterias.push(
            this.formBuilder.group({
              name: [criteriaValue, Validators.required],
              vacancyWeight: [this.defaultWeights[i], Validators.required],
            })
          );
          i++;
        });
      });
    let i = 0;
  }

  ngOnInit() {}

  convertToMap(obj: Object): Map<string, number> {
    return new Map(Object.entries(obj));
  }

  onFileSelect(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.uploadForm.patchValue({
        file: file,
      });
    }
  }

  showResult$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  methodName: any;

  onSubmit() {
    console.log('here = ', this.uploadForm.get('methodName')?.value);
    const formData = new FormData();
    formData.append('file', this.uploadForm.get('file')?.value);
    formData.append('method', this.uploadForm.get('methodName')?.value);
    let criterias = [] as any;
    criterias = this.uploadForm.get('criterias')?.value;
    console.log('cri');
    criterias = criterias.map((criteria: any) => criteria.vacancyWeight);
    console.log('criterias = ', criterias);
    let criteriasString = JSON.stringify(criterias);
    console.log('criteriasString = ', criteriasString.toString());
    formData.append('criterias', criteriasString.toString());
    this.methodName = this.uploadForm.get('methodName')?.value;
    // Replace 'your-api-url' with the actual URL of your file upload API
    this.AssesmentData$ = this.httpClient
      .post<AssesmentResponse>(
        'http://localhost:5292/api/Assesments/' + this.vacancyId,
        formData
      )
      .pipe(
        map((data) => {
          this.single1 = data.vikor_Pie_Grid;
          console.log('data = ', data);
          this.single1_topsis = data.topsis_Pie_Grid;
          return data;
        })
      );
    this.combinedData_starPIS_starNIS$ = this.combinedData_PIS_NIS$ =
      this.AssesmentData$.pipe(
        map((data) => {
          console.log('data = ', data);
          let x = data.topsis_D_starPIS.map((pis, index) => ({
            star_pis: pis,
            star_nis: data.topsis_D_starNIS[index],
            proximity_pis: data.topsis_C_proximityPIS[index],
          }));
          this.data_topsis_pis_graphs = data.topsis_Vertical_Chart_Model;
          this.data_topsis_nis_graphs = data.topsis_Vertical_Chart_Model_NIS;
          this.PIS_NIS_CHART = data.piS_NIS_Horizontal_Chart;

          console.log('this.single1 = ', this.single1);
          return x;
        })
      );
    this.combinedData_PIS_NIS$ = this.AssesmentData$.pipe(
      map((data) => {
        return data.topsis_PIS.map((pis, index) => ({
          pis: pis,
          nis: data.topsis_NIS[index],
        }));
      })
    );
    this.mergedData$ = this.AssesmentData$.pipe(
      map((data) => {
        console.log('data = ', data);
        this.QRS_CHART = data.qrS_Horizontal_Chart;
        console.log('this.QRS_CHART = ', this.QRS_CHART);
        const convertedData = new AssesmentResponse();
        convertedData.alternativesInOrder = this.convertToMap(
          data.alternativesInOrder
        );
        console.log(
          'data.alternativesInOrder_S = ',
          data.alternativesInOrder_S
        );
        convertedData.alternativesInOrder_S_Dictionary = this.convertToMap(
          data.alternativesInOrder_S_Dictionary
        );

        convertedData.alternativesInOrder_R_Dictionary = this.convertToMap(
          data.alternativesInOrder_R_Dictionary
        );

        console.log('convertedData = ', convertedData);
        const mapArray = Array.from(convertedData.alternativesInOrder);
        const mapArray_S = Array.from(
          convertedData.alternativesInOrder_S_Dictionary
        );
        const mapArray_R = Array.from(
          convertedData.alternativesInOrder_R_Dictionary
        );
        mapArray.sort((a, b) => a[1] - b[1]);
        mapArray_S.sort((a, b) => Number(a[0]) - Number(b[0]));
        mapArray_R.sort((a, b) => Number(a[0]) - Number(b[0]));

        const sortedMap = new Map<string, number>(mapArray);
        const sortedMap_S = new Map<string, number>(mapArray_S);
        const sortedMap_R = new Map<string, number>(mapArray_R);

        convertedData.alternativesInOrder = sortedMap;
        convertedData.alternativesInOrder_S_Dictionary = sortedMap_S;
        convertedData.alternativesInOrder_R_Dictionary = sortedMap_R;
        console.log(
          'data.alternativesInOrder_S = ',
          data.alternativesInOrder_S
        );
        const merged = [];
        convertedData.alternativesInOrder_R_Dictionary.forEach((value, key) => {
          let j = parseInt(key) + 1;
          let i = parseInt(key);
          let k = i + 1;
          let VALUE = sortedMap_R.keys[key];
          this.single_R.push({
            name: j,
            value: value,
          });
        });

        convertedData.alternativesInOrder_S_Dictionary.forEach((value, key) => {
          let j = parseInt(key) + 1;
          let i = parseInt(key);
          let k = i + 1;
          let VALUE = sortedMap_S.keys[key];
          console.log('VALUE = ', VALUE);
          this.single_S.push({
            name: j,
            value: value,
          });
        });
        let j = 0;
        convertedData.alternativesInOrder.forEach((value, key) => {
          let z = j + 1;
          let i = parseInt(key);
          let k = i + 1;
          let VALUE = convertedData.alternativesInOrder.get(j.toString());
          console.log('VALUE = ', VALUE);
          this.single.push({
            name: z,
            value: VALUE,
          });
          merged.push({
            key: k,
            value,
            value_S: data.alternativesInOrder_S[j] + 1 || 0,
            value_R: data.alternativesInOrder_R[j] + 1 || 0,
            bestAlternatives: data.bestAlternatives,
          });

          j = j + 1;
        });
        this.bestAlternatives.push(data.bestAlternatives);
        console.log('this.single = ', this.single);
        Object.assign(this, {
          single: this.single,
        });
        this.data = this.single;
        this.data_S = this.single_S;
        this.data_R = this.single_R;
        console.log('merged = ', merged);

        return merged;
      })
    );

    this.showResult$.next(true);
  }

  bestAlternatives: any[] = [];

  openFormPopUp() {
    const dialogRef = this.dialog.open(PopupFormComponent, {
      width: '250px',

      data: {
        vacancyId: this.vacancyId,
        bestAlternatives: this.bestAlternatives,
        amountOfCandidates: 15,
      }, // You can pass data to the dialog here
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
      // handle any actions after closing the dialog
    });
  }

  valueOrder = (
    a: KeyValue<string, number>,
    b: KeyValue<string, number>
  ): number => {
    return a.value - b.value; // For number comparison
  };
}
