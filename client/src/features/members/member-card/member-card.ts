import { Component, input } from '@angular/core';
import { Member } from '../../../types/member';
import { RouterLink } from "@angular/router";
import { AgePipe } from '../../../core/pipe/age-pipe';

@Component({
  selector: 'app-member-card',
  imports: [RouterLink , AgePipe],
  templateUrl: './member-card.html',
  styleUrl: './member-card.css'
})
export class MemberCard {
  // this is an input property , provide by angular core
  // it is now a signal
  public member = input.required<Member>();


}
