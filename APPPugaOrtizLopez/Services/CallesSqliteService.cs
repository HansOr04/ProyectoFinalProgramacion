using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using APPPugaOrtizLopez.Models;

namespace APPPugaOrtizLopez.Services
{
    public interface ICallesSqliteService
    {
        Task InitializeAsync();
        Task<List<CalleSqlite>> GetCallesAsync();
        Task<CalleSqlite> GetCalleByIdAsync(int id);
        Task<bool> AddCalleAsync(CalleSqlite calle);
        Task<bool> DeleteCalleAsync(int id);
        Task<bool> UpdateCalleAsync(CalleSqlite calle);
    }

      public class CallesSqliteService : ICallesSqliteService
    {
        private SQLiteAsyncConnection _database;
        private const string DbName = "calles.db3";

        public CallesSqliteService()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<CalleSqlite>().Wait(); 
        }

        public async Task InitializeAsync()
        {
            if (_database != null)
            {
                await _database.CreateTableAsync<CalleSqlite>();
            }
        }

        public async Task<List<CalleSqlite>> GetCallesAsync()
        {
            return await _database.Table<CalleSqlite>().ToListAsync();
        }

        public async Task<CalleSqlite> GetCalleByIdAsync(int id)
        {
            return await _database.Table<CalleSqlite>()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddCalleAsync(CalleSqlite calle)
        {
            if (calle == null) return false;

            calle.FechaCreacion = DateTime.Now;
            int result = await _database.InsertAsync(calle);
            return result > 0;
        }

        public async Task<bool> DeleteCalleAsync(int id)
        {
            int result = await _database.DeleteAsync<CalleSqlite>(id);
            return result > 0;
        }

        public async Task<bool> UpdateCalleAsync(CalleSqlite calle)
        {
            if (calle == null) return false;

            int result = await _database.UpdateAsync(calle);
            return result > 0;
        }
    }
}
