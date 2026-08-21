import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Profesor } from '../../../shared/models/profesor.model';
import { dividirNombreCompleto, unirNombreCompleto } from '../../../shared/utils/nombre.util';

export interface ProfesorFormDialogData {
  modo: 'crear' | 'editar';
  profesor?: Profesor;
}

@Component({
  selector: 'app-profesor-form-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule],
  templateUrl: './profesor-form-dialog.component.html'
})
export class ProfesorFormDialogComponent {
  dialogRef = inject(MatDialogRef<ProfesorFormDialogComponent>);
  data = inject<ProfesorFormDialogData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);

  private valoresIniciales = dividirNombreCompleto(this.data.profesor?.nombre ?? '');

  form = this.fb.nonNullable.group({
    nombres: [this.valoresIniciales.nombres, [Validators.required, Validators.maxLength(100)]],
    apellidos: [this.valoresIniciales.apellidos, [Validators.required, Validators.maxLength(100)]]
  });

  get tituloDialogo(): string {
    return this.data.modo === 'crear' ? 'Crear profesor' : 'Editar profesor';
  }

  get inicialesPreview(): string {
    const { nombres, apellidos } = this.form.getRawValue();
    return `${nombres.trim().charAt(0)}${apellidos.trim().charAt(0)}`.toUpperCase() || '?';
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const { nombres, apellidos } = this.form.getRawValue();
    this.dialogRef.close({ nombre: unirNombreCompleto(nombres, apellidos) });
  }
}
