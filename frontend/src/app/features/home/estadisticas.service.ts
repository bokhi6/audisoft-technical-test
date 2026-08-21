import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/api-config';
import { Estadisticas } from '../../shared/models/estadisticas.model';

@Injectable({ providedIn: 'root' })
export class EstadisticasService {
  private http = inject(HttpClient);

  obtenerResumen(): Observable<Estadisticas> {
    return this.http.get<Estadisticas>(`${API_BASE_URL}/estadisticas`);
  }
}
