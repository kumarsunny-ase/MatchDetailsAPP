import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldControl } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatchListService } from '../../services/match-list.service';
import { CommonModule } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-match-list',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatProgressBarModule,
    CommonModule
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

  constructor( private fb: FormBuilder, private matchListService: MatchListService) {
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
            this.successMessage =
              responseBody.message || 'File uploaded successfully.';
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
}

