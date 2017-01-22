import {Component, OnInit} from '@angular/core';
import {WebsocketService} from "../../api/websocket.service";
import {UserService} from "../../services/user.service";
import {PlayerService} from "../../api/player-service";
import {Player} from "../../model/Player";
import {StatisticsService} from "../../api/statistics-service";
import {PlayerStats} from "../../model/PlayerStats";

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.sass']
})
export class DashboardComponent implements OnInit {
    private isLoggedIn: boolean;
    private isAdmin: boolean;
    private currentUser: string;
    private currentPlayer: Player;
    private currentStats: PlayerStats;
    private loading: boolean;

    constructor(private userService: UserService,
                private playerService: PlayerService,
                private statsService: StatisticsService) {
        this.isLoggedIn = userService.IsUser() || userService.IsAdmin();
        this.isAdmin = userService.IsAdmin();
        if (this.isLoggedIn) {
            this.currentUser = userService.CurrentUser();
            playerService.playerGetByUsername(this.currentUser)
                .subscribe(res => this.currentPlayer = res);
            this.currentStats = statsService.statisticsGetPlayerStats(this.currentUser)
                .subscribe(res => this.currentStats = res);
        }
    }

    ngOnInit() {
    }

}
