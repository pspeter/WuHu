import {Injectable} from "@angular/core";
import {CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot, Router} from "@angular/router";
import {UserService} from "../user.service";


@Injectable()
export class LogoutGuard implements CanActivate {
    constructor(private userService : UserService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return !this.userService.IsUser();
    }
}
