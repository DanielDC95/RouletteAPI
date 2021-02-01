using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static RouletteAPI.DataBase.DataBase;

namespace RouletteAPI.Models
{
    public class Bets
    {
        public struct Bet
        {
            public int roullete_id;
            public int user_id;
            public string betType;
            public string betValue;
            public int amountToBet;
        }
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

        public static bool toBet(Bet bet)
        {
            bool betCorrectly = false;
            switch (bet.betType)
            {
                case "color":
                    betCorrectly = addBetByColor(bet);
                    break;
                case "number":
                    betCorrectly = addBetByNumber(bet);
                    break;
            }

            return betCorrectly;
        }

        public static bool rouletteHasBetsOpen(int roullete_id)
        {
            bool hasBetsOpen = false;
            if(getRouletteBetID(roullete_id) > 0)
            {
                hasBetsOpen = true;
            }
            else
            {
                hasBetsOpen = false;
            }

            return hasBetsOpen;
        }

        public static int getRouletteBetID(int roullete_id)
        {
            int id = 0;
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
                id = Int32.Parse(findAFieldValueInTable(table: table, fieldName: "id", lineNum: 0));
            }
            catch
            {
                id = 0;
            }

            return id;
        }

        public static bool addBetByColor(Bet bet)
        {
            DataToQueryInsert queryData = new DataToQueryInsert();
            int roulette_bets_id = getRouletteBetID(bet.roullete_id);
            bool created = false;
            try
            {
                queryData.tableName = "bet_by_color";
                FieldAndValue field = new FieldAndValue();
                List<FieldAndValue> fieldsToInsert = new List<FieldAndValue>();
                field.filedName = "roulette_bets_id";
                field.value = roulette_bets_id.ToString();
                fieldsToInsert.Add(field);
                field.filedName = "user_id";
                field.value = bet.user_id.ToString();
                fieldsToInsert.Add(field);
                field.filedName = "color";
                field.value = bet.betValue.ToString();
                fieldsToInsert.Add(field);
                field.filedName = "amount";
                field.value = bet.amountToBet.ToString();
                fieldsToInsert.Add(field);
                queryData.fields = fieldsToInsert;
                dataBaseConnect();
                created = dataBaseInsert(queryData);
                dataBaseDesconnect();
            }
            catch
            {
                created = false;
            }

            return created;
        }

        public static bool addBetByNumber(Bet bet)
        {
            DataToQueryInsert queryData = new DataToQueryInsert();
            int roulette_bets_id = getRouletteBetID(bet.roullete_id);
            bool created = false;
            try
            {
                queryData.tableName = "bet_by_number";
                FieldAndValue field = new FieldAndValue();
                List<FieldAndValue> fieldsToInsert = new List<FieldAndValue>();
                field.filedName = "roulette_bets_id";
                field.value = roulette_bets_id.ToString();
                fieldsToInsert.Add(field);
                field.filedName = "user_id";
                field.value = bet.user_id.ToString();
                fieldsToInsert.Add(field);
                field.filedName = "number";
                field.value = bet.betValue.ToString();
                fieldsToInsert.Add(field);
                field.filedName = "amount";
                field.value = bet.amountToBet.ToString();
                fieldsToInsert.Add(field);
                queryData.fields = fieldsToInsert;
                dataBaseConnect();
                created = dataBaseInsert(queryData);
                dataBaseDesconnect();
            }
            catch
            {
                created = false;
            }

            return created;
        }
    }
}
