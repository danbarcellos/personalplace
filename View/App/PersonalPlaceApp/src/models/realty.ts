export class Realty {
    clientId : string;
    addressDetail: string;
    description: string;
    furnished: boolean;
    disabilityAccess: boolean;
    totalRooms: number;
    totalSuites: number;
    age: number;
    rentValue: string;
    saleValue: string;
    postDateTime: string;
    images: string[];
        
    constructor(clientId : string,
                addressDetail: string,
                description: string,
                furnished: boolean,
                disabilityAccess: boolean,
                totalRooms: number,
                totalSuites: number,
                age: number,
                rentValue: string,
                saleValue: string,
                postDateTime: string,
                images: string[]) {
                    this.clientId = clientId;
                    this.addressDetail = addressDetail;
                    this.description= description;
                    this.furnished = furnished;
                    this.disabilityAccess =  disabilityAccess;
                    this.totalRooms = totalRooms;
                    this.totalSuites = totalSuites;
                    this.age = age;
                    this.rentValue = rentValue;
                    this.saleValue = saleValue;
                    this.postDateTime = postDateTime;
                    this.images = images;
    }
        // public Address Address { get; set; }        
        // public IEnumerable<FloorplanDTO> Floorplans { get; set; }
        // public IEnumerable<AmenityDTO> Amenities { get; set; }
        // public IEnumerable<CommentDTO> Comments { get; set; }
}