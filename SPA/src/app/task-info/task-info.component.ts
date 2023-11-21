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
  taskId: string;
  userId: string;

  constructor(
    private taskService: TasksService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id');
    this.userId = this.route.snapshot.paramMap.get('userId');

    this.task$ = this.taskService.getTaskById(
      this.userId,
      parseInt(this.taskId)
    );
  }

  refreshPage() {
    this.router.navigate(['/tasks']);
  }
}
