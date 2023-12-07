﻿using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebInventoryManagement.Models.Data;

namespace WebInventoryManagement.Repository
{
    public class StoreRepository
    {
        private readonly string _connectionString;

        public StoreRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public IEnumerable<Store> GetAll() {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getAllStoresCommand = new OracleCommand();
            getAllStoresCommand.Connection = con;
            getAllStoresCommand.CommandText = "SELECT * FROM stores";
            OracleDataReader reader = getAllStoresCommand.ExecuteReader();

            List<Store> tmp = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Store store = new Store { Id = Convert.ToInt32(reader["store_id"]), StoreName = reader["store_name"].ToString(), ShelfCount = Convert.ToInt32(reader["shelf_count"]) };

                    tmp.Add(store);
                }
            }

            con.Close();

            return tmp;
        }

        public Store GetById(int id)
        {
            Store store = null;
            // get store by id from database logic

            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();
            OracleCommand getByIdCommand = new OracleCommand();
            getByIdCommand.Connection = con;
            getByIdCommand.CommandText = "SELECT * FROM STORES WHERE STORES.STORE_ID = :storeId";
            getByIdCommand.Parameters.Add(new OracleParameter("storeId", id));
            OracleDataReader reader = getByIdCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    store = new Store { Id = Convert.ToInt32(reader["store_id"]), StoreName = reader["store_name"].ToString(), ShelfCount = Convert.ToInt32(reader["shelf_count"]) };

                }
            }
            if (store == null) throw new NullReferenceException();
            return store;
        }

        public void Save(Store store)
        {

            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand insertStoreCommand = new("INSERT_STORE", con);
            insertStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            insertStoreCommand.Parameters.Add("STORE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = store.StoreName;

            insertStoreCommand.ExecuteNonQuery();

            con.Close();

        }

        public void Update(Store store)
        {
            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand updateStoreCommand = new("UPDATE_STORE", con);
            updateStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            updateStoreCommand.Parameters.Add("OLD_STORE_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = store.Id;
            updateStoreCommand.Parameters.Add("NEW_STORE_NAME_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = store.StoreName;


            updateStoreCommand.ExecuteNonQuery();

            con.Close();
        }

        public void Delete(Store store)
        {
            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand deleteStoreCommand = new("DELETE_STORE_BY_ID", con);
            deleteStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            deleteStoreCommand.Parameters.Add("STORE_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = store.Id;

            deleteStoreCommand.ExecuteNonQuery();

            con.Close();
        }
    }
}
