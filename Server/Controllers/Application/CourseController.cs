using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController, iBaseController<Course>
    {

        public CourseController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("Get/{KeyValue1}")]
        public async Task<IActionResult> Get(int KeyValue1)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass both primary keys <url>/course_no/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }


        [HttpDelete]
        [Route("Delete")]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass both primary keys <url>/course_no/school_id");
        }


        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
                _context.Courses.Remove(itmCourse);
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _crse = await _context.Courses.Where(x => x.CourseNo == _Item.CourseNo).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_crse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Course already exists!");
                }

                _crse = new Course();
                _crse.SchoolId = _Item.SchoolId;
                _crse.CourseNo = _Item.CourseNo;
                _crse.Cost = _Item.Cost;
                _crse.Description = _Item.Description;
                _crse.Prerequisite = _Item.Prerequisite;
                _crse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                _context.Courses.Add(_crse);
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
        public async Task<IActionResult> Put([FromBody] Course _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _crse = await _context.Courses.Where(x => x.CourseNo == _Item.CourseNo).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_crse != null)
                {
                    _crse.Cost = _Item.Cost;
                    _crse.Description = _Item.Description;
                    _crse.Prerequisite = _Item.Prerequisite;
                    _crse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                    _crse.ModifiedBy = _Item.ModifiedBy;
                    _context.Courses.Update(_crse);
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
