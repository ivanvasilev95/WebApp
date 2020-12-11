import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './auth/register/register.component';
import { AlertifyService } from './_services/alertify.service';
import { AdListComponent } from './ads/ad-list/ad-list.component';
import { UserFavoritesComponent } from './users/user-favorites/user-favorites.component';
import { UserMessagesComponent } from './users/user-messages/user-messages.component';
import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { AdService } from './_services/ad.service';
import { MyAdsComponent } from './users/my-ads/my-ads.component';
import { AdComponent } from './ads/ad/ad.component';
import { AdDetailComponent } from './ads/ad-detail/ad-detail.component';
import { AdDetailResolver } from './_resolvers/ad-detail.resolver';
import { AdListResolver } from './_resolvers/ad-list.resolver';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { UserMessagesResolver } from './_resolvers/user-messages.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { UserAdsComponent } from './users/user-ads/user-ads.component';
import { AdEditComponent } from './ads/ad-edit/ad-edit.component';
import { AdEditResolver } from './_resolvers/ad-edit.resolver';
import { CategoriesResolver } from './_resolvers/categories.resolver';
import { TimeAgoPipe } from 'time-ago-pipe';
import { NewAdComponent } from './ads/ad-add/ad-add.component';
import { AdMessagesComponent } from './ads/ad-messages/ad-messages.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PhotoEditorComponent } from './ads/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { LoginComponent } from './auth/login/login.component';
import { NewAdGuard } from './_guards/new-ad.guard';
import { AboutComponent } from './about/about.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { AdManagementComponent } from './admin/ad-management/ad-management.component';
import { CategoryManagementComponent } from './admin/category-management/category-management.component';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';

import { IsApprovedPipe } from './_pipes/isApproved.pipe';
import { UserService } from './_services/user.service';
import { CategoryService } from './_services/category.service';
import { PhotoService } from './_services/photo.service';
import { MessageService } from './_services/message.service';
import { AuthPanelComponent } from './auth/auth-panel/auth-panel.component';
import { LikeService } from './_services/like.service';
import { ScrollTopComponent } from './scroll-top/scroll-top.component';

export function tokenGetter() {
   return localStorage.getItem('token');
}

export class CustomHammerConfig extends HammerGestureConfig  {
   overrides = {
       pinch: { enable: false },
       rotate: { enable: false }
   };
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      AdListComponent,
      UserFavoritesComponent,
      UserMessagesComponent,
      MyAdsComponent,
      UserAdsComponent,
      AdComponent,
      AdDetailComponent,
      UserEditComponent,
      AdEditComponent,
      NewAdComponent,
      AdMessagesComponent,
      PhotoEditorComponent,
      TimeAgoPipe,
      IsApprovedPipe,
      LoginComponent,
      AuthPanelComponent,
      AboutComponent,
      AdminPanelComponent,
      HasRoleDirective,
      UserManagementComponent,
      AdManagementComponent,
      CategoryManagementComponent,
      RolesModalComponent,
      ScrollTopComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      NgxGalleryModule,
      RouterModule.forRoot(appRoutes),
      BsDropdownModule.forRoot(),
      ModalModule.forRoot(),
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
      LikeService,
      AdDetailResolver,
      AdListResolver,
      AdEditResolver,
      UserEditResolver,
      UserMessagesResolver,
      CategoriesResolver,
      PreventUnsavedChanges,
      UserService,
      MessageService,
      PhotoService,
      CategoryService,
      ErrorInterceptorProvider,
      {
         provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig
      }
   ],
   entryComponents: [
      RolesModalComponent
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
