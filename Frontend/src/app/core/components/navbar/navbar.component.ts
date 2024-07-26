import { Component, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '../../../features/auth/services/auth.service';
import { User } from '../../../features/models/user.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    RouterLink,
    MatFormFieldModule,
    CommonModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  user?: User;
  isLoading = true; // Add loading state

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    // Fetch the user and handle the loading state
    this.authService.user().subscribe({
      next: (response) => {
        this.user = response;
        this.isLoading = false; // Update loading state when data is received
      },
      error: (err) => {
        console.error('Error fetching user data:', err);
        this.isLoading = false; // Stop loading in case of error
      },
    });
    this.user = this.authService.getUser();
  }

  onLogout() {
    this.authService.logout();
    this.router.navigateByUrl('/'); // Navigate to root after logout
  }
}
