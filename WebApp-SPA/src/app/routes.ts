import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdListComponent } from './ads/ad-list/ad-list.component';
import { UserFavoritesComponent } from './users/user-favorites/user-favorites.component';
import { UserMessagesComponent } from './users/user-messages/user-messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MyAdsComponent } from './users/my-ads/my-ads.component';
import { AdDetailComponent } from './ads/ad-detail/ad-detail.component';
import { AdDetailResolver } from './_resolvers/ad-detail.resolver';
import { AdListResolver } from './_resolvers/ad-list.resolver';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { UserAdsComponent } from './users/user-ads/user-ads.component';
import { AdEditComponent } from './ads/ad-edit/ad-edit.component';
import { NewAdComponent } from './ads/ad-add/ad-add.component';
import { UserMessagesResolver } from './_resolvers/user-messages.resolver';
import { NewAdGuard } from './_guards/new-ad.guard';
import { AboutComponent } from './about/about.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { LoginGuard } from './_guards/login.guard';
import { AdEditResolver } from './_resolvers/ad-edit.resolver';
import { AuthPanelComponent } from './auth/auth-panel/auth-panel.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'about', component: AboutComponent },
    { path: 'auth', component: AuthPanelComponent, runGuardsAndResolvers: 'always', canActivate: [LoginGuard] },
    { path: 'user/:id/ads', component: UserAdsComponent },
    { path: 'ads', component: AdListComponent,
        resolve: {ads: AdListResolver} },
    { path: 'ads/:id', component: AdDetailComponent,
        resolve: {ad: AdDetailResolver} },
    { path: 'ad/new', component: NewAdComponent, runGuardsAndResolvers: 'always', canActivate: [NewAdGuard] },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'favorites', component: UserFavoritesComponent },
            { path: 'messages', component: UserMessagesComponent,
                resolve: {messages: UserMessagesResolver} },
            { path: 'user/edit', component: UserEditComponent,
                resolve: {user: UserEditResolver}, canDeactivate: [PreventUnsavedChanges] },
            { path: 'user/ads', component: MyAdsComponent },
            { path: 'ads/:id/edit', component: AdEditComponent,
                resolve: {ad: AdEditResolver} },
            { path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin', 'Moderator']} }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
