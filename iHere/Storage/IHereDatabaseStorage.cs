using System;
using System.Threading.Tasks;
using SQLite.Net.Interop;
using Xamarin.Core;

namespace iHere.Shared
{
    public abstract class IHereDatabaseStorage : BaseDatabaseStorage
    {
        protected IHereDatabaseStorage(ISQLitePlatform platform, string folderPath) : base(platform, folderPath, "iHere.db")
        {
        }

		protected override async Task CreateTables()
		{
		}
    }
}
