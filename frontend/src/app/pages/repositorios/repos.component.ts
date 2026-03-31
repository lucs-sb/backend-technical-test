import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RepositorioService } from '../../services/repositorio.service';

@Component({
  selector: 'app-repositorios',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule], 
  templateUrl: './repos.component.html',
  styleUrls: ['./repos.component.css']
})
export class RepositoriosComponent implements OnInit {
  repos: string = '';
  repositorios: any[] = [];

  constructor(private repositorioService: RepositorioService) {}

  ngOnInit(): void {}

  buscarRepos(): void {
    if (!this.repos) return;

    this.repositorioService.getRepositorios(this.repos);
  }
}