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
    public class GradeController : BaseController, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        [Route("Delete/{KeyValue1}/{KeyValue2}/{KeyValue3}")]
        [Route("Delete/{KeyValue1}/{KeyValue2}/{KeyValue3}/{KeyValue4}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/grade_code_occurrence/grade_type_code/student_id/section_id/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}/{KeyValue3}/{KeyValue4}/{KeyValue5}")]
        public async Task<IActionResult> Delete(int KeyValue1, string KeyValue2, int KeyValue3, int KeyValue4, int KeyValue5)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Grade itmGrade = await _context.Grades.Where(x => x.GradeCodeOccurrence == KeyValue1).Where(x => x.GradeTypeCode == KeyValue2)
                    .Where(x => x.StudentId == KeyValue3)
                    .Where(x => x.SectionId == KeyValue4).Where(x => x.SchoolId == KeyValue5).FirstOrDefaultAsync();
                _context.Grades.Remove(itmGrade);
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
            List<Grade> lstGrade = await _context.Grades.OrderBy(x => x.GradeCodeOccurrence).ToListAsync();
            return Ok(lstGrade);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        [Route("Get/{KeyValue1}/{KeyValue2}/{KeyValue3}")]
        [Route("Get/{KeyValue1}/{KeyValue2}/{KeyValue3}/{KeyValue4}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/grade_code_occurrence/grade_type_code/student_id/section_id/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}/{KeyValue3}/{KeyValue4}/{KeyValue5}")]
        public async Task<IActionResult> Get(int KeyValue1, string KeyValue2, int KeyValue3, int KeyValue4, int KeyValue5)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.GradeCodeOccurrence == KeyValue1).Where(x => x.GradeTypeCode == KeyValue2)
                    .Where(x => x.StudentId == KeyValue3)
                    .Where(x => x.SectionId == KeyValue4).Where(x => x.SchoolId == KeyValue5).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grade = await _context.Grades.Where(x => x.GradeCodeOccurrence == _Item.GradeCodeOccurrence).Where(x => x.GradeTypeCode == _Item.GradeTypeCode)
                    .Where(x => x.StudentId == _Item.StudentId).Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_grade != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade already exists!");
                }

                _grade = new Grade();
                _grade.SchoolId = _Item.SchoolId;
                _grade.StudentId = _Item.StudentId;
                _grade.SectionId = _Item.SectionId;
                _grade.GradeTypeCode = _Item.GradeTypeCode;
                _grade.GradeCodeOccurrence = _Item.GradeCodeOccurrence;
                _grade.NumericGrade = _Item.NumericGrade;
                _grade.Comments = _Item.Comments;
                _grade.CreatedBy = _Item.CreatedBy;
                _grade.CreatedDate = _Item.CreatedDate;
                _grade.ModifiedBy = _Item.ModifiedBy;
                _grade.ModifiedDate = _Item.ModifiedDate;
                _context.Grades.Add(_grade);
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
        public async Task<IActionResult> Put([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grade = await _context.Grades.Where(x => x.GradeCodeOccurrence == _Item.GradeCodeOccurrence).Where(x => x.GradeTypeCode == _Item.GradeTypeCode)
                    .Where(x => x.StudentId == _Item.StudentId).Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_grade != null)
                {
                    _grade.NumericGrade = _Item.NumericGrade;
                    _grade.Comments = _Item.Comments;
                    _grade.CreatedBy = _Item.CreatedBy;
                    _grade.CreatedDate = _Item.CreatedDate;
                    _grade.ModifiedBy = _Item.ModifiedBy;
                    _grade.ModifiedDate = _Item.ModifiedDate;
                    _context.Grades.Update(_grade);
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
