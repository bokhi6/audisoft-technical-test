import { Component, OnInit, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Profesor } from '../../../shared/models/profesor.model';
import { NotificacionService } from '../../../core/services/notificacion.service';
import { ProfesoresService } from '../profesores.service';
import { ProfesorFormDialogComponent, ProfesorFormDialogData } from '../profesor-form-dialog/profesor-form-dialog.component';

@Component({
  selector: 'app-profesores-list',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatButtonModule, MatIconModule, MatDialogModule],
  templateUrl: './profesores-list.component.html'
})
export class ProfesoresListComponent implements OnInit {
  private profesoresService = inject(ProfesoresService);
  private dialog = inject(MatDialog);
  private notificacion = inject(NotificacionService);

  columnasVisibles = ['id', 'nombre', 'acciones'];
  profesores = signal<Profesor[]>([]);
  totalCount = signal(0);
  pageSize = signal(3);
  pageIndex = signal(0);

  ngOnInit(): void {
    this.cargarPagina();
  }

  cargarPagina(): void {
    this.profesoresService.obtenerPaginado(this.pageIndex() + 1, this.pageSize()).subscribe(resultado => {
      this.profesores.set(resultado.items);
      this.totalCount.set(resultado.totalCount);
    });
  }

  onPageChange(evento: PageEvent): void {
    this.pageIndex.set(evento.pageIndex);
    this.pageSize.set(evento.pageSize);
    this.cargarPagina();
  }

  abrirCrear(): void {
    const data: ProfesorFormDialogData = { modo: 'crear' };
    const ref = this.dialog.open(ProfesorFormDialogComponent, { width: '420px', panelClass: 'dialog-no-padding', data });

    ref.afterClosed().subscribe(resultado => {
      if (!resultado) return;
      this.profesoresService.crear(resultado).subscribe({
        next: () => {
          this.notificacion.exito('El profesor se ha creado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }

  abrirEditar(profesor: Profesor): void {
    const data: ProfesorFormDialogData = { modo: 'editar', profesor };
    const ref = this.dialog.open(ProfesorFormDialogComponent, { width: '420px', panelClass: 'dialog-no-padding', data });

    ref.afterClosed().subscribe(resultado => {
      if (!resultado) return;
      this.profesoresService.actualizar(profesor.id, resultado).subscribe({
        next: () => {
          this.notificacion.exito('El profesor se ha actualizado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }

  abrirEliminar(profesor: Profesor): void {
    const data: ConfirmDialogData = {
      titulo: 'Eliminar profesor',
      mensaje: `¿Está seguro de que desea eliminar a "${profesor.nombre}"?`
    };
    const ref = this.dialog.open(ConfirmDialogComponent, { width: '380px', panelClass: 'dialog-no-padding', data });

    ref.afterClosed().subscribe(confirmado => {
      if (!confirmado) return;
      this.profesoresService.eliminar(profesor.id).subscribe({
        next: () => {
          this.notificacion.exito('El profesor se ha eliminado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }
}
