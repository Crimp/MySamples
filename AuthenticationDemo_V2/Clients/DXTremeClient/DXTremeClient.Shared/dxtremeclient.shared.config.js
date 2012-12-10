
// NOTE object below must be a valid JSON
window.DXTremeClient = $.extend(true, window.DXTremeClient, {
    "config": {
        "endpoints": {
            "db": {
                "local": "http://localhost:54002/CustomAuthenticationDataService.svc",
                "production": "http://localhost:54002/CustomAuthenticationDataService.svc"
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
