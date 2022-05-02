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
    public class InstructorController : BaseController, iBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/instructor_id/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Instructor itmInst = await _context.Instructors.Where(x => x.InstructorId == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
                _context.Instructors.Remove(itmInst);
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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lstInst = await _context.Instructors.OrderBy(x => x.InstructorId).ToListAsync();
            return Ok(lstInst);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/instructor_id/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2)
        {
            Instructor itmInst = await _context.Instructors.Where(x => x.InstructorId == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
            return Ok(itmInst);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _inst = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_inst != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Instructor already exists!");
                }

                _inst = new Instructor();
                _inst.SchoolId = _Item.SchoolId;
                _inst.InstructorId = _Item.InstructorId;
                _inst.Salutation = _Item.Salutation;
                _inst.FirstName = _Item.FirstName;
                _inst.LastName = _Item.LastName;
                _inst.StreetAddress = _Item.StreetAddress;
                _inst.Zip = _Item.Zip;
                _inst.Phone = _Item.Phone;
                _inst.CreatedBy = _Item.CreatedBy;
                _inst.CreatedDate = _Item.CreatedDate;
                _inst.ModifiedBy = _Item.ModifiedBy;
                _inst.ModifiedDate = _Item.ModifiedDate;
                _context.Instructors.Add(_inst);
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
        public async Task<IActionResult> Put([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _inst = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_inst != null)
                {
                    _inst.Salutation = _Item.Salutation;
                    _inst.FirstName = _Item.FirstName;
                    _inst.LastName = _Item.LastName;
                    _inst.StreetAddress = _Item.StreetAddress;
                    _inst.Zip = _Item.Zip;
                    _inst.Phone = _Item.Phone;
                    _inst.CreatedBy = _Item.CreatedBy;
                    _inst.CreatedDate = _Item.CreatedDate;
                    _inst.ModifiedBy = _Item.ModifiedBy;
                    _inst.ModifiedDate = _Item.ModifiedDate;
                    _context.Instructors.Update(_inst);
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
