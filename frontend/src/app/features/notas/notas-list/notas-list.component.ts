import { Component, OnInit, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Nota } from '../../../shared/models/nota.model';
import { NotificacionService } from '../../../core/services/notificacion.service';
import { NotasService } from '../notas.service';
import { NotaFormDialogComponent, NotaFormDialogData } from '../nota-form-dialog/nota-form-dialog.component';

@Component({
  selector: 'app-notas-list',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatButtonModule, MatIconModule, MatMenuModule, MatDialogModule],
  templateUrl: './notas-list.component.html'
})
export class NotasListComponent implements OnInit {
  private notasService = inject(NotasService);
  private dialog = inject(MatDialog);
  private notificacion = inject(NotificacionService);

  columnasVisibles = ['id', 'nombre', 'estudiante', 'profesor', 'valor', 'acciones'];
  notas = signal<Nota[]>([]);
  totalCount = signal(0);
  pageSize = signal(3);
  pageIndex = signal(0);

  ngOnInit(): void {
    this.cargarPagina();
  }

  cargarPagina(): void {
    this.notasService.obtenerPaginado(this.pageIndex() + 1, this.pageSize()).subscribe(resultado => {
      this.notas.set(resultado.items);
      this.totalCount.set(resultado.totalCount);
    });
  }

  onPageChange(evento: PageEvent): void {
    this.pageIndex.set(evento.pageIndex);
    this.pageSize.set(evento.pageSize);
    this.cargarPagina();
  }

  abrirCrear(): void {
    const data: NotaFormDialogData = { modo: 'crear' };
    const ref = this.dialog.open(NotaFormDialogComponent, { width: '480px', panelClass: 'dialog-no-padding', data });

    ref.afterClosed().subscribe(resultado => {
      if (!resultado) return;
      this.notasService.crear(resultado).subscribe({
        next: () => {
          this.notificacion.exito('La nota se ha creado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }

  abrirEditar(nota: Nota): void {
    const data: NotaFormDialogData = { modo: 'editar', nota };
    const ref = this.dialog.open(NotaFormDialogComponent, { width: '480px', panelClass: 'dialog-no-padding', data });

    ref.afterClosed().subscribe(resultado => {
      if (!resultado) return;
      this.notasService.actualizar(nota.id, resultado).subscribe({
        next: () => {
          this.notificacion.exito('La nota se ha actualizado exitosamente.');
          this.cargarPagina();
        },
        error: () => { /* el interceptor global ya muestra el mensaje de error */ }
      });
    });
  }

  async abrirEliminar(nota: Nota): Promise<void> {
    const confirmado = await this.notificacion.confirmarEliminar(
      'Eliminar nota',
      `¿Está seguro de que desea eliminar la nota "${nota.nombre}"?`
    );
    if (!confirmado) return;

    this.notasService.eliminar(nota.id).subscribe({
      next: () => {
        this.notificacion.exito('La nota se ha eliminado exitosamente.');
        this.cargarPagina();
      },
      error: () => { /* el interceptor global ya muestra el mensaje de error */ }
    });
  }
}
