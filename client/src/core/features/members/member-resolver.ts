import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { MemberService } from '../../services/member-service';
import { Member } from '../../../types/member';
import { EMPTY } from 'rxjs';

// this is to add data to a route , so whenever that route is accessed there is data in it.
// so in this case we are attaching the data of the member with the id , where the id is a parameter in the route
// we can see in the app.route.ts we are using it under the member/:id route
export const memberResolver: ResolveFn<Member> = (route, state) => {
  const memberService = inject(MemberService);

  const router = inject(Router);

  // get the id from route
  const memberId = route.paramMap.get('id');

  if(!memberId) {
    console.log(memberId)
    router.navigateByUrl('/not-found');
    return EMPTY;
  };

  return memberService.getMember(memberId);
};
