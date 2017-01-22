import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {UserService} from "../../services/user.service";
import {FormBuilder, Validators, FormGroup} from "@angular/forms";
import {logger} from "codelyzer/util/logger";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {
    private errorMessage: string = "";
    private loginModel: {username: string, password: string} = {username: "", password: ""};

    constructor(private userService: UserService, private router: Router) { }

    ngOnInit() {
    }

    private onChange() {
        this.errorMessage = "";
    }

    private login() {
        this.userService
            .login(this.loginModel.username, this.loginModel.password)
            .subscribe(
                response => {
                    this.router.navigate(['dashboard']);
                    this.errorMessage = "";
                },
                error => {
                    if (error.status == 400) {
                        this.errorMessage = "Falscher Nutzername oder Passwort";
                    }
                    else if (error.status == 500) {
                        this.errorMessage = "Oops. Da ging was schief"
                    }
                    else {
                        this.errorMessage = "Verbindungsproblem"
                    }
                }
            );

    }


}
