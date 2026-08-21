export interface Profesor {
  id: number;
  nombre: string;
  cantidadNotas: number;
}

export interface CrearProfesor {
  nombre: string;
}

export interface ActualizarProfesor {
  nombre: string;
}
