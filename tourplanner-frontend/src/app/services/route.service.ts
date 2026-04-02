import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RouteInfo {
  distance: number;
  estimatedTime: string;
  coordinates: number[][];
}

@Injectable({ providedIn: 'root' })
export class RouteService {
  private readonly apiUrl = 'http://localhost:5210/api/route';

  constructor(private http: HttpClient) {}

  getRoute(from: string, to: string, transportType: string): Observable<RouteInfo> {
    return this.http.get<RouteInfo>(this.apiUrl, {
      params: { from, to, transportType }
    });
  }
}
