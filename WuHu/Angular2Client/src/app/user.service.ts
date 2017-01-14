import {Injectable} from '@angular/core';
import {HttpAuthService} from "./http-auth.service";
import {environment} from "../environments/environment";
import 'rxjs/add/operator/map'
import {RequestOptionsArgs, Headers, URLSearchParams} from "@angular/http";
import {Router} from "@angular/router";

@Injectable()
export class UserService {
    private loggedIn = false;

    constructor(private httpAuthenticated: HttpAuthService, private router: Router) {
        this.loggedIn = !!localStorage.getItem('auth_token');
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
                    localStorage.setItem('auth_token', json.access_token);
                    this.loggedIn = true;
                    return true;
                } else {
                    return false;
                }

            });
    }

    logout() {
        localStorage.removeItem('auth_token');
        this.router.navigate(["login"]);
        this.loggedIn = false;
    }

    isLoggedIn() {
        return this.loggedIn;
    }
}
