using MunicpalTax.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MunicipalTax.Service
{
    public enum RateType
    {
        Yearly = 1,
        Monthly,
        Weekly,
        Daily
    }

    public class TaxRateController : ApiController
    {
        private TaxRateContext _dbContext;

        public TaxRateController()
        {
            _dbContext = new TaxRateContext();
        }

        // GET api/taxrate
        [HttpGet]
        public async Task<double> GetRate([FromUri]string municipality, [FromUri]DateTime date)
        {
            try
            {
                var taxRates = await _dbContext.TaxRates
                    .Where(tr => tr.MunicipalityName == municipality && tr.StartDate <= date && tr.EndDate >= date)
                    .OrderByDescending(tr => tr.RateType)
                    .ToListAsync();

                if (taxRates.Any())
                {
                    return taxRates.First().Rate;
                }
                else
                {
                    // TODO: log exception
                    throw new HttpResponseException(
                        Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Tax rate not found for the municipality {municipality} at date {date}"));
                }
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception exc)
            {
                // TODO: log exception
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error occured during search of tax rate."));
            }
        }

        // GET api/taxrate/{id}
        [HttpGet]
        public async Task<TaxRate> Get(Guid id)
        {
            TaxRate taxRate;

            try
            {
                taxRate = await _dbContext.TaxRates.FirstOrDefaultAsync(tr => tr.Id == id);
            }
            catch (Exception exc)
            {
                //TODO: log exception
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Errorr occured during search of tax rates."));
            }

            if (taxRate != null)
            {
                return taxRate;
            }
            else
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tax rate not found"));
            }
        }

        // POST api/taxrate
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]TaxRate taxRate)
        {
            if (taxRate.Id == Guid.Empty)
            {
                taxRate.Id = Guid.NewGuid();
            }

            try
            {
                // TODO: analyse business rules - required attributes, overlapping tax dates by type

                _dbContext.TaxRates.Add(taxRate);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                // TODO: log exception
                return InternalServerError();
            }

            return Ok(taxRate);
        }

        // PUT api/taxrate/{id}
        [HttpPut]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]TaxRate taxRate)
        {
            try
            {
                var existingTaxRate = await _dbContext.TaxRates.FirstOrDefaultAsync(tr => tr.Id == id);
                if (existingTaxRate == null)
                {
                    return NotFound();
                }

                // TODO: analyse business rules - required attributes, overlapping tax dates by type

                if (taxRate.Id != id)
                {
                    // TODO: log error
                    return InternalServerError();
                }

                existingTaxRate.Name = taxRate.Name;
                existingTaxRate.MunicipalityName = taxRate.MunicipalityName;
                existingTaxRate.RateType = taxRate.RateType;
                existingTaxRate.StartDate = taxRate.StartDate;
                existingTaxRate.EndDate = taxRate.EndDate;
                existingTaxRate.Rate = taxRate.Rate;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                // TODO: log error
                return InternalServerError();
            }

            return Ok(taxRate);
        }

        // DELETE api/taxrate/{id} 
        [HttpDelete]
        public async Task Delete(Guid id)
        {
            var entity = await _dbContext.TaxRates.FirstOrDefaultAsync(tr => tr.Id == id);

            try
            {
                if (entity != null)
                {
                    _dbContext.TaxRates.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception exc)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to delete tax rate."));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
