import { Routes } from '@angular/router';
import { FileUploadComponent } from './features/xmlFileUpload/file-upload/file-upload.component';
import { MatchDetailsComponent } from './features/match/match-details/match-details.component';
import { AuthComponent } from './features/auth/auth/auth.component';
import { authGuard } from './features/auth/guards/auth.guard';
import { MatchDateComponent } from './features/match/match-date/match-date.component';

export const routes: Routes = [
  {
    path: 'match/list',
    component: FileUploadComponent,
    canActivate: [authGuard],
  },
  {
    path: 'match/details',
    component: MatchDetailsComponent,
    canActivate: [authGuard],
  },
  {
    path: '',
    component: AuthComponent,
  },
  {
    path: 'match/date',
    component: MatchDateComponent,
    canActivate: [authGuard],
  }
];
