import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Profesor } from '../../../shared/models/profesor.model';

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

  form = this.fb.nonNullable.group({
    nombre: [this.data.profesor?.nombre ?? '', [Validators.required, Validators.maxLength(200)]]
  });

  get tituloDialogo(): string {
    return this.data.modo === 'crear' ? 'Crear profesor' : 'Editar profesor';
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.dialogRef.close(this.form.getRawValue());
  }
}
