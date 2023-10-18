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
}

export enum TaskCategory {
  Fitness = 'Fitness', //0
  Food = 'Food', //1
  Work = 'Work', //2
  University = 'University', //3
  Health = 'Health', //4
  Friends = 'Friends', //5
  Family = 'Family', //6
}

export enum TaskType {
  Task = 'Task',
  SubTask = 'SubTask',
}

export enum TaskStatus {
  None = 'None',
  Progress = 'Progress',
  Done = 'Done',
  Rejected = 'Rejected',
}
