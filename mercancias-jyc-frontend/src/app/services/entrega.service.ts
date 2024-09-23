import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Entrega {
  entregaID: number;
  pedidoID: number;
  fechaEntregaReal: Date;
  estadoEntrega: string;
}

@Injectable({
  providedIn: 'root'
})
export class EntregaService {
  private apiUrl = 'http://localhost:5224/api/entregas';

  constructor(private http: HttpClient) { }

  getEntregas(): Observable<Entrega[]> {
    return this.http.get<Entrega[]>(this.apiUrl);
  }

  getEntrega(id: number): Observable<Entrega> {
    return this.http.get<Entrega>(`${this.apiUrl}/${id}`);
  }

  createEntrega(entrega: Entrega): Observable<Entrega> {
    return this.http.post<Entrega>(this.apiUrl, entrega);
  }

  updateEntrega(entrega: Entrega): Observable<Entrega> {
    return this.http.put<Entrega>(`${this.apiUrl}/${entrega.entregaID}`, entrega);
  }

  deleteEntrega(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}