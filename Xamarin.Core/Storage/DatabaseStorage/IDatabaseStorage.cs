using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Core
{
    public interface IDatabaseStorage
    {
        Task<List<T>> GetAll<T>() where T : class, new();

        Task<T> GetFirst<T>() where T : class, new();

        Task<int> DeleteAll<T>() where T : new();

        Task<int> Delete<T>(int id) where T : new();

        Task<int> Insert<T>(T record) where T : new();

        Task<int> InsertAll<T>(IEnumerable<T> record) where T : new();
    }
}
