import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { BuscaRepositoriosParams } from '../models/busca-repositorios.model';
import { PaginacaoResultado } from '../models/paginacao-resultado.model';
import { Repositorio } from '../models/repositorio.model';

@Injectable({
  providedIn: 'root'
})
export class RepositorioApiService {
  private readonly apiUrl = `${API_BASE_URL}/repos`;

  constructor(private http: HttpClient) {}

  buscarRepositorios(params: BuscaRepositoriosParams): Observable<PaginacaoResultado<Repositorio>> {
    const httpParams = new HttpParams()
      .set('nome', params.nome)
      .set('pagina', params.pagina)
      .set('tamanhoPagina', params.tamanhoPagina);

    return this.http.get<PaginacaoResultado<Repositorio>>(this.apiUrl, {
      params: httpParams
    });
  }
}
