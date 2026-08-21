import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Nota } from '../../../shared/models/nota.model';
import { ItemLista } from '../../../shared/models/item-lista.model';
import { EstudiantesService } from '../../estudiantes/estudiantes.service';
import { ProfesoresService } from '../../profesores/profesores.service';

export interface NotaFormDialogData {
  modo: 'crear' | 'editar';
  nota?: Nota;
}

@Component({
  selector: 'app-nota-form-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  templateUrl: './nota-form-dialog.component.html'
})
export class NotaFormDialogComponent implements OnInit {
  dialogRef = inject(MatDialogRef<NotaFormDialogComponent>);
  data = inject<NotaFormDialogData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private estudiantesService = inject(EstudiantesService);
  private profesoresService = inject(ProfesoresService);

  estudiantes = signal<ItemLista[]>([]);
  profesores = signal<ItemLista[]>([]);

  form = this.fb.nonNullable.group({
    nombre: [this.data.nota?.nombre ?? '', [Validators.required, Validators.maxLength(200)]],
    idEstudiante: [this.data.nota?.idEstudiante ?? null, [Validators.required]],
    idProfesor: [this.data.nota?.idProfesor ?? null, [Validators.required]],
    valor: [this.data.nota?.valor ?? null, [Validators.required, Validators.min(0), Validators.max(5)]]
  });

  get tituloDialogo(): string {
    return this.data.modo === 'crear' ? 'Crear nota' : 'Editar nota';
  }

  ngOnInit(): void {
    this.estudiantesService.obtenerLista().subscribe(lista => this.estudiantes.set(lista));
    this.profesoresService.obtenerLista().subscribe(lista => this.profesores.set(lista));
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.dialogRef.close(this.form.getRawValue());
  }
}
