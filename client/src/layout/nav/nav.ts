import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink,RouterLinkActive  } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';



@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {
  // here the account service is injected 
  protected accountService = inject(AccountService)
  private router = inject(Router)
  private toast = inject(ToastService)

  protected creds: any = {}
  protected loggedIn = signal(false)
  

  login() {
    console.log("Login Func from nav.ts is called by ngSubmit from loginform in nav.html");
    // const res = this.accountService.login(this.creds);

    // res.subscribe({
    //   next: result => console.log(result),
    //   error: error => alert(error.message)
    // })
    /**
     * alright so ,
     * Here the login method from accountservice returns an observable object (similar to response)
     * and We need to subscribe to this for something to happen
     * if we dont subscibe nothing happens when it comes to observables.
     */
    return this.accountService.login(this.creds).subscribe({
      next: () => {
        // rerouting the use when logged in
              this.router.navigateByUrl("/members");
              this.loggedIn.set(true);
              this.creds = {};
            },
      error:error => {
        this.toast.error(error.error)
      }
    })

  }

  logout(){
    this.accountService.logout();
    // rerouring the use when logged out
    this.router.navigateByUrl("/");
  }
}// class ends here
