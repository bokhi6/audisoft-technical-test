import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from '../api-config';

const CLAVE_TOKEN = 'audisoft_token';
const CLAVE_USUARIO = 'audisoft_usuario';

export interface LoginRequest {
  nombreUsuario: string;
  password: string;
}

export interface TokenResponse {
  token: string;
  nombreUsuario: string;
  expiraEn: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);

  private token = signal<string | null>(sessionStorage.getItem(CLAVE_TOKEN));
  private nombreUsuario = signal<string | null>(sessionStorage.getItem(CLAVE_USUARIO));

  estaAutenticado = computed(() => this.token() !== null);
  usuarioActual = this.nombreUsuario.asReadonly();

  login(dto: LoginRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${API_BASE_URL}/auth/login`, dto).pipe(
      tap(resultado => {
        sessionStorage.setItem(CLAVE_TOKEN, resultado.token);
        sessionStorage.setItem(CLAVE_USUARIO, resultado.nombreUsuario);
        this.token.set(resultado.token);
        this.nombreUsuario.set(resultado.nombreUsuario);
      })
    );
  }

  logout(): void {
    sessionStorage.removeItem(CLAVE_TOKEN);
    sessionStorage.removeItem(CLAVE_USUARIO);
    this.token.set(null);
    this.nombreUsuario.set(null);
  }

  getToken(): string | null {
    return this.token();
  }
}
