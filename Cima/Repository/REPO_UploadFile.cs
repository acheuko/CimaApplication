using Cima.Models;
using Cima.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;

namespace Cima.Repository
{
    public class REPO_UploadFile : AbstractRepository, IREPO_UploadFile
    {
        public int DeleteTmpFileByFileNameAndUserId(string FileName, string UserId)
        {
            SqlConnection con = this.connect(CONNECTION_STRING_SYSMAN);

            //Replaced Parameters with Value
            string query = @"DELETE FROM sysman.tmpUploadingFiles WHERE FileName = @FileName AND USERID = @UserID";

            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@FileName", FileName);
            cmd.Parameters.AddWithValue("@UserID", UserId);

            int response = 0;

            try
            {
                response = cmd.ExecuteNonQuery();
                Console.WriteLine("Records deleted Successfully");
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return response;
        }

        /**
        * 
        */
        public void GetTmpFileByUserId(string UserId)
        {
            MemoryStream memoryStream = new MemoryStream();
            
            using (var sqlQuery = new SqlCommand(@"SELECT * FROM sysman.tmpUploadingFiles WHERE USERID = @UserId", this.connect(CONNECTION_STRING_SYSMAN)))
            {
                sqlQuery.Parameters.AddWithValue("@UserID", UserId);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        sqlQueryResult.Read();
                        var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                        sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                        //using (var fs = new MemoryStream(memoryStream, FileMode.Create, FileAccess.Write)) {
                        memoryStream.Write(blob, 0, blob.Length);
                        //}
                    }
            }

        }


        /*
         * 
         */
        public int SaveTmpFile(UploadingFile uploadingFile)
        {
            SqlConnection con = this.connect(CONNECTION_STRING_SYSMAN);
            
            //Replaced Parameters with Value
            string query = "INSERT INTO sysman.tmpUploadingFiles (FileName, FileSize, UploadDate, ID_Company, USERID, Contents) VALUES(@FileName,@FileSize, @UploadDate, @IDCompany,@UserID, @File)";
            
            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@FileName", uploadingFile.FileName);
            cmd.Parameters.AddWithValue("@FileSize", uploadingFile.FileSize);
            cmd.Parameters.AddWithValue("@UploadDate", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@IDCompany", uploadingFile.IdCompany);
            cmd.Parameters.AddWithValue("@UserID", uploadingFile.UserId);

            byte[] file = uploadingFile.File;
            cmd.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;

            int response = 0;

            try
            {
                response = cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return response;
        }

        protected override IDbCommand GetCommand(string query, IDbConnection sqlconnection)
        {
            SqlCommand sqlcommand = new SqlCommand(query)
            {
                Connection = (SqlConnection)sqlconnection
            };

            return sqlcommand;
        }
    }
}