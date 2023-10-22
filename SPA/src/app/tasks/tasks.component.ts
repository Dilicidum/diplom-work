import { Component } from '@angular/core';
import { TasksService } from '../services/tasks.service';
import { OnInit } from '@angular/core';
import { TaskStatus, TaskStatusSort, Tasks } from '../models/tasks';
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

  tasks$: Observable<Tasks[]>;
  ngOnInit(): void {
    this.StatusSort = TaskStatusSort.All;
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.tasks$ = this.taskService.getTasks();
  }

  StatusSortChange(value: any) {
    this.StatusSort = value;
    if (this.StatusSort != TaskStatusSort.All) {
      this.tasks$ = this.taskService.getTasksFiltered(this.StatusSort);
    } else {
      this.tasks$ = this.taskService.getTasks();
    }
  }

  refreshPage() {
    this.tasks$ = this.taskService.getTasks();
  }
}
