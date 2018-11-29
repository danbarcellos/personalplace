import { Component } from "@angular/core";
import { NavParams, ViewController } from "ionic-angular";
import { Product } from '../../models/product'

@Component({
    templateUrl:'offer.html',
    selector: 'offer'
})
export class OfferPage{
    product: Product;

    constructor(private navParams: NavParams, private view: ViewController){
        this.product = navParams.get('product')
    }
    
    onCloseOffer()
    {
        this.view.dismiss();
    }
}