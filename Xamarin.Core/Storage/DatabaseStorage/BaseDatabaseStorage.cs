using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Xamarin.Core
{
	public abstract class BaseDatabaseStorage : IDatabaseStorage
	{
		private readonly string _dbPath;

		protected SQLiteAsyncConnection DBConnection { get; set; }

		protected BaseDatabaseStorage(ISQLitePlatform platform, string folderPath, string dbName)
		{
			_dbPath = Path.Combine(folderPath, dbName);
			DBConnection = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(platform, new SQLiteConnectionString(_dbPath, false)));
		}

		public virtual async Task InitDatabase()
		{
			if (!IsDatabaseCreated(_dbPath))
			{
				await CreateTables();
			}
		}

		protected abstract Task CreateTables();

		protected abstract bool IsDatabaseCreated(string dbPath);

		public async Task<List<T>> GetAll<T>() where T : class, new()
		{
			try
			{
				var result = await DBConnection.Table<T>().ToListAsync();
				return result;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				return null;
			}
		}

		public async Task<T> GetFirst<T>() where T : class, new()
		{
			try
			{
				var result = await DBConnection.Table<T>().FirstOrDefaultAsync();
				return result;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				return null;
			}
		}

		public async Task<int> DeleteAll<T>() where T : new()
		{
			try
			{
				int count = await DBConnection.DeleteAllAsync<T>();
				return count;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				return 0;
			}
		}

		public async Task<int> Delete<T>(int id) where T : new()
		{
			try
			{
				int count = await DBConnection.DeleteAsync<T>(id);
				return count;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				return 0;
			}
		}

		public async Task<int> Insert<T>(T record) where T : new()
		{
			try
			{
				int count = await DBConnection.InsertAsync(record);
				return count;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				return 0;
			}
		}

		public async Task<int> InsertAll<T>(IEnumerable<T> record) where T : new()
		{
			try
			{
				int count = await DBConnection.InsertAllAsync(record);
				return count;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				return 0;
			}
		}
	}
}
