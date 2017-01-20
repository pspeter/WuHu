import {Component, OnInit, OnDestroy, NgZone} from '@angular/core';
import {Match} from "../../model/Match";
import {WebsocketService} from "../../api/websocket.service";

@Component({
    selector: 'app-live',
    templateUrl: './live.component.html',
    styleUrls: ['./live.component.sass']
})
export class LiveComponent implements OnInit, OnDestroy {
    private matches: Array<Match>;
    private tournamentName: string;
    private matchesSubscription;
    private nameSubscription;
    private errorSubscription;
    private errorMessage: string = "";
    private infoMessage: string = "";
    private loading: boolean;

    constructor(private ngZone: NgZone, private websocketService: WebsocketService) {
        this.websocketService.start();
    }

    ngOnInit() {
        this.ngZone.run(() => this.loading = true);
        this.nameSubscription = this.websocketService.tournamentNameSubject.subscribe(
            name => this.ngZone.run(() => this.tournamentName = name)
        );
        this.matchesSubscription = this.websocketService.matchListSubject.subscribe({
            next: matchList => {
                this.ngZone.run(() => this.infoMessage = "");
                this.ngZone.run(() => this.errorMessage = "");
                this.matches = matchList;
                for (let i = 0; i < this.matches.length; ++i) {
                    if (this.matches[i].ScoreTeam1 == null)
                        this.matches[i].ScoreTeam1 = 0;
                    if (this.matches[i].ScoreTeam2 == null)
                        this.matches[i].ScoreTeam2 = 0;
                }
                console.log("next");

                this.ngZone.run(() => this.errorMessage = "");
                if (this.matches.length == 0) {
                    this.ngZone.run(() => this.infoMessage = "Keine Spiele gefunden");
                }
                this.ngZone.run(() => this.loading = false);
            }
        });

        this.errorSubscription = this.websocketService.errorSubject.subscribe(
            error => {
                console.error("connection error", error);
                this.ngZone.run(() => this.loading = false);
                this.ngZone.run(() => this.errorMessage = "Verbindungsfehler");
                this.ngZone.run(() => this.infoMessage = "");
            }
        )
    }

    ngOnDestroy() {
        this.matchesSubscription.unsubscribe();
        this.websocketService.stop();
    }
}
