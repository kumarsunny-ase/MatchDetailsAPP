import { Component, OnInit } from '@angular/core';
import { MatchListService } from '../../services/match-list.service';
import { MatchDateValueDto } from '../../models/match-dateValue-request';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatNativeDateModule } from '@angular/material/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-match-date',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatTableModule,
    MatNativeDateModule,
    MatSelectModule,
    CommonModule,
  ],
  providers: [DatePipe],
  templateUrl: './match-date.component.html',
  styleUrl: './match-date.component.css',
})
export class MatchDateComponent implements OnInit {
  matches: MatchDateValueDto[] = [];
  matchDate?: Date;
  matchDates: Date[] = [];
  errorMessage: string = '';

  displayedColumns: string[] = ['homeTeamName', 'guestTeamName', 'stadiumName'];

  constructor(
    private matchListService: MatchListService,
    private route: ActivatedRoute,
    private router: Router,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.loadMatches();
  }

  loadMatches(): void {
    this.matchListService.matchDates$.subscribe((matchDates) => {
      this.matchDates = matchDates;
    });
    this.route.queryParams.subscribe((params) => {
      const matchDateParam = params['date'];
      if (matchDateParam) {
        this.matchDate = new Date(matchDateParam);
        this.matchListService
          .getMatchesByDate(this.matchDate)
          .subscribe((response: MatchDateValueDto[]) => {
            this.matches = response;
          });
      } else {
        this.errorMessage = 'Invalid match day.';
      }
    });
  }

  onMatchDateSelect(selectedMatchDate: Date) {
    if (selectedMatchDate) {
      this.router.navigate(['/match/date'], {
        queryParams: { date: selectedMatchDate },
      });
    } else {
      console.error('Selected matchDate is undefined or invalid');
      this.errorMessage = 'Please select a valid match date.';
    }
  }

  formatDate(date: Date): string | null {
    return this.datePipe.transform(date, 'fullDate'); // Customize format as needed
  }
}
