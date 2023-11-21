import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskCategory, TaskStatus, Tasks } from '../models/tasks';
import { OnChanges } from '@angular/core';
import { formatDate } from '@angular/common';
import { TasksService } from '../services/tasks.service';
@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.css'],
})
export class TaskCardComponent implements OnChanges {
  @Input() task: Tasks;
  @Input() isDetailed: boolean = false;
  @Output() deleteEvent: EventEmitter<void> = new EventEmitter<void>();

  taskForm: FormGroup;
  TaskCategory = TaskCategory;
  TaskStatus = TaskStatus;
  taskCategories: string[];
  taskStatuses: string[];

  constructor(
    private formBuilder: FormBuilder,
    private taskService: TasksService
  ) {
    this.taskCategories = Object.keys(this.TaskCategory);
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.taskForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
      dueDate: ['', Validators.required],
      category: ['', Validators.required],
      status: ['', Validators.required],
    });
    this.taskForm.disable();
  }

  ngOnChanges(changes: any): void {
    this.updateForm(this.task);
  }

  updateForm(data: Tasks): void {
    if (!(this.task.dueDate instanceof Date)) {
      this.task.dueDate = new Date(this.task.dueDate);
    }

    let formattedDate = formatDate(this.task.dueDate, 'yyyy-MM-dd', 'en-US');

    this.taskForm.patchValue({
      name: this.task.name,
      description: this.task.description,
      category: this.TaskCategory[this.task.category],
      status: this.TaskStatus[this.task.status],
      dueDate: formattedDate,
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
    this.task.description = this.taskForm.get('description').value;
    this.task.category = this.taskForm.get('category').value;
    this.task.status = this.taskForm.get('status').value;
    this.task.dueDate = this.taskForm.get('dueDate').value;
    this.taskService.updateTask(this.task).subscribe((data) => {});
    this.taskForm.disable();
  }

  Delete() {
    this.taskService
      .deleteTask(this.task.userId, this.task.id)
      .subscribe((data) => {
        this.deleteEvent.emit();
      });
  }
}
