import {Component, OnInit} from '@angular/core';
import {WebsocketService} from "../../api/websocket.service";

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.sass']
})
export class DashboardComponent implements OnInit {
    constructor() {

    }

    ngOnInit() {
    }

}
