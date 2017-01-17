import {Component, OnInit} from '@angular/core';
import {MatchService} from "../../api/match-service";
import {Match} from "../../model/Match";
import {UserService} from "../../services/user.service";

@Component({
    selector: 'app-scores',
    templateUrl: './scores.component.html',
    styleUrls: ['./scores.component.sass']
})
export class ScoresComponent implements OnInit {
    private matches: Array<Match>;
    private errorString: string = "";

    constructor(private matchService: MatchService, private userService: UserService) {
        this.loadMatches();
    }

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

    private submitMatch(match: Match) {
        this.matchService.matchPostIntermediateResult(match)
            .subscribe({
                error: res => {
                    console.log(res);
                    if (res.status == 400) {
                        //this.displayError("Fehlerhafter Spieler");
                    }
                    else if (res.status == 500) {
                        //this.displayError("Oops. Da ging was schief " + res._body);
                    }
                    else {
                        //this.displayError("Server offline");
                    }
                }
            })
    }

    private incScore1(match: Match) {
        match.ScoreTeam1 += 1;
        this.submitMatch(match);
    }

    private incScore2(match: Match) {
        match.ScoreTeam2 += 1;
        this.submitMatch(match);
    }

    ngOnInit() {
    }

}
