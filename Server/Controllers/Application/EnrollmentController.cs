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
    public class EnrollmentController : BaseController, iBaseController<Enrollment>
    {

        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/student_id/section_id/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}/{KeyValue3}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2, int KeyValue3)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Enrollment itmEnr = await _context.Enrollments.Where(x => x.StudentId == KeyValue1).Where(x => x.SectionId == KeyValue2).Where(x => x.SchoolId == KeyValue3).FirstOrDefaultAsync();
                _context.Enrollments.Remove(itmEnr);
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
            List<Enrollment> lstEnr = await _context.Enrollments.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstEnr);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/student_id/section_id/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}/{KeyValue3}")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2, int KeyValue3)
        {
            Enrollment itmEnr = await _context.Enrollments.Where(x => x.StudentId == KeyValue1).Where(x => x.SectionId == KeyValue2).Where(x => x.SchoolId == KeyValue3).FirstOrDefaultAsync();
            return Ok(itmEnr);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _enr = await _context.Enrollments.Where(x => x.StudentId == _Item.StudentId).Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_enr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment already exists!");
                }

                _enr = new Enrollment();
                _enr.StudentId = _Item.StudentId;
                _enr.SectionId = _Item.SectionId;
                _enr.SchoolId = _Item.SchoolId;
                _enr.EnrollDate = _Item.EnrollDate;
                _enr.FinalGrade = _Item.FinalGrade;
                _enr.CreatedBy = _Item.CreatedBy;
                _enr.ModifiedBy = _Item.ModifiedBy;
                _context.Enrollments.Add(_enr);
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
        public async Task<IActionResult> Put([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _enr = await _context.Enrollments.Where(x => x.StudentId == _Item.StudentId).Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_enr != null)
                {
                    _enr.EnrollDate = _Item.EnrollDate;
                    _enr.FinalGrade = _Item.FinalGrade;
                    _enr.CreatedBy = _Item.CreatedBy;
                    _enr.ModifiedBy = _Item.ModifiedBy;
                    _context.Enrollments.Update(_enr);
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
