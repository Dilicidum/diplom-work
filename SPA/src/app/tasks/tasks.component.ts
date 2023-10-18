import { Component } from '@angular/core';
import { TasksService } from '../services/tasks.service';
import { OnInit } from '@angular/core';
import { Tasks } from '../models/tasks';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css'],
})
export class TasksComponent implements OnInit {
  constructor(private taskService: TasksService) {}

  Tasks: Tasks[] = [];
  tasks$: Observable<Tasks[]>;
  ngOnInit(): void {
    this.tasks$ = this.taskService.getTasks();
  }
}
