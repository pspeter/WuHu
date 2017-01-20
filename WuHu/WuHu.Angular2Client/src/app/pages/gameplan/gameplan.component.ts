import {Component, OnInit, OnDestroy} from '@angular/core';
import {TournamentService} from "../../api/tournament.service";
import {TournamentData} from "../../model/TournamentData";
import {PlayerService} from "../../api/player-service";
import {Player} from "../../model/Player";
import {UserService} from "../../services/user.service";
import {WebsocketService} from "../../api/websocket.service";

@Component({
    selector: 'app-gameplan',
    templateUrl: './gameplan.component.html',
    styleUrls: ['./gameplan.component.sass']
})
export class GameplanComponent implements OnInit, OnDestroy {
    private tournamentModel: TournamentData;
    private players: Player[];
    private loading: boolean;
    private showWarning: boolean;
    private editMode: boolean;
    private errorMessage: string;
    private successMessage: string;

    constructor(private tournamentService: TournamentService,
                private playerService: PlayerService,
                private userService: UserService,
                private socketService: WebsocketService) { }

    private getTournament() {
        this.loading = true;
        this.tournamentService
            .tournamentGetCurrentTournament()
            .subscribe(
                res => {
                    this.loading = false;
                    this.tournamentModel = res;
                },
                error => {
                   this.loading = false;
                   this.displayError("Spieplan konnte nicht geladen werden");
                }
            )
    }

    private warningConfirm() {
        this.showWarning = false;
        this.getTournament();
    }

    private warningCancel() {
        this.showWarning = false;
        this.unlockTournament();
    }

    private editTournament() {
        this.reset();
        this.editMode = true;
        this.loading = true;
        this.tournamentService.tournamentLockTournament()
            .subscribe(
                res => { },
                error => {
                    this.loading = false;
                    if (error.status == 409) {
                        this.showWarning = true;
                    } else {
                        this.errorMessage = "Verbindungsproblem";
                    }
                },
                () => {
                    this.getTournament();
                }
            )
    }

    private selectPlayer(player: Player) {
        player.IsSelected = !player.IsSelected;
    }

    private addTournament() {
        this.editMode = false;
        this.errorMessage = "";
        this.successMessage = "";
        this.tournamentModel = {
            Name: "",
            Players: [],
            Amount: 1
        }
    }

    private reset() {
        this.unlockTournament();
        this.tournamentModel = null;
        this.loading = false;
        this.errorMessage = "";
        this.successMessage = "";
        this.showWarning = false;
    }

    private displayError(message: string) {
        this.errorMessage = message;
        this.successMessage = "";
        setTimeout(() => this.errorMessage = "", 5000)
    }

    private displaySuccess(message: string) {
        this.errorMessage = "";
        this.successMessage = message;
        setTimeout(() => this.successMessage = "", 5000)
    }

    private onSubmit() {
        let submitPlayers: Player[] = [];
        for (let i = 0; i < this.players.length; ++i) {
            if (this.players[i].IsSelected) submitPlayers.push(this.players[i]);
        }
        this.tournamentModel.Players = submitPlayers;
        this.loading = true;
        if (this.editMode) {
            this.tournamentService
                .tournamentPutTournament(this.tournamentModel)
                .subscribe(
                    res => {
                        this.socketService.refreshMatches();
                    },
                    error => {
                        if (error.status == 400) {
                            this.displayError("Fehlerhaftes Turnier");
                        }
                        else if (error.status == 403) {
                            this.displayError("Nicht eingeloggt");
                            setTimeout(() => this.userService.logout(), 2000)
                        }
                        else if (error.status == 500) {
                            this.displayError("Oops. Da ging was schief");
                        }
                        else {
                            this.displayError("Verbindungsproblem");
                        }
                        this.loading = false;
                    },
                    () => {
                        this.displaySuccess("Spielplan geupdated");
                        this.loading = false;
                    }
                );
        } else {
            this.tournamentService
                .tournamentPostTournament(this.tournamentModel)
                .subscribe(
                    res => {
                        this.socketService.refreshMatches();
                    },
                    error => {
                        if (error.status == 400) {
                            this.displayError("Fehlerhaftes Turnier");
                        }
                        else if (error.status == 403) {
                            this.displayError("Nicht eingeloggt");
                            setTimeout(() => this.userService.logout(), 2000)
                        }
                        else if (error.status == 500) {
                            this.displayError("Oops. Da ging was schief");
                        }
                        else {
                            this.displayError("Verbindungsproblem");
                        }
                        this.loading = false;
                    },
                    () => {
                        this.displaySuccess("Spielplan erstellt");
                        this.loading = false;
                    }
                );
        }
        this.tournamentModel = null;
    }

    private unlockTournament() {
        if (!!this.tournamentModel && !!this.tournamentModel.TournamentId)
            this.tournamentService
                .tournamentUnlockTournament(this.tournamentModel.TournamentId)
                .subscribe();
    }

    ngOnInit() {
        this.playerService.playerGetAll()
            .subscribe(
                res => {
                    let players = res;
                    for (let i = 0; i < players.length; ++i) {
                        let day = new Date().getDay();

                        switch(day) {
                            case(1): {
                                players[i].IsSelected = players[i].PlaysMondays;
                            } break;
                            case(2): {
                                players[i].IsSelected = players[i].PlaysTuesdays;
                            } break;
                            case(3): {
                                players[i].IsSelected = players[i].PlaysWednesdays;
                            } break;
                            case(4): {
                                players[i].IsSelected = players[i].PlaysThursdays;
                            } break;
                            case(5): {
                                players[i].IsSelected = players[i].PlaysFridays;
                            } break;
                            case(6): {
                                players[i].IsSelected = players[i].PlaysSaturdays;
                            } break;
                            case(7): {
                                players[i].IsSelected = players[i].PlaysSundays;
                            } break;
                            default: {
                                players[i].IsSelected = false;
                            }
                        }
                    }
                    this.players = players;
                },
                error => {
                    if (error.status == 403) {
                        this.displayError("Nicht eingeloggt");
                        setTimeout(() => this.userService.logout(), 2000)
                    }
                    else if (error.status == 500) {
                        this.displayError("Oops. Da ging was schief");
                    }
                    else {
                        this.errorMessage = "Verbindungsproblem";
                    }
                }
            )
    }

    ngOnDestroy() {
        this.unlockTournament();
    }
}
