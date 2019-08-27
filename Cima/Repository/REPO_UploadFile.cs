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
using System.Transactions;

namespace Cima.Repository
{
    public class REPO_UploadFile : SqlRepository<UploadingFile>, IREPO_UploadFile
    {
        /**
         * Supprimer les fichiers temporaires pour un nom et une company donnée
         **/
        public int DeleteTmpFileByFileNameAndIdCompany(string FileName, string IdCompany)
        {
            SqlConnection con = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN);

            //Replaced Parameters with Value
            string query = @"DELETE FROM sysman.tmpUploadingFiles WHERE FileName = @FileName AND ID_COMPANY = @IdCompany";

            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@FileName", FileName);
            cmd.Parameters.AddWithValue("@IdCompany", IdCompany);

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
         * récupérer le nom des fichiers temporaires pour une company donnée
         **/ 
        public ObservableCollection<UploadingFile> GetTmpFileNameByIdCompany(string IdCompany)
        {
            SqlConnection con = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN);
            ObservableCollection<UploadingFile> items = new ObservableCollection<UploadingFile>();
            using (var sqlQuery = new SqlCommand(@"SELECT FileName FROM sysman.tmpUploadingFiles WHERE ID_COMPANY = @UserId", con))
            {
                sqlQuery.Parameters.AddWithValue("@UserID", IdCompany);
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

        protected override UploadingFile MapItem(SqlDataReader sqlDataReader)
        {
            return new UploadingFile()
            {
                FileName = sqlDataReader.GetString(0)
            };
        }

        /**
        * Récupérer les fichiers temporaires pour une company donnée
        */
        public ObservableCollection<UploadingFile> GetTmpFileByIdCompany(string IdCompany)
        {
            ObservableCollection<UploadingFile> Files = new ObservableCollection<UploadingFile>();
            using (var sqlConnection = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
            using (var sqlQuery = new SqlCommand(@"SELECT FileName, FileSize,Contents FROM sysman.tmpUploadingFiles WHERE ID_COMPANY = @IdCompany", sqlConnection))
            {
                sqlQuery.Parameters.AddWithValue("@IdCompany", IdCompany);
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
         * Sauvegarder les fichiers temporaires
         */
        public int SaveTmpFile(UploadingFile uploadingFile)
        {
            SqlConnection con = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN);
            
            //Replaced Parameters with Value
            string query = "INSERT INTO sysman.tmpUploadingFiles (FileName, FileMask, FileSize, UploadDate, ID_Company, USERID, Contents) VALUES(@FileName,@FileMask,@FileSize, @UploadDate, @IDCompany,@UserID, @File)";
            
            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@FileName", uploadingFile.FileName);
            cmd.Parameters.AddWithValue("@FileMask", uploadingFile.FileMask);
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


        /**
         * Supprimer les fichiers temporaires pour une company donnée
         **/ 
        public int DeleteTmpFileByIdCompany(string IdCompany)
        {
            SqlConnection con = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN);

            //Replaced Parameters with Value
            string query = @"DELETE FROM sysman.tmpUploadingFiles WHERE ID_COMPANY = @IdCompany";

            SqlCommand cmd = (SqlCommand)this.GetCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@IdCompany", IdCompany);

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
         * Sauvegarder le Batch de chargement des fichiers 
         **/ 
        public void SaveBatchFiles(BatchModel batchModel)
        {
            using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    using (SqlConnection objConn = (SqlConnection)this.Connect(CONNECTION_STRING_SYSMAN))
                    {
                        // insertion du Batch
                        string sqlQuery1 = @"INSERT INTO sysman.tblBatch(BatchNumber,ID_Company,NbFiles,BatchClosed,DateBatch) VALUES(@BatchNumber, @IdCompany,@NbFiles,'N', GETDATE())";
                        SqlCommand objCmd1 = (SqlCommand)this.GetCommand(sqlQuery1, objConn);
                        //Pass values to Parameters
                        objCmd1.Parameters.AddWithValue("@BatchNumber", batchModel.BatchNumber);
                        objCmd1.Parameters.AddWithValue("@IdCompany", batchModel.IdCompany);
                        objCmd1.Parameters.AddWithValue("@NbFiles", batchModel.NbFiles);

                        // Insertion des fichiers du batch
                        string sqlQuery2 = @"INSERT INTO sysman.tblBatchFiles "+
                                           "SELECT @BatchNumber as [BatchNumber]"+
                                            ",@BatchNumber+'_'+[FileName] as [FileName]" +
                                            ",[FileMask]"+
                                            ",'N' as [FileIntegrated]"+
                                            ",GETDATE()"+
                                            ",ID_Company"+
                                            ",USERID "+
                                            "FROM sysman.tmpUploadingFiles "+
                                            "WHERE ID_Company = @IdCompany";

                        SqlCommand objCmd2 = (SqlCommand)this.GetCommand(sqlQuery2, objConn);
                        objCmd2.Parameters.AddWithValue("@BatchNumber", batchModel.BatchNumber);
                        objCmd2.Parameters.AddWithValue("@IdCompany", batchModel.IdCompany);

                        // Suppression des fichiers corespondant à une company dans la table temporaire
                        string sqlQuery3 = @"DELETE FROM sysman.tmpUploadingFiles WHERE ID_Company = @IdCompany";
                        SqlCommand objCmd3 = (SqlCommand)this.GetCommand(sqlQuery3, objConn);
                        //Pass values to Parameters
                        objCmd3.Parameters.AddWithValue("@IdCompany", batchModel.IdCompany);


                        try
                        {
                            objCmd1.ExecuteNonQuery();
                            objCmd2.ExecuteNonQuery();
                            objCmd3.ExecuteNonQuery();

                            //The Transaction will be completed    
                            txscope.Complete();
                        }
                        catch (SqlException e)
                        {
                           Console.WriteLine("Error SqlException Generated. Details: " + e.ToString());
                           txscope.Dispose();
                            throw e;
                        }
                        finally
                        {
                            objCmd1.Dispose();
                            objCmd2.Dispose();
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    // Log error 
                    txscope.Dispose();
                    throw ex;
                }
            }
        }

        
    }
}