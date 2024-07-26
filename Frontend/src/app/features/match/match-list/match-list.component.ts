import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatchListService } from '../../services/match-list.service';
import { CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { MatFormField, MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatOption } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { ValueDto } from '../../models/ValueDto';
import { Router } from '@angular/router';


@Component({
  selector: 'app-match-list',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatFormField,
    MatOption,
    MatLabel,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressBarModule,
    CommonModule,
  ],
  templateUrl: './match-list.component.html',
  styleUrl: './match-list.component.css',
})
export class MatchListComponent {
  selectedFile: File | null = null;
  uploadForm!: FormGroup;
  uploadProgress: number = -1;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  matchDays: number[] = [];
  selectedMatchDay: number | null = null;
  matchDetails: any[] = [];

  constructor(
    private fb: FormBuilder,
    private matchListService: MatchListService,
    private router: Router
  ) {
    this.uploadForm = this.fb.group({});
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload(): void {
    if (this.selectedFile) {
      this.successMessage = null; // Clear any previous success message
      this.errorMessage = null; // Clear any previous error message
      this.matchListService.uploadXml(this.selectedFile).subscribe(
        (response) => {
          // Check for a successful response
          if (
            response instanceof HttpResponse &&
            (response.status === 200 || response.status === 201)
          ) {
            const responseBody: any = response.body || response;
            this.successMessage = responseBody.message;
            this.matchDays = responseBody.matchDays || [];
            this.matchListService.setMatchDays(this.matchDays);
          } else {
            this.errorMessage = `Unexpected response status: ${response.status}`;
          }
        },
        (error) => {
          console.error('Upload error:', error); // Log the error for debugging
          this.errorMessage = `Error uploading file: ${error.message}`;
        }
      );
    }
  }

  onMatchDaySelect(selectedMatchDay: number): void {
    if (selectedMatchDay) {
      // Navigate to match details page with the selected match day as a query parameter
      this.router.navigate(['/match/details'], {
        queryParams: { day: selectedMatchDay },
      });
    } else {
      console.error('Selected matchDay is undefined or invalid');
      this.errorMessage = 'Please select a valid match day.';
    }
  }
}
