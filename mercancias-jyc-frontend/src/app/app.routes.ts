import { Routes } from '@angular/router';
import { ClientesComponent } from './components/clientes/clientes.component';
import { ProductosComponent } from './components/productos/productos.component';
import { PedidosComponent } from './components/pedidos/pedidos.component';
import { EntregasComponent } from './components/entregas/entregas.component';

export const routes: Routes = [
  { path: 'clientes', component: ClientesComponent },
  { path: 'productos', component: ProductosComponent },
  { path: 'pedidos', component: PedidosComponent },
  { path: 'entregas', component: EntregasComponent },
  { path: '', redirectTo: '/clientes', pathMatch: 'full' },
  { path: '**', redirectTo: '/clientes' }
];
