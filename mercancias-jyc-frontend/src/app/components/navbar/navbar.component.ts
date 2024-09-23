import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  template: `
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
  <a class="navbar-brand" href="#">Mercancías JyC</a>
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>
  <div class="collapse navbar-collapse" id="navbarNav">
    <ul class="navbar-nav ml-auto">
      <li class="nav-item active">
        <a class="nav-link" routerLink="/pedidos">Pedidos</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" routerLink="/productos">Productos</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" routerLink="/clientes">Clientes</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" routerLink="/entregas">Entregas</a>
      </li>
    </ul>
  </div>
</nav>

  `,
  styles: [`
    nav {
      background-color: #f8f9fa;
      padding: 10px;
    }
    ul {
      list-style-type: none;
      padding: 0;
      display: flex;
      justify-content: space-around;
    }
    a {
      text-decoration: none;
      color: #007bff;
    }
  `]
})
export class NavbarComponent { }