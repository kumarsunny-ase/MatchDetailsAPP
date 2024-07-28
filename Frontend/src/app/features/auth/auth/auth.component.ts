import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, NgForm } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LoginRequest } from '../../models/login-request.model';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { RegisterRequest } from '../../models/register-request.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MatCardModule,
    MatTabsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    FormsModule,
    MatSnackBarModule,
    CommonModule,
  ],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css',
})
export class AuthComponent {
  loginModel: LoginRequest = { email: '', password: '' };
  registerModel: RegisterRequest = { email: '', password: '' };
  isSubmitting = false;
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private cookieService: CookieService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.loginModel = {
      email: '',
      password: '',
    };
    this.registerModel = {
      email: '',
      password: '',
    };
  }

  onLogin(form: NgForm): void {
    if (form.invalid) {
      // Handle form validation error
      this.snackBar.open('Please fill in all required fields.', 'Close', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      return;
    }

    this.authService.login(this.loginModel).subscribe({
      next: (response) => {
        // Set Auth Cookie
        this.cookieService.set(
          'Authorization',
          `Bearer ${response.token}`,
          undefined,
          '/',
          undefined,
          true,
          'Strict'
        );

        // Set User
        this.authService.setUser({
          email: response.email,
          roles: response.roles,
        });

        // Redirect back to Upload Page
        this.router.navigateByUrl('match/list');
      },
      error: (err) => {
        console.error('Login failed', err);
        this.snackBar.open('Login failed. Please try again.', 'Close', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      },
    });
  }

  onRegister(form: NgForm): void {
    if (form.invalid) {
      this.snackBar.open('Please fill in all required fields.', 'Close', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      return;
    }

    this.isSubmitting = true;
    const registerRequest: RegisterRequest = this.registerModel;

    this.authService.register(registerRequest).subscribe({
      next: (response) => {
        // Show success message
        this.snackBar.open('Registration successful! Please Login', 'Close', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

        form.resetForm();
        this.registerModel = { email: '', password: '' }; // Clear form
        this.isSubmitting = false;
        this.router.navigateByUrl('');
      },
      error: (err) => {
        // Handle registration error
        console.error('Registration failed', err);
        this.errorMessage = 'Registration failed. Please try again.';
        this.snackBar.open('Registration failed. Please try again.', 'Close', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.isSubmitting = false;
      },
    });
  }

  isValidEmail(email: string): boolean {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
  }
}
