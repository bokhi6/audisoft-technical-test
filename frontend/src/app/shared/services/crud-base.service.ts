import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/api-config';
import { PagedResult } from '../models/paged-result.model';

export abstract class CrudBaseService<T, TCrear = Partial<T>, TActualizar = Partial<T>> {
  protected readonly resourceUrl: string;

  constructor(protected http: HttpClient, resource: string) {
    this.resourceUrl = `${API_BASE_URL}/${resource}`;
  }

  obtenerPaginado(pageNumber: number, pageSize: number): Observable<PagedResult<T>> {
    return this.http.get<PagedResult<T>>(this.resourceUrl, {
      params: { pageNumber, pageSize }
    });
  }

  obtenerPorId(id: number): Observable<T> {
    return this.http.get<T>(`${this.resourceUrl}/${id}`);
  }

  crear(dto: TCrear): Observable<T> {
    return this.http.post<T>(this.resourceUrl, dto);
  }

  actualizar(id: number, dto: TActualizar): Observable<T> {
    return this.http.put<T>(`${this.resourceUrl}/${id}`, dto);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.resourceUrl}/${id}`);
  }
}
