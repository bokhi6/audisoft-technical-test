import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EstudiantesService } from '../estudiantes/estudiantes.service';
import { ProfesoresService } from '../profesores/profesores.service';
import { NotasService } from '../notas/notas.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  private estudiantesService = inject(EstudiantesService);
  private profesoresService = inject(ProfesoresService);
  private notasService = inject(NotasService);

  totalEstudiantes = signal<number | null>(null);
  totalProfesores = signal<number | null>(null);
  totalNotas = signal<number | null>(null);

  ngOnInit(): void {
    this.estudiantesService.obtenerPaginado(1, 1).subscribe(r => this.totalEstudiantes.set(r.totalCount));
    this.profesoresService.obtenerPaginado(1, 1).subscribe(r => this.totalProfesores.set(r.totalCount));
    this.notasService.obtenerPaginado(1, 1).subscribe(r => this.totalNotas.set(r.totalCount));
  }
}
