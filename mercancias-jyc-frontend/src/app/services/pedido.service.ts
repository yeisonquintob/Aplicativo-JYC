import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Cliente {
  clienteID: number;
  nombre: string;
}

export interface DetallePedido {
  productoID: number;
  cantidad: number;
  precioUnitario: number;
}

export interface Pedido {
  pedidoID?: number;
  clienteID: number;
  fechaPedido: string;
  fechaEntregaProgramada: string;
  estadoPedido: string;
  cliente: Cliente;
  detallesPedido: DetallePedido[];
}

@Injectable({
  providedIn: 'root'
})
export class PedidoService {
  private apiUrl = 'http://localhost:5224/api/pedidos'; // Ajusta esta URL según tu backend

  constructor(private http: HttpClient) { }

  getPedidos(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(this.apiUrl);
  }

  getPedido(id: number): Observable<Pedido> {
    return this.http.get<Pedido>(`${this.apiUrl}/${id}`);
  }

  createPedido(pedido: Pedido): Observable<Pedido> {
    return this.http.post<Pedido>(this.apiUrl, pedido);
  }

  updatePedido(pedido: Pedido): Observable<Pedido> {
    return this.http.put<Pedido>(`${this.apiUrl}/${pedido.pedidoID}`, pedido);
  }

  deletePedido(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
