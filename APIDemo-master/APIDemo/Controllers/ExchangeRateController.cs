using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDemo.Data;
using APIDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIDemo
{
    //https://localhost:44332/api/exchangerate
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {

        // GET: api/<ExchangeRateController>
        [HttpGet]
        public IEnumerable<ExchangeRate> Get([FromQuery] string BaseCurrency)
        {
            if (BaseCurrency != null && BaseCurrency.ToLower() == "usd")
                return Singleton.Instance.ExchangeRates.Select(x => new ExchangeRate { Id = x.Id, Date = x.Date, GTQ = x.GTQ / x.USD, USD = 1 });

            return Singleton.Instance.ExchangeRates;
        }

        // GET api/<ExchangeRateController>/2020-08-01
        [HttpGet("{date}")]
        public ActionResult GetByDate([FromRoute] string date)
        {
            var result = Singleton.Instance.ExchangeRates.Where(x => x.Date == Convert.ToDateTime(date)).FirstOrDefault<ExchangeRate>();
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST api/<ExchangeRateController>
        [HttpPost]
        public ActionResult Post([FromBody] ExchangeRate newValue)
        {
            try
            {
                var result = Singleton.Instance.ExchangeRates.Where(x => x.Date == newValue.Date).FirstOrDefault<ExchangeRate>();
                if (result != null) return BadRequest();

                newValue.Id = Singleton.Instance.LastId + 1;
                Singleton.Instance.ExchangeRates.Add(newValue);
                Singleton.Instance.LastId++;
                return Created("",newValue);
            }
            catch (Exception ex)
            {
                return BadRequest(); 
            }
            
        }

        // PUT api/<ExchangeRateController>/2020-08-01
        [HttpPut("{date}")]
        public ActionResult Put(string date, [FromBody] ExchangeRate value)
        {
            var result = Singleton.Instance.ExchangeRates.Where(x => x.Date == Convert.ToDateTime(date)).FirstOrDefault<ExchangeRate>();
            if (result == null) return NotFound();
            value.Id = result.Id;
            Singleton.Instance.ExchangeRates.RemoveAll(x => x.Date == Convert.ToDateTime(date));
            Singleton.Instance.ExchangeRates.Add(value);
            return NoContent();
        }

        // DELETE api/<ExchangeRateController>/5
        [HttpDelete("{date}")]
        public ActionResult Delete(string date)
        {
            var result = Singleton.Instance.ExchangeRates.Where(x => x.Date == Convert.ToDateTime(date)).FirstOrDefault<ExchangeRate>();
            if (result == null) return NotFound();

            Singleton.Instance.ExchangeRates.RemoveAll(x => x.Date == Convert.ToDateTime(date));
            return NoContent();
        }
    }
}
