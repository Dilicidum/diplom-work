import { Component } from '@angular/core';
import { TasksService } from '../services/tasks.service';
import { OnInit } from '@angular/core';
import {
  TaskCategorySort,
  TaskStatus,
  TaskStatusSort,
  TaskType,
  Tasks,
} from '../models/tasks';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css'],
})
export class TasksComponent implements OnInit {
  constructor(private taskService: TasksService) {}

  StatusSort: any;
  CategorySort: any;
  TaskStatus = TaskStatusSort;
  TaskCategory = TaskCategorySort;
  taskStatuses: string[];
  taskCategories: string[];
  Tasks: Tasks[] = [];
  taskType = TaskType.task;

  tasks$: Observable<Tasks[]>;
  ngOnInit(): void {
    this.StatusSort = TaskStatusSort.All;
    this.CategorySort = TaskCategorySort.All;
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.taskCategories = Object.keys(this.TaskCategory);
    this.taskType = TaskType.task;
    console.log(this.taskType);
    this.tasks$ = this.taskService.getTasks(this.taskType);
  }

  StatusSortChange(value: any) {
    this.StatusSort = value;
    this.tasks$ = this.taskService.getTasks(
      this.taskType,
      this.StatusSort,
      this.CategorySort
    );
  }

  CategorySortChange(value: any) {
    this.CategorySort = value;
    this.tasks$ = this.taskService.getTasks(
      this.taskType,
      this.StatusSort,
      this.CategorySort
    );
  }

  refreshPage() {
    this.tasks$ = this.taskService.getTasks(this.taskType);
  }
}
