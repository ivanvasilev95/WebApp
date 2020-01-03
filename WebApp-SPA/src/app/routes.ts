import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdListComponent } from './ads/ad-list/ad-list.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MyAdsComponent } from './ads/my-ads/my-ads.component';
import { AdDetailComponent } from './ads/ad-detail/ad-detail.component';
import { AdDetailResolver } from './_resolvers/ad-detail.resolver';
import { AdListResolver } from './_resolvers/ad-list.resolver';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { UserEditComponent } from './user-edit/user-edit.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { UserAdsComponent } from './ads/user-ads/user-ads.component';
import { AdEditComponent } from './ads/ad-edit/ad-edit.component';
import { NewAddComponent } from './ads/ad-add/ad-add.component';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { LoginComponent } from './login/login.component';
import { NewAdGuard } from './_guards/new-ad.guard';
import { AboutComponent } from './about/about.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'about', component: AboutComponent },
    { path: 'login', component: LoginComponent },
    { path: 'user/:id/ads', component: UserAdsComponent },
    { path: 'ads', component: AdListComponent,
    resolve: {ads: AdListResolver} },
    { path: 'ads/:id', component: AdDetailComponent,
    resolve: {ad: AdDetailResolver} },
    { path: 'ad/new', component: NewAddComponent, runGuardsAndResolvers: 'always', canActivate: [NewAdGuard] },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'favorites', component: FavoritesComponent },
            { path: 'messages', component: MessagesComponent,
            resolve: {messages: MessagesResolver} },
            { path: 'user/edit', component: UserEditComponent,
            resolve: {user: UserEditResolver}, canDeactivate: [PreventUnsavedChanges] },
            { path: 'user/ads', component: MyAdsComponent },
            { path: 'user/ad/:id/edit', component: AdEditComponent },
            { path: 'ads/:id/:senderId', component: AdDetailComponent,
            resolve: {ad: AdDetailResolver} },
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
