import { Routes } from '@angular/router';
import { authGuard } from './auth/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/tours', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'tours',
    loadComponent: () => import('./pages/tours/tours.component').then(m => m.ToursComponent),
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: '/tours' }
];
