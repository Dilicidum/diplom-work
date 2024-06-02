import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskCategory, TaskStatus, Tasks } from '../models/tasks';
import { OnChanges } from '@angular/core';
import { formatDate } from '@angular/common';
import { TasksService } from '../services/tasks.service';
import { HttpClient } from '@angular/common/http';
import { Criteria } from '../models/criteria';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.css'],
})
export class TaskCardComponent implements OnChanges, OnInit {
  @Input() showCriterias: boolean;
  @Input() task: Tasks;
  @Input() isDetailed: boolean = false;
  @Output() deleteEvent: EventEmitter<void> = new EventEmitter<void>();

  taskForm: FormGroup;
  TaskCategory = TaskCategory;
  TaskStatus = TaskStatus;
  taskCategories: string[];
  taskStatuses: string[];

  constructor(
    private formBuilder: FormBuilder,
    private taskService: TasksService,
    private http: HttpClient,
    private route: ActivatedRoute
  ) {
    this.taskCategories = Object.keys(this.TaskCategory);
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.taskForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
      dueDate: ['', Validators.required],
      category: ['', Validators.required],
      status: ['', Validators.required],
      criterias: this.formBuilder.array([]),
    });
    console.log(this.showCriterias);

    if (this.showCriterias) {
      this.addCriterias();
    }
    this.taskForm.disable();
  }

  ngOnInit(): void {
    if (this.isDetailed) {
      this.addCriterias();
    }
    console.log('isDetailed = ', this.isDetailed);
  }

  ngOnChanges(changes: any): void {
    this.updateForm(this.task);
  }

  get criterias() {
    return this.taskForm.get('criterias') as FormArray;
  }

  private defaultCriterias = ['', '', '', '', '', '', '', '', ''];
  private defaultWeights = [0.16, 0.07, 0.02, 0.2, 0.16, 0.1, 0.05, 0.07, 0.17];
  addCriterias() {
    this.http
      .get<Criteria[]>('http://localhost:5292/api/criterias/' + this.task.id)
      .subscribe((data) => {
        console.log('criterias = ', data);
      });
    let i = 0;
    this.defaultCriterias.forEach((criteriaValue) => {
      this.criterias.push(
        this.formBuilder.group({
          name: [criteriaValue, Validators.required],
          vacancyWeight: [this.defaultWeights[i], Validators.required],
        })
      );
      i++;
    });
  }

  updateForm(data: Tasks): void {
    if (!(this.task.dueDate instanceof Date)) {
      this.task.dueDate = new Date(this.task.dueDate);
    }

    let formattedDate = formatDate(this.task.dueDate, 'yyyy-MM-dd', 'en-US');

    this.taskForm.patchValue({
      name: this.task.name,
      description: this.task.description,
      category: this.TaskCategory[this.task.category],
      status: this.TaskStatus[this.task.status],
      dueDate: formattedDate,
    });
  }

  edit() {
    this.taskForm.enable();
  }

  discard() {
    this.taskForm.reset();
    this.taskForm.disable();
  }

  onSubmit() {
    this.task.name = this.taskForm.get('name').value;
    console.log(this.taskForm.get('description').value);
    this.task.description = this.taskForm.get('description').value;
    this.task.category = this.taskForm.get('category').value;
    this.task.status = this.taskForm.get('status').value;
    this.task.dueDate = this.taskForm.get('dueDate').value;
    this.taskService.updateTask(this.task).subscribe((data) => {});
    this.taskForm.disable();
  }

  Delete() {
    this.taskService
      .deleteTask(this.task.userId, this.task.id)
      .subscribe((data) => {
        this.deleteEvent.emit();
      });
  }
}
