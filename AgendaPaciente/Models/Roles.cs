﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AgendaPaciente.Models
{
    public class Roles : RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
          {
              throw new NotImplementedException();
          }
   
          public override string ApplicationName
          {
              get
              {
                  throw new NotImplementedException();
              }
              set
              {
                  throw new NotImplementedException();
              }
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
              //DEVMEDIAEntities db = new DEVMEDIAEntities();
   
              //string sRoles = db.ACESSO.Where(p => p.EMAIL == username).FirstOrDefault().PERFIL;
              string[] retorno = { "ADMIN" };
              return retorno;
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