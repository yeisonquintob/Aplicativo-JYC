import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductoService, Producto } from '../../services/producto.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container mt-4">
      <h2>Productos</h2>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Descripción</th>
            <th>Precio</th>
            <th>Stock</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let producto of productos">
            <td>{{producto.productoID}}</td>
            <td>{{producto.nombre}}</td>
            <td>{{producto.descripcion}}</td>
            <td>{{producto.precio | currency}}</td>
            <td>{{producto.stock}}</td>
            <td>
              <button class="btn btn-primary btn-sm" (click)="editProducto(producto)">Editar</button>
              <button class="btn btn-danger btn-sm" (click)="deleteProducto(producto.productoID!)">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>

      <h3>{{editingProducto ? 'Editar' : 'Crear'}} Producto</h3>
      <form (ngSubmit)="saveProducto()" class="mt-3">
        <div class="form-group">
          <label for="nombre">Nombre</label>
          <input [(ngModel)]="currentProducto.nombre" name="nombre" id="nombre" class="form-control" placeholder="Nombre" required>
        </div>
        <div class="form-group">
          <label for="descripcion">Descripción</label>
          <input [(ngModel)]="currentProducto.descripcion" name="descripcion" id="descripcion" class="form-control" placeholder="Descripción">
        </div>
        <div class="form-group">
          <label for="precio">Precio</label>
          <input [(ngModel)]="currentProducto.precio" name="precio" id="precio" class="form-control" type="number" step="0.01" placeholder="Precio" required>
        </div>
        <div class="form-group">
          <label for="stock">Stock</label>
          <input [(ngModel)]="currentProducto.stock" name="stock" id="stock" class="form-control" type="number" placeholder="Stock" required>
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
export class ProductosComponent implements OnInit {
  productos: Producto[] = [];
  currentProducto: Producto = this.initializeProducto();
  editingProducto = false;

  constructor(private productoService: ProductoService) { }

  ngOnInit(): void {
    this.loadProductos();
  }

  loadProductos(): void {
    this.productoService.getProductos().subscribe(
      (data: Producto[]) => this.productos = data,
      (error: HttpErrorResponse) => console.error('Error al cargar productos:', error)
    );
  }

  editProducto(producto: Producto): void {
    this.currentProducto = { ...producto };
    this.editingProducto = true;
  }

  deleteProducto(id: number): void {
    if (confirm('¿Estás seguro de que quieres eliminar este producto?')) {
      this.productoService.deleteProducto(id).subscribe(
        () => this.loadProductos(),
        (error: HttpErrorResponse) => console.error('Error al eliminar producto:', error)
      );
    }
  }

  saveProducto(): void {
    const productoToSave: Producto = { ...this.currentProducto };

    if (this.editingProducto) {
      this.productoService.updateProducto(productoToSave).subscribe(
        () => this.loadProductos(),
        (error: HttpErrorResponse) => console.error('Error al actualizar producto:', error)
      );
    } else {
      this.productoService.createProducto(productoToSave).subscribe(
        () => this.loadProductos(),
        (error: HttpErrorResponse) => console.error('Error al crear producto:', error)
      );
    }

    this.cancelEdit();
  }

  cancelEdit(): void {
    this.currentProducto = this.initializeProducto();
    this.editingProducto = false;
  }

  private initializeProducto(): Producto {
    return {
      nombre: '',
      descripcion: '',
      precio: 0,
      stock: 0
    };
  }
}
