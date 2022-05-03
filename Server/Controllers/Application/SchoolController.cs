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
    public class SchoolController : BaseController, iBaseController<School>
    {
        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass the primary key <url>/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                School itmSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Schools.Remove(itmSchool);
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
            List<School> lstSchool = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstSchool);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmSchool);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _school = await _context.Schools.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_school != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "School already exists!");
                }

                _school = new School();
                _school.SchoolId = _Item.SchoolId;
                _school.SchoolName = _Item.SchoolName;
                _school.CreatedBy = _Item.CreatedBy;
                _school.ModifiedBy = _Item.ModifiedBy;
                _context.Schools.Add(_school);
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
        public async Task<IActionResult> Put([FromBody] School _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _school = await _context.Schools.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_school != null)
                {
                    _school.SchoolName = _Item.SchoolName;
                    _school.CreatedBy = _Item.CreatedBy;
                    _school.ModifiedBy = _Item.ModifiedBy;
                    _context.Schools.Update(_school);
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
