import {Injectable} from "@angular/core";
import {CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot, Router} from "@angular/router";
import {UserService} from "./user.service";


@Injectable()
export class AdminGuard implements CanActivate {
    constructor(private userService : UserService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        let loggedIn = this.userService.IsAdmin();
        if (!loggedIn) {
            this.router.navigate(["login"]);
        }
        return loggedIn;
    }
}
