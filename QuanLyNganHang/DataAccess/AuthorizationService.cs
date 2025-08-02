using QuanLyNganHang.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public class AuthorizationService
{
    private AuthorizationManager _authorization;

    public AuthorizationService()
    {
        _authorization = new AuthorizationManager();
    }

    public List<string> GetUsers()
    {
        var dt = _authorization.Get_User();
        return dt?.AsEnumerable().Select(r => r[0].ToString()).ToList() ?? new List<string>();
    }

    public List<string> GetRoles()
    {
        var dt = _authorization.Get_Roles();
        return dt?.AsEnumerable().Select(r => r[0].ToString()).ToList() ?? new List<string>();
    }

    public List<string> GetProcedures(string userOwner, string type)
    {
        var dt = _authorization.Get_Procedure_User(userOwner, type);
        return dt?.AsEnumerable().Select(r => r[0].ToString()).ToList() ?? new List<string>();
    }

    public List<string> GetTables(string userOwner)
    {
        var dt = _authorization.Get_Table_User(userOwner);
        return dt?.AsEnumerable().Select(r => r[0].ToString()).ToList() ?? new List<string>();
    }

    public DataTable GetRolesUser(string user)
    {
        return _authorization.Get_Roles_User(user);
    }

    public DataTable GetGrantUser(string user)
    {
        return _authorization.Get_Grant_User(user);
    }

    public DataTable GetGrant(string principal, string schema, string objectName)
    {
        return _authorization.Get_Grant(principal, schema, objectName);
    }

    public int GetRolesUserCheck(string user, string role)
    {
        return _authorization.Get_Roles_User_Check(user, role);
    }

    // Tham số theo đúng method trong AuthorizationManager:
    // username, userschema, tablename, typepro, dk
    public bool GrantRevokePro(string username, string userschema, string tablename, string typepro, int dk)
    {
        return _authorization.Grant_Revoke_Pro(username, userschema, tablename, typepro, dk);
    }

    // Tham số theo đúng method ở AuthorizationManager:
    // username, role, dk
    public bool GrantRevokeRole(string username, string role, int dk)
    {
        return _authorization.Grant_Revoke_Role(username, role, dk);
    }
    public bool GrantLogin(string username)
    {
        return _authorization.GrantLogin(username);
    }
    public bool RevokeLogin(string username)
    {
        return _authorization.RevokeLogin(username);
    }

    public bool GrantAllTables(string username)
    {
        return _authorization.GrantAllTables(username);
    }
}
