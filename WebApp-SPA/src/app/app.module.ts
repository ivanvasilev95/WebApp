import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDropdownModule, TabsModule, TooltipModule, PaginationModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';

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
import { MyAdsComponent } from './ads/my-ads/my-ads.component';
import { AdComponent } from './ads/ad/ad.component';
import { AdDetailComponent } from './ads/ad-detail/ad-detail.component';
import { AdDetailResolver } from './_resolvers/ad-detail.resolver';
import { AdListResolver } from './_resolvers/ad-list.resolver';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { UserAdsComponent } from './ads/user-ads/user-ads.component';
import { AdEditComponent } from './ads/ad-edit/ad-edit.component';
import {TimeAgoPipe} from 'time-ago-pipe';
import { NewAddComponent } from './ads/ad-add/ad-add.component';
import { AdMessagesComponent } from './ads/ad-messages/ad-messages.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PhotoEditorComponent } from './ads/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { LoginComponent } from './login/login.component';
import { NewAdGuard } from './_guards/new-ad.guard';
import { AboutComponent } from './about/about.component';

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
      MyAdsComponent,
      UserAdsComponent,
      AdComponent,
      AdDetailComponent,
      UserEditComponent,
      AdEditComponent,
      NewAddComponent,
      AdMessagesComponent,
      PhotoEditorComponent,
      TimeAgoPipe,
      LoginComponent,
      AboutComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      NgxGalleryModule,
      RouterModule.forRoot(appRoutes),
      BsDropdownModule.forRoot(),
      PaginationModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/auth', 'localhost:5000/ads']
         }
      }),
      TabsModule.forRoot(),
      TooltipModule.forRoot(),
      BrowserAnimationsModule,
      FileUploadModule
   ],
   providers: [
      AuthService,
      AlertifyService,
      AuthGuard,
      NewAdGuard,
      AdService,
      AdDetailResolver,
      AdListResolver,
      UserEditResolver,
      MessagesResolver,
      PreventUnsavedChanges // ,
      // UserService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
