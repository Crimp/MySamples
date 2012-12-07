
(function() {
    DXTremeClient.ContactViewModel = function(data) {
            this.oid = ko.observable();
            this.FirstName = ko.observable();
            this.LastName = ko.observable();
            this.Email = ko.observable();
            this.Photo = ko.observable();
            if(data)
                this.fromJS(data);
    };

    $.extend(DXTremeClient.ContactViewModel.prototype, {
        toJS: function() {
            return {
                oid: this.oid(),
                FirstName: this.FirstName(),
                LastName: this.LastName(),
                Email: this.Email(),
                Photo: this.Photo(),
            };
        },

        toJS_Save: function () {
            return {
                FirstName: this.FirstName(),
                LastName: this.LastName(),
                Email: this.Email(),
            };
        },

        fromJS: function(data) {
            if(data) {
                this.oid(data.oid);
                this.FirstName(data.FirstName);
                this.LastName(data.LastName);
                this.Email(data.Email);
                this.Photo(DXTremeClient.getImageUrl(data.Photo));
            }
        }
    });
})();