import { Component, OnInit } from '@angular/core';
import { TasksService } from '../services/tasks.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Tasks } from '../models/tasks';
import { HttpClient } from '@angular/common/http';
import { Candidate } from '../models/candidate';

@Component({
  selector: 'app-task-info',
  templateUrl: './task-info.component.html',
  styleUrls: ['./task-info.component.css'],
})
export class TaskInfoComponent implements OnInit {
  task$: Observable<Tasks>;
  taskId: string;
  userId: string;
  candidates$: Observable<Candidate[]>;
  constructor(
    private taskService: TasksService,
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id');
    this.userId = this.route.snapshot.paramMap.get('userId');
    this.candidates$ = this.http.get<Candidate[]>(
      'http://localhost:5292/api/Candidates/' + this.taskId
    );

    this.task$ = this.taskService.getTaskById(
      this.userId,
      parseInt(this.taskId)
    );
  }

  refreshPage() {
    this.router.navigate(['/tasks']);
  }
}
