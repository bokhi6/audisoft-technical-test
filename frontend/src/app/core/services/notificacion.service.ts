import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

const toast = Swal.mixin({
  toast: true,
  position: 'top-end',
  showConfirmButton: false,
  timer: 3500,
  timerProgressBar: true,
  didOpen: (el) => {
    el.addEventListener('mouseenter', Swal.stopTimer);
    el.addEventListener('mouseleave', Swal.resumeTimer);
  }
});

@Injectable({ providedIn: 'root' })
export class NotificacionService {
  exito(mensaje: string): void {
    toast.fire({ icon: 'success', title: mensaje });
  }

  error(mensaje: string): void {
    toast.fire({ icon: 'error', title: mensaje, timer: 5000 });
  }

  async confirmarEliminar(titulo: string, mensaje: string): Promise<boolean> {
    const resultado = await Swal.fire({
      title: titulo,
      text: mensaje,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Eliminar',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#e11d48',
      cancelButtonColor: '#64748b',
      reverseButtons: true
    });
    return resultado.isConfirmed;
  }
}
