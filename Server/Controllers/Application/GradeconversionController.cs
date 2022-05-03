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
    public class GradeconversionController : BaseController, iBaseController<GradeConversion>
    {
        public GradeconversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/letter_grade/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(string KeyValue1, int KeyValue2)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeConversion itmGc = await _context.GradeConversions.Where(x => x.LetterGrade == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
                _context.GradeConversions.Remove(itmGc);
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
            List<GradeConversion> lstGc = await _context.GradeConversions.OrderBy(x => x.LetterGrade).ToListAsync();
            return Ok(lstGc);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/letter_grade/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(string KeyValue1, int KeyValue2)
        {
            GradeConversion itmGc = await _context.GradeConversions.Where(x => x.LetterGrade == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
            return Ok(itmGc);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gc = await _context.GradeConversions.Where(x => x.LetterGrade == _Item.LetterGrade).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_gc != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Conversion already exists!");
                }

                _gc = new GradeConversion();
                _gc.SchoolId = _Item.SchoolId;
                _gc.LetterGrade = _Item.LetterGrade;
                _gc.GradePoint = _Item.GradePoint;
                _gc.MaxGrade = _Item.MaxGrade;
                _gc.MinGrade = _Item.MinGrade;
                _gc.CreatedBy = _Item.CreatedBy;
                _gc.CreatedDate = _Item.CreatedDate;
                _gc.ModifiedBy = _Item.ModifiedBy;
                _gc.ModifiedDate = _Item.ModifiedDate;
                _context.GradeConversions.Add(_gc);
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
        public async Task<IActionResult> Put([FromBody] GradeConversion _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gc = await _context.GradeConversions.Where(x => x.LetterGrade == _Item.LetterGrade).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_gc != null)
                {
                    _gc.GradePoint = _Item.GradePoint;
                    _gc.MaxGrade = _Item.MaxGrade;
                    _gc.MinGrade = _Item.MinGrade;
                    _gc.CreatedBy = _Item.CreatedBy;
                    _gc.CreatedDate = _Item.CreatedDate;
                    _gc.ModifiedBy = _Item.ModifiedBy;
                    _gc.ModifiedDate = _Item.ModifiedDate;
                    _context.GradeConversions.Update(_gc);
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
