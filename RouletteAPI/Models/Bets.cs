using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static RouletteAPI.DataBase.DataBase;

namespace RouletteAPI.Models
{
    public class Bets
    {
        public static string error;
        public struct Bet
        {
            public int roullete_id;
            public int user_id;
            public string betType;
            public string betValue;
            public int amountToBet;
        }

        public struct winners
        {
            public int user_id;
            public string bet;
            public int ammount;
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

        public static int closeRouletteBet(int roullete_id)
        {
            DataToQueryUpdate queryData = new DataToQueryUpdate();
            bool hasBetsOpen = rouletteHasBetsOpen(roullete_id);
            bool created = false;
            int openRouletteID = 0;
            try
            {
                if (hasBetsOpen)
                {
                    queryData.tableName = "roulette_bets";
                    FieldAndValue field = new FieldAndValue();
                    List<FieldAndValue> fieldsToInsert = new List<FieldAndValue>();
                    field.filedName = "roullete_id";
                    field.value = roullete_id.ToString();
                    fieldsToInsert.Add(field);
                    field.filedName = "state_id";
                    field.value = State.getID("close");
                    fieldsToInsert.Add(field);
                    field.filedName = "winner_number";
                    field.value = generateWinninNumber(roullete_id).ToString();
                    fieldsToInsert.Add(field);
                    queryData.fields = fieldsToInsert;
                    openRouletteID = getRouletteBetID(roullete_id);
                    queryData.id = getRouletteBetID(roullete_id); ;
                    dataBaseConnect();
                    created = dataBaseUpdate(queryData);
                    dataBaseDesconnect();
                }
            }
            catch
            {
                created = false;
            }

            return openRouletteID;
        }

        public static bool toBet(Bet bet)
        {
            bool betCorrectly = false;
            bool validBet = validateBet(bet);
            if (!validBet)
            {
                return betCorrectly;
            }
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

        public static List<Dictionary<string, string>> getWinnersList(int roulette_bet_id)
        {
            int winning_number = getWinnigNumber(roulette_bet_id);
            string winning_color = getWinningColor(winning_number);
            DataTable winners = getWinnersByNumber(roulette_bet_id: roulette_bet_id, number: winning_number);
            List<Dictionary<string, string>> winnersList = new List<Dictionary<string, string>>();
            winnersList = listWinners(table: winners, winnersList: winnersList) ;
            winners = getWinnersByColor(roulette_bet_id: roulette_bet_id, color: winning_color);
            winnersList = listWinners(table: winners, winnersList: winnersList);

            return winnersList;
        }

        public static int generateWinninNumber(int roullete_id)
        {
            Roulette roulette = new Roulette();
            roulette.loadRoulette(roullete_id);
            Random number = new Random();
            return number.Next(0, roulette.getLastNumber() + 1);
        }

        public static DataTable getWinnersByColor(int roulette_bet_id, string color)
        {
            DataTable table = new DataTable();
            DataToQuerySelect queryData = new DataToQuerySelect();
            try
            {
                queryData.tableName = "bet_by_color";
                List<string> fields = new List<string>();
                fields.Add("user_id");
                fields.Add("amount");
                fields.Add("'color' \"betType\"");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "roulette_bets_id";
                condition.condition = "=";
                condition.value = roulette_bet_id.ToString();
                conditionsList.Add(condition);
                condition.filedName = "color";
                condition.condition = "=";
                condition.value = color;
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                table = dataBaseSelect(queryData);
                dataBaseDesconnect();
            }
            catch
            {
                table = null;
            }

            return table;
        }

        public static DataTable getWinnersByNumber(int roulette_bet_id, int number)
        {
            DataTable table = new DataTable();
            DataToQuerySelect queryData = new DataToQuerySelect();
            try
            {
                queryData.tableName = "bet_by_number";
                List<string> fields = new List<string>();
                fields.Add("user_id");
                fields.Add("amount");
                fields.Add("'number' \"betType\"");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "roulette_bets_id";
                condition.condition = "=";
                condition.value = roulette_bet_id.ToString();
                conditionsList.Add(condition);
                condition.filedName = "number";
                condition.condition = "=";
                condition.value = number.ToString();
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                table = dataBaseSelect(queryData);
                dataBaseDesconnect();
            }
            catch
            {
                table = null;
            }

            return table;
        }

        public static int getWinnigNumber(int roulette_bet_id)
        {
            DataTable table = new DataTable();
            DataToQuerySelect queryData = new DataToQuerySelect();
            int winningNumber = 0;
            try
            {
                queryData.tableName = "roulette_bets";
                List<string> fields = new List<string>();
                fields.Add("winner_number");
                queryData.fields = fields;
                Conditions condition = new Conditions();
                List<Conditions> conditionsList = new List<Conditions>();
                condition.filedName = "id";
                condition.condition = "=";
                condition.value = roulette_bet_id.ToString();
                conditionsList.Add(condition);
                queryData.conditions = conditionsList;
                dataBaseConnect();
                table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                winningNumber = Int32.Parse(findAFieldValueInTable(table: table,fieldName: "winner_number", lineNum: 0));
            }
            catch
            {
                winningNumber = 0;
            }

            return winningNumber;
        }

        public static string getWinningColor(int number)
        {
            string color;
            if((number%2) == 0)
            {
                color = "Red";
            }
            else
            {
                color = "Black";
            }

            return color;
        }

        public static List<Dictionary<string, string>> listWinners(DataTable table, List<Dictionary<string, string>> winnersList)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string user_id = findAFieldValueInTable(table: table, fieldName: "user_id", i);
                string ammount = findAFieldValueInTable(table: table, fieldName: "amount", i);
                string betType = findAFieldValueInTable(table: table, fieldName: "betType", i);
                double reward = calculateReward(betType, Int32.Parse(ammount));
                Dictionary<string, string> winner = new Dictionary<string, string>();
                winner.Add("user_id", user_id);
                winner.Add("reward", reward.ToString());
                winnersList.Add(winner);
            }

            return winnersList;
        }

        public static double calculateReward(string betType, int ammount)
        {
            double reward = ammount;
            switch (betType)
            {
                case "color":
                    reward = ammount * 1.8;
                    break;
                case "number":
                    reward = ammount * 5;
                    break;
                default:
                    reward = 0;
                    break;
            }

            return reward;
        }

        private static bool validateBet(Bet bet)
        {
            error = "";
            try
            {
                if (!(bet.betType.Equals("number") || bet.betType.Equals("color")))
                {
                    error = "The valid bet type are \"number\" and \"color\".";
                    return false;
                }
                if (bet.betType.Equals("number") && (Int32.Parse(bet.betValue) < 0 || Int32.Parse(bet.betValue) > 36))
                {
                    error = "The number must be between 0 and 36.";
                    return false;
                }
                if (bet.betType.Equals("color") && !(bet.betValue.Equals("Red") || bet.betValue.Equals("Black")))
                {
                    error = "The number must be \"Red\" or \"Black\".";
                    return false;
                }
                if (bet.amountToBet > 10000 || bet.amountToBet <= 0)
                {
                    error = "The amount must be between 1 and 10000.";
                    return false;
                }
            }
            catch
            {
                error = "An error has occurred.";
                return false;
            }

            return true;
        }
    }
}
