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
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MatchDateValueDto } from '../../models/match-dateValue-request';
import { Observable } from 'rxjs';
import { MatIcon } from '@angular/material/icon';


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
    MatIcon,
    CommonModule,
  ],
  providers: [DatePipe],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.css',
})
export class FileUploadComponent {
  selectedFile: File | null = null;
  uploadForm!: FormGroup;
  uploadProgress: number = -1;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  matchDays: number[] = [];
  matchDates: Date[] = [];
  selectedMatchDay: number | null = null;
  matchDetails: any[] = [];
  matches!: Observable<MatchDateValueDto[]>;
  showGoToSelectMatchButton: boolean = false;

  constructor(
    private fb: FormBuilder,
    private matchListService: MatchListService,
    private datePipe: DatePipe,
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
            if (Array.isArray(responseBody.matchDates)) {
              this.matchDates = responseBody.matchDates
                .map((dateStr: string) => {
                  const date = new Date(dateStr);
                  return isNaN(date.getTime()) ? null : date;
                })
                .filter((date: Date | null): date is Date => date !== null);
            } else {
              this.matchDates = [];
            }
            this.matchListService.setMatchDays(this.matchDays);
            this.matchListService.setMatchDates(this.matchDates);
            this.showGoToSelectMatchButton = true;
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

  goToSelectMatch(): void {
    this.router.navigate(['/match/select']);
  }

  // Method to format date using DatePipe
  formatDate(date: Date): string | null {
    return this.datePipe.transform(date, 'fullDate'); // Customize format as needed
  }
}
