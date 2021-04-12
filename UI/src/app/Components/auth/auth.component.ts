import { Component, OnInit } from '@angular/core';
import { UrlModel } from '../../Models/url-model';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../Services/Auth/auth.service';
import { AuthorizationService } from '../../Services/Authorization/authorization.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {

  /**
   * Constructs the Authorization Component which checks if the user has permissions to access the site
   * 
   * @param route activated route
   * @param router router for navigation
   * @param auth authorization service to check DB for user credentials
   */
  constructor(private route: ActivatedRoute, private router: Router, private auth: AuthService) {
      this.route.fragment.subscribe((x: string) => {
        if(x.includes("access_token")) {
          var params = x.split("&");
          var token = params[0];
          var accessToken = token.replace(/access_token=/gi, "");

          this.auth.validateToken(accessToken).subscribe(rsp => {
            if (rsp.Error === '' || rsp.Error == null) {
              localStorage.setItem('user_id', rsp.userId);
              localStorage.setItem('name', rsp.userName);
              localStorage.setItem('app_token_JWT', rsp.appJwt);
              localStorage.setItem('app_token', accessToken);
              localStorage.setItem('email', rsp.email);
              const expDate = new Date(rsp.expireDateTime);
              localStorage.setItem('expire_date', expDate.valueOf().toString());
  
              const urlObj: UrlModel = <UrlModel>JSON.parse(localStorage.getItem('currentPage'));                              
  
              if (urlObj.URL !== '') {
                this.router.navigate([`${urlObj.URL}`], { queryParams: urlObj.QueryParams });
              } else {
                this.router.navigate(['']);
              }             
            } else { 
              localStorage.setItem('AuthErrorMessage', rsp.Error);
              this.router.navigate([`/error/`]);
            }
          });
        }
        
        
      });
  }

  ngOnInit() {
  }

}
