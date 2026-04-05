import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { Favorito } from '../models/favorito.model';
import { FavoritoRequest } from '../models/favorito-request.model';

@Injectable({
  providedIn: 'root'
})
export class FavoritosService {
  private readonly apiUrl = `${API_BASE_URL}/favoritos`;

  constructor(private http: HttpClient) {}

  listarFavoritos(): Observable<Favorito[]> {
    return this.http.get<Favorito[]>(this.apiUrl);
  }

  adicionarFavorito(favorito: FavoritoRequest): Observable<void> {
    return this.http.post<void>(this.apiUrl, favorito);
  }

  removerFavorito(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
