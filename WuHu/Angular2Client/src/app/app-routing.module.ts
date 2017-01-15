import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./pages/login/login.component";
import {DashboardComponent} from "./pages/dashboard/dashboard.component";
import {LoginGuard} from "./guards/login-guard";
import {PlayersComponent} from "./pages/players/players.component";
import {AdminGuard} from "./guards/admin-guard";
import {GameplanComponent} from "./pages/gameplan/gameplan.component";
import {LiveComponent} from "./pages/live/live.component";
import {RanklistComponent} from "./pages/ranklist/ranklist.component";
import {StatisticsComponent} from "./pages/statistics/statistics.component";
import {NotfoundComponent} from "./pages/notfound/notfound.component";
import {LogoutGuard} from "./guards/logout-guard";

const routes: Routes = [
    {path: '', pathMatch: 'full', redirectTo: '/dashboard'},

    /* Login */
    {path: "login", component: LoginComponent, canActivate:[LogoutGuard]},
    {path: "dashboard", component: DashboardComponent},
    {path: "live", component: LiveComponent},
    {path: "rangliste", component: RanklistComponent},
    {path: "statistik", component: StatisticsComponent},
    {path: "spielstand", component: PlayersComponent, canActivate:[LoginGuard]},
    {path: "spielplan", component: GameplanComponent, canActivate:[AdminGuard]},
    {path: "spieler", component: PlayersComponent, canActivate:[AdminGuard]},
    {path: "**", component: NotfoundComponent, canActivate:[AdminGuard]},
];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class AppRoutingModule { }
