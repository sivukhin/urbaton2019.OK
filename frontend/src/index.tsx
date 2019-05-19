import * as React from "react";
import * as ReactDOM from "react-dom";
import {HashRouter} from "react-router-dom";
import {ApplicationComponent} from "./ApplicationComponent";
import BackendApi from "./BackendApi";

const backendApi = new BackendApi("68.183.45.217:8888");

ReactDOM.render(
    <HashRouter>
        <ApplicationComponent backendApi={backendApi}/>
    </HashRouter>,
    document.getElementById("app"),
);
