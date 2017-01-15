import {Component} from '@angular/core';
import {UserService} from "./user.service";
import {Router} from "@angular/router";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.sass']
})
export class AppComponent {

    constructor(private userService : UserService) { }

    logout() {
        this.userService.logout();
    }
}
