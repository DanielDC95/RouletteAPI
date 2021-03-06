﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
//using RouletteAPI.DataBase;
using static RouletteAPI.DataBase.DataBase;

namespace RouletteAPI.Models
{
    public class Roulette
    {
        private int id;
        private int last_number;
        private string state;
        private string temporalState;
        private static bool errorInConsult = false;

        public Roulette(int _id = 0, int _last_number = 36)
        {
            id = _id;
            last_number = _last_number;
            state = State.getState("2");
        }

        public int getID()
        {
            return id;
        }

        public string getState()
        {
            return state;
        }

        public int getLastNumber()
        {
            return last_number;
        }

        public bool create()
        {
            DataToQueryInsert queryData = new DataToQueryInsert();
            var exists = rouletteExists();
            bool created = false;
            try
            {
                if (exists == false && errorInConsult == false)
                {
                    queryData.tableName = "roulettes";
                    FieldAndValue field = new FieldAndValue();
                    List<FieldAndValue> fieldsToInsert = new List<FieldAndValue>();
                    field.filedName = "last_number";
                    field.value = last_number.ToString();
                    fieldsToInsert.Add(field);
                    field.filedName = "state_id";
                    field.value = "2";
                    fieldsToInsert.Add(field);
                    queryData.fields = fieldsToInsert;
                    dataBaseConnect();
                    created = dataBaseInsert(queryData);
                    dataBaseDesconnect();
                    if (created)
                    {
                        id = getCreatedId();
                    }
                }
                
            }
            catch
            {
                errorInConsult = true;
            }

            return created;
        }

        public bool Open()
        {
            DataToQueryUpdate queryData = new DataToQueryUpdate();
            bool updated = false;
            try
            {
                queryData.tableName = "roulettes";
                FieldAndValue field = new FieldAndValue();
                List<FieldAndValue> fieldsToInsert = new List<FieldAndValue>();
                field.filedName = "state_id";
                field.value = State.getID("open");
                fieldsToInsert.Add(field);
                field.filedName = "last_number";
                field.value = last_number.ToString();
                fieldsToInsert.Add(field);
                queryData.fields = fieldsToInsert;
                queryData.id = id;
                dataBaseConnect();
                updated = dataBaseUpdate(queryData);
                dataBaseDesconnect();
                if (updated)
                {
                    state = "open";
                }
            }
            catch
            {
                updated = false;
            }

            return updated;
        }

        public bool Close()
        {
            DataToQueryUpdate queryData = new DataToQueryUpdate();
            bool updated = false;
            try
            {
                queryData.tableName = "roulettes";
                FieldAndValue field = new FieldAndValue();
                List<FieldAndValue> fieldsToInsert = new List<FieldAndValue>();
                field.filedName = "state_id";
                field.value = State.getID("close");
                fieldsToInsert.Add(field);
                field.filedName = "last_number";
                field.value = last_number.ToString();
                fieldsToInsert.Add(field);
                queryData.fields = fieldsToInsert;
                queryData.id = id;
                dataBaseConnect();
                updated = dataBaseUpdate(queryData);
                dataBaseDesconnect();
                if (updated)
                {
                    state = "close";
                }
            }
            catch
            {
                updated = false;
            }

            return updated;
        }

        public bool rouletteExists()
        {
            bool exists;
            DataToQuerySelect queryData = new DataToQuerySelect();
            try
            {
                queryData.tableName = "roulettes";
                List<string> fields = new List<string>();
                fields.Add("*");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "id";
                condition.condition = "=";
                condition.value = id.ToString();
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                DataTable table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                if (table != null && table.Rows.Count > 0)
                {
                    exists = true;
                }
                else
                {
                    exists = false;
                }
            }
            catch
            {
                errorInConsult = true;
                exists = false;
            }

            return exists;
        }
        
        private int getCreatedId()
        {
            int createdID = 0;
            DataToQuerySelect queryData = new DataToQuerySelect();
            try
            {
                queryData.tableName = "roulettes";
                queryData.limit = "1";
                List<string> fields = new List<string>();
                fields.Add("id");
                queryData.fields = fields;
                FieldsToOrder field = new FieldsToOrder();
                List<FieldsToOrder> orderBy = new List<FieldsToOrder>();
                field.filedName = "id";
                field.orderType = "desc";
                orderBy.Add(field);
                queryData.orderBy = orderBy;
                dataBaseConnect();
                DataTable table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                if(table != null)
                {
                    createdID = Int32.Parse(findAFieldValueInTable(table, "id", 0));
                }
            }
            catch
            {
                errorInConsult = true;
            }

            return createdID;
        }
        
        public bool loadRoulette(int _id)
        {
            DataToQuerySelect queryData = new DataToQuerySelect();
            id = _id;
            bool loaded = false;
            bool exists = rouletteExists();
            if (exists)
            {
                queryData.tableName = "roulettes";
                List<string> fields = new List<string>();
                fields.Add("state_id");
                fields.Add("last_number");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "id";
                condition.condition = "=";
                condition.value = id.ToString();
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                DataTable table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                last_number = Int32.Parse(findAFieldValueInTable(table: table, fieldName: "last_number", lineNum: 0));
                state = findAFieldValueInTable(table: table, fieldName: "state_id", lineNum: 0);
                state = State.getState(state);
                loaded = true;
            }
            else
            {
                id = 0;
            }

            return loaded;
        }
    }
}
