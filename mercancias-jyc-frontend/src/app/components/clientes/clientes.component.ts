import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClienteService, Cliente } from '../../services/cliente.service';

@Component({
  selector: 'app-clientes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container mt-4">
      <h2>Clientes</h2>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Dirección</th>
            <th>Teléfono</th>
            <th>Email</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let cliente of clientes">
            <td>{{cliente.clienteID}}</td>
            <td>{{cliente.nombre}}</td>
            <td>{{cliente.direccion}}</td>
            <td>{{cliente.telefono}}</td>
            <td>{{cliente.email}}</td>
            <td>
              <button class="btn btn-primary btn-sm" (click)="editCliente(cliente)">Editar</button>
              <button class="btn btn-danger btn-sm" (click)="deleteCliente(cliente.clienteID)">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>

      <h3>{{editingCliente ? 'Editar' : 'Crear'}} Cliente</h3>
      <form (ngSubmit)="saveCliente()" class="mt-3">
        <div class="form-group">
          <label for="nombre">Nombre</label>
          <input [(ngModel)]="currentCliente.nombre" name="nombre" id="nombre" class="form-control" placeholder="Nombre" required>
        </div>
        <div class="form-group">
          <label for="direccion">Dirección</label>
          <input [(ngModel)]="currentCliente.direccion" name="direccion" id="direccion" class="form-control" placeholder="Dirección" required>
        </div>
        <div class="form-group">
          <label for="telefono">Teléfono</label>
          <input [(ngModel)]="currentCliente.telefono" name="telefono" id="telefono" class="form-control" placeholder="Teléfono" required>
        </div>
        <div class="form-group">
          <label for="email">Email</label>
          <input [(ngModel)]="currentCliente.email" name="email" id="email" class="form-control" placeholder="Email" required>
        </div>
        <button type="submit" class="btn btn-primary">Guardar</button>
        <button type="button" class="btn btn-secondary" (click)="cancelEdit()">Cancelar</button>
      </form>
    </div>
  `,
  styles: [`
    .container {
      margin-top: 20px;
    }
    .table th, .table td {
      text-align: left;
    }
    .form-group {
      margin-bottom: 15px;
    }
  `]
})
export class ClientesComponent implements OnInit {
  clientes: Cliente[] = [];
  currentCliente: Cliente = this.initializeCliente();
  editingCliente = false;

  constructor(private clienteService: ClienteService) { }

  ngOnInit(): void {
    this.loadClientes();
  }

  loadClientes(): void {
    this.clienteService.getClientes().subscribe(
      (data: Cliente[]) => this.clientes = data,
      (error: any) => console.error('Error al cargar clientes:', error)
    );
  }

  editCliente(cliente: Cliente): void {
    this.currentCliente = { ...cliente };
    this.editingCliente = true;
  }

  deleteCliente(id: number): void {
    if (confirm('¿Estás seguro de que quieres eliminar este cliente?')) {
      this.clienteService.deleteCliente(id).subscribe(
        () => this.loadClientes(),
        (error: any) => console.error('Error al eliminar cliente:', error)
      );
    }
  }

  saveCliente(): void {
    const clienteToSave: Cliente = { ...this.currentCliente };

    if (this.editingCliente) {
      this.clienteService.updateCliente(clienteToSave).subscribe(
        () => this.loadClientes(),
        (error: any) => console.error('Error al actualizar cliente:', error)
      );
    } else {
      this.clienteService.createCliente(clienteToSave).subscribe(
        () => this.loadClientes(),
        (error: any) => console.error('Error al crear cliente:', error)
      );
    }

    this.cancelEdit();
  }

  cancelEdit(): void {
    this.currentCliente = this.initializeCliente();
    this.editingCliente = false;
  }

  private initializeCliente(): Cliente {
    return {
      clienteID: 0,
      nombre: '',
      direccion: '',
      telefono: '',
      email: ''
    };
  }
}
