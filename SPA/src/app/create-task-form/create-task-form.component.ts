import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskCategory, TaskStatus, TaskType, Tasks } from '../models/tasks';
import { OnChanges } from '@angular/core';
import { formatDate } from '@angular/common';
import { TasksService } from '../services/tasks.service';
import { ActivatedRoute, Router } from '@angular/router';
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
    private route: ActivatedRoute
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

  onSubmit() {
    let newTask: Tasks = this.taskForm.value;
    newTask.taskType = this.TaskType;
    this.taskForm.disable();
    newTask.userId = localStorage.getItem('userId') || '';
    if (this.TaskType == TaskType.subTask) {
      newTask.baseTaskId = this.baseTaskId;
    }

    this.taskService.createTask(newTask).subscribe((data) => {
      this.router.navigate(['tasks']);
    });
  }
}
