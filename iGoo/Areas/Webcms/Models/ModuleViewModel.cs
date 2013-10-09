using System;
using System.Data;
using System.Linq;
using iGoo.Classes;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using iGoo.Helpers;

namespace iGoo.Areas.Webcms.Models
{
    public class ModuleViewModel:clsADM_Modules
    {
        public DataTable MenuAll { get; set; }
        public List<ModuleViewModel> Menu { get; set; }

        public SqlGuid RollID  { get; set; }

        //Select ADM_Module
        public List<ModuleViewModel> SelectMenu(Guid ParentID)
        {
            List<ModuleViewModel> list = new List<ModuleViewModel>();
            foreach (DataRow row in MenuAll.Select("ParentID='" + ParentID.ToString() + "'").OrderBy(row =>row["Order"]).ToList())
            {
                if (!ParentID.Equals(row["ModuleID"]))
                {
                    ModuleViewModel m = new ModuleViewModel();
                    m.Name = row["Name"].ToString();
                    m.UrlLink = row["UrlLink"].ToString();
                    m.Menu = SelectMenu(new Guid(row["ModuleID"].ToString()));
                    String per = Libs.GetPermission(row["Code"].ToString());
                    if (per.IndexOf("S") >= 0 || row["Code"].ToString().Equals(String.Empty))
                        list.Add(m);
                }
            }
            return list;
        }

        //Select ADM_Module and permission of roll
        public DataTable SelectModule()
		{
			SqlCommand	cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_ADM_Modules_SelectModule]";
			cmdToExecute.CommandType = CommandType.StoredProcedure;
			DataTable toReturn = new DataTable("ADM_Modules");
			SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

			// Use base class' connection object
			cmdToExecute.Connection = _mainConnection;

			try
			{
                cmdToExecute.Parameters.Add(new SqlParameter("@guidRollID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, RollID));

				if(_mainConnectionIsCreatedLocal)
				{
					// Open connection.
					_mainConnection.Open();
				}
				else
				{
					if(_mainConnectionProvider.IsTransactionPending)
					{
						cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
					}
				}

				// Execute query.
				adapter.Fill(toReturn);
				return toReturn;
			}
			catch(Exception ex)
			{
				// some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("clsADM_Modules::SelectModule::Error occured.", ex);
			}
			finally
			{
				if(_mainConnectionIsCreatedLocal)
				{
					// Close connection.
					_mainConnection.Close();
				}
				cmdToExecute.Dispose();
				adapter.Dispose();
			}
		}
    }
}