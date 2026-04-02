import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TourLog } from '../models/tour-log.model';

@Injectable({ providedIn: 'root' })
export class TourLogService {
  private readonly baseUrl = 'http://localhost:5210/api/tours';

  constructor(private http: HttpClient) {}

  getAll(tourId: string): Observable<TourLog[]> {
    return this.http.get<TourLog[]>(`${this.baseUrl}/${tourId}/logs`);
  }

  create(tourId: string, log: Partial<TourLog>): Observable<TourLog> {
    return this.http.post<TourLog>(`${this.baseUrl}/${tourId}/logs`, log);
  }

  update(tourId: string, logId: string, log: Partial<TourLog>): Observable<TourLog> {
    return this.http.put<TourLog>(`${this.baseUrl}/${tourId}/logs/${logId}`, log);
  }

  delete(tourId: string, logId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${tourId}/logs/${logId}`);
  }
}
