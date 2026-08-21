export interface Estudiante {
  id: number;
  nombre: string;
  cantidadNotas: number;
}

export interface CrearEstudiante {
  nombre: string;
}

export interface ActualizarEstudiante {
  nombre: string;
}
