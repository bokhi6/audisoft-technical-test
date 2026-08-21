import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface ConfirmDialogData {
  titulo: string;
  mensaje: string;
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  template: `
    <div class="w-[380px] max-w-full p-6">
      <div class="w-12 h-12 rounded-full bg-rose-50 text-rose-600 flex items-center justify-center mb-4">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M3 6h18M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2m3 0v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6h14Z"/>
        </svg>
      </div>
      <h2 class="text-lg font-semibold text-slate-800">{{ data.titulo }}</h2>
      <p class="text-sm text-slate-500 mt-2">{{ data.mensaje }}</p>

      <div class="flex justify-end gap-3 mt-6">
        <button type="button" (click)="dialogRef.close(false)"
          class="px-4 py-2 rounded-full text-sm font-medium text-slate-600 hover:bg-slate-100 transition-colors">
          Cancelar
        </button>
        <button type="button" (click)="dialogRef.close(true)"
          class="px-5 py-2 rounded-full text-sm font-semibold text-white bg-gradient-to-r from-rose-600 to-red-600 shadow-md hover:shadow-lg hover:brightness-105 transition-all">
          Eliminar
        </button>
      </div>
    </div>
  `
})
export class ConfirmDialogComponent {
  dialogRef = inject(MatDialogRef<ConfirmDialogComponent>);
  data = inject<ConfirmDialogData>(MAT_DIALOG_DATA);
}
