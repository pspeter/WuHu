import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {UserService} from "../../user.service";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {
    private falseLogin: boolean;

    constructor(private userService: UserService, private router: Router) {
    }

    login(event, username, password) {
        this.userService
            .login(username, password)
            .subscribe(
                result => {
                    this.router.navigate(['dashboard']);
                    console.log("Successful login");
                    this.falseLogin = false;
                },
                error => {
                    if (error.status == 400) {
                        this.falseLogin = true;
                    }
                    console.log("failed login");
                }
            );
    }

    ngOnInit() {
    }

}
