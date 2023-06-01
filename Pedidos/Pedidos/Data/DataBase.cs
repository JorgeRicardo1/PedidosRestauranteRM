
using PedidosRestaurante.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PedidosRestaurante.Data
{
    public class DataBase
    {

        readonly SQLiteAsyncConnection Connection;
        public DataBase(string dbPath, SQLiteAsyncConnection _)
        {

            _ = new SQLiteAsyncConnection(dbPath);
            _.QueryAsync<XxxxartiModel>("drop table if exists XxxxartiModel ");// ELIMINA EL CONTENIDO DE LA TABLA SE EXISTE          
            //_.QueryAsync<EmpresasModel>("drop table if exists EmpresasModel ");
            _.CreateTableAsync<XxxxartiModel>().Wait();
            _.CreateTableAsync<ImpresoraModel>().Wait();
            _.CreateTableAsync<EmpresasModel>().Wait();
            Connection = _;
        }
        public async Task Deletearti()
        {
            await Connection.QueryAsync<XxxxartiModel>("drop table if exists XxxxartiModel ");// ELIMINA EL CONTENIDO DE LA TABLA SE EXISTE
            Connection.CreateTableAsync<XxxxartiModel>().Wait();
        }

        public async Task<int> InsertxxxxartiAsync(List<XxxxartiModel> arti)
        {
            return await Connection.InsertAllAsync(arti);
        }

        public async Task<List<XxxxartiModel>> GetxxxxartiAsync()
        {

            return await Connection.Table<XxxxartiModel>().ToListAsync().ConfigureAwait(false);
        }
        public async Task<List<XxxxartiModel>> GetGrupxxxxartiAsync()
        {
            return await Connection.QueryAsync<XxxxartiModel>("SELECT * FROM XxxxartiModel group by Artigrupo ");
            //return await Connection.QueryAsync<XxxxartiModel>("SELECT  * FROM XxxxartiModel where Artigrupo='"+codigo+"'");
        }
        public async Task<List<XxxxartiModel>> GetCodexxxxartiAsync(string codigo)
        {
            return await Connection.QueryAsync<XxxxartiModel>("SELECT  * FROM XxxxartiModel where Artigrupo='" + codigo + "'");
        }

        //funciones para impresora
        public async Task<int> InsertprinterAsync(ImpresoraModel printer)
        {
            return await Connection.InsertAsync(printer);
        }

        public async Task<List<ImpresoraModel>> GetprinterAsync()
        {
            return await Connection.Table<ImpresoraModel>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateprinterAsync(ImpresoraModel printer)
        {
            return await Connection.UpdateAsync(printer);
        }

        public async Task<int> InsertEmpresaAsync(EmpresasModel empresa)
        {
            return await Connection.InsertAsync(empresa);
        }

        public async Task<List<EmpresasModel>> GetEmpresaAsync()
        {
            return await Connection.Table<EmpresasModel>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateEmpresaAsync(EmpresasModel empresa)
        {
            return await Connection.UpdateAsync(empresa);
        }
    }
}
