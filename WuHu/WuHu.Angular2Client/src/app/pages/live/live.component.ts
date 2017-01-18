import {Component, OnInit, OnDestroy} from '@angular/core';
import {Match} from "../../model/Match";
import {WebsocketService} from "../../api/websocket.service";

@Component({
    selector: 'app-live',
    templateUrl: './live.component.html',
    styleUrls: ['./live.component.sass']
})
export class LiveComponent implements OnInit, OnDestroy {
    private matches: Array<Match>;
    private subscription;

    constructor(private websocketService: WebsocketService) {
        this.websocketService.start();

    }

    ngOnInit() {
        this.subscription = this.websocketService.matchListSubject.subscribe({
            next: matchList => {
                this.matches = matchList;
                for (let i = 0; i < this.matches.length; ++i) {
                    if (this.matches[i].ScoreTeam1 == null)
                        this.matches[i].ScoreTeam1 = 0;
                    if (this.matches[i].ScoreTeam2 == null)
                        this.matches[i].ScoreTeam2 = 0;
                }
                console.log(this.matches);
            }
        })
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.websocketService.stop();
    }
}
