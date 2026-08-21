import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificacionService {
  private snackBar = inject(MatSnackBar);

  exito(mensaje: string): void {
    this.snackBar.open(mensaje, 'Cerrar', {
      duration: 4000,
      panelClass: ['snackbar-exito'],
      horizontalPosition: 'end',
      verticalPosition: 'top'
    });
  }

  error(mensaje: string): void {
    this.snackBar.open(mensaje, 'Cerrar', {
      duration: 6000,
      panelClass: ['snackbar-error'],
      horizontalPosition: 'end',
      verticalPosition: 'top'
    });
  }
}
