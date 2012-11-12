(function($, DX, undefined) {

    var NAVIGATION_TOGGLE_DURATION = 400;

    DX.framework.html.NavbarLayoutController = DX.framework.html.DefaultLayoutController.inherit({
        _updateNavigationSelectedItem: function(viewInfo) {
            var $markup = viewInfo.renderResult.$markup;
            var navBar = $markup.find("#navBar").data("dxNavBar");

            //TODO there should be a capability to obtain non-observable items from widget
            var items = navBar.option("items");
            var currentUri = "#" + viewInfo.uri;
            for(var i = 0; i < items.length; i++) {
                if(items[i].uri === currentUri) {
                    navBar.option("selectedIndex", i);
                    break;
                }
            }
        }
    });

    DX.framework.html.layoutControllers.navbar = new DX.framework.html.NavbarLayoutController();

})(jQuery, DevExpress);