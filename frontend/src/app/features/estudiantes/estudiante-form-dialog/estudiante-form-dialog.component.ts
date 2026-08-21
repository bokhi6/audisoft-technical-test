import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Estudiante } from '../../../shared/models/estudiante.model';

export interface EstudianteFormDialogData {
  modo: 'crear' | 'editar';
  estudiante?: Estudiante;
}

@Component({
  selector: 'app-estudiante-form-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './estudiante-form-dialog.component.html'
})
export class EstudianteFormDialogComponent {
  dialogRef = inject(MatDialogRef<EstudianteFormDialogComponent>);
  data = inject<EstudianteFormDialogData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);

  form = this.fb.nonNullable.group({
    nombre: [this.data.estudiante?.nombre ?? '', [Validators.required, Validators.maxLength(200)]]
  });

  get tituloDialogo(): string {
    return this.data.modo === 'crear' ? 'Crear estudiante' : 'Editar estudiante';
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.dialogRef.close(this.form.getRawValue());
  }
}
