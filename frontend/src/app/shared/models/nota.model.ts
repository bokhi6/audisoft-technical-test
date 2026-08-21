export interface Nota {
  id: number;
  nombre: string;
  idEstudiante: number;
  nombreEstudiante: string;
  idProfesor: number;
  nombreProfesor: string;
  valor: number;
}

export interface CrearNota {
  nombre: string;
  idEstudiante: number;
  idProfesor: number;
  valor: number;
}

export interface ActualizarNota {
  nombre: string;
  idEstudiante: number;
  idProfesor: number;
  valor: number;
}
