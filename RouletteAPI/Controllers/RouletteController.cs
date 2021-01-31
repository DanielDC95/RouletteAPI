using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using static RouletteAPI.DataBase.DataBase;

namespace RouletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        // GET: api/Roulette
        [HttpGet]
        public List<Dictionary<string, string>> Get()
        {
            DataToQuerySelect queryData = new DataToQuerySelect();
            List<Dictionary<string, string>> response = new List<Dictionary<string, string>>();
            try 
            {
                queryData.tableName = "roulettes";
                List<string> fields = new List<string>();
                fields.Add("id");
                fields.Add("state_id");
                queryData.fields = fields;
                FieldsToOrder field = new FieldsToOrder();
                List<FieldsToOrder> orderBy = new List<FieldsToOrder>();
                field.filedName = "id";
                field.orderType = "asc";
                orderBy.Add(field);
                queryData.orderBy = orderBy;
                dataBaseConnect();
                DataTable table = dataBaseSelect(queryData);
                dataBaseDesconnect();
                response = getRoulettes(table);
            }
            catch
            {
                response = null;
            }

            return response;
        }

        // GET: api/Roulette/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Roulette
        [HttpPost]
        public Dictionary<string, string> Post()
        {
            Roulette roulette = new Roulette();
            bool created = roulette.create();
            Dictionary<string, string> response = new Dictionary<string, string>();
            if (created)
            {
                response.Add("result", "Success");
            }
            else
            {
                response.Add("result", "Failure");
            }
            response.Add("id", roulette.getID().ToString());
            return response;
        }

        // PUT: api/Roulette/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public List<Dictionary<string, string>> getRoulettes(DataTable table)
        {
            List<Dictionary<string, string>> roulettesLists = new List<Dictionary<string, string>>();
            try
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string id = findAFieldValueInTable(table: table, fieldName: "id", lineNum: i);
                    string state = findAFieldValueInTable(table: table, fieldName: "state_id", lineNum: i);
                    state = State.getState(state);
                    Dictionary<string, string> roulette = new Dictionary<string, string>();
                    roulette.Add("id", id);
                    roulette.Add("state", state);
                    roulettesLists.Add(roulette);
                }
            }
            catch
            {
                roulettesLists = null;
            }
            
            return roulettesLists;
        }
    }
}
