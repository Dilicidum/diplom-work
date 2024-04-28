import { Component, Input } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskCategory, TaskStatus, TaskType, Tasks } from '../models/tasks';
import { OnChanges } from '@angular/core';
import { formatDate } from '@angular/common';
import { TasksService } from '../services/tasks.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Criteria } from '../models/criteria';
@Component({
  selector: 'app-create-task-form',
  templateUrl: './create-task-form.component.html',
  styleUrls: ['./create-task-form.component.css'],
})
export class CreateTaskFormComponent {
  taskForm: FormGroup;
  TaskCategory = TaskCategory;
  TaskStatus = TaskStatus;
  TaskType = TaskType.task;
  taskCategories: string[];
  taskStatuses: string[];
  taskTypes: string[];

  constructor(
    private formBuilder: FormBuilder,
    private taskService: TasksService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.taskCategories = Object.keys(this.TaskCategory);
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.taskForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
      type: ['', Validators.required],
      dueDate: ['', Validators.required],
      category: ['', Validators.required],
      status: ['', Validators.required],
      criterias: this.fb.array([]),
    });
    this.addCriterias();
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
  private defaultWeights = [0.16, 0.07, 0.02, 0.2, 0.16, 0.1, 0.05, 0.07, 0.17];
  addCriterias() {
    let i = 0;
    this.defaultCriterias.forEach((criteriaValue) => {
      this.criterias.push(
        this.fb.group({
          name: [criteriaValue, Validators.required],
          vacancyWeight: [this.defaultWeights[i], Validators.required],
        })
      );
      i++;
    });
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
      //criteria.CandidateCriterias = [];
    });
    newTask.criterias = criterias;
    newTask.userId = localStorage.getItem('userId') || '';
    if (this.TaskType == TaskType.subTask) {
      newTask.baseTaskId = this.baseTaskId;
    }

    this.taskService.createTask(newTask).subscribe((data) => {
      this.router.navigate(['tasks']);
    });
  }
}
