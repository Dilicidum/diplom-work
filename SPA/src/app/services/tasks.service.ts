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

  getTasks(): Observable<Tasks[]> {
    return this.http.get<Tasks[]>(this.baseUrl);
  }

  deleteTask(id: number) {
    return this.http.delete(this.baseUrl + '/' + id);
  }
}
