import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/repositorios/repos.component').then(m => m.RepositoriosComponent),
  },
  {
    path: 'favoritos',
    loadComponent: () =>
      import('./pages/favoritos/favoritos.component').then(m => m.FavoritosComponent),
  },
  {
    path: '**',
    redirectTo: ''
  }
];
