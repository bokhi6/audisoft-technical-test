import { Component, OnInit, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Estudiante } from '../../../shared/models/estudiante.model';
import { NotificacionService } from '../../../core/services/notificacion.service';
import { EstudiantesService } from '../estudiantes.service';
import { EstudianteFormDialogComponent, EstudianteFormDialogData } from '../estudiante-form-dialog/estudiante-form-dialog.component';

@Component({
  selector: 'app-estudiantes-list',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatButtonModule, MatIconModule, MatToolbarModule, MatDialogModule],
  templateUrl: './estudiantes-list.component.html'
})
export class EstudiantesListComponent implements OnInit {
  private estudiantesService = inject(EstudiantesService);
  private dialog = inject(MatDialog);
  private notificacion = inject(NotificacionService);

  columnasVisibles = ['id', 'nombre', 'acciones'];
  estudiantes = signal<Estudiante[]>([]);
  totalCount = signal(0);
  pageSize = signal(3);
  pageIndex = signal(0);

  ngOnInit(): void {
    this.cargarPagina();
  }

  cargarPagina(): void {
    this.estudiantesService.obtenerPaginado(this.pageIndex() + 1, this.pageSize()).subscribe(resultado => {
      this.estudiantes.set(resultado.items);
      this.totalCount.set(resultado.totalCount);
    });
  }

  onPageChange(evento: PageEvent): void {
    this.pageIndex.set(evento.pageIndex);
    this.pageSize.set(evento.pageSize);
    this.cargarPagina();
  }

  abrirCrear(): void {
    const data: EstudianteFormDialogData = { modo: 'crear' };
    const ref = this.dialog.open(EstudianteFormDialogComponent, { width: '420px', data });

    ref.afterClosed().subscribe(resultado => {
      if (!resultado) return;
      this.estudiantesService.crear(resultado).subscribe({
        next: () => {
          this.notificacion.exito('El estudiante se ha creado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }

  abrirEditar(estudiante: Estudiante): void {
    const data: EstudianteFormDialogData = { modo: 'editar', estudiante };
    const ref = this.dialog.open(EstudianteFormDialogComponent, { width: '420px', data });

    ref.afterClosed().subscribe(resultado => {
      if (!resultado) return;
      this.estudiantesService.actualizar(estudiante.id, resultado).subscribe({
        next: () => {
          this.notificacion.exito('El estudiante se ha actualizado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }

  abrirEliminar(estudiante: Estudiante): void {
    const data: ConfirmDialogData = {
      titulo: 'Eliminar estudiante',
      mensaje: `¿Está seguro de que desea eliminar a "${estudiante.nombre}"?`
    };
    const ref = this.dialog.open(ConfirmDialogComponent, { width: '400px', data });

    ref.afterClosed().subscribe(confirmado => {
      if (!confirmado) return;
      this.estudiantesService.eliminar(estudiante.id).subscribe({
        next: () => {
          this.notificacion.exito('El estudiante se ha eliminado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }
}
