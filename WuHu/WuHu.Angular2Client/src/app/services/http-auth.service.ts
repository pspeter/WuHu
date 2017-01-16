import {Injectable} from '@angular/core';
import {Headers, Http, RequestOptionsArgs} from "@angular/http";
import {AccountApi} from "../api/AccountApi";

@Injectable()
export class HttpAuthService {
  constructor(private http: Http) {
  }

  addAuthorizationHeader(header) {
    let authToken = localStorage.getItem('auth_token');
    header.set('Authorization', `Bearer ${authToken}`);
    header.set('Content-Type', 'application/json');
  }

  request(url, options: RequestOptionsArgs) {
    this.addAuthorizationHeader(options.headers);
    return this.http.request(url, options);
  }

  post(url, data, options: RequestOptionsArgs) {
    this.addAuthorizationHeader(options.headers);
    return this.http.post(url, data, options);
  }

  /*
   get(url, options : RequestOptionsArgs) {
   addAuthorizationHeader(options);
   return this.http.get(
   url,
   {headers: this.allHeaders}
   );
   }


   put(url, data) {
   this.allHeaders = new Headers();
   this.createAuthorizationHeader();
   return this.http.put(url, data, {
   headers: this.allHeaders
   });
   }*/
}
