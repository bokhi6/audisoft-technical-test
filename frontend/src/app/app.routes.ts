import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: '',
    pathMatch: 'full',
    canActivate: [authGuard],
    loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'estudiantes',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/estudiantes/estudiantes-list/estudiantes-list.component')
        .then(m => m.EstudiantesListComponent)
  },
  {
    path: 'profesores',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/profesores/profesores-list/profesores-list.component')
        .then(m => m.ProfesoresListComponent)
  },
  {
    path: 'notas',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/notas/notas-list/notas-list.component')
        .then(m => m.NotasListComponent)
  },
  { path: '**', redirectTo: '' }
];
