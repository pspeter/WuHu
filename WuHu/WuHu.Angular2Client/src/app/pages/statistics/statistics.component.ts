import {Component, OnInit} from '@angular/core';
import {Player} from "../../model/Player";

@Component({
    selector: 'app-statistics',
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.sass']
})
export class StatisticsComponent implements OnInit {

    constructor() {
    }

    dataLoaded() {

    }

    private selectPlayer(player: Player) {
        player.IsSelected = !player.IsSelected;
    }

    ngOnInit() {
    }


}
