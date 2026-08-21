import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { NotificacionService } from './core/services/notificacion.service';

@Component({
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  private authService = inject(AuthService);
  private notificacion = inject(NotificacionService);
  private router = inject(Router);

  estaAutenticado = this.authService.estaAutenticado;
  usuarioActual = this.authService.usuarioActual;

  cerrarSesion(): void {
    this.authService.logout();
    this.notificacion.exito('Sesión cerrada.');
    this.router.navigateByUrl('/login');
  }
}
