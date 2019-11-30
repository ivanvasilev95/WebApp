import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { AlertifyService } from './_services/alertify.service';
import { AdListComponent } from './ads/ad-list/ad-list.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { AdService } from './_services/ad.service';
import { UserAdsComponent } from './user-ads/user-ads.component';
import { AdComponent } from './ads/ad/ad.component';
import { AdDetailComponent } from './ads/ad-detail/ad-detail.component';

export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      AdListComponent,
      FavoritesComponent,
      MessagesComponent,
      UserAdsComponent,
      AdComponent,
      AdDetailComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      RouterModule.forRoot(appRoutes),
      BsDropdownModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/auth', 'localhost:5000/ads']
         }
      })
   ],
   providers: [
      AuthService,
      AlertifyService,
      AuthGuard,
      AdService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
