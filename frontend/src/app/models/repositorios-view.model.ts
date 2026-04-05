import { Repositorio } from './repositorio.model';

export interface RepositoriosViewModel {
  termoBusca: string;
  termoPesquisado: string;
  repositorios: Repositorio[];
  paginaAtual: number;
  totalItens: number;
  totalPaginas: number;
  tamanhoPagina: number;
  carregando: boolean;
  mensagem: string;
  erro: string;
}
