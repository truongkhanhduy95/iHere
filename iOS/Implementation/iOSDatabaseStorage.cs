using System;
using Foundation;
using iHere.Shared;
using SQLite.Net.Interop;

namespace iHere.iOS
{
    public class iOSDatabaseStorage : IHereDatabaseStorage
    {
		public iOSDatabaseStorage(ISQLitePlatform platform) : base(platform, Environment.GetFolderPath(Environment.SpecialFolder.Personal))
		{
		}

		protected override bool IsDatabaseCreated(string dbPath)
		{
			return NSFileManager.DefaultManager.FileExists(dbPath);
		}
    }
}
