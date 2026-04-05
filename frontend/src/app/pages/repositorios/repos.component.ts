import { AsyncPipe, CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { BehaviorSubject, Subject, catchError, combineLatest, map, merge, of, shareReplay, startWith, switchMap } from 'rxjs';
import { BuscaRepositoriosParams } from '../../models/busca-repositorios.model';
import { PaginacaoResultado } from '../../models/paginacao-resultado.model';
import { Repositorio } from '../../models/repositorio.model';
import { RepositoriosViewModel } from '../../models/repositorios-view.model';
import { ReposResultsListComponent } from './components/repos-results-list/repos-results-list.component';
import { ReposSearchFormComponent } from './components/repos-search-form/repos-search-form.component';
import { RepositorioService } from '../../services/repositorio.service';

const TAMANHO_PAGINA_PADRAO = 10;
const MENSAGEM_BUSCA_VAZIA = 'Informe um termo para buscar repositórios.';
const MENSAGEM_SEM_RESULTADOS = 'Nenhum repositório foi encontrado para a busca informada.';
const MENSAGEM_ERRO_API = 'Não foi possível carregar os repositórios agora. Tente novamente em instantes.';

const ESTADO_INICIAL_BUSCA: Omit<RepositoriosViewModel, 'termoBusca'> = {
  termoPesquisado: '',
  repositorios: [],
  paginaAtual: 1,
  totalItens: 0,
  totalPaginas: 0,
  tamanhoPagina: TAMANHO_PAGINA_PADRAO,
  carregando: false,
  mensagem: MENSAGEM_BUSCA_VAZIA,
  erro: ''
};

@Component({
  selector: 'app-repositorios',
  standalone: true,
  imports: [CommonModule, AsyncPipe, ReposSearchFormComponent, ReposResultsListComponent],
  templateUrl: './repos.component.html',
  styleUrls: ['./repos.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RepositoriosComponent {
  private readonly repositorioService = inject(RepositorioService);
  private readonly termoBuscaSubject = new BehaviorSubject<string>('');
  private readonly comandoBuscaSubject = new Subject<BuscaRepositoriosParams>();
  private readonly estadoManualSubject = new Subject<Omit<RepositoriosViewModel, 'termoBusca'>>();

  private readonly estadoBusca$ = mergeEstadoBusca(
    this.comandoBuscaSubject,
    this.estadoManualSubject,
    this.repositorioService
  );

  readonly vm$ = combineLatest([
    this.termoBuscaSubject,
    this.estadoBusca$
  ]).pipe(
    map(([termoBusca, estadoBusca]) => ({
      termoBusca,
      ...estadoBusca
    })),
    shareReplay({ bufferSize: 1, refCount: true })
  );

  atualizarTermoBusca(termo: string): void {
    this.termoBuscaSubject.next(termo);
  }

  buscarRepositorios(): void {
    const termo = this.obterTermoNormalizado();

    if (!termo) {
      this.estadoManualSubject.next({
        ...ESTADO_INICIAL_BUSCA,
        mensagem: MENSAGEM_BUSCA_VAZIA
      });
      return;
    }

    this.executarBusca(1, termo);
  }

  mudarPagina(pagina: number): void {
    const termo = this.obterTermoNormalizado();

    if (!termo || pagina < 1) {
      return;
    }

    this.executarBusca(pagina, termo);
  }

  private executarBusca(pagina: number, nome: string): void {
    this.comandoBuscaSubject.next({
      nome,
      pagina,
      tamanhoPagina: TAMANHO_PAGINA_PADRAO
    });
  }

  private obterTermoNormalizado(): string {
    return this.termoBuscaSubject.value.trim();
  }
}

function mergeEstadoBusca(
  comandoBusca$: Subject<BuscaRepositoriosParams>,
  estadoManual$: Subject<Omit<RepositoriosViewModel, 'termoBusca'>>,
  repositorioService: RepositorioService
) {
  return merge(
    estadoManual$,
    comandoBusca$.pipe(
      switchMap((params) =>
        repositorioService.buscarRepositorios(params).pipe(
          map((resultado) => mapearSucessoBusca(resultado, params)),
          startWith(mapearCarregandoBusca(params)),
          catchError(() => of(mapearErroBusca(params)))
        )
      ),
    )
  ).pipe(
    startWith(ESTADO_INICIAL_BUSCA)
  );
}

function mapearCarregandoBusca(params: BuscaRepositoriosParams): Omit<RepositoriosViewModel, 'termoBusca'> {
  return {
    termoPesquisado: params.nome,
    repositorios: [],
    paginaAtual: params.pagina,
    totalItens: 0,
    totalPaginas: 0,
    tamanhoPagina: params.tamanhoPagina,
    carregando: true,
    mensagem: '',
    erro: ''
  };
}

function mapearSucessoBusca(
  resultado: PaginacaoResultado<Repositorio>,
  params: BuscaRepositoriosParams
): Omit<RepositoriosViewModel, 'termoBusca'> {
  return {
    termoPesquisado: params.nome,
    repositorios: resultado.itens,
    paginaAtual: resultado.pagina,
    totalItens: resultado.totalItens,
    totalPaginas: calcularTotalPaginas(resultado.totalItens, resultado.tamanhoPagina),
    tamanhoPagina: resultado.tamanhoPagina,
    carregando: false,
    mensagem: resultado.itens.length ? '' : MENSAGEM_SEM_RESULTADOS,
    erro: ''
  };
}

function mapearErroBusca(params: BuscaRepositoriosParams): Omit<RepositoriosViewModel, 'termoBusca'> {
  return {
    termoPesquisado: params.nome,
    repositorios: [],
    paginaAtual: 1,
    totalItens: 0,
    totalPaginas: 0,
    tamanhoPagina: params.tamanhoPagina,
    carregando: false,
    mensagem: '',
    erro: MENSAGEM_ERRO_API
  };
}

function calcularTotalPaginas(totalItens: number, tamanhoPagina: number): number {
  return totalItens > 0 ? Math.ceil(totalItens / tamanhoPagina) : 0;
}
