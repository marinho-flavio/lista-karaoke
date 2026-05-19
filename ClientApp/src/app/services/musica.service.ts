import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Musica {
  id: number;
  cantor: string;
  codigo: string;
  titulo: string;
  inicioLetra: string;
}

export interface ResultadoPaginado {
  total: number;
  pagina: number;
  tamanhoPagina: number;
  itens: Musica[];
}

@Injectable({
  providedIn: 'root'
})
export class MusicaService {
  private apiUrl = '/api/karaoke';

  constructor(private http: HttpClient) { }

  buscar(filtro: string = '', pagina: number = 1): Observable<ResultadoPaginado> {
    return this.http.get<ResultadoPaginado>(`${this.apiUrl}?filtro=${filtro}&pagina=${pagina}`);
  }
}
