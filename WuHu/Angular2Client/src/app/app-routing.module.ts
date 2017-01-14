import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./pages/login/login.component";
import {DashboardComponent} from "./pages/dashboard/dashboard.component";
import {LoginGuard} from "./login-guard";
import {PlayersComponent} from "./pages/players/players.component";

const routes: Routes = [
    {path: '', pathMatch: 'full', redirectTo: '/dashboard'},

    /* Login */
    {path: "login", component: LoginComponent},
    {path: "dashboard", component: DashboardComponent}, //, canActivate: [LoginGuard]
    {path: "players", component: PlayersComponent, canActivate:[LoginGuard]},
];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class AppRoutingModule {
}
