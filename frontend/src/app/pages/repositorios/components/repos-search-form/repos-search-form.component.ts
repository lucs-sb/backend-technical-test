import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-repos-search-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './repos-search-form.component.html',
  styleUrl: './repos-search-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReposSearchFormComponent {
  @Input({ required: true }) termoBusca = '';
  @Input() carregando = false;

  @Output() termoBuscaChange = new EventEmitter<string>();
  @Output() buscar = new EventEmitter<void>();

  atualizarTermoBusca(termo: string): void {
    this.termoBuscaChange.emit(termo);
  }

  submeterBusca(): void {
    this.buscar.emit();
  }
}
