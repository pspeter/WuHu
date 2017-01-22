import {Component, OnInit} from '@angular/core';
import {Player} from "../../model/Player";
import {PlayerService} from "../../api/player-service";
import {RatingService} from "../../api/rating-service";

@Component({
    selector: 'app-statistics',
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.sass']
})
export class StatisticsComponent implements OnInit {
    private errorMessage: string = "";
    private infoMessage: string = "";
    private loading: boolean;
    private openRequests: number;
    private players: Array<Player>;

    constructor(private ratingService: RatingService,
                private playerService: PlayerService) {
        this.openRequests = 0;
        this.loading = true;

        ++this.openRequests;
        this.playerService.playerGetAll()
            .subscribe(
                res => {
                    this.players = res;
                    this.onLoadPartial();
                    this.loadRatings();
                },
                error => {
                    this.onError()
                }
            );
    }

    loadRatings() {
        ++this.openRequests;
        this.ratingService.ratingGetPageCount()
            .subscribe(
                res => {
                    for(let i = 0; i < res; ++i) {
                        this.loadRatingPage(i);
                    }
                    this.onLoadPartial();
                },
                error => {
                    this.onError();
                }
            );
    }

    loadRatingPage(pageNumber: number) {
        ++this.openRequests;
        this.ratingService.ratingGetAllRatings(pageNumber)
            .subscribe(
                res => {
                    for(let i = 0; i < res.length; ++i) {
                        let player = this.players.find(p => p.PlayerId == res[i].Player.PlayerId)
                        if (!player.RatingData) {
                            player.RatingData = []
                        }
                        player.RatingData.push(
                            [Math.floor(new Date(res[i].Datetime).getTime()), res[i].Value]);
                    }
                    this.onLoadPartial();
                },
                error => {
                    this.onError();
                }
            );
    }

    private onLoadPartial() {
        --this.openRequests;
        if (this.openRequests == 0) {
            this.loading = false;
        }
    }

    onError() {
        this.errorMessage = "Verbindungsfehler";
    }


    ngOnInit() {
    }


}
