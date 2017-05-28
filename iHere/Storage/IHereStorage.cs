using System;
using SQLite.Net.Interop;
using Xamarin.Core;

namespace iHere.Shared.Storage
{
    public abstract class IHereStorage : BaseDatabaseStorage
    {
        protected IHereStorage(ISQLitePlatform platform, string folderPath) : base(platform, folderPath, "iHere.db")
        {
        }

        //protected override async Task CreateTable()
        //{
        //}
    }
}
