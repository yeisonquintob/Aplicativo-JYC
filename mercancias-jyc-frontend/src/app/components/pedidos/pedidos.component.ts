import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PedidoService, Pedido, Cliente, DetallePedido } from '../../services/pedido.service';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-pedidos',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  providers: [PedidoService],
  template: `
    <div class="container mt-4">
      <h2>Pedidos</h2>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>ID</th>
            <th>Cliente</th>
            <th>Fecha Pedido</th>
            <th>Fecha Entrega Programada</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let pedido of pedidos">
            <td>{{pedido.pedidoID}}</td>
            <td>{{pedido.cliente?.nombre || 'N/A'}}</td>
            <td>{{pedido.fechaPedido | date:'short'}}</td>
            <td>{{pedido.fechaEntregaProgramada | date:'shortDate'}}</td>
            <td>{{pedido.estadoPedido}}</td>
            <td>
              <button class="btn btn-primary btn-sm" (click)="editPedido(pedido)">Editar</button>
              <button class="btn btn-danger btn-sm" (click)="deletePedido(pedido.pedidoID!)">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>

      <h3>{{editingPedido ? 'Editar' : 'Crear'}} Pedido</h3>
      <form (ngSubmit)="savePedido()" class="mt-3">
        <h4>Información del Cliente</h4>
        <div class="form-group">
          <label for="clienteID">ID del Cliente</label>
          <input [(ngModel)]="currentPedido.cliente.clienteID" name="clienteID" id="clienteID" class="form-control" type="number" placeholder="ID del Cliente" required>
        </div>
        <div class="form-group">
          <label for="clienteNombre">Nombre del Cliente</label>
          <input [(ngModel)]="currentPedido.cliente.nombre" name="clienteNombre" id="clienteNombre" class="form-control" placeholder="Nombre del Cliente" required>
        </div>

        <h4>Información del Pedido</h4>
        <div class="form-group">
          <label for="fechaPedido">Fecha del Pedido</label>
          <input [(ngModel)]="currentPedido.fechaPedido" name="fechaPedido" id="fechaPedido" class="form-control" type="datetime-local" placeholder="Fecha del Pedido" required>
        </div>
        <div class="form-group">
          <label for="fechaEntregaProgramada">Fecha de Entrega Programada</label>
          <input [(ngModel)]="currentPedido.fechaEntregaProgramada" name="fechaEntregaProgramada" id="fechaEntregaProgramada" class="form-control" type="date" placeholder="Fecha de Entrega Programada" required>
        </div>
        <div class="form-group">
          <label for="estadoPedido">Estado del Pedido</label>
          <input [(ngModel)]="currentPedido.estadoPedido" name="estadoPedido" id="estadoPedido" class="form-control" placeholder="Estado del Pedido" required>
        </div>

        <h4>Detalles del Pedido</h4>
        <div *ngFor="let detalle of currentPedido.detallesPedido; let i = index" class="form-group">
          <div class="form-row">
            <div class="col">
              <input [(ngModel)]="detalle.productoID" name="productoID{{i}}" class="form-control" type="number" placeholder="ID del Producto" required>
            </div>
            <div class="col">
              <input [(ngModel)]="detalle.cantidad" name="cantidad{{i}}" class="form-control" type="number" placeholder="Cantidad" required>
            </div>
            <div class="col">
              <input [(ngModel)]="detalle.precioUnitario" name="precioUnitario{{i}}" class="form-control" type="number" placeholder="Precio Unitario" required>
            </div>
            <div class="col">
              <button type="button" class="btn btn-danger btn-sm" (click)="removeDetalle(i)">Eliminar</button>
            </div>
          </div>
        </div>
        <button type="button" class="btn btn-success btn-sm mt-2" (click)="addDetalle()">Agregar Detalle</button>

        <button type="submit" class="btn btn-primary mt-3">Guardar</button>
        <button type="button" class="btn btn-secondary mt-3" (click)="cancelEdit()">Cancelar</button>
      </form>
    </div>
  `,
  styles: [`
    .container {
      margin-top: 20px;
    }
    .form-group {
      margin-bottom: 15px;
    }
    .form-row {
      display: flex;
      justify-content: space-between;
    }
    .form-row .col {
      flex: 1;
      margin-right: 10px;
    }
    .form-row .col:last-child {
      margin-right: 0;
    }
  `]
})
export class PedidosComponent implements OnInit {
  pedidos: Pedido[] = [];
  currentPedido: Pedido = this.initializePedido();
  editingPedido = false;

  constructor(private pedidoService: PedidoService) { }

  ngOnInit(): void {
    this.loadPedidos();
  }

  loadPedidos(): void {
    this.pedidoService.getPedidos().subscribe(
      (data: Pedido[]) => {
        this.pedidos = data;
      },
      (error: HttpErrorResponse) => console.error('Error al cargar pedidos:', error)
    );
  }

  editPedido(pedido: Pedido): void {
    this.currentPedido = { ...pedido };
    this.editingPedido = true;
  }

  deletePedido(id: number): void {
    if (confirm('¿Estás seguro de que quieres eliminar este pedido?')) {
      this.pedidoService.deletePedido(id).subscribe(
        () => this.loadPedidos(),
        (error: HttpErrorResponse) => console.error('Error al eliminar pedido:', error)
      );
    }
  }

  savePedido(): void {
    const pedidoToSave: Pedido = {
      ...this.currentPedido
    };

    if (this.editingPedido) {
      this.pedidoService.updatePedido(pedidoToSave).subscribe(
        () => this.loadPedidos(),
        (error: HttpErrorResponse) => console.error('Error al actualizar pedido:', error)
      );
    } else {
      this.pedidoService.createPedido(pedidoToSave).subscribe(
        () => this.loadPedidos(),
        (error: HttpErrorResponse) => console.error('Error al crear pedido:', error)
      );
    }

    this.cancelEdit();
  }

  cancelEdit(): void {
    this.currentPedido = this.initializePedido();
    this.editingPedido = false;
  }

  addDetalle(): void {
    this.currentPedido.detallesPedido.push({ productoID: 0, cantidad: 0, precioUnitario: 0 });
  }

  removeDetalle(index: number): void {
    this.currentPedido.detallesPedido.splice(index, 1);
  }

  private initializePedido(): Pedido {
    return {
      clienteID: 0,
      fechaPedido: '',
      fechaEntregaProgramada: '',
      estadoPedido: '',
      cliente: { clienteID: 0, nombre: '' },
      detallesPedido: []
    };
  }
}
