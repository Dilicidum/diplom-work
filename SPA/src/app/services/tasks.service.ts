import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tasks } from '../models/tasks';
@Injectable({
  providedIn: 'root',
})
export class TasksService {
  baseUrl: string = 'http://localhost:5292/Tasks';

  constructor(private http: HttpClient) {}

  getTasks(
    userId: string,
    taskType: string,
    status: string = '',
    taskCategory: string = ''
  ): Observable<Tasks[]> {
    return this.http.get<Tasks[]>(
      `http://localhost:5292/Users/${userId}/Tasks` +
        '/?TaskType=' +
        taskType +
        '&Status=' +
        status +
        '&Category=' +
        taskCategory
    );
  }

  deleteTask(userId: string, taskId: number) {
    return this.http.delete(
      `http://localhost:5292/Users/${userId}/Tasks/` + taskId
    );
  }

  createTask(task: Tasks): Observable<any> {
    return this.http.post(
      `http://localhost:5292/Users/${task.userId}/Tasks/`,
      task
    );
  }

  updateTask(task: Tasks): Observable<any> {
    return this.http.put(
      `http://localhost:5292/Users/${task.userId}/Tasks/` + task.id,
      task
    );
  }

  getTaskById(userId: string, taskId: number): Observable<Tasks> {
    return this.http.get<Tasks>(
      `http://localhost:5292/Users/${userId}/Tasks/` + taskId
    );
  }
}
