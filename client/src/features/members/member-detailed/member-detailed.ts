import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { filter } from 'rxjs';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AgePipe } from '../../../core/pipe/age-pipe';
import { AccountService } from '../../../core/services/account-service';
import { MemberService } from '../../../core/services/member-service';

@Component({
  selector: 'app-member-detailed',
  imports: [  RouterLink , RouterLinkActive , RouterOutlet , AgePipe],
  templateUrl: './member-detailed.html',
  styleUrl: './member-detailed.css'
})
export class MemberDetailed implements OnInit {;
  private route= inject(ActivatedRoute);
  private router = inject(Router);
 
  protected accountService = inject(AccountService)
  protected memberService = inject(MemberService);
  protected title = signal<string | undefined>('Profile');
  protected isCurrentUser = computed(() => {
    // here we are using a computer signal where it take a callback function where it compares the current use id from 
    // account service and the id from the parameter of the route
    console.log("This is the current user from  isCurrentUse compute signal :" +this.accountService.currentUser()?.id )
    console.log("This is the current user from route param :" + this.route.snapshot.paramMap.get('id'))
    return this.accountService.currentUser()?.id=== this.route.snapshot.paramMap.get('id');
  });

  ngOnInit(): void {

    this.title.set(this.route.firstChild?.snapshot?.title);

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(
      {next : () =>{
        this.title.set(this.route.firstChild?.snapshot?.title)
      }}
    )
  }


}
