using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static RouletteAPI.DataBase.DataBase;

namespace RouletteAPI.Models
{
    public class State
    {
        public static string getState(string id)
        {
            string stateResult = "";
            DataToQuerySelect queryData = new DataToQuerySelect();
            try
            {
                queryData.tableName = "states";
                List<string> fields = new List<string>();
                fields.Add("state");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "id";
                condition.condition = "=";
                condition.value = id;
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                DataTable table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                stateResult = findAFieldValueInTable(table: table, fieldName: "state", lineNum: 0);
            }
            catch
            {
                stateResult = "NotFound";
            }
            return stateResult;
        }

        public static string getID(string state)
        {
            string stateResult = "";
            DataToQuerySelect queryData = new DataToQuerySelect();
            try
            {
                queryData.tableName = "states";
                List<string> fields = new List<string>();
                fields.Add("id");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "state";
                condition.condition = "=";
                condition.value = state;
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                DataTable table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                stateResult = findAFieldValueInTable(table: table, fieldName: "id", lineNum: 0);
            }
            catch
            {
                stateResult = "NotFound";
            }

            return stateResult;
        }

    }
}
