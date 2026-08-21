import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'estudiantes', pathMatch: 'full' },
  {
    path: 'estudiantes',
    loadComponent: () =>
      import('./features/estudiantes/estudiantes-list/estudiantes-list.component')
        .then(m => m.EstudiantesListComponent)
  },
  {
    path: 'profesores',
    loadComponent: () =>
      import('./features/profesores/profesores-list/profesores-list.component')
        .then(m => m.ProfesoresListComponent)
  },
  {
    path: 'notas',
    loadComponent: () =>
      import('./features/notas/notas-list/notas-list.component')
        .then(m => m.NotasListComponent)
  },
  { path: '**', redirectTo: 'estudiantes' }
];
