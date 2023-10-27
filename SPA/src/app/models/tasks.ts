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
}

export enum TaskCategory {
  fitness = 'Fitness', //0
  food = 'Food', //1
  work = 'Work', //2
  university = 'University', //3
  health = 'Health', //4
  friends = 'Friends', //5
  family = 'Family', //6
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
  fitness = 'Fitness', //0
  food = 'Food', //1
  work = 'Work', //2
  university = 'University', //3
  health = 'Health', //4
  friends = 'Friends', //5
  family = 'Family', //6
}
