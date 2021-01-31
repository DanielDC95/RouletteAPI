using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace RouletteAPI.DataBase
{
    public class DataBase
    {
        private static NpgsqlConnection dataBaseConnection = new NpgsqlConnection("Server = localhost; User Id = postgres; Password = 1234; DataBase = roulette_db");
        private static bool connected = false;

        #region Structurs
        public struct DataToQuerySelect
        {
            public string tableName;
            public string limit;
            public List<string> fields;
            public List<Conditions> conditions;
            public List<FieldsToOrder> orderBy;
        }

        public struct DataToQueryInsert
        {
            public string tableName;
            public List<FieldAndValue> fields;
        }

        public struct DataToQueryUpdate
        {
            public string tableName;
            public int id;
            public List<FieldAndValue> fields;
        }

        public struct FieldAndValue
        {
            public string filedName;
            public string value;
        }

        public struct Conditions
        {
            public string filedName;
            public string condition;
            public string value;
        }

        public struct FieldsToOrder
        {
            public string filedName;
            public string orderType;
        }
        #endregion

        public static void dataBaseConnect()
        {
            try
            {
                if(connected == false)
                {
                    dataBaseConnection.Open();
                    connected = true;
                }
            }
            catch
            {
                connected = false;
            }
        }

        public static void dataBaseDesconnect()
        {
            try
            {
                if(connected == true)
                {
                    dataBaseConnection.Close();
                    connected = false;
                }
            }
            catch
            {
                connected = true;
            }
        }

        public static bool isConnected()
        {
            return connected;
        }

        public static DataTable dataBaseSelect(DataToQuerySelect queryData)
        {
            DataTable table = new DataTable();
            string fields = getFieldsForSelect(queryData);
            string limit = getLimit(queryData);
            string orderBy = getOrderBy(queryData);
            string conditions = getConditionsForSelect(queryData);
            string query = "Select " + fields + " From " + queryData.tableName + conditions + orderBy + limit + ";";
            try
            {
                if (connected == true)
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, dataBaseConnection);
                    NpgsqlDataAdapter data = new NpgsqlDataAdapter(command);
                    data.Fill(table);
                }
                else
                {
                    table = null;
                }
            }
            catch (Exception)
            {
                table = null;
            }
            
            return table;
        }

        public static bool dataBaseInsert(DataToQueryInsert queryData)
        {
            bool insertResult;
            int rowsAffected = 0;
            DataTable table = new DataTable();
            string fields = getFieldsForInsert(queryData);
            string values = getValuesForInsert(queryData);
            string query = "Insert Into " + queryData.tableName +  "(" + fields + ") values (" + values + ");";
            try
            {
                if (connected == true)
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, dataBaseConnection);
                    rowsAffected = command.ExecuteNonQuery();
                    if(rowsAffected > 0)
                    {
                        insertResult = true;
                    }
                    else
                    {
                        insertResult = false;
                    }
                }
                else
                {
                    insertResult = false;
                }

            }
            catch (Exception)
            {
                insertResult = false;
            }

            return insertResult;
        }

        public static bool dataBaseUpdate(DataToQueryUpdate queryData)
        {
            bool updateResult;
            int rowsAffected = 0;
            DataTable table = new DataTable();
            string fields = getFieldsForUpdate(queryData);
            string query = "Update " + queryData.tableName + " set " + fields + " Where id = " + queryData.id + " ;";
            try
            {
                if (connected == true)
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, dataBaseConnection);
                    rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        updateResult = true;
                    }
                    else
                    {
                        updateResult = false;
                    }
                }
                else
                {
                    updateResult = false;
                }

            }
            catch (Exception)
            {
                updateResult = false;
            }

            return updateResult;
        }

        private static string getLimit(DataToQuerySelect queryData)
        {
            string top = "";
            if (!String.IsNullOrEmpty(queryData.limit))
            {
                top = " limit " + queryData.limit + " ";
            }
            return top;
        }

        private static string getFieldsForSelect(DataToQuerySelect queryData)
        {
            string fields = "";
            foreach(string field in queryData.fields)
            {
                if (String.IsNullOrEmpty(fields))
                {
                    fields = field;
                }
                else
                {
                    fields = fields + ", " + field;
                }
            }

            return fields;
        }

        private static string getConditionsForSelect(DataToQuerySelect queryData)
        {
            string condition = "";
            string resultConditions = "";
            try
            {
                foreach(Conditions conditions in queryData.conditions)
                {
                    switch (conditions.condition)
                    {
                        case "=":
                            condition = conditions.filedName + " " + conditions.condition + " '" + conditions.value + "'";
                            break;
                        default:
                            condition = conditions.filedName + " " + conditions.condition + " " + conditions.value;
                            break;
                    }
                    if (String.IsNullOrEmpty(resultConditions))
                    {
                        resultConditions = condition;
                    }
                    else
                    {
                        resultConditions = resultConditions + " and " + condition;
                    }
                    condition = "";
                }
                if (!String.IsNullOrEmpty(resultConditions))
                {
                    resultConditions = " Where " + resultConditions;
                }
            }
            catch
            {
                resultConditions = "";
            }

            return resultConditions;
        }

        private static string getOrderBy(DataToQuerySelect queryData)
        {
            string orderBy = "";
            try
            {
                foreach (FieldsToOrder field in queryData.orderBy)
                {
                    if (String.IsNullOrEmpty(orderBy))
                    {
                        orderBy = field.filedName + " " + field.orderType;
                    }
                    else
                    {
                        orderBy = orderBy + ", " + field.filedName + " " + field.orderType; ;
                    }
                }
                if (!String.IsNullOrEmpty(orderBy))
                {
                    orderBy = " Order By " + orderBy;
                }
            }
            catch
            {
                orderBy = "";
            }
            
            return orderBy;
        }

        private static string getFieldsForInsert(DataToQueryInsert queryData)
        {
            string fields = null;
            try
            {
                foreach (FieldAndValue field in queryData.fields)
                {
                    if (String.IsNullOrEmpty(fields))
                    {
                        fields = field.filedName;
                    }
                    else
                    {
                        fields = fields + ", " + field.filedName;
                    }
                }
            }
            catch
            {
                fields = "";
            }
            
            return fields;
        }

        public static string getFieldsForUpdate(DataToQueryUpdate queryData)
        {
            string fields = null;
            try
            {
                foreach (FieldAndValue field in queryData.fields)
                {
                    if (String.IsNullOrEmpty(fields))
                    {
                        fields = field.filedName + " = '" + field.value + "'";
                    }
                    else
                    {
                        fields = fields + ", " + field.filedName + " = '" + field.value + "'";
                    }
                }
            }
            catch
            {
                fields = "";
            }

            return fields;
        }

        private static string getValuesForInsert(DataToQueryInsert queryData)
        {
            string values = null;
            try
            {
                foreach (FieldAndValue field in queryData.fields)
                {
                    if (String.IsNullOrEmpty(values))
                    {
                        values = "'" + field.value + "'";
                    }
                    else
                    {
                        values = values + ", '" + field.value + "'";
                    }
                }
            }
            catch
            {
                values = "";
            }
            
            return values;
        }

        public static string findAFieldValueInTable(DataTable table, string fieldName, int lineNum)
        {
            string value = "";
            int linesCount = 0;
            int columnsCount = 0;
            
            foreach(DataRow row in table.Rows)
            {
                if(linesCount < lineNum)
                {
                    linesCount++;
                    continue;
                }
                foreach (DataColumn column in table.Columns)
                {
                    if (column.ColumnName.Equals(fieldName))
                    {
                        value = row[column].ToString();
                        break;
                    }
                    columnsCount++;
                }
                if(linesCount >= lineNum)
                {
                    break;
                }
            }

            return value;
        }
    }
}
