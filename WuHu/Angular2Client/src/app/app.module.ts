import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import {AppRoutingModule} from "./app-routing.module";
import {UserService} from "./user.service";
import {HttpAuthService} from "./http-auth.service";
import { NavComponent } from './components/nav/nav.component';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import {LoginGuard} from "./login-guard";
import { NotfoundComponent } from './pages/notfound/notfound.component';
import { PlayersComponent } from './pages/players/players.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavComponent,
    DashboardComponent,
    NotfoundComponent,
    PlayersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpModule
  ],
  providers: [UserService, HttpAuthService, LoginGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
