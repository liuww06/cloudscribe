// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-01-15
// 
//
// You must not remove this notice, or any other, from this software.
// 

using cloudscribe.DbHelpers.MySql;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace cloudscribe.Core.Repositories.MySql
{
    internal static class DBSiteFolder
    {
       
        public static int Add(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteFolders (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FolderName )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?FolderName );");


            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[2].Value = folderName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SiteFolders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = ?SiteGuid, ");
            sqlCommand.Append("FolderName = ?FolderName ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ;");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[2].Value = folderName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > -1);

        }


        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetSiteGuid(string folderName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[0].Value = folderName;

            Guid siteGuid = Guid.Empty;

            sqlCommand.Append("SELECT SiteGuid ");
            sqlCommand.Append("FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = ?FolderName ;");

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    siteGuid = new Guid(reader["SiteGuid"].ToString());
                }
            }

            if (siteGuid == Guid.Empty)
            {

                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT SiteGuid ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append("LIMIT 1 ;");


                using (IDataReader reader = AdoHelper.ExecuteReader(
                    ConnectionString.GetReadConnectionString(),
                    sqlCommand.ToString(),
                    null))
                {

                    if (reader.Read())
                    {
                        siteGuid = new Guid(reader["SiteGuid"].ToString());
                    }
                }

            }

            return siteGuid;

        }

        public static bool Exists(string folderName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = ?FolderName ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[0].Value = folderName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }


        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("sf.Guid, ");
            sqlCommand.Append("sf.FolderName ");

            sqlCommand.Append("FROM	mp_SiteFolders sf ");

            sqlCommand.Append("JOIN	mp_Sites s ");

            sqlCommand.Append("ON sf.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("ORDER BY sf.FolderName ");

            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null));
        }

        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("sf.Guid, ");
            sqlCommand.Append("sf.FolderName ");

            sqlCommand.Append("FROM	mp_SiteFolders sf  ");
            sqlCommand.Append("JOIN	mp_Sites s ");

            sqlCommand.Append("ON sf.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("ORDER BY sf.FolderName ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[1].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

    }
}
