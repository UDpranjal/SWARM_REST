using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipcodeController : BaseController, iBaseController<Zipcode>
    {
        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Zipcode itmZip = await _context.Zipcodes.Where(x => x.Zip == KeyValue).FirstOrDefaultAsync();
                _context.Zipcodes.Remove(itmZip);
                _context.SaveChanges();
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public Task<IActionResult> Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZip = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lstZip);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            Zipcode itmZip = await _context.Zipcodes.Where(x => x.Zip == KeyValue).FirstOrDefaultAsync();
            return Ok(itmZip);
        }

        public Task<IActionResult> Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _zip = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();

                if (_zip != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Zip already exists!");
                }

                _zip = new Zipcode();
                _zip.Zip = _Item.Zip;
                _zip.City = _Item.City;
                _zip.State = _Item.State;
                _zip.CreatedBy = _Item.CreatedBy;
                _zip.ModifiedBy = _Item.ModifiedBy;
                _zip.CreatedDate = _Item.CreatedDate;
                _zip.ModifiedDate = _Item.ModifiedDate;
                _context.Zipcodes.Add(_zip);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _zip = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();

                if (_zip != null)
                {
                    _zip.City = _Item.City;
                    _zip.State = _Item.State;
                    _zip.CreatedBy = _Item.CreatedBy;
                    _zip.ModifiedBy = _Item.ModifiedBy;
                    _zip.CreatedDate = _Item.CreatedDate;
                    _zip.ModifiedDate = _Item.ModifiedDate;
                    _context.Zipcodes.Update(_zip);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                    return Ok();
                }
                _context.Database.CloseConnection();
                await this.Post(_Item);
                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
