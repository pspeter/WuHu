import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';

import {AppComponent} from './app.component';
import {AppRoutingModule} from "./app-routing.module";
import {UserService} from "./user.service";
import {HttpAuthService} from "./http-auth.service";
import {NavComponent} from './components/nav/nav.component';
import {LoginComponent} from './pages/login/login.component';
import {DashboardComponent} from './pages/dashboard/dashboard.component';
import {LoginGuard} from "./guards/login-guard";
import {NotfoundComponent} from './pages/notfound/notfound.component';
import {PlayersComponent} from './pages/players/players.component';
import {AdminGuard} from "./guards/admin-guard";
import { RanklistComponent } from './pages/ranklist/ranklist.component';
import { GameplanComponent } from './pages/gameplan/gameplan.component';
import { StatisticsComponent } from './pages/statistics/statistics.component';
import { ScoresComponent } from './pages/scores/scores.component';
import { LiveComponent } from './pages/live/live.component';
import {LogoutGuard} from "./guards/logout-guard";

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        NavComponent,
        DashboardComponent,
        NotfoundComponent,
        PlayersComponent,
        RanklistComponent,
        GameplanComponent,
        StatisticsComponent,
        ScoresComponent,
        LiveComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        HttpModule
    ],
    providers: [UserService, HttpAuthService, LoginGuard, LogoutGuard, AdminGuard],
    bootstrap: [AppComponent]
})
export class AppModule {
}
