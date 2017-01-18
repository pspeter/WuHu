import {Component, OnInit, OnDestroy} from '@angular/core';
import {MatchService} from "../../api/match-service";
import {Match} from "../../model/Match";
import {UserService} from "../../services/user.service";
import {WebsocketService} from "../../api/websocket.service";

@Component({
    selector: 'app-scores',
    templateUrl: './scores.component.html',
    styleUrls: ['./scores.component.sass']
})
export class ScoresComponent implements OnInit, OnDestroy {
    private matches: Match[];
    private subscription;
    private errorString: string = "";
    private message: string = "";
    private loading: boolean;

    constructor(private matchService: MatchService,
                private userService: UserService,
                private websocketService: WebsocketService) {
        this.loading = true;
        this.websocketService.start();
    }

    /*
    private loadMatches() {
        this.matchService.matchGetUnfinishedForPlayer(this.userService.CurrentUser())
            .subscribe({
                next: res => {
                    this.matches = res;
                    for (let i = 0; i < this.matches.length; ++i) {
                        if (this.matches[i].ScoreTeam1 == null)
                            this.matches[i].ScoreTeam1 = 0;
                        if (this.matches[i].ScoreTeam2 == null)
                            this.matches[i].ScoreTeam2 = 0;
                    }
                },
                error: res => console.log(res)
            });
    }
*/

    private incScore1(match: Match) {
        match.ScoreTeam1 += 1;
        this.websocketService.updateMatch(match);
    }

    private incScore2(match: Match) {
        match.ScoreTeam2 += 1;
        this.websocketService.updateMatch(match);
    }

    ngOnInit() {
        this.subscription = this.websocketService.matchListSubject.subscribe({
            next: matchList => {
                this.matches = [];
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
                        this.matches.push(matchList[i]);
                    }
                }
                this.loading = false;
                console.log("matches", this.matches);
            }
        })
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.websocketService.stop();
    }

}
