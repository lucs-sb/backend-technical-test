import { ChangeDetectionStrategy, Component, DestroyRef, EventEmitter, Input, Output, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-repos-search-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './repos-search-form.component.html',
  styleUrl: './repos-search-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReposSearchFormComponent {
  private readonly destroyRef = inject(DestroyRef);
  private ultimoTermoBusca = '';

  @Input({ required: true })
  set termoBusca(valor: string) {
    if (valor === this.ultimoTermoBusca) {
      return;
    }

    this.ultimoTermoBusca = valor;
    this.termoBuscaControl.setValue(valor, { emitEvent: false });
  }

  @Input() carregando = false;

  @Output() termoBuscaChange = new EventEmitter<string>();
  @Output() buscar = new EventEmitter<void>();

  readonly termoBuscaControl = new FormControl('', { nonNullable: true });

  constructor() {
    this.termoBuscaControl.valueChanges
      .pipe(
        distinctUntilChanged(),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe((termo) => {
        this.ultimoTermoBusca = termo;
        this.termoBuscaChange.emit(termo);
      });
  }

  submeterBusca(): void {
    this.buscar.emit();
  }
}
