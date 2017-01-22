import {Component, OnInit, OnDestroy, NgZone} from '@angular/core';
import {MatchService} from "../../api/match-service";
import {Match} from "../../model/Match";
import {UserService} from "../../services/user.service";
import {WebsocketService} from "../../api/websocket.service";
import {TournamentService} from "../../api/tournament.service";

@Component({
    selector: 'app-scores',
    templateUrl: './scores.component.html',
    styleUrls: ['./scores.component.sass']
})
export class ScoresComponent implements OnInit, OnDestroy {
    private matches: Match[] = [];
    private subscription;
    private errorSubscription;
    private errorMessage: string = "";
    private infoMessage: string = "";
    private loading: boolean;

    constructor(private ngZone: NgZone,
                private userService: UserService,
                private websocketService: WebsocketService) {
        this.websocketService.start();
    }

    private displayError(message: string) {
        this.ngZone.run(() => this.errorMessage = message);
        setTimeout(() => this.ngZone.run(() => this.errorMessage = ""), 5000)
    }

    private incScore1(match: Match) {
        match.ScoreTeam1 += 1;
        this.websocketService.updateMatch(match);
    }

    private incScore2(match: Match) {
        match.ScoreTeam2 += 1;
        this.websocketService.updateMatch(match);
    }

    ngOnInit() {
        this.ngZone.run(() => this.loading = true);
        this.subscription = this.websocketService.matchListSubject.subscribe(
            matchList => {
                this.matches = [];
                this.ngZone.run(() => this.infoMessage = "");
                this.ngZone.run(() => this.errorMessage = "");
                for (let i = 0; i < matchList.length; ++i) {
                    if (matchList[i].ScoreTeam1 == null)
                        matchList[i].ScoreTeam1 = 0;
                    if (matchList[i].ScoreTeam2 == null)
                        matchList[i].ScoreTeam2 = 0;
                    let user = this.userService.CurrentUser();
                    if (matchList[i].Player1.Username == user ||
                        matchList[i].Player2.Username == user ||
                        matchList[i].Player3.Username == user ||
                        matchList[i].Player4.Username == user) {
                        this.ngZone.run(() => this.matches.push(matchList[i]));
                    }
                }

                this.ngZone.run(() => this.errorMessage = "");
                if (this.matches.length == 0) {
                    this.ngZone.run(() => this.infoMessage = "Keine Spiele gefunden");
                }
                this.ngZone.run(() => this.loading = false);
            },
            error => {
                this.displayError("Verbindungsproblem");
            },
            () => this.displayError("Verbindung beendet")
        );

        this.errorSubscription = this.websocketService.errorSubject.subscribe(
            error => {
                this.ngZone.run(() => this.loading = false);
                this.ngZone.run(() => this.errorMessage = "Verbindungsfehler");
                this.ngZone.run(() => this.infoMessage = "");
            }
        )
    }

    ngOnDestroy() {
        this.errorSubscription.unsubscribe();
        this.subscription.unsubscribe();
        this.websocketService.stop();
    }

}
