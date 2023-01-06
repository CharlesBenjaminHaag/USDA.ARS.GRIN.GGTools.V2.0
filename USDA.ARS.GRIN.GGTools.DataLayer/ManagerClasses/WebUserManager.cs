using System;
using System.Collections.Generic;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class WebUserManager : AppDataManagerBase
    {
        public WebUser Get(int entityId, string environment = "")
        {
            WebUser webUser = new WebUser();

            SQL = "usp_GRINGlobal_Web_User_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_user_id", (object)entityId, false)
            };
            webUser = GetRecord<WebUser>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            parameters.Clear();
            return webUser;
        }

        public virtual List<WebUser> Search(WebUserSearch searchEntity)
        {
            WebUser webUser = new WebUser();
            List<WebUser> webUsers = new List<WebUser>();

            SQL = " SELECT * FROM vw_GGTools_GRINGLobal_WebUsers";
            SQL += " WHERE  (@UserName  IS NULL OR  UserName = @UserName)";
            SQL += " AND    (@ID        IS NULL OR  ID = @ID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("UserName", (object)searchEntity.WebUserName ?? DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true)
            };

            webUser = GetRecord<WebUser>(SQL, parameters.ToArray());
            if (webUser.WebUserID == 0)
            {
                return webUsers;
            }
            parameters.Clear();

            webUsers.Add(webUser);
            RowsAffected = webUsers.Count;
            return webUsers;
        }
        public virtual List<SysGroupUserMap> SelectGroups(int sysUserId)
        {
            List<SysGroupUserMap> sysGroupUserMaps = new List<SysGroupUserMap>();
            SQL = "usp_GRINGlobal_Sys_Group_User_Map_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("sys_user_id", (object)sysUserId, false)
            };
            sysGroupUserMaps = GetRecords<SysGroupUserMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            parameters.Clear();

            RowsAffected = sysGroupUserMaps.Count;
            return sysGroupUserMaps;
        }
        
        public int InsertWebUserPasswordResetToken(int sysUserId, string passwordResetToken)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GGTools_GRINGlobal_WebUserPasswordResetToken_Insert";

            AddParameter("sys_user_id", (object)sysUserId, false);
            AddParameter("password_reset_token", (object)passwordResetToken, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_user_password_reset_token_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            var errorCode = GetParameterValue<string>("@out_error_number", "");
            return RowsAffected;
        }

        public int ValidateWebUserPasswordResetToken(string passwordResetToken)
        {
            WebUser sysUser = new WebUser();
            SQL = "usp_GGTools_GRINGlobal_WebUserPasswordResetToken_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("password_reset_token", (object)passwordResetToken, false)
            };
            sysUser = GetRecord<WebUser>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysUser.ID;
        }

        public int Insert(WebUser entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebUser>(entity);
            SQL = "usp_GGTools_GRINGLobal_WebUser_Insert";

            AddParameter("user_name", String.IsNullOrEmpty(entity.WebUserName) ? DBNull.Value : (object)entity.WebUserName, true);
            AddParameter("password", String.IsNullOrEmpty(entity.WebUserPassword) ? DBNull.Value : (object)entity.WebUserPassword, true);
            AddParameter("web_cooperator_id", entity.WebCooperatorID == 0 ? DBNull.Value : (object)entity.WebCooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_web_user_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_web_user_id", -1);
            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;

        }
        public int Update(WebUser entity)
        {
            
            return RowsAffected;
        }
        public int UpdatePassword(WebUser entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GGTools_GRINGlobal_WebUserPassword_Update";

            AddParameter("sys_user_id", (object)entity.WebUserID, false);
            AddParameter("password", (object)entity.WebUserPassword, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("DB Error");
            }

            return RowsAffected;
        }
    }
}
