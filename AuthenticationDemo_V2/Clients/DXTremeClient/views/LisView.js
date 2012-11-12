DXTremeClient.LisView = function (params) {
    var contacts = ko.observableArray();
    var handleContactListItemClick = function (e) {
        var uri = DXTremeClient.app.router.format({
            action: "DetailView",
            oid: e.itemData.oid
        });
        DXTremeClient.app.navigate(uri);
    };
    var handleLogOffClick = function (e) {
        DXTremeClient.currentUser.UserName("");
        DXTremeClient.currentUser.Password("");
        DXTremeClient.app.navigate("LogOn/null");
    };
    return {
        contacts: contacts,
        handleContactListItemClick: handleContactListItemClick,
        handleLogOffClick: handleLogOffClick,
        viewShowing: function () {
            DXTremeClient.db.Contact.load({
                sort: [{ field: "FirstName", desc: false }],
                //select: ["oid", "FirstName", "LastName", "Photo"]
            }).done(function (response) {
                contacts($.map(response, function (contact, index) {
                    return {
                        oid: contact.oid,
                        FirstName: contact.FirstName,
                        LastName: contact.LastName,
                        Photo: DXTremeClient.getImageUrl(contact.Photo)
                    }
                }));
            });
        }
    };
};