import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MusicaService, Musica } from './services/musica.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatToolbarModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  filtro: string = '';
  alfabeto: string[] = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('');
  musicas: Musica[] = [];
  total = 0;
  tamanhoPagina = 50;
  opcoesTamanhoPagina = [50, 100, 250, 500];
  paginaAtual = 1;
  carregando = false;
  displayedColumns: string[] = ['codigo', 'cantor', 'titulo'];

  constructor(
    private musicaService: MusicaService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.buscar();
  }

  buscarPorLetra(letra: string) {
    this.filtro = letra;
    this.buscar(true);
  }

  buscar(novaBusca: boolean = false) {
    if (novaBusca) {
      this.paginaAtual = 1;
    }
    
    this.carregando = true;
    this.musicaService.buscar(this.filtro, this.paginaAtual).subscribe({
      next: (res) => {
        this.musicas = res.itens;
        this.total = res.total;
        this.carregando = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.carregando = false;
        this.cdr.detectChanges();
        alert('Erro ao buscar músicas.');
      }
    });
  }

  aoMudarPagina(event: PageEvent) {
    this.paginaAtual = event.pageIndex + 1;
    this.tamanhoPagina = event.pageSize;
    this.buscar();
  }
}
