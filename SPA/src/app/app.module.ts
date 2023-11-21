import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { JwtModule } from '@auth0/angular-jwt';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TasksComponent } from './tasks/tasks.component';
import { AuthGuard } from './helpers/AuthGuard';
import { AppMenuComponent } from './app-menu/app-menu.component';
import { TaskCardComponent } from './task-card/task-card.component';
import { JWTTokenInterceptor } from './helpers/JWTTokenInterceptor';
import { CreateTaskFormComponent } from './create-task-form/create-task-form.component';
import { TaskInfoComponent } from './task-info/task-info.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationItemComponent } from './notification-item/notification-item.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'tasks',
    component: TasksComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'create-task',
    component: CreateTaskFormComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'tasks/:id/:userId',
    component: TaskInfoComponent,
    canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/login', pathMatch: 'full' },
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    TasksComponent,
    AppMenuComponent,
    TaskCardComponent,
    CreateTaskFormComponent,
    TaskInfoComponent,
    NotificationsComponent,
    NotificationItemComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    FormsModule,
    ReactiveFormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ['http://localhost:5292'],
      },
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JWTTokenInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
