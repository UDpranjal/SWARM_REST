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
    public class SchooluserController : BaseController, iBaseController<SchoolUser>
    {
        public SchooluserController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/user_name/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue1, string KeyValue2)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                SchoolUser itmSU = await _context.SchoolUsers.Where(x => x.SchoolId == KeyValue1).Where(x => x.UserName == KeyValue2).FirstOrDefaultAsync();
                _context.SchoolUsers.Remove(itmSU);
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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<SchoolUser> lstSU = await _context.SchoolUsers.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstSU);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/user_name/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue1, string KeyValue2)
        {
            SchoolUser itmSU = await _context.SchoolUsers.Where(x => x.SchoolId == KeyValue1).Where(x => x.UserName == KeyValue2).FirstOrDefaultAsync();
            return Ok(itmSU);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SchoolUser _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _su = await _context.SchoolUsers.Where(x => x.UserName == _Item.UserName).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_su != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "School User already exists!");
                }

                _su = new SchoolUser();
                _su.UserName = _Item.UserName;
                _su.SchoolId = _Item.SchoolId;
                _context.SchoolUsers.Add(_su);
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
        public async Task<IActionResult> Put([FromBody] SchoolUser _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _su = await _context.SchoolUsers.Where(x => x.UserName == _Item.UserName).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_su != null)
                {
                    _su = new SchoolUser();
                    _su.UserName = _Item.UserName;
                    _su.SchoolId = _Item.SchoolId;
                    _context.SchoolUsers.Update(_su);
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
