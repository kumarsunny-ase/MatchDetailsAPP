import { Component, OnInit } from '@angular/core';
import { MatchListService } from '../../services/match-list.service';
import { ActivatedRoute } from '@angular/router';
import { TeamDetailsDto } from '../../models/team-details.model';
import { CommonModule, DatePipe, Location } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-team-details',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatTooltipModule,
    MatIcon,
  ],
  providers: [DatePipe],
  templateUrl: './team-details.component.html',
  styleUrl: './team-details.component.css',
})
export class TeamDetailsComponent implements OnInit {
  teamDetails: TeamDetailsDto[] = [];
  teamName: string = '';

  displayedColumns: string[] = [
    'matchDay',
    'matchType',
    'guestTeamName',
    'plannedKickoffTime',
    'stadiumId',
    'stadiumName',
    'season',
    'competitionId',
    'competitionName',
    'competitionType',
    'matchDateFixed',
    'startDate',
    'endDate',
  ];

  constructor(
    private teamService: MatchListService,
    private route: ActivatedRoute,
    private location: Location,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const teamName = params['tName'];
      if (teamName) {
        this.teamName = teamName;
        this.teamService.getTeamDetails(teamName).subscribe((details) => {
          this.teamDetails = details;
        });
      }
    });
  }

  formatDate(date: Date): string | null {
    return this.datePipe.transform(date, 'fullDate');
  }

  goBack(): void {
    this.location.back(); // Navigate back to the previous page
  }
}
