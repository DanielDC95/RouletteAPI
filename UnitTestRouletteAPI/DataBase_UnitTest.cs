using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouletteAPI.DataBase;

namespace UnitTestRouletteAPI
{
    [TestClass]
    public class DataBase_UnitTest
    {
        [TestMethod]
        public void TestConnect()
        {
            DataBase.connect();
            var connectResult = DataBase.isConnected();
            Assert.IsTrue(connectResult);
        }

        [TestMethod]
        public void TestDesonnect()
        {
            DataBase.desconnect();
            var connectResult = DataBase.isConnected();
            Assert.IsFalse(connectResult);
        }

        [TestMethod]
        public void TestSelect()
        {
            DataBase.connect();
            List<string> fields = new List<string>();
            
            DataBase.DataToQuerySelect data = new DataBase.DataToQuerySelect();
            fields.Add("id");
            fields.Add("state");
            data.tableName = "states";
            data.fields = fields;
            DataBase.FieldsToOrder field = new DataBase.FieldsToOrder();
            List<DataBase.FieldsToOrder> orderBy = new List<DataBase.FieldsToOrder>();
            field.filedName = "id";
            field.orderType = "desc";
            orderBy.Add(field);
            data.orderBy = orderBy;
            var tableResult = DataBase.select(data);
            DataBase.desconnect();
            Assert.IsNotNull(tableResult);
        }

        [TestMethod]
        public void TestInsert_Ok()
        {
            DataBase.connect();
            DataBase.FieldAndValue field = new DataBase.FieldAndValue();
            List <DataBase.FieldAndValue> fields = new List<DataBase.FieldAndValue>();
            DataBase.DataToQueryInsert data = new DataBase.DataToQueryInsert();
            field.filedName = "last_number";
            field.value = "35";
            fields.Add(field);
            field.filedName = "state_id";
            field.value = "2";
            fields.Add(field);
            data.tableName = "roulettes";
            data.fields = fields;
            var insertResult = DataBase.insert(data);
            DataBase.desconnect();
            Assert.IsTrue(insertResult);
        }

        [TestMethod]
        public void TestInsertWithoutFieldsOrValues()
        {
            DataBase.connect();
            DataBase.FieldAndValue field = new DataBase.FieldAndValue();
            List<DataBase.FieldAndValue> fields = new List<DataBase.FieldAndValue>();
            DataBase.DataToQueryInsert data = new DataBase.DataToQueryInsert();
            field.filedName = null;
            field.value = null;
            fields.Add(field);
            data.tableName = "roulettes";
            data.fields = fields;
            var insertResult = DataBase.insert(data);
            DataBase.desconnect();
            Assert.IsFalse(insertResult);
        }

        [TestMethod]
        public void TestUpdate_Ok()
        {
            DataBase.connect();
            DataBase.FieldAndValue field = new DataBase.FieldAndValue();
            List<DataBase.FieldAndValue> fields = new List<DataBase.FieldAndValue>();
            DataBase.DataToQueryUpdate data = new DataBase.DataToQueryUpdate();
            field.filedName = "last_number";
            field.value = "36";
            fields.Add(field);
            field.filedName = "state_id";
            field.value = "1";
            fields.Add(field);
            data.tableName = "roulettes";
            data.id = 1;
            data.fields = fields;
            var updateResult = DataBase.update(data);
            DataBase.desconnect();
            Assert.IsTrue(updateResult);
        }

        [TestMethod]
        public void TestUpdateWithoutTable()
        {
            DataBase.connect();
            DataBase.FieldAndValue field = new DataBase.FieldAndValue();
            List<DataBase.FieldAndValue> fields = new List<DataBase.FieldAndValue>();
            DataBase.DataToQueryUpdate data = new DataBase.DataToQueryUpdate();
            field.filedName = "last_number";
            field.value = "36";
            fields.Add(field);
            field.filedName = "state_id";
            field.value = "2";
            fields.Add(field);
            data.tableName = "";
            data.id = 1;
            data.fields = fields;
            var updateResult = DataBase.update(data);
            DataBase.desconnect();
            Assert.IsFalse(updateResult);
        }
    }
}
