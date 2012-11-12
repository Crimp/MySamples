using BusinessObjectsLibrary;
using DataProvider;
using DBCreator.Properties;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;

namespace DBCreator {
    public class DBUpdater {
        IObjectSpace objectSpace;
        public DBUpdater(string connectionString, Assembly[] assemblies) {
            DataServiceHelper dataServiceHelper = new DataServiceHelper(connectionString, assemblies, "");
            objectSpace = dataServiceHelper.ObjectSpaceProvider.CreateUpdatingObjectSpace(true);
        }
        public DBUpdater(IObjectSpace objectSpace) {
            this.objectSpace = objectSpace;
        }
        public void CreateDB() {
            CreateTestData();
        }
        public void AddNewUser(string userName, string lastName, string email, System.Drawing.Image photo, string password) {
            string criteria = string.Format("UserName == '{0}'", userName);
            SecuritySystemUser newUser = objectSpace.FindObject<SecuritySystemUser>(CriteriaOperator.Parse(criteria));
            if(newUser == null) {
                newUser = objectSpace.CreateObject<SecuritySystemUser>();
                newUser.UserName = userName;
                newUser.SetPassword(password);

                SecuritySystemRole userRole = objectSpace.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Users"));
                newUser.Roles.Add(userRole);
                newUser.Save();
                objectSpace.CommitChanges();
            }
            AddContact(userName, lastName, email, photo, newUser.Oid);
        }
        public void AddContact(string firstName, string lastName, string email, System.Drawing.Image photo, Guid ownerId) {
            string criteria = string.Format("FirstName == '{0}' && LastName == '{1}'", firstName, lastName);
            Contact newUserContact = objectSpace.FindObject<Contact>(CriteriaOperator.Parse(criteria));
            if(newUserContact == null) {
                newUserContact = objectSpace.CreateObject<Contact>();
                newUserContact.FirstName = firstName;
                newUserContact.LastName = lastName;
                newUserContact.Email = email;
                newUserContact.OwnerId = ownerId;
                newUserContact.Photo = photo;
                objectSpace.CommitChanges();
            }
        }
        private void CreateTestData() {
            CreateAdminRole();
            CreateUserRole();
            objectSpace.CommitChanges();

            AddContact("Mary", "Tellitson", "mary_tellitson@md.com", null, Guid.NewGuid());
            AddContact("Advanced User", "Protected", "Protected Object.com", null, Guid.NewGuid());
            AddNewUser("Sam", "Tellitson", "sam_tellitson@md.com", Resource.Sam, "");
            AddNewUser("John", "Nilsen", "john_nilsen@md.com", Resource.John, "");
            //AddNewUser(WindowsIdentity.GetCurrent().Name, "", "", null, "");

            objectSpace.CommitChanges();
        }
        private void CreateAdminRole() {
            SecuritySystemUser userAdmin = objectSpace.FindObject<SecuritySystemUser>(new BinaryOperator("UserName", "Admin"));
            if(userAdmin == null) {
                userAdmin = objectSpace.CreateObject<SecuritySystemUser>();
                userAdmin.UserName = "Admin";
                userAdmin.SetPassword("");

                SecuritySystemRole adminRole = objectSpace.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Administrators"));
                if(adminRole == null) {
                    adminRole = objectSpace.CreateObject<SecuritySystemRole>();
                    adminRole.Name = "Administrators";
                }
                adminRole.IsAdministrative = true;
                userAdmin.Roles.Add(adminRole);
            }
        }
        private void CreateUserRole() {
            SecuritySystemRole userRole = objectSpace.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Users"));
            if(userRole == null) {
                userRole = objectSpace.CreateObject<SecuritySystemRole>();
                userRole.Name = "Users";

                SecuritySystemTypePermissionObject typePermissions = objectSpace.CreateObject<SecuritySystemTypePermissionObject>();
                typePermissions.TargetType = typeof(Contact);
                typePermissions.AllowNavigate = true;
                typePermissions.Save();
                userRole.TypePermissions.Add(typePermissions);

                SecuritySystemMemberPermissionsObject readOnlyMemberPermission = objectSpace.CreateObject<SecuritySystemMemberPermissionsObject>();
                readOnlyMemberPermission.Members = "FirstName; Photo";
                readOnlyMemberPermission.AllowRead = true;
                readOnlyMemberPermission.Save();
                typePermissions.MemberPermissions.Add(readOnlyMemberPermission);

                SecuritySystemObjectPermissionsObject fullAccessObjectPermission = objectSpace.CreateObject<SecuritySystemObjectPermissionsObject>();
                fullAccessObjectPermission.Criteria = "[OwnerId] = CurrentUserId()";
                fullAccessObjectPermission.AllowNavigate = true;
                fullAccessObjectPermission.AllowRead = true;
                fullAccessObjectPermission.AllowWrite = true;
                fullAccessObjectPermission.Save();
                typePermissions.ObjectPermissions.Add(fullAccessObjectPermission);

                SecuritySystemObjectPermissionsObject protectedContentObjectPermission = objectSpace.CreateObject<SecuritySystemObjectPermissionsObject>();
                protectedContentObjectPermission.Criteria = "[LastName] Not Like '%Protected%'";
                protectedContentObjectPermission.AllowNavigate = true;
                protectedContentObjectPermission.AllowRead = true;
                protectedContentObjectPermission.Save();
                typePermissions.ObjectPermissions.Add(protectedContentObjectPermission);
            }
        }
    }
}