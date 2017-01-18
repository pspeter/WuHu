import {Injectable, Inject} from '@angular/core';
import {Observable, Subject, BehaviorSubject} from "rxjs";
import {Match} from "../model/Match";
import any = jasmine.any;

// wrapper for JQuery
@Injectable()
export class SignalrWindow extends Window {
    public $: any;
}

@Injectable()
export class WebsocketService {
    public matchListSubject = new Subject<Array<Match>>();
    public errorSubject = new Subject<any>();

    private hubConnection: any;
    private hubProxy: any;

    constructor(private window: SignalrWindow) {
        if (this.window.$ === undefined || this.window.$.hubConnection === undefined) {
            throw new Error("The variable '$' or the .hubConnection() function are not defined... please check the SignalR scripts have been loaded properly");
        }

        this.hubConnection = this.window.$.hubConnection();
        this.hubConnection.url = "http://localhost:4649/signalr";
        this.hubProxy = this.hubConnection.createHubProxy("liveMatchHub");

        this.hubConnection.error((error: any) => {
            this.errorSubject.next(error);
        });

        this.hubProxy.on("broadcastMatches", (matches: Array<Match>) => {
            console.log("broadcast matches", matches);
            this.matchListSubject.next(matches);
        });
    }

    start(): void {
        this.hubConnection.start()
            .done(() => {
                console.log("Connection established");
            })
            .fail((error: any) => {
                this.errorSubject.next(error);
                console.error("start() failed")
            });
    }

    stop() {
        this.hubConnection.stop();
    }

    updateMatch(match: Match) {
        console.log("matchSave", match);
        this.hubProxy.invoke("matchSave", match.MatchId, match.ScoreTeam1, match.ScoreTeam2)
            .fail(error => this.errorSubject.next(error));
    }
}
