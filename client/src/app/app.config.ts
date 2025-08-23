import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection  } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { InitService } from '../core/services/init-service';
import { lastValueFrom } from 'rxjs';


// here we add stuff that is used by the whole app
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes,withViewTransitions()),
    provideHttpClient(),
    provideAppInitializer(async () => {
      const initService = inject(InitService);

      return new Promise<void> ((resolve) =>{
        setTimeout(async () =>{
          try{
            return lastValueFrom(initService.init())
          }
          finally {
            const splash = document.getElementById('initial-splash');
            if(splash) {
              splash.remove()
            }
        resolve()
        }
        },500)

      })
      

    })
  ]
};
