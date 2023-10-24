import { Component } from '@angular/core';
import { TasksService } from '../services/tasks.service';
import { OnInit } from '@angular/core';
import { TaskStatus, TaskStatusSort, TaskType, Tasks } from '../models/tasks';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css'],
})
export class TasksComponent implements OnInit {
  constructor(private taskService: TasksService) {}

  StatusSort: any;
  TaskStatus = TaskStatusSort;
  taskStatuses: string[];
  Tasks: Tasks[] = [];
  taskType = TaskType.task;

  tasks$: Observable<Tasks[]>;
  ngOnInit(): void {
    this.StatusSort = TaskStatusSort.All;
    this.taskStatuses = Object.keys(this.TaskStatus);

    this.taskType = TaskType.task;
    console.log(this.taskType);
    this.tasks$ = this.taskService.getTasks('', this.taskType);
  }

  StatusSortChange(value: any) {
    this.taskType = TaskType.task;
    this.StatusSort = value;
    if (this.StatusSort != TaskStatusSort.All) {
      this.tasks$ = this.taskService.getTasks(this.StatusSort, this.taskType);
    } else {
      this.tasks$ = this.taskService.getTasks('', this.taskType);
    }
  }

  refreshPage() {
    this.tasks$ = this.taskService.getTasks();
  }
}
