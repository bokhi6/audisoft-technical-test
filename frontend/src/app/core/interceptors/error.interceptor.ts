import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificacionService } from '../services/notificacion.service';

interface ProblemDetails {
  title?: string;
  detail?: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notificacion = inject(NotificacionService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const problemDetails = error.error as ProblemDetails | undefined;
      const mensaje = problemDetails?.detail
        ?? 'Ocurrió un error al comunicarse con el servidor. Intente nuevamente.';

      notificacion.error(mensaje);
      return throwError(() => error);
    })
  );
};
