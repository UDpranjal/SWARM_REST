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
    public class DevicecodesController : BaseController, iBaseController<DeviceCode>
    {
        public DevicecodesController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                DeviceCode itmDC = await _context.DeviceCodes.Where(x => x.UserCode == KeyValue).FirstOrDefaultAsync();
                _context.DeviceCodes.Remove(itmDC);
                _context.SaveChanges();
                await trans.CommitAsync();
                return Ok("Success");
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
            List<DeviceCode> lstDC = await _context.DeviceCodes.OrderBy(x => x.UserCode).ToListAsync();
            return Ok(lstDC);
        }

        public Task<IActionResult> Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            DeviceCode itmDC = await _context.DeviceCodes.Where(x => x.UserCode == KeyValue).FirstOrDefaultAsync();
            return Ok(itmDC);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DeviceCode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _dc = await _context.DeviceCodes.Where(x => x.UserCode == _Item.UserCode).FirstOrDefaultAsync();

                if (_dc != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Device Code already exists!");
                }

                _dc = new DeviceCode();
                _dc.UserCode = _Item.UserCode;
                _dc.DeviceCode1 = _Item.DeviceCode1;
                _dc.SubjectId = _Item.SubjectId;
                _dc.SessionId = _Item.SessionId;
                _dc.ClientId = _Item.ClientId;
                _dc.Description = _Item.Description;
                _dc.CreationTime = _Item.CreationTime;
                _dc.Expiration = _Item.Expiration;
                _dc.Data = _Item.Data;
                _context.DeviceCodes.Add(_dc);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Success");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DeviceCode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _dc = await _context.DeviceCodes.Where(x => x.UserCode == _Item.UserCode).FirstOrDefaultAsync();

                if (_dc != null)
                {
                    _dc.DeviceCode1 = _Item.DeviceCode1;
                    _dc.SubjectId = _Item.SubjectId;
                    _dc.SessionId = _Item.SessionId;
                    _dc.ClientId = _Item.ClientId;
                    _dc.Description = _Item.Description;
                    _dc.CreationTime = _Item.CreationTime;
                    _dc.Expiration = _Item.Expiration;
                    _dc.Data = _Item.Data;
                    _context.DeviceCodes.Update(_dc);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                    return Ok("Success");
                }
                _context.Database.CloseConnection();
                await this.Post(_Item);
                return Ok("Success");

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
