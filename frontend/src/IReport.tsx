import {IGeoLocation} from "./IGeoLocation";

export interface IReport {
    creationDate: Date;
    location: IGeoLocation;
    subject: string;
    reportText: string;
    attachments: any[];
    responsibleId: string;
}