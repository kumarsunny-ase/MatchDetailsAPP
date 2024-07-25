import { Routes } from '@angular/router';
import { MatchListComponent } from './features/match/match-list/match-list.component';
import { MatchDetailsComponent } from './features/match/match-details/match-details.component';

export const routes: Routes = [
    {
        path: '',
        component: MatchListComponent
    },
    {
        path: 'match/details',
        component: MatchDetailsComponent
    }
];
