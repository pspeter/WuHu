import {Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {Player} from "../../model/Player";
import {PlayerApi} from "../../api/PlayerApi";
import {RestoreService} from "../../services/restore.service";
import {PlayerService} from "../../api/player-service";

@Component({
    selector: 'app-players',
    templateUrl: './players.component.html',
    styleUrls: ['./players.component.sass']
})
export class PlayersComponent implements OnInit {
    private players: Player[];

    set playerModel(patient: Player) {
        this.restoreService.setItem(patient);
    }

    get playerModel() {
        return this.restoreService.getItem();
    }

    resetPlayer() {
        this.playerModel = this.restoreService.restoreItem();
    }

    savePlayer() {
        let player = this.restoreService.getItemFinal();
        let success;
        this.playerService.playerPutPlayer(player).subscribe(s => success = s);
        if (!success) {
            //show error
        }
        else {
            console.log("save", player);
            let i = this.players.findIndex(p => p.PlayerId == player.PlayerId);
            this.players[i] = player;
        }
    }

    private selectPlayer(player) {
        this.playerModel = player;
    }

    getPlayers() {
        this.playerService.playerGetAll()
            .subscribe({
                next: p => this.players = p
            });
    }


    constructor(private playerService: PlayerService, private restoreService: RestoreService<Player>) { }

    ngOnInit() {
        this.getPlayers();
    }

}
