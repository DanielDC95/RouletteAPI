using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static RouletteAPI.DataBase.DataBase;

namespace RouletteAPI.Models
{
    public class Bets
    {
        public static bool openRouletteBet(int roullete_id)
        {
            DataToQueryInsert queryData = new DataToQueryInsert();
            bool hasBetsOpen = rouletteHasBetsOpen(roullete_id);
            bool created = false;
            try
            {
                if (hasBetsOpen == false)
                {
                    queryData.tableName = "roulette_bets";
                    FieldAndValue field = new FieldAndValue();
                    List<FieldAndValue> fieldsToInsert = new List<FieldAndValue>();
                    field.filedName = "roullete_id";
                    field.value = roullete_id.ToString();
                    fieldsToInsert.Add(field);
                    field.filedName = "state_id";
                    field.value = State.getID("open");
                    fieldsToInsert.Add(field);
                    queryData.fields = fieldsToInsert;
                    dataBaseConnect();
                    created = dataBaseInsert(queryData);
                    dataBaseDesconnect();
                }

            }
            catch
            {
                created = false;
            }

            return created;
        }

        public static bool rouletteHasBetsOpen(int roullete_id)
        {
            bool hasBetsOpen = false;
            DataToQuerySelect queryData = new DataToQuerySelect();
            try
            {
                queryData.tableName = "roulette_bets";
                List<string> fields = new List<string>();
                fields.Add("*");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "roullete_id";
                condition.condition = "=";
                condition.value = roullete_id.ToString();
                conditionsList.Add(condition);
                condition.filedName = "state_id";
                condition.condition = "=";
                condition.value = State.getID("open");
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                DataTable table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                if (table != null && table.Rows.Count > 0)
                {
                    hasBetsOpen = true;
                }
                else
                {
                    hasBetsOpen = false;
                }
            }
            catch
            {
                hasBetsOpen = false;
            }
            return hasBetsOpen;
        }
    }
}
