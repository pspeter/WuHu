import {Injectable} from '@angular/core';
import {HttpAuthService} from "./http-auth.service";
import {environment} from "../environments/environment";
import 'rxjs/add/operator/map'
import {RequestOptionsArgs, Headers, URLSearchParams} from "@angular/http";
import {Router} from "@angular/router";

@Injectable()
export class UserService {
    private username: string = "";
    private role: string = "Guest";

    constructor(private httpAuthenticated: HttpAuthService, private router: Router) {
        this.username = localStorage.getItem("username");
        this.role = localStorage.getItem("role");
    }

    login(username, password) {

        let headerParams: Headers = new Headers();
        let queryParameters = new URLSearchParams();

        headerParams.append('Content-Type', 'application/x-www-form-urlencoded');
        let body = 'grant_type=password&username=' + username + '&password=' + password;


        let requestOptions: RequestOptionsArgs = {
            method: 'POST',
            headers: headerParams,
            search: queryParameters
        };


        return this.httpAuthenticated
            .post(environment.baseApiUrl + 'token', body, requestOptions)
            .map((res) => {
                let json = res.json();
                if (json) {
                    localStorage.setItem("auth_token", json.access_token);
                    localStorage.setItem("role", json.role);
                    localStorage.setItem("username", json.username);
                    this.username = json.username;
                    this.role = json.role;
                    return true;
                } else {
                    return false;
                }

            });
    }

    logout() {
        localStorage.removeItem('auth_token');
        localStorage.removeItem('role');
        localStorage.removeItem('username');
        this.router.navigate(["login"]);
        this.role = "Guest";
        this.username = "";
    }

    IsUser() {
        return this.role == "User" || this.role == "Admin";
    }

    IsAdmin() {
        return this.role == "Admin";
    }
}
