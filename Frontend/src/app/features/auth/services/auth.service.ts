import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../../models/login-request.model';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginResponse } from '../../models/login-response.model';
import { User } from '../../models/user.model';
import { CookieService } from 'ngx-cookie-service';
import { RegisterRequest } from '../../models/register-request.model';
import { RegisterResponse } from '../../models/register-response.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7296/api/Auth';
  $user = new BehaviorSubject<User | undefined>(undefined);

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, {
      email: request.email,
      password: request.password,
    });
  }

  setUser(user: User): void {
    this.$user.next(user);
    if (typeof window !== 'undefined') {
      localStorage.setItem('user-email', user.email);
      localStorage.setItem('user-roles', user.roles.join(','));
    }
  }

  user(): Observable<User | undefined> {
    return this.$user.asObservable();
  }

  getUser(): User | undefined {
    if (typeof window !== 'undefined') {
      const email = localStorage.getItem('user-email');
      const roles = localStorage.getItem('user-roles');

      if (email && roles) {
        const user: User = {
          email: email,
          roles: roles.split(','),
        };
        return user;
      }
    }
    return undefined;
  }

  logout(): void {
    if (this.isLocalStorageAvailable()) {
      localStorage.clear();
    } else {
      console.warn('localStorage is not available.');
    }
    this.cookieService.delete('Authorization', '/');
    this.$user.next(undefined);
  }

  // Clear the local storage
  private isLocalStorageAvailable(): boolean {
    try {
      const testKey = 'test';
      localStorage.setItem(testKey, 'testValue');
      localStorage.removeItem(testKey);
      return true;
    } catch (error) {
      return false;
    }
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, request);
  }
}
