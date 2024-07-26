import { Routes } from '@angular/router';
import { MatchListComponent } from './features/match/match-list/match-list.component';
import { MatchDetailsComponent } from './features/match/match-details/match-details.component';
import { AuthComponent } from './features/auth/auth/auth.component';
import { authGuard } from './features/auth/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'match/list',
        component: MatchListComponent,
        canActivate: [authGuard]
    },
    {
        path: 'match/details',
        component: MatchDetailsComponent,
        canActivate: [authGuard]
    },
    {
        path: '',
        component: AuthComponent
    }
];
