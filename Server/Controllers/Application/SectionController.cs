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
    public class SectionController : BaseController, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete")]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass both primary keys <url>/section_id/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Section itmSection= await _context.Sections.Where(x => x.SectionId == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
                _context.Sections.Remove(itmSection);
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
            List<Section> lstSection= await _context.Sections.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lstSection);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass both primary keys <url>/section_id/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionId == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
            return Ok(itmSection);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sect = await _context.Sections.Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_sect != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Section already exists!");
                }

                _sect = new Section();
                _sect.SectionId = _Item.SectionId;
                _sect.SchoolId = _Item.SchoolId;
                _sect.SectionNo = _Item.SectionNo;
                _sect.CourseNo = _Item.CourseNo;
                _sect.StartDateTime = _Item.StartDateTime;
                _sect.Location = _Item.Location;
                _sect.InstructorId = _Item.InstructorId;
                _sect.Capacity = _Item.Capacity;
                _sect.CreatedBy = _Item.CreatedBy;
                _sect.ModifiedBy = _Item.ModifiedBy;
                _context.Sections.Add(_sect);
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
        public async Task<IActionResult> Put([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sect = await _context.Sections.Where(x => x.SectionId == _Item.SectionId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_sect != null)
                {
                    _sect.SectionNo = _Item.SectionNo;
                    _sect.CourseNo = _Item.CourseNo;
                    _sect.StartDateTime = _Item.StartDateTime;
                    _sect.Location = _Item.Location;
                    _sect.InstructorId = _Item.InstructorId;
                    _sect.Capacity = _Item.Capacity;
                    _sect.ModifiedBy = _Item.ModifiedBy;
                    _context.Sections.Update(_sect);
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
