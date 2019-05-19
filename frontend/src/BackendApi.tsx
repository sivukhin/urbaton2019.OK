import axios from "axios";
import * as React from "react";
import {IReport} from "./IReport";
import {IResponsible} from "./IResponsible";

export interface IBackendApi {
    getReport(reportId: string): Promise<IReport>;

    getReports(start: number, count: number): Promise<IReport[]>;

    getReportsOfResponsible(responsibleId: string, start: number, count: number): Promise<IReport[]>;

    getResponsibles(start: number, count: number): Promise<IResponsible[]>;

    getResponsible(responsibleId: string): Promise<IResponsible>;

    addDoubler(responsibleId: string, doubler: IResponsible): Promise<void>;

    getImageLink(reportId: string, pictureId: number): string;
}

export default class BackendApi implements IBackendApi {
    private serverAddress: string;

    constructor(serverAddress) {
        this.serverAddress = serverAddress;
    }

    public async makeGetRequest(path: string): Promise<any> {
        return await axios.get(`http://${this.serverAddress}/api/${path}`);
    }

    public async makePostRequest(path: string, data: any): Promise<any> {
        return await axios.post(`http://${this.serverAddress}/api/${path}`, data);
    }

    public async addDoubler(responsibleId: string, doubler: IResponsible): Promise<void> {
        await this.makePostRequest("add-doubler", {
            responsibleId: responsibleId,
            doubler: doubler
        });
    }

    public async getReport(reportId: string): Promise<IReport> {
        const result = await this.makeGetRequest(`report/${reportId}`);
        return result.data;
    }

    public async getReports(start: number, count: number): Promise<IReport[]> {
        const result = await this.makeGetRequest(`reports?start=${start}&count=${count}`);
        return result.data;
    }

    public async getReportsOfResponsible(responsibleId: string, start: number, count: number): Promise<IReport[]> {
        const result = await this.makeGetRequest(`reports/${responsibleId}?start=${start}&count=${count}`);
        return result.data;
    }

    public async getResponsibles(start: number, count: number): Promise<IResponsible[]> {
        const result = await this.makeGetRequest(`responsibles?start=${start}&count=${count}`);
        return result.data;
    }

    public getImageLink(reportId: string, pictureId: number): string {
        return `http://${this.serverAddress}/api/report/${reportId}/image/${pictureId}`;
    }

    public async getResponsible(responsibleId: string): Promise<IResponsible> {
        const result = await this.makeGetRequest(`responsible/${responsibleId}`);
        return result.data;
    }
}
