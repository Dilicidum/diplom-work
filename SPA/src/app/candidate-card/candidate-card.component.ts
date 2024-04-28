import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskCategory, TaskStatus, Tasks } from '../models/tasks';
import { OnChanges } from '@angular/core';
import { formatDate } from '@angular/common';
import { TasksService } from '../services/tasks.service';
import { Candidate } from '../models/candidate';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-candidate-card',
  templateUrl: './candidate-card.component.html',
  styleUrls: ['./candidate-card.component.css'],
})
export class CandidateCardComponent implements OnChanges {
  @Input() task: Candidate;
  @Input() isDetailed: boolean = false;
  @Input() userId: string;
  @Output() deleteEvent: EventEmitter<void> = new EventEmitter<void>();

  taskForm: FormGroup;
  TaskCategory = TaskCategory;
  TaskStatus = TaskStatus;
  taskCategories: string[];
  taskStatuses: string[];

  constructor(
    private formBuilder: FormBuilder,
    private taskService: TasksService,
    private http: HttpClient
  ) {
    this.taskCategories = Object.keys(this.TaskCategory);
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.taskForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      email: ['', Validators.required],
      phone: ['', Validators.required],
    });
    this.taskForm.disable();
  }

  ngOnChanges(changes: any): void {
    this.updateForm(this.task);
  }

  updateForm(data: Candidate): void {
    console.log('data = ', data);
    console.log('this task = ', this.task);
    this.taskForm.patchValue({
      name: data.name,
      email: data.email,
      phone: data.phone,
    });
  }

  edit() {
    this.taskForm.enable();
  }

  discard() {
    this.taskForm.reset();
    this.taskForm.disable();
  }

  onSubmit() {
    this.task.name = this.taskForm.get('name').value;
    console.log(this.taskForm.get('description').value);
    this.task.email = this.taskForm.get('email').value;
    this.task.phone = this.taskForm.get('phone').value;

    this.taskForm.disable();
  }

  Delete() {
    console.log('here');
    console.log('this task = ', this.task);
    this.http
      .delete('http://localhost:5292/api/Candidates/' + this.task.id)
      .subscribe((data) => {});
    this.deleteEvent.emit();
    //this.taskService
    //.deleteTask(this.task.userId, this.task.id)
    //.subscribe((data) => {
    //this.deleteEvent.emit();
    //});
  }
}
