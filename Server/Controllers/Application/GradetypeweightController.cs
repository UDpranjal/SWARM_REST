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
    public class GradetypeweightController : BaseController, iBaseController<GradeTypeWeight>
    {
        public GradetypeweightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/grade_type_code/section_id/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}/{KeyValue3}")]
        public async Task<IActionResult> Delete(string KeyValue1, int KeyValue2, int KeyValue3)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeTypeWeight itmGtw = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == KeyValue1).Where(x => x.SectionId == KeyValue2).Where(x => x.SchoolId == KeyValue3).FirstOrDefaultAsync();
                _context.GradeTypeWeights.Remove(itmGtw);
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
            List<GradeTypeWeight> lstEnr = await _context.GradeTypeWeights.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lstEnr);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/grade_type_code/section_id/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}/{KeyValue3}")]
        public async Task<IActionResult> Get(string KeyValue1, int KeyValue2, int KeyValue3)
        {
            GradeTypeWeight itmGtw = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == KeyValue1).Where(x => x.SectionId == KeyValue2).Where(x => x.SchoolId == KeyValue3).FirstOrDefaultAsync();
            return Ok(itmGtw);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gtw = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_gtw != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Type Weight already exists!");
                }

                _gtw = new GradeTypeWeight();
                _gtw.SchoolId = _Item.SchoolId;
                _gtw.SectionId = _Item.SectionId;
                _gtw.GradeTypeCode = _Item.GradeTypeCode;
                _gtw.NumberPerSection = _Item.NumberPerSection;
                _gtw.PercentOfFinalGrade = _Item.PercentOfFinalGrade;
                _gtw.DropLowest = _Item.DropLowest;
                _gtw.CreatedBy = _Item.CreatedBy;
                _gtw.CreatedDate = _Item.CreatedDate;
                _gtw.ModifiedBy = _Item.ModifiedBy;
                _gtw.ModifiedDate = _Item.ModifiedDate;
                _context.GradeTypeWeights.Add(_gtw);
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
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gtw = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_gtw != null)
                {
                    _gtw.NumberPerSection = _Item.NumberPerSection;
                    _gtw.PercentOfFinalGrade = _Item.PercentOfFinalGrade;
                    _gtw.DropLowest = _Item.DropLowest;
                    _gtw.CreatedBy = _Item.CreatedBy;
                    _gtw.CreatedDate = _Item.CreatedDate;
                    _gtw.ModifiedBy = _Item.ModifiedBy;
                    _gtw.ModifiedDate = _Item.ModifiedDate;
                    _context.GradeTypeWeights.Update(_gtw);
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
