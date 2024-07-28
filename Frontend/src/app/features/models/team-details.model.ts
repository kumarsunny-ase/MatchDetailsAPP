export interface TeamDetailsDto {
  MatchDay: number;
  MatchType: string;
  GuestTeamName: string;
  PlannedKickoffTime: Date;
  StadiumId: string;
  StadiumName: string;
  Season: string;
  CompetitionId: string;
  CompetitionName: string;
  CompetitionType: string;
  MatchDateFixed: boolean;
  StartDate: Date;
  EndDate: Date;
}