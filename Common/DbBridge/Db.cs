using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DbBridge
{
	public class Db : IDisposable
	{
		private MySqlConnection _connection;
		private MySqlTransaction _transaction;

		public MySqlConnection Connection
		{
			get { return _connection; }
		}

		public MySqlTransaction Transaction
		{
			get { return _transaction; }
		}

		public bool InTransaction
		{
			get { return _transaction != null; }
		}

		public Db(string connectionString)
		{
			CreateNewConnection(connectionString);
			OpenConnection();
		}

		public MySqlConnection CreateNewConnection(string connectionString)
		{
			_connection = new MySqlConnection(connectionString);
			return _connection;
		}

		private void OpenConnection()
		{
			if (_connection == null)
				throw new ApplicationException("Call the method CreateNewConnection.");

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();
		}

		private void CloseConnection()
		{
			if (_connection == null)
				throw new ApplicationException("Call the method CreateNewConnection.");

			if (_connection.State == ConnectionState.Open)
			{
				try
				{
					_connection.Close();
					_connection.Dispose();
					_connection = null;
				}
				finally { }
			}
		}

		public MySqlTransaction BeginTransaction()
		{
			_transaction = _connection.BeginTransaction();
			return _transaction;
		}

		public void Rollback()
		{
			if (_transaction == null)
				return;

			try
			{
				_transaction.Rollback();
				_transaction.Dispose();
				_transaction = null;
			}
			finally { }
		}

		public void Commit()
		{
			if (_transaction == null)
				throw new ApplicationException("You can not rollback empty transaction.");

			try
			{
				_transaction.Commit();
				_transaction.Dispose();
				_transaction = null;
			}
			finally { }
		}

		public void Dispose()
		{
			try
			{
				Rollback();
				CloseConnection();
			}
			finally { }
		}
	}
}
