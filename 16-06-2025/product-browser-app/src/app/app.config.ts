import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { appRoutes } from './app.routes';

export const appConfig = {
  providers: [
    provideHttpClient(),
    provideRouter(appRoutes)
  ]
};
