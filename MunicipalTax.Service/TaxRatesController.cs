using MunicpalTax.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MunicipalTax.Service
{
    public class TaxRatesController : ApiController
    {
        private TaxRateContext _dbContext;

        public TaxRatesController()
        {
            _dbContext = new TaxRateContext();
        }

        // GET api/taxrates
        [HttpGet]
        public async Task<IEnumerable<TaxRate>> Get()
        {
            try
            {
                return await _dbContext.TaxRates.ToListAsync();
            }
            catch (Exception exc)
            {
                // TODO: log exception
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Errorr occured during search of tax rates."));
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
