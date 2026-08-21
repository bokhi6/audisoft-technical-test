import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { CrudBaseService } from '../../shared/services/crud-base.service';
import { ActualizarNota, CrearNota, Nota } from '../../shared/models/nota.model';

@Injectable({ providedIn: 'root' })
export class NotasService extends CrudBaseService<Nota, CrearNota, ActualizarNota> {
  constructor() {
    super(inject(HttpClient), 'notas');
  }
}
