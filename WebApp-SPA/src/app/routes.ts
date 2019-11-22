import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdsComponent } from './ads/ads.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'ads', component: AdsComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'favorites', component: FavoritesComponent },
            { path: 'messages', component: MessagesComponent }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];

/*
export const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'ads', component: AdsComponent },
    { path: 'favorites', component: FavoritesComponent, canActivate: [AuthGuard] },
    { path: 'messages', component: MessagesComponent, canActivate: [AuthGuard] },
    { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
*/
