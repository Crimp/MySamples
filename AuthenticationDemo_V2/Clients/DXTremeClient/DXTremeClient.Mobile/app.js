
$(function() {
    
    DXTremeClient.app = new DevExpress.framework.html.HtmlApplication({
        ns: DXTremeClient,
        viewPortNode: document.getElementById("viewPort"),
        defaultLayout: DXTremeClient.config.defaultLayout,
        navigation: DXTremeClient.config.navigation
    });


    DXTremeClient.getImageUrl = function (base64Data) {
        if (!base64Data) {
            return "images/NoImage.jpg";
        }
        return "data:image/jpg;base64," + base64Data;
    };

    DXTremeClient.DataManipulationRight = DataManipulationRight(DXTremeClient.db._url);

    DXTremeClient.app.router.register(":view/:id", { view: "Contact", id: undefined });
    DXTremeClient.app.router.register(":view/:id", { view: "About", id: undefined });
    DXTremeClient.app.router.register("LogOn/:oid", { view: "LogOn", oid: undefined });
});
