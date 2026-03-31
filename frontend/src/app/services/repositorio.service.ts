import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RepositorioService {
  [x: string]: any;
  constructor(private http: HttpClient) {}

  getRepositorios(repos: string) {
    return this.http.get<any[]>('');
  }
}