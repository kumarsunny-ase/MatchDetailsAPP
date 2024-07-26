import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ValueDto } from '../models/ValueDto';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class MatchListService {
  private apiUrl = 'https://localhost:7296/api/XmlFile';

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  uploadXml(file: File): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);

    const headers = new HttpHeaders({
      Accept: 'application/json',
    });

    return this.http.post(`${this.apiUrl}/upload`, formData, {
      observe: 'response',
      headers: {
        Authorization: this.cookieService.get('Authorization'),
      },
    });
  }

  getMatchDetailsByDay(matchDay: number): Observable<ValueDto[]> {
    return this.http.get<ValueDto[]>(`${this.apiUrl}/byMatchDay/${matchDay}`, {
      headers: {
        'Authorization': this.cookieService.get('Authorization')
      }
    });
  }
  private matchDaysSubject = new BehaviorSubject<any[]>([]);
  matchDays$ = this.matchDaysSubject.asObservable();

  setMatchDays(matchDays: any[]) {
    this.matchDaysSubject.next(matchDays);
  }
}
