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
    public class GradetypeController : BaseController, iBaseController<GradeType>
    {
        public GradetypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/grade_type_code/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(string KeyValue1, int KeyValue2)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeType itmGt = await _context.GradeTypes.Where(x => x.GradeTypeCode == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
                _context.GradeTypes.Remove(itmGt);
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
            List<GradeType> lstGt = await _context.GradeTypes.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lstGt);
        }

        public Task<IActionResult> Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(string KeyValue1, int KeyValue2)
        {
            GradeType itmGt = await _context.GradeTypes.Where(x => x.GradeTypeCode == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
            return Ok(itmGt);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gt = await _context.GradeTypes.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_gt != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Type already exists!");
                }

                _gt = new GradeType();
                _gt.SchoolId = _Item.SchoolId;
                _gt.GradeTypeCode = _Item.GradeTypeCode;
                _gt.Description = _Item.Description;
                _gt.CreatedBy = _Item.CreatedBy;
                _gt.CreatedDate = _Item.CreatedDate;
                _gt.ModifiedBy = _Item.ModifiedBy;
                _gt.ModifiedDate = _Item.ModifiedDate;
                _context.GradeTypes.Add(_gt);
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
        public async Task<IActionResult> Put([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gt = await _context.GradeTypes.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_gt != null)
                {
                    _gt.Description = _Item.Description;
                    _gt.CreatedBy = _Item.CreatedBy;
                    _gt.CreatedDate = _Item.CreatedDate;
                    _gt.ModifiedBy = _Item.ModifiedBy;
                    _gt.ModifiedDate = _Item.ModifiedDate;
                    _context.GradeTypes.Update(_gt);
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
