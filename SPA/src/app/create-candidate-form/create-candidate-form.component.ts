import { Component, Input } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskCategory, TaskStatus, TaskType, Tasks } from '../models/tasks';
import { OnChanges } from '@angular/core';
import { formatDate } from '@angular/common';
import { TasksService } from '../services/tasks.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Criteria } from '../models/criteria';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CandidateCardComponent } from '../candidate-card/candidate-card.component';
import { CandidateCriteria } from '../models/candidateCriteria';
import { Candidate } from '../models/candidate';
@Component({
  selector: 'app-create-candidate-form',
  templateUrl: './create-candidate-form.component.html',
  styleUrls: ['./create-candidate-form.component.css'],
})
export class CreateCandidateFormComponent {
  taskForm: FormGroup;
  TaskCategory = TaskCategory;
  TaskStatus = TaskStatus;
  TaskType = TaskType.task;
  taskCategories: string[];
  taskStatuses: string[];
  taskTypes: string[];
  criterias$: Observable<Criteria[]>;
  criteriaIds: number[] = [];
  constructor(
    private formBuilder: FormBuilder,
    private taskService: TasksService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private http: HttpClient
  ) {
    this.http
      .get<Criteria[]>(
        'http://localhost:5292/api/Criterias/' +
          this.route.snapshot.queryParams['baseTaskId']
      )
      .subscribe((data) => {
        let criteriasNames = data.map((criteria) => criteria.name);
        this.criteriaIds = data.map((criteria) => criteria.id);
        this.addCriterias(criteriasNames);
        console.log('data = ', data);
        console.log('criteriaIds = ', this.criteriaIds);
      });
    this.taskCategories = Object.keys(this.TaskCategory);
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.taskForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
      type: ['', Validators.required],
      dueDate: ['', Validators.required],
      email: ['', Validators.required],
      phone: ['', Validators.required],
      criterias: this.fb.array([]),
    });

    this.taskForm.get('type').disable();

    if (this.route.snapshot.queryParams['type']) {
      this.TaskType = this.route.snapshot.queryParams['type'];
    }
    if (this.route.snapshot.queryParams['baseTaskId']) {
      this.baseTaskId = this.route.snapshot.queryParams['baseTaskId'];
    }
  }

  baseTaskId: number = 0;

  get criterias() {
    return this.taskForm.get('criterias') as FormArray;
  }

  private defaultCriterias = ['', '', '', '', '', '', '', '', ''];
  private defaultWeights = [0, 0, 0, 0, 0, 0, 0, 0, 0];

  addCriterias(defaultCriterias: string[]) {
    let i = 0;
    defaultCriterias.forEach((criteriaValue) => {
      this.criterias.push(
        this.fb.group({
          name: [{ value: criteriaValue, disabled: true }, Validators.required],
          vacancyWeight: [this.defaultWeights[i], Validators.required],
        })
      );
      i++;
    });
    console.log('criterias = ', this.criterias);
  }

  onSubmit() {
    let newTask: Tasks = this.taskForm.value;
    newTask.taskType = this.TaskType;
    this.taskForm.disable();
    let criterias = [] as Criteria[];
    criterias = this.taskForm.get('criterias')?.value;
    console.log('criterias = ', criterias);
    //criterias = criterias.map((criteria: any) => criteria.value);
    console.log('criterias = ', criterias);
    let criteriasString = JSON.stringify(criterias);
    criterias.forEach((criteria: any) => {
      //criteria.vacancy = {};
    });
    newTask.criterias = criterias;
    newTask.userId = localStorage.getItem('userId') || '';
    if (this.TaskType == TaskType.subTask) {
      newTask.baseTaskId = this.baseTaskId;
    }
    let candidateCriterias: CandidateCriteria[] = [];
    let candidate: Candidate = new Candidate();
    candidate.name = newTask.name;
    candidate.email = this.taskForm.get('email')?.value;
    candidate.phone = this.taskForm.get('phone')?.value;
    let i = 0;
    criterias.forEach((criteria: any) => {
      let candidateCriteria = new CandidateCriteria();
      candidateCriteria.criteriaId = this.criteriaIds[i];
      candidateCriteria.value = criteria.vacancyWeight;
      console.log('candidateCriteria = ', candidateCriteria);
      candidateCriterias.push(candidateCriteria);
      i++;
    });
    candidate.CandidateCriterias = candidateCriterias;
    console.log('candidate = ', candidate);
    this.http
      .post(
        'http://localhost:5292/api/Criterias/' +
          this.route.snapshot.queryParams['baseTaskId'],
        candidate
      )
      .subscribe((data) => {
        console.log('data = ', data);
      });
    this.router.navigate(['/tasks']);
  }
}
