export interface PaginacaoResultado<T> {
  totalItens: number;
  pagina: number;
  tamanhoPagina: number;
  itens: T[];
}
