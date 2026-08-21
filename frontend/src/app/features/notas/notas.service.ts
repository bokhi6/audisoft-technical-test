import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CrudBaseService } from '../../shared/services/crud-base.service';
import { ActualizarNota, CrearNota, Nota } from '../../shared/models/nota.model';
import { PagedResult } from '../../shared/models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class NotasService extends CrudBaseService<Nota, CrearNota, ActualizarNota> {
  constructor() {
    super(inject(HttpClient), 'notas');
  }

  obtenerPaginadoFiltrado(
    pageNumber: number,
    pageSize: number,
    idEstudiante?: number | null,
    idProfesor?: number | null
  ): Observable<PagedResult<Nota>> {
    const params: Record<string, number> = { pageNumber, pageSize };
    if (idEstudiante) params['idEstudiante'] = idEstudiante;
    if (idProfesor) params['idProfesor'] = idProfesor;
    return this.http.get<PagedResult<Nota>>(this.resourceUrl, { params });
  }
}
