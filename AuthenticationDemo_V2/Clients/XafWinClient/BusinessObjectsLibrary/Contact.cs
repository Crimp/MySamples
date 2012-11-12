using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectsLibrary {
    [DefaultClassOptions]
    public class Contact : BaseObject {
        string firstName;
        string lastName;
        string email;
        Guid ownerId;

        public Contact(Session session)
            : base(session) {
        }
        public string FirstName {
            get {
                return firstName;
            }
            set {
                SetPropertyValue("FirstName", ref firstName, value);
            }
        }
        public string LastName {
            get {
                return lastName;
            }
            set {
                SetPropertyValue("LastName", ref lastName, value);
            }
        }
        public string Email {
            get {
                return email;
            }
            set {
                SetPropertyValue("Email", ref email, value);
            }
        }
        [Size(SizeAttribute.Unlimited), ValueConverter(typeof(ImageValueConverter))]
        public Image Photo {
            get { return GetDelayedPropertyValue<Image>("Photo"); }
            set { SetDelayedPropertyValue<Image>("Photo", value); }
        }
        [Browsable(false)]
        public Guid OwnerId {
            get {
                return ownerId;
            }
            set {
                SetPropertyValue("OwnerId", ref ownerId, value);
            }
        }
    }
}
