using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Data;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Repository;

namespace WebInventoryManagement.Services
{
    public class ItemService
    {
        private ItemRepository itemRepository;
        public ItemService(IConfiguration configuration)
        {
            itemRepository = new ItemRepository(configuration);
        }

        public IEnumerable<Item> GetAll() {

            //OracleConnection con = new OracleConnection(_connectionString);
            //con.Open();

            //OracleCommand getAllItemsCommand = new OracleCommand();
            //getAllItemsCommand.Connection = con;
            //getAllItemsCommand.CommandText = "SELECT * FROM ITEMS";
            //OracleDataReader reader = getAllItemsCommand.ExecuteReader();

            //List<Item> tmp = new();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        Item item = fillItemData(reader);
            //        tmp.Add(item);
            //    }
            //}

            //con.Close();

            //return tmp;

            return itemRepository.GetAll();
        }


        public void Save(Item item)
        {
            //OracleConnection con = new(_connectionString);

            //con.Open();
            //OracleCommand insertStoreCommand = new("INSERT_ITEM", con);
            //insertStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            //insertStoreCommand.Parameters.Add("ITEM_NAME_P", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = item.ItemName;
            //insertStoreCommand.Parameters.Add("PRICE_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.Price;
            //insertStoreCommand.Parameters.Add("QUANTITY_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.Quantity;
            //insertStoreCommand.Parameters.Add("DESCRIPTION_P", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = item.Description;
            //insertStoreCommand.Parameters.Add("CATEGORY_ID_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.CategoryId;
            //insertStoreCommand.Parameters.Add("SHELF_ID_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.ShelfId;

            //insertStoreCommand.ExecuteNonQuery();

            //con.Close();

            itemRepository.Save(item);
        }
        public void Delete(Item item)
        {
            //OracleConnection con = new(_connectionString);
            //con.Open();
            //OracleCommand deleteItemCommand = new("DELETE_ITEM_BY_ID", con);
            //deleteItemCommand.CommandType = System.Data.CommandType.StoredProcedure;

            //deleteItemCommand.Parameters.Add("ITEM_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = item.Id;

            //deleteItemCommand.ExecuteNonQuery();

            //con.Close();
        
            itemRepository.Delete(item);
        }
        public void UpdateItem(Item item)
        {

            //OracleConnection con = new(_connectionString);
            //con.Open();
            //OracleCommand updateStoreCommand = new("UPDATE_ITEM", con);
            //updateStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            //updateStoreCommand.Parameters.Add("OLD_ITEM_ID_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.Id;
            //updateStoreCommand.Parameters.Add("ITEM_NAME_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = item.ItemName;
            //updateStoreCommand.Parameters.Add("PRICE_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.Price;
            //updateStoreCommand.Parameters.Add("QUANTITY_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.Quantity;
            //updateStoreCommand.Parameters.Add("DESCRIPTION_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = item.Description;
            //updateStoreCommand.Parameters.Add("CATEGORY_ID_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.CategoryId;
            //updateStoreCommand.Parameters.Add("SHELF_ID_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.ShelfId;


            //updateStoreCommand.ExecuteNonQuery();

            //con.Close();

            itemRepository.Update(item);
        } 
        public Item GetById(int id)
        {
            //OracleConnection con = new OracleConnection(_connectionString);
            //con.Open();
            //OracleCommand getByIdCommand = new OracleCommand();
            //getByIdCommand.Connection = con;
            //getByIdCommand.CommandText = $"SELECT * FROM ITEMS WHERE ITEMS.ITEM_ID = {id}";
            //getByIdCommand.Parameters.Add(new OracleParameter("itemId", id));
            //OracleDataReader reader = getByIdCommand.ExecuteReader();

            //Item Item = new Item();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        Item.Id = id;
            //        Item.ItemName = reader["ITEM_NAME"].ToString();
            //        Item.Price = Convert.ToInt32(reader["PRICE"]);
            //        Item.Quantity = Convert.ToInt32(reader["QUANTITY"]);
            //        Item.Description = reader["DESCRIPTION"].ToString();

            //        Item.Category = GetCategoryById(Convert.ToInt32(reader["category_id_fk"]));
            //        Item.Shelf = getShelfById(Convert.ToInt32(reader["shelf_id_fk"]));
            //    }
            //}
            //if (Item == null) throw new NullReferenceException();
            //return Item;

            return itemRepository.GetById(id);
        }
       

        
    }
}
