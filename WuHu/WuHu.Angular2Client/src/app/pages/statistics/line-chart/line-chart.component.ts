import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {Player} from "../../../model/Player";
import {Rating} from "../../../model/Rating";
import {PlayerService} from "../../../api/player-service";
import {RatingService} from "../../../api/rating-service";

@Component({
    selector: 'app-line-chart',
    templateUrl: './line-chart.component.html',
    styleUrls: ['./line-chart.component.sass']
})
export class LineChartComponent implements OnInit {

    private chart: any;
    private options: Object;
    private ratings: Array<Rating>;
    private players: Array<Player>;

    constructor(private ratingService: RatingService,
                private playerService: PlayerService) {
        this.options = {
            chart: {
                zoomType: "x",
                style: {"fill": "#444", "background-color": "red"}
            },

            title: {
                text: "Spieler Wertungen"
            },

            xAxis: {
                type: 'datetime'
            },

            yAxis: { // left y axis
                title: {
                    text: "Wertung"
                },
                min: 0
            },

            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.x:%e. %b}: {point.y:.2f}'
            },

            plotOptions: {
            }
        };

        this.playerService.playerGetAll()
            .subscribe(
                res => {
                    this.players = res;
                    this.onLoadPartial();
                    console.log(res);
                },
                error => {
                    this.onError.emit();
                }
            );

        this.ratingService.ratingGetAllRatings()
            .subscribe(
                res => {
                    this.ratings = res;
                    this.onLoadPartial();
                },
                error => {
                    this.onError.emit();
                }
            );
    }

    @Output() dataLoaded = new EventEmitter();
    @Output() onError = new EventEmitter();

    private onLoadPartial() {
        if (!!this.ratings && !!this.players && this.chart) {
            this.createCharts();
            this.dataLoaded.emit();
        }
    }


    saveInstance(chart) {
        this.chart = chart;
        this.onLoadPartial();
    }

    ngOnInit() {
    }

    private createCharts() {
        console.log(this.ratings.slice(0, 10));

        for (let i = 0; i < this.players.length; ++i) {
            let playerSeries = [];

            for (let j = 0; j < this.ratings.length; ++j) {
                if (this.ratings[j].Player.PlayerId == this.players[i].PlayerId) {
                    let date: Date = new Date(this.ratings[j].Datetime);
                    let utc = Date.UTC(date.getUTCFullYear(),
                        date.getUTCMonth(),
                        date.getUTCDate(),
                        date.getUTCHours(),
                        date.getUTCMinutes(),
                        date.getUTCSeconds(),
                        date.getUTCMilliseconds());
                    playerSeries.push([utc, this.ratings[j].Value]);
                }
            }
            console.log({
                name: this.players[i].Firstname + " " + this.players[i].Lastname ,
                data: playerSeries
            });

            this.chart.addSeries({
                name: this.players[i].Firstname + " " + this.players[i].Lastname,
                data: playerSeries.sort((a, b) => a[0] - b[0])
            });
        }
        console.log(this.chart);
    }
}
