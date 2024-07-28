import { Component, OnInit } from '@angular/core';
import { MatchListService } from '../../services/match-list.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import {
  MatFormField,
  MatFormFieldModule,
  MatLabel,
} from '@angular/material/form-field';
import { MatOption } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CommonModule } from '@angular/common';
import { ValueDto } from '../../models/ValueDto';

@Component({
  selector: 'app-match-select',
  standalone: true,
  imports: [
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
  templateUrl: './match-select.component.html',
  styleUrl: './match-select.component.css',
})
export class MatchSelectComponent implements OnInit {
  matchDay?: number;
  matchDays: number[] = [];
  matchDates: Date[] = [];

  constructor(
    private matchListService: MatchListService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.matchListService.matchDays$.subscribe((matchDays) => {
      this.matchDays = matchDays;

      this.matchListService.matchDates$.subscribe((matchDates) => {
        this.matchDates = matchDates;
      });
    });
  }

  onMatchDaySelect(selectedMatchDay: number): void {
    if (selectedMatchDay) {
      this.router.navigate(['/match/details'], {
        queryParams: { day: selectedMatchDay },
      });
    } else {
      console.error('Selected matchDay is undefined or invalid');
    }
  }

  onMatchDateSelect(selectedDate: Date): void {
    if (selectedDate) {
      this.router.navigate(['/match/date'], {
        queryParams: { date: selectedDate },
      });
    } else {
      console.error('Selected date is undefined or invalid');
    }
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }
  goToSelectMatch(): void {
    // Navigate to the desired component or perform an action
    console.log('Navigating to select match');
  }
}
