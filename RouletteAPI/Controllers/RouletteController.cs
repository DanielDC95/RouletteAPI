using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
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
        [Route("[action]/")]
        [HttpGet]
        public List<Dictionary<string, string>> ToList()
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
            return id.ToString();
        }

        // POST: api/Roulette
        [Route("[action]/")]
        [HttpPost]
        public Dictionary<string, string> Create()
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
        [Route("[action]/")]
        [HttpPut]
        public Dictionary<string, string> Open([FromBody] JsonElement body)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            try
            {
                int id = Int32.Parse(body.GetProperty("id").ToString());
                Roulette roulette = new Roulette();
                bool loaded = roulette.loadRoulette(id);
                bool opened = false;
                if (loaded)
                {
                    opened = roulette.Open();
                }
                if (opened)
                {
                    Bets.openRouletteBet(id);
                    response.Add("result", "Success");
                    response.Add("id", roulette.getID().ToString());
                    response.Add("state", roulette.getState());
                }
                else
                {
                    response.Add("result", "Failure");
                    response.Add("id", roulette.getID().ToString());
                    response.Add("state", roulette.getState());
                    response.Add("message", "Update failed.");
                }
            }
            catch
            {
                response.Add("result", "Failure");
                response.Add("message", "Has hapenned an error during the process.");
            }
            
            return response;
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
                    roulette.Add("roulette_id", id);
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
