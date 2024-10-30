import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) {}

  get<T>(url: string): Observable<any> {
    return this.http.get<T>(`${this.apiUrl}/${url}`);
  }

  post<T>(url: string, body: any) {
    return this.http.post<T>(`${this.apiUrl}/${url}`, body);
  }

  put<T>(url: string, body: any) {
    return this.http.put<T>(`${this.apiUrl}/${url}`, body);
  }

  delete<T>(url: string): Observable<any> {
    return this.http.delete<T>(`${this.apiUrl}/${url}`);
  }
}
