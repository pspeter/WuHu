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
    private selectedCount: number = 0;
    private warnMaxSelected: boolean = false;
    private warnTimout;
    private _players: Array<Player>;
    private readonly MAX_SERIES = 5;

    @Input()
    set players(players: Array<Player>) {
        this._players = players;
    }
    get players() { return this._players; }

    private bgColor = "#fff";
    private primaryColor = "#95a5a6";
    private secondaryColor = "#b4bcc2";
    private hiddenColor = "#ecf0f1";

    constructor() {
        this.options = {
            chart: {
                zoomType: "x",
                backgroundColor: this.bgColor,
                width: null,
            },

            title: {
                text: "Spieler Wertungen",
                style: {
                    color: this.secondaryColor
                }
            },

            legend: {
                itemStyle: {
                    "color": this.secondaryColor
                },
                itemHoverStyle: {
                    "color": this.primaryColor
                },
                itemHiddenStyle: {
                    "color": this.hiddenColor
                }
            },

            labels: {
                style: {
                    "color": this.secondaryColor
                }
            },

            xAxis: {
                type: 'datetime',
                title: {
                    text: "Wertung",

                    style: {
                        "color": this.secondaryColor
                    }
                },
                lineColor: this.secondaryColor,
                labels: {
                    style: {
                        "color": this.secondaryColor
                    }
                }
            },

            yAxis: { // left y axis
                title: {
                    text: "Wertung",

                    style: {
                        "color": this.secondaryColor
                    }
                },
                min: 0,
                labels: {
                    style: {
                        "color": this.secondaryColor
                    }
                }
            },

            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.x:%e. %b}: {point.y:.2f}'
            },

            plotOptions: {
            }
        };
    }

    saveInstance(chart) {
        this.chart = chart;
        let chartResizer = setInterval(() => this.chart.reflow(), 50);
        setTimeout(() => clearInterval(chartResizer), 2000);
    }

    ngOnInit() {
    }

    private selectPlayer(player: Player) {
        if (player.IsSelected) {
            --this.selectedCount;
            player.IsSelected = !player.IsSelected;
            this.createCharts();
        } else {
            if (this.selectedCount < this.MAX_SERIES) {
                ++this.selectedCount;
                player.IsSelected = !player.IsSelected;
                this.createCharts();
            } else {
                this.warnMaxSelected = true;
                clearTimeout(this.warnTimout);
                this.warnTimout = setTimeout(() => this.warnMaxSelected = false, 3000);
            }
        }
    }

    private createCharts() {
        while(this.chart.series.length > 0) {
            this.chart.series[0].remove(true);
        }
        for (let i = 0; i < this.players.length; ++i) {
            if (this.players[i].IsSelected) {
                this.chart.addSeries({
                    name: this.players[i].Firstname + " " + this.players[i].Lastname,
                    data: this.players[i].RatingData.sort((a, b) => a[0] - b[0]),

                });
            }
        }
    }
}
