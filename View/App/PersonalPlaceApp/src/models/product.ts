export class Product {
    id: string;
    name: string;
    img: string;
    price: string; 
    rate: number; 
    feature1: string;
    feature2: string;
    feature3: string;
    feature4: string;
    feature5: string;

    constructor(id: string, name: string, img: string, price: string, rate: number, feature1: string, feature2: string, feature3: string, feature4: string, feature5: string) {
        this.id = id
        this.name = name
        this.img = img
        this.price = price
        this.rate = rate
        this.feature1 = feature1
        this.feature2 = feature2
        this.feature3 = feature3
        this.feature4 = feature4
        this.feature5 = feature5
    }
}