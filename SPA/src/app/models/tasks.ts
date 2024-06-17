import { Criteria } from './criteria';

export interface Tasks {
  id: number;
  name: string;
  description: string;
  category: TaskCategory;
  status: TaskStatus;
  baseTaskId: number;
  taskType: TaskType;
  userId: string;
  dueDate: Date;
  subTasks: Tasks[];
  criterias: Criteria[];
}

export enum TaskCategory {
  development = 'Development', //0
  sales = 'Sales', //1
  finances = 'Finances', //2
  security = 'Security', //3
  analytics = 'Analytics', //4
}

export enum TaskType {
  task = 'Task',
  subTask = 'SubTask',
}

export enum TaskStatus {
  none = 'None',
  progress = 'Progress',
  done = 'Done',
  rejected = 'Rejected',
}

export enum TaskStatusSort {
  none = 'None',
  progress = 'Progress',
  done = 'Done',
  rejected = 'Rejected',
  All = '',
}

export enum TaskCategorySort {
  All = '',
  fitness = 'Development', //0
  food = 'Sales', //1
  work = 'Finances', //2
  university = 'Security', //3
  health = 'Analytics', //4
}
