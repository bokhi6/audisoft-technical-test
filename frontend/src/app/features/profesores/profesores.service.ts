import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/api-config';
import { CrudBaseService } from '../../shared/services/crud-base.service';
import { ActualizarProfesor, CrearProfesor, Profesor } from '../../shared/models/profesor.model';
import { ItemLista } from '../../shared/models/item-lista.model';

@Injectable({ providedIn: 'root' })
export class ProfesoresService extends CrudBaseService<Profesor, CrearProfesor, ActualizarProfesor> {
  constructor() {
    super(inject(HttpClient), 'profesores');
  }

  obtenerLista(): Observable<ItemLista[]> {
    return this.http.get<ItemLista[]>(`${API_BASE_URL}/profesores/lista`);
  }
}
