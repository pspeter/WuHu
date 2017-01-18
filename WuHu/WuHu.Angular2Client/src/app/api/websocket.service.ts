import {Injectable, Inject} from '@angular/core';
import {Observable, Subject} from "rxjs";
import {Match} from "../model/Match";

// wrapper for JQuery
@Injectable()
export class SignalrWindow extends Window {
    public $: any;
}

@Injectable()
export class WebsocketService {
    starting$: Observable<any>;
    error$: Observable<string>;

    private startingSubject = new Subject<any>();
    private errorSubject = new Subject<any>();

    public matchListSubject = new Subject<Array<Match>>();

    private hubConnection: any;
    private hubProxy: any;

    constructor(private window: SignalrWindow) {
        if (this.window.$ === undefined || this.window.$.hubConnection === undefined) {
            throw new Error("The variable '$' or the .hubConnection() function are not defined... please check the SignalR scripts have been loaded properly");
        }

        this.starting$ = this.startingSubject.asObservable();
        this.error$ = this.errorSubject.asObservable();

        this.hubConnection = this.window.$.hubConnection();
        this.hubConnection.url = "http://localhost:4649/signalr";
        this.hubProxy = this.hubConnection.createHubProxy("liveMatchHub");

        this.hubConnection.error((error: any) => {
            this.errorSubject.next(error);
        });

        this.hubProxy.on("broadcastMatches", (matches: Array<Match>) => {
            console.log(matches);
            this.matchListSubject.next(matches);
        });
    }

    start(): void {
        this.hubConnection.start()
            .done(() => {
                this.startingSubject.next();
            })
            .fail((error: any) => {
                this.startingSubject.error(error);
            });
    }

    stop() {
        this.hubConnection.stop();
    }

    updateMatch(match: Match) {
        console.log("matchSave", match);
        this.hubProxy.invoke("matchSave", match);
    }
}
