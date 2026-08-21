import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { NotificacionService } from '../../../core/services/notificacion.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private notificacion = inject(NotificacionService);
  private router = inject(Router);

  cargando = signal(false);

  form = this.fb.nonNullable.group({
    nombreUsuario: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  ingresar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.cargando.set(true);
    this.authService.login(this.form.getRawValue()).subscribe({
      next: () => {
        this.notificacion.exito('Sesión iniciada correctamente.');
        this.router.navigateByUrl('/');
      },
      error: () => {
        this.cargando.set(false);
        /* el interceptor global ya muestra el mensaje de error */
      }
    });
  }
}
