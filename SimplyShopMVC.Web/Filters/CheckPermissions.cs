using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SimplyShopMVC.Web.Filters
{
    public class CheckPermissions : Attribute, IAuthorizationFilter
    {
        private readonly string _permission;
        public CheckPermissions(string permission)
        {
            _permission = permission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
           bool isAuthorized = CeckUserPermission(context.HttpContext.User, _permission);
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }         
        }

        private bool CeckUserPermission(ClaimsPrincipal user, string permission)
        {
            //połącz z bazą danych
            //pobierz użytkownika
            
            //sprawdz czy ma uprawnienia
            return permission == "Read";
        }
    }
}
