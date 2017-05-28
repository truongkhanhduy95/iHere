using System;
using iHere.Shared;
using Java.IO;
using SQLite.Net.Interop;

namespace iHere.Droid
{
    public class AndroidDatabaseStorage : IHereDatabaseStorage
    {
		public AndroidDatabaseStorage(ISQLitePlatform platform) : base(platform, Environment.GetFolderPath(Environment.SpecialFolder.Personal))
		{
		}

		protected override bool IsDatabaseCreated(string dbPath)
		{
			var file = new File(dbPath);
			return file.Exists();
		}
    }
}
