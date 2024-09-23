import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Producto {
  productoID?: number;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductoService {
  private apiUrl = 'http://localhost:5224/api/productos'; // Ajusta esta URL según tu backend

  constructor(private http: HttpClient) { }

  getProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(this.apiUrl);
  }

  getProducto(id: number): Observable<Producto> {
    return this.http.get<Producto>(`${this.apiUrl}/${id}`);
  }

  createProducto(producto: Producto): Observable<Producto> {
    return this.http.post<Producto>(this.apiUrl, producto);
  }

  updateProducto(producto: Producto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${producto.productoID}`, producto);
  }

  deleteProducto(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
