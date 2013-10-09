using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace iGoo.Classes
{
	/// <summary>
	/// Nguyen Thanh Binh - Thanhbinh101287@gmail.com
	/// Purpose: Data Access class for the table 'CMS_Polls'.
	/// </summary>
	public class clsCMS_Polls : clsDBInteractionBase
	{
		/// <summary>
		/// Purpose: Class constructor.
		/// </summary>
		public clsCMS_Polls()
		{
			// Nothing for now.
		}


		/// <summary>
		/// Purpose: Insert method. This method will insert one new row into the database.
		/// </summary>
		/// <returns>True if succeeded, otherwise an Exception is thrown. </returns>
		/// <remarks>
		/// </remarks>
		public override bool Insert()
		{
			SqlCommand	cmdToExecute = new SqlCommand();
			cmdToExecute.CommandText = "dbo.[sp_CMS_Polls_Insert]";
			cmdToExecute.CommandType = CommandType.StoredProcedure;

			// Use base class' connection object
			cmdToExecute.Connection = _mainConnection;

			try
			{
				cmdToExecute.Parameters.Add(new SqlParameter("@guidPollID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _pollID));
				cmdToExecute.Parameters.Add(new SqlParameter("@sTitle", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _title));
				cmdToExecute.Parameters.Add(new SqlParameter("@sCode", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _code));
				cmdToExecute.Parameters.Add(new SqlParameter("@iStatus", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _status));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse1", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response1));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote1", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote1));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse2", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response2));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote2", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote2));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse3", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response3));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote3", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote3));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse4", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response4));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote4", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote4));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse5", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response5));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote5", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote5));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse6", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response6));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote6", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote6));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse7", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response7));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote7", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote7));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse8", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response8));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote8", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote8));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse9", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response9));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote9", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote9));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse10", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response10));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote10", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote10));
				cmdToExecute.Parameters.Add(new SqlParameter("@iTotalVote", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _totalVote));
				cmdToExecute.Parameters.Add(new SqlParameter("@daCreated", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _created));

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
				cmdToExecute.ExecuteNonQuery();
				return true;
			}
			catch(Exception ex)
			{
				// some error occured. Bubble it to caller and encapsulate Exception object
				throw new Exception("clsCMS_Polls::Insert::Error occured.", ex);
			}
			finally
			{
				if(_mainConnectionIsCreatedLocal)
				{
					// Close connection.
					_mainConnection.Close();
				}
				cmdToExecute.Dispose();
			}
		}


		/// <summary>
		/// Purpose: Update method. This method will Update one existing row in the database.
		/// </summary>
		/// <returns>True if succeeded, otherwise an Exception is thrown. </returns>
		/// <remarks>
		/// </remarks>
		public override bool Update()
		{
			SqlCommand	cmdToExecute = new SqlCommand();
			cmdToExecute.CommandText = "dbo.[sp_CMS_Polls_Update]";
			cmdToExecute.CommandType = CommandType.StoredProcedure;

			// Use base class' connection object
			cmdToExecute.Connection = _mainConnection;

			try
			{
				cmdToExecute.Parameters.Add(new SqlParameter("@guidPollID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _pollID));
				cmdToExecute.Parameters.Add(new SqlParameter("@sTitle", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _title));
				cmdToExecute.Parameters.Add(new SqlParameter("@sCode", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _code));
				cmdToExecute.Parameters.Add(new SqlParameter("@iStatus", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _status));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse1", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response1));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote1", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote1));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse2", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response2));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote2", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote2));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse3", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response3));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote3", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote3));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse4", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response4));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote4", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote4));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse5", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response5));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote5", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote5));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse6", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response6));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote6", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote6));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse7", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response7));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote7", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote7));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse8", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response8));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote8", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote8));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse9", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response9));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote9", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote9));
				cmdToExecute.Parameters.Add(new SqlParameter("@sResponse10", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _response10));
				cmdToExecute.Parameters.Add(new SqlParameter("@iVote10", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _vote10));
				cmdToExecute.Parameters.Add(new SqlParameter("@iTotalVote", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _totalVote));
				cmdToExecute.Parameters.Add(new SqlParameter("@daCreated", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _created));

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
				cmdToExecute.ExecuteNonQuery();
				return true;
			}
			catch(Exception ex)
			{
				// some error occured. Bubble it to caller and encapsulate Exception object
				throw new Exception("clsCMS_Polls::Update::Error occured.", ex);
			}
			finally
			{
				if(_mainConnectionIsCreatedLocal)
				{
					// Close connection.
					_mainConnection.Close();
				}
				cmdToExecute.Dispose();
			}
		}


		/// <summary>
		/// Purpose: Delete method. This method will Delete one existing row in the database, based on the Primary Key.
		/// </summary>
		/// <returns>True if succeeded, otherwise an Exception is thrown. </returns>
		/// <remarks>
		/// </remarks>
		public override bool Delete()
		{
			SqlCommand	cmdToExecute = new SqlCommand();
			cmdToExecute.CommandText = "dbo.[sp_CMS_Polls_Delete]";
			cmdToExecute.CommandType = CommandType.StoredProcedure;

			// Use base class' connection object
			cmdToExecute.Connection = _mainConnection;

			try
			{
				cmdToExecute.Parameters.Add(new SqlParameter("@guidPollID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _pollID));

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
				cmdToExecute.ExecuteNonQuery();
				return true;
			}
			catch(Exception ex)
			{
				// some error occured. Bubble it to caller and encapsulate Exception object
				throw new Exception("clsCMS_Polls::Delete::Error occured.", ex);
			}
			finally
			{
				if(_mainConnectionIsCreatedLocal)
				{
					// Close connection.
					_mainConnection.Close();
				}
				cmdToExecute.Dispose();
			}
		}


		/// <summary>
		/// Purpose: Select method. This method will Select one existing row from the database, based on the Primary Key.
		/// </summary>
		/// <returns>DataTable object if succeeded, otherwise an Exception is thrown. </returns>
		/// <remarks>
		/// Will fill all properties corresponding with a field in the table with the value of the row selected.
		/// </remarks>
		public override DataTable SelectOne()
		{
			SqlCommand	cmdToExecute = new SqlCommand();
			cmdToExecute.CommandText = "dbo.[sp_CMS_Polls_SelectOne]";
			cmdToExecute.CommandType = CommandType.StoredProcedure;
			DataTable toReturn = new DataTable("CMS_Polls");
			SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

			// Use base class' connection object
			cmdToExecute.Connection = _mainConnection;

			try
			{
				cmdToExecute.Parameters.Add(new SqlParameter("@guidPollID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _pollID));

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
				throw new Exception("clsCMS_Polls::SelectOne::Error occured.", ex);
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


		/// <summary>
		/// Purpose: SelectAll method. This method will Select all rows from the table.
		/// </summary>
		/// <returns>DataTable object if succeeded, otherwise an Exception is thrown. </returns>
		/// <remarks>
		/// </remarks>
		public override DataTable SelectAll()
		{
			SqlCommand	cmdToExecute = new SqlCommand();
			cmdToExecute.CommandText = "dbo.[sp_CMS_Polls_SelectAll]";
			cmdToExecute.CommandType = CommandType.StoredProcedure;
			DataTable toReturn = new DataTable("CMS_Polls");
			SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

			// Use base class' connection object
			cmdToExecute.Connection = _mainConnection;

			try
			{

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
				throw new Exception("clsCMS_Polls::SelectAll::Error occured.", ex);
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


		#region Class Property Declarations
		private SqlInt32 _pageIndex = SqlInt32.Null;
		public SqlInt32 PageIndex
		{
			get
			{
				return _pageIndex;
			}
			set
			{
				_pageIndex = value;
			}
		}
		private SqlInt32 _pageSize = SqlInt32.Null;
		public SqlInt32 PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = value;
			}
		}
		private SqlInt32 _totalRows = SqlInt32.Null;
		public SqlInt32 TotalRows
		{
			get
			{
				return _totalRows;
			}
			set
			{
				_totalRows = value;
			}
		}
		private SqlString _orderBy = SqlString.Null;
		public SqlString OrderBy
		{
			get
			{
				return _orderBy;
			}
			set
			{
				_orderBy = value;
			}
		}


		private SqlGuid _pollID = SqlGuid.Null;
		public SqlGuid PollID
		{
			get
			{
				return _pollID;
			}
			set
			{
				_pollID = value;
			}
		}


		private SqlString _title = SqlString.Null;
		public SqlString Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
			}
		}


		private SqlString _code = SqlString.Null;
		public SqlString Code
		{
			get
			{
				return _code;
			}
			set
			{
				_code = value;
			}
		}


		private SqlInt32 _status = SqlInt32.Null;
		public SqlInt32 Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}


		private SqlString _response1 = SqlString.Null;
		public SqlString Response1
		{
			get
			{
				return _response1;
			}
			set
			{
				_response1 = value;
			}
		}


		private SqlInt32 _vote1 = SqlInt32.Null;
		public SqlInt32 Vote1
		{
			get
			{
				return _vote1;
			}
			set
			{
				_vote1 = value;
			}
		}


		private SqlString _response2 = SqlString.Null;
		public SqlString Response2
		{
			get
			{
				return _response2;
			}
			set
			{
				_response2 = value;
			}
		}


		private SqlInt32 _vote2 = SqlInt32.Null;
		public SqlInt32 Vote2
		{
			get
			{
				return _vote2;
			}
			set
			{
				_vote2 = value;
			}
		}


		private SqlString _response3 = SqlString.Null;
		public SqlString Response3
		{
			get
			{
				return _response3;
			}
			set
			{
				_response3 = value;
			}
		}


		private SqlInt32 _vote3 = SqlInt32.Null;
		public SqlInt32 Vote3
		{
			get
			{
				return _vote3;
			}
			set
			{
				_vote3 = value;
			}
		}


		private SqlString _response4 = SqlString.Null;
		public SqlString Response4
		{
			get
			{
				return _response4;
			}
			set
			{
				_response4 = value;
			}
		}


		private SqlInt32 _vote4 = SqlInt32.Null;
		public SqlInt32 Vote4
		{
			get
			{
				return _vote4;
			}
			set
			{
				_vote4 = value;
			}
		}


		private SqlString _response5 = SqlString.Null;
		public SqlString Response5
		{
			get
			{
				return _response5;
			}
			set
			{
				_response5 = value;
			}
		}


		private SqlInt32 _vote5 = SqlInt32.Null;
		public SqlInt32 Vote5
		{
			get
			{
				return _vote5;
			}
			set
			{
				_vote5 = value;
			}
		}


		private SqlString _response6 = SqlString.Null;
		public SqlString Response6
		{
			get
			{
				return _response6;
			}
			set
			{
				_response6 = value;
			}
		}


		private SqlInt32 _vote6 = SqlInt32.Null;
		public SqlInt32 Vote6
		{
			get
			{
				return _vote6;
			}
			set
			{
				_vote6 = value;
			}
		}


		private SqlString _response7 = SqlString.Null;
		public SqlString Response7
		{
			get
			{
				return _response7;
			}
			set
			{
				_response7 = value;
			}
		}


		private SqlInt32 _vote7 = SqlInt32.Null;
		public SqlInt32 Vote7
		{
			get
			{
				return _vote7;
			}
			set
			{
				_vote7 = value;
			}
		}


		private SqlString _response8 = SqlString.Null;
		public SqlString Response8
		{
			get
			{
				return _response8;
			}
			set
			{
				_response8 = value;
			}
		}


		private SqlInt32 _vote8 = SqlInt32.Null;
		public SqlInt32 Vote8
		{
			get
			{
				return _vote8;
			}
			set
			{
				_vote8 = value;
			}
		}


		private SqlString _response9 = SqlString.Null;
		public SqlString Response9
		{
			get
			{
				return _response9;
			}
			set
			{
				_response9 = value;
			}
		}


		private SqlInt32 _vote9 = SqlInt32.Null;
		public SqlInt32 Vote9
		{
			get
			{
				return _vote9;
			}
			set
			{
				_vote9 = value;
			}
		}


		private SqlString _response10 = SqlString.Null;
		public SqlString Response10
		{
			get
			{
				return _response10;
			}
			set
			{
				_response10 = value;
			}
		}


		private SqlInt32 _vote10 = SqlInt32.Null;
		public SqlInt32 Vote10
		{
			get
			{
				return _vote10;
			}
			set
			{
				_vote10 = value;
			}
		}


		private SqlInt32 _totalVote = SqlInt32.Null;
		public SqlInt32 TotalVote
		{
			get
			{
				return _totalVote;
			}
			set
			{
				_totalVote = value;
			}
		}


		private SqlDateTime _created = SqlDateTime.Null;
		public SqlDateTime Created
		{
			get
			{
				return _created;
			}
			set
			{
				_created = value;
			}
		}
		#endregion
	}
}
