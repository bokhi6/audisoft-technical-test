import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { NotificacionService } from '../services/notificacion.service';
import { AuthService } from '../services/auth.service';

interface ProblemDetails {
  title?: string;
  detail?: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notificacion = inject(NotificacionService);
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const problemDetails = error.error as ProblemDetails | undefined;
      const mensaje = problemDetails?.detail
        ?? 'Ocurrió un error al comunicarse con el servidor. Intente nuevamente.';

      notificacion.error(mensaje);

      if (error.status === 401) {
        authService.logout();
        router.navigate(['/login']);
      }

      return throwError(() => error);
    })
  );
};
