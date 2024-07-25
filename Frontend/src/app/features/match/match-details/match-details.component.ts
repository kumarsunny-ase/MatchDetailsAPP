import { Component, OnInit } from '@angular/core';
import { ValueDto } from '../../models/ValueDto';
import { MatchListService } from '../../services/match-list.service';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-match-details',
  standalone: true,
  imports: [MatCardModule, CommonModule, MatTableModule],
  templateUrl: './match-details.component.html',
  styleUrl: './match-details.component.css',
})
export class MatchDetailsComponent implements OnInit {
  matchDay?: number;
  matchDetails: ValueDto[] = [];
  errorMessage?: string;

  displayedColumns: string[] = [
    'homeTeamName',
    'guestTeamName',
    'plannedKickoffTime',
    'stadiumName',
  ];

  constructor(
    private route: ActivatedRoute,
    private matchListService: MatchListService
  ) {}

  ngOnInit(): void {
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
}
