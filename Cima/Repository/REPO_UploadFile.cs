using Cima.Models;
using Cima.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.ObjectModel;

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

        public ObservableCollection<UploadingFile> GetTmpFileNameByUserId(string UserId)
        {
            SqlConnection con = this.connect(CONNECTION_STRING_SYSMAN);
            ObservableCollection<UploadingFile> items = new ObservableCollection<UploadingFile>();
            using (var sqlQuery = new SqlCommand(@"SELECT FileName FROM sysman.tmpUploadingFiles WHERE USERID = @UserId", con))
            {
                sqlQuery.Parameters.AddWithValue("@UserID", UserId);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                       
                        while (sqlQueryResult.Read())
                        {
                            try
                            {
                                items.Add(this.MapItem(sqlQueryResult));
                            }
                            catch
                            {
                                sqlQueryResult.Close();
                                con.Close();
                                throw new System.ArgumentException("Echec de chargement des données !  Consulter l'administrateur");
                            }
                           
                        }

                        sqlQueryResult.Close();
                        con.Close();

                    }
            }

            return items;
        } 

        private UploadingFile MapItem(SqlDataReader sqlDataReader)
        {
            return new UploadingFile()
            {
                FileName = sqlDataReader.GetString(0)
            };
        }

        /**
        * 
        */
        public ObservableCollection<UploadingFile> GetTmpFileByUserId(string UserId)
        {
            ObservableCollection<UploadingFile> Files = new ObservableCollection<UploadingFile>();
            using (var sqlConnection = this.connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT FileName, FileSize,Contents FROM sysman.tmpUploadingFiles WHERE USERID = @UserId", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@UserID", UserId);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            var blob = new Byte[(sqlQueryResult.GetBytes(2, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(2, 0, blob, 0, blob.Length);

                            UploadingFile uploadingFile = new UploadingFile()
                            {
                                FileName = sqlQueryResult.GetString(0),
                                FileSize = sqlQueryResult.GetInt32(1),
                                File = blob
                                
                            };

                            Files.Add(uploadingFile);
                        }
                       
                    }
            }

            return Files;

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

        public int DeleteTmpFileByUserId(string UserId)
        {
            SqlConnection con = this.connect(CONNECTION_STRING_SYSMAN);

            //Replaced Parameters with Value
            string query = @"DELETE FROM sysman.tmpUploadingFiles WHERE USERID = @UserID";

            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
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
    }
}