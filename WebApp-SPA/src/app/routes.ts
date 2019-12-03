import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdListComponent } from './ads/ad-list/ad-list.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserAdsComponent } from './user-ads/user-ads.component';
import { AdDetailComponent } from './ads/ad-detail/ad-detail.component';
import { AdDetailResolver } from './_resolvers/ad-detail.resolver';
import { AdListResolver } from './_resolvers/ad-list.resolver';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { UserEditComponent } from './user-edit/user-edit.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'ads', component: AdListComponent,
    resolve: {ads: AdListResolver} },
    { path: 'ads/:id', component: AdDetailComponent,
    resolve: {ad: AdDetailResolver} },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'favorites', component: FavoritesComponent },
            { path: 'messages', component: MessagesComponent },
            { path: 'user/edit', component: UserEditComponent,
            resolve: {user: UserEditResolver}, canDeactivate: [PreventUnsavedChanges] },
            { path: 'user/ads', component: UserAdsComponent }
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
