import {Component, OnInit} from '@angular/core';
import {PlayerApi} from "../../api/PlayerApi";
import {RatingApi} from "../../api/RatingApi";
import {Player} from "../../model/Player";
import {PlayerService} from "../../api/player-service";
import {RatingService} from "../../api/rating-service";

@Component({
    selector: 'app-ranklist',
    templateUrl: './ranklist.component.html',
    styleUrls: ['./ranklist.component.sass']
})
export class RanklistComponent implements OnInit {

    private players : Array<Player>;

    private sortedPlayers = [];

    private sortedIndex(value, array) {
        let low = 0,
            high = array.length;

        while (low < high) {
            let mid = (low + high) >>> 1;
            if (array[mid].CurrentRating.Value > value) low = mid + 1;
            else high = mid;
        }
        return low;
    }

    private insertSorted(value, array) {
        array.splice(this.sortedIndex(value.CurrentRating.Value, array), 0, value);
        return array;
    }

    sortByRank() {
        this.players.sort((a, b) => {
            if (!a.CurrentRating) return 1;
            if (!b.CurrentRating) return -1;
            return b.CurrentRating.Value - a.CurrentRating.Value
        });
    }

    getPlayers() {
        this.playerService.playerGetAll()
            .subscribe({
                next: p => this.players = p,
                complete: () => {
                    for (let i = 0; i < this.players.length; ++i) {
                        this.ratingService
                            .ratingGetCurrentByPlayerId(this.players[i].PlayerId)
                            .subscribe({
                                next: r => {
                                    this.players[i].CurrentRating = r;
                                    this.insertSorted(this.players[i], this.sortedPlayers)
                                }
                            })
                    }
                    /*
                    let sortStarter = setInterval(() => {
                        if (done == this.players.length) {
                            this.sortByRank();
                            clearInterval(sortStarter);
                        }
                    }, 100);*/

                    /*let allRatings;
                     this.ratingService.ratingGetAll()
                     .subscribe({
                     next: all => allRatings = all,
                     complete: () => {
                     for(let i = 0; i < allRatings.length; ++i) {
                     this.players.find(p => p.PlayerId == allRatings[i].PlayerId)
                     .CurrentRating = allRatings[i];
                     }
                     }
                     })*/
                }
            });
    }

    constructor(private playerService: PlayerService, private ratingService: RatingService) {
    }

    ngOnInit() {
        this.getPlayers();
    }
}
