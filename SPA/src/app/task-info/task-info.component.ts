import { Component, OnInit } from '@angular/core';
import { TasksService } from '../services/tasks.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Tasks } from '../models/tasks';

@Component({
  selector: 'app-task-info',
  templateUrl: './task-info.component.html',
  styleUrls: ['./task-info.component.css'],
})
export class TaskInfoComponent implements OnInit {
  task$: Observable<Tasks>;
  constructor(
    private taskService: TasksService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    let taskId = this.route.snapshot.params['id'];
    this.task$ = this.taskService.getTaskById(taskId);
  }

  refreshPage() {
    this.router.navigate(['/tasks']);
  }
}
