window.DXTremeClient = window.DXTremeClient || {};
$.support.cors = true;
$(function () {
    var URL = "http://minakov-w8.corp.devexpress.com/CustomAuthenticationService/CustomAuthenticationDataService.svc/"

    DXTremeClient.serviceUrl = URL;
    DXTremeClient.getImageUrl = function (base64Data) {
        if (!base64Data) {
            return "images/NoImage.png";
        }
        return "data:image/jpg;base64," + base64Data;
    };
    DXTremeClient.DataManipulationRight = DataManipulationRight(DXTremeClient.serviceUrl);
    app = DXTremeClient.app = new DevExpress.framework.html.HtmlApplication({
        ns: DXTremeClient,
        viewPortNode: document.getElementById("viewPort"),
        defaultLayout: "navbar",
        navigation: [
            new DevExpress.framework.Command({
                title: "About",
                uri: "about",
                icon: "about",
                location: "navigation"
            })
        ]
    });

    app.router.register("ContactEditView/:oid/:index", { view: "ContactEditView" });
    app.router.register("DetailView/:oid", { view: "DetailView" });
    app.router.register("LogOn/:oid", { view: "LogOn", oid: undefined });
    app.router.register(":view", { view: "LisView" });
});
