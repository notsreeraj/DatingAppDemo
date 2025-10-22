import { HttpEvent, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BusyService } from './busy-service';
import { delay, finalize, of, tap } from 'rxjs';

const cache = new Map<string , HttpEvent<unknown>>();


export const loadingInterceptor: HttpInterceptorFn = (req, next) => {

  const busyService = inject(BusyService);

  if(req.method ==='GET'){
    // we are trying to get the value of the current req url from the cache if there is any
    const cachedResponse = cache.get(req.url);
    if(cachedResponse){
      return of(cachedResponse);
    }
  }

  // incremen the busy count
  busyService.busy()
  return next(req).pipe(
    delay(500),
    // this is the side effect , we use tap method to assign sideeffect which affects the outside state
    // we are adding the response and request to the caches dictionary or haspmap , 
    tap(response =>{
      cache.set(req.url,response)
    }),
    finalize(() =>{
        busyService.idle();
    })
  );
};
