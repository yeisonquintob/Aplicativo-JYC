import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EntregaService, Entrega } from '../../services/entrega.service';

@Component({
  selector: 'app-entregas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container mt-4">
      <h2>Entregas</h2>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>ID</th>
            <th>Pedido ID</th>
            <th>Fecha Entrega Real</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let entrega of entregas">
            <td>{{entrega.entregaID}}</td>
            <td>{{entrega.pedidoID}}</td>
            <td>{{entrega.fechaEntregaReal | date}}</td>
            <td>{{entrega.estadoEntrega}}</td>
            <td>
              <button class="btn btn-primary btn-sm" (click)="editEntrega(entrega)">Editar</button>
              <button class="btn btn-danger btn-sm" (click)="deleteEntrega(entrega.entregaID)">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>

      <h3>{{editingEntrega ? 'Editar' : 'Crear'}} Entrega</h3>
      <form (ngSubmit)="saveEntrega()" class="mt-3">
        <div class="form-group">
          <label for="pedidoID">ID del Pedido</label>
          <input [(ngModel)]="currentEntrega.pedidoID" name="pedidoID" id="pedidoID" type="number" class="form-control" placeholder="ID del Pedido" required>
        </div>
        <div class="form-group">
          <label for="fechaEntregaReal">Fecha de Entrega Real</label>
          <input [(ngModel)]="currentEntrega.fechaEntregaReal" name="fechaEntregaReal" id="fechaEntregaReal" type="date" class="form-control" placeholder="Fecha de Entrega Real" required>
        </div>
        <div class="form-group">
          <label for="estadoEntrega">Estado de la Entrega</label>
          <input [(ngModel)]="currentEntrega.estadoEntrega" name="estadoEntrega" id="estadoEntrega" class="form-control" placeholder="Estado de la Entrega" required>
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
export class EntregasComponent implements OnInit {
  entregas: Entrega[] = [];
  currentEntrega: Entrega = {} as Entrega;
  editingEntrega = false;

  constructor(private entregaService: EntregaService) { }

  ngOnInit(): void {
    this.loadEntregas();
  }

  loadEntregas(): void {
    this.entregaService.getEntregas().subscribe(
      (data) => this.entregas = data,
      (error) => console.error('Error al cargar entregas:', error)
    );
  }

  editEntrega(entrega: Entrega): void {
    this.currentEntrega = { ...entrega };
    this.editingEntrega = true;
  }

  deleteEntrega(id: number): void {
    if (confirm('¿Estás seguro de que quieres eliminar esta entrega?')) {
      this.entregaService.deleteEntrega(id).subscribe(
        () => this.loadEntregas(),
        (error) => console.error('Error al eliminar entrega:', error)
      );
    }
  }

  saveEntrega(): void {
    if (this.editingEntrega) {
      this.entregaService.updateEntrega(this.currentEntrega).subscribe(
        () => {
          this.loadEntregas();
          this.cancelEdit();
        },
        (error) => console.error('Error al actualizar entrega:', error)
      );
    } else {
      this.entregaService.createEntrega(this.currentEntrega).subscribe(
        () => {
          this.loadEntregas();
          this.cancelEdit();
        },
        (error) => console.error('Error al crear entrega:', error)
      );
    }
  }

  cancelEdit(): void {
    this.currentEntrega = {} as Entrega;
    this.editingEntrega = false;
  }
}
