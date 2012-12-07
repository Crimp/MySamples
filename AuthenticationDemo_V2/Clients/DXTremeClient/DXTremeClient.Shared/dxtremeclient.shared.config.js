
// NOTE object below must be a valid JSON
window.DXTremeClient = $.extend(true, window.DXTremeClient, {
    "config": {
        "endpoints": {
            "db": {
                "local": "http://minakov-w8.corp.devexpress.com/CustomAuthenticationService/CustomAuthenticationDataService.svc",
                "production": "http://minakov-w8.corp.devexpress.com/CustomAuthenticationService/CustomAuthenticationDataService.svc"
            }
        },
        "services": {
            "db": {
                "entities": {
					"Contact": { 
						key: "oid", 
						keyType: "Guid" 
					},
                }
            }
        }
    }
});
