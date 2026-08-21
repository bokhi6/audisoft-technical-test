import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/api-config';
import { CrudBaseService } from '../../shared/services/crud-base.service';
import { ActualizarEstudiante, CrearEstudiante, Estudiante } from '../../shared/models/estudiante.model';
import { ItemLista } from '../../shared/models/item-lista.model';

@Injectable({ providedIn: 'root' })
export class EstudiantesService extends CrudBaseService<Estudiante, CrearEstudiante, ActualizarEstudiante> {
  constructor() {
    super(inject(HttpClient), 'estudiantes');
  }

  obtenerLista(): Observable<ItemLista[]> {
    return this.http.get<ItemLista[]>(`${API_BASE_URL}/estudiantes/lista`);
  }
}
