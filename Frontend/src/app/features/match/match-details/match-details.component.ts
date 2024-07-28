import { Component, OnInit } from '@angular/core';
import { ValueDto } from '../../models/ValueDto';
import { MatchListService } from '../../services/match-list.service';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { CommonModule, DatePipe, Location } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOption, MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { MatTooltip } from '@angular/material/tooltip';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-match-details',
  standalone: true,
  imports: [
    MatCardModule,
    CommonModule,
    MatTableModule,
    MatFormFieldModule,
    MatOption,
    MatSelectModule,
    MatTooltip,
    MatIcon,
  ],
  providers: [DatePipe],
  templateUrl: './match-details.component.html',
  styleUrl: './match-details.component.css',
})
export class MatchDetailsComponent implements OnInit {
  matchDay?: number;
  matchDetails: ValueDto[] = [];
  errorMessage?: string;
  selectedMatchDay: number | null = null;
  matchDays: number[] = [];

  displayedColumns: string[] = [
    'homeTeamName',
    'guestTeamName',
    'plannedKickoffTime',
    'stadiumName',
  ];

  constructor(
    private route: ActivatedRoute,
    private matchListService: MatchListService,
    private router: Router,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.matchListService.matchDays$.subscribe((matchDays) => {
      this.matchDays = matchDays;
    });
    this.route.queryParams.subscribe((params) => {
      const matchDayParam = params['day'];
      if (matchDayParam) {
        this.matchDay = +matchDayParam; // Convert to number
        this.matchListService.getMatchDetailsByDay(this.matchDay).subscribe(
          (response: ValueDto[]) => {
            this.matchDetails = response;
            // this.errorMessage = null; // Clear any previous error message
          },
          (error) => {
            console.error('Error fetching match details:', error);
            this.errorMessage = `Error fetching match details: ${error.message}`;
          }
        );
      } else {
        this.errorMessage = 'Invalid match day.';
      }
    });
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

  selectTeamName(teamName: string): void {
    if (teamName) {
      this.router.navigate(['/team/details'], {
        queryParams: { tName: teamName },
      });
    }
  }

  formatDate(date: Date): string | null {
    return this.datePipe.transform(date, 'fullDate'); // Customize format as needed
  }
}
