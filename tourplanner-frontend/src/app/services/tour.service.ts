import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tour } from '../models/tour.model';

@Injectable({ providedIn: 'root' })
export class TourService {
  private readonly apiUrl = 'http://localhost:5210/api/tours';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Tour[]> {
    return this.http.get<Tour[]>(this.apiUrl);
  }

  getById(id: string): Observable<Tour> {
    return this.http.get<Tour>(`${this.apiUrl}/${id}`);
  }

  create(tour: Partial<Tour>): Observable<Tour> {
    return this.http.post<Tour>(this.apiUrl, tour);
  }

  update(id: string, tour: Partial<Tour>): Observable<Tour> {
    return this.http.put<Tour>(`${this.apiUrl}/${id}`, tour);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
