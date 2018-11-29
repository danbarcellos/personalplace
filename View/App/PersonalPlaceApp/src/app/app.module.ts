import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';

import { MyApp } from './app.component';
import { HomePage } from '../pages/home/home';
import { ListPage } from '../pages/list/list';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { IonicRatingModule } from 'ionic-rating';
import { HttpClientModule} from '@angular/common/http'
import { OfferPage } from '../pages/offer/offer';

@NgModule({
  declarations: [
    MyApp,
    HomePage,
    ListPage,
    OfferPage
  ],
  imports: [
    BrowserModule,
    IonicModule.forRoot(MyApp),
    IonicRatingModule,
    HttpClientModule
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    HomePage,
    ListPage,
    OfferPage
  ],
  providers: [
    StatusBar,
    SplashScreen,
    {provide: ErrorHandler, useClass: IonicErrorHandler}
  ]
})
export class AppModule {}
