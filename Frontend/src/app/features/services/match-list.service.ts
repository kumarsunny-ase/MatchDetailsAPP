import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ValueDto } from '../models/ValueDto';

@Injectable({
  providedIn: 'root',
})
export class MatchListService {
  private apiUrl = 'https://localhost:7296/api/XmlFile';

  constructor(private http: HttpClient) {}

  uploadXml(file: File): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);

    const headers = new HttpHeaders({
      Accept: 'application/json',
    });

    return this.http.post(`${this.apiUrl}/upload`, formData, {
      headers,
      observe: 'response',
    });
  }

  getMatchDetailsByDay(matchDay: number): Observable<ValueDto[]> {
    return this.http.get<ValueDto[]>(`${this.apiUrl}/byMatchDay/${matchDay}`);
  }
}
