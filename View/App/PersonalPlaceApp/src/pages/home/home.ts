import { Component, ChangeDetectorRef } from '@angular/core';
import { Platform, ModalController, Modal } from 'ionic-angular'
import { NavController } from 'ionic-angular';
import { HttpClient } from '@angular/common/http';
import { Product } from '../../models/product'
import { OfferPage } from '../offer/offer';

declare var evothings: any;

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {

  beacon: any;
  products: Product[] = [];
  selectedItem: Product;
  timer: number;
  currentModal: Modal;
  currentProductId: string;

  constructor(public navCtrl: NavController,
    private http: HttpClient,
    private platform: Platform,
    private change: ChangeDetectorRef,
    private modal: ModalController) {
    http.get<any>("http://localhost:60164/")
      .subscribe(p => {
        for (let product of p) {
          this.products.push(
            new Product(product.id,
              product.name,
              product.img,
              product.price,
              product.rate,
              product.feature1,
              product.feature2,
              product.feature3,
              product.feature4,
              product.feature5));

          console.info(product.name);
        }

        if (this.products.length > 0)
          this.selectedItem = this.products[0];

        this.currentModal = this.modal.create(OfferPage, { product: this.selectedItem })
        this.currentModal.present();
      });

    if (this.platform.is('cordova')) {
      this.timer = setInterval(() => {
        evothings.eddystone.startScan(b => {
          this.beacon = b;
          var filteredProduct = this.products.filter(p => { return p.id == this.beacon.address });

          if (filteredProduct.length < 1)
            return;

          var product = filteredProduct[0];

          if (this.currentProductId == product.id)
            return;

          this.currentProductId = product.id;

          if (this.currentModal)
            this.currentModal.dismiss();

          this.currentModal = this.modal.create(OfferPage, { product: product })
          this.currentModal.present();
          setTimeout(() => this.change.detectChanges(), 1000);
        })
      }, 5000)
    }
  }

  onSelected(item: Product) {
    this.selectedItem = item;
  }
}
