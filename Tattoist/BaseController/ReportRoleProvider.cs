using TattoistDAL.EntityFramework;
using TattoistDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using TattoistDAL.EntityFramework.Concrete;

namespace Tattoist.BaseController
{
    public class ReportRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            UnitOfWork unitOfWork = new UnitOfWork(SingletonContext<AppDbContext>.Instance);
                var list = new List<string>();

            var user = unitOfWork.User.Get(x => x.UserName == username);
            string usertype = string.Empty;
            switch (user.UserType)
            {
                case 1:
                    usertype = "Admin";
                    break;
                case 2:
                    usertype = "Designer";
                    break;
                case 3:
                    usertype = "Tattoo";
                    break;
                case 4:
                    usertype = "Report";
                    break;
                default:
                    break;
            }

            list.Add(usertype);

            return list.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}