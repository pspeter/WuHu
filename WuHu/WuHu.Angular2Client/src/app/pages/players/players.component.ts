import {Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {Player} from "../../model/Player";
import {PlayerApi} from "../../api/PlayerApi";

@Component({
    selector: 'app-players',
    templateUrl: './players.component.html',
    styleUrls: ['./players.component.sass']
})
export class PlayersComponent implements OnInit {
    private players: Observable<Player[]>;

    getPlayers() {
        this.players = this.playerService.playerGetAll();
    }

    constructor(private playerService: PlayerApi) { }

    ngOnInit() {
        this.getPlayers();
    }

}
