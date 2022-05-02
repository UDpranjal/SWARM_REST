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
    public class StudentController : BaseController, iBaseController<Student>
    {
        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/student_id/school_id");
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Student itmStu = await _context.Students.Where(x => x.StudentId == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
                _context.Students.Remove(itmStu);
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
            List<Student> lstStu = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstStu);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Please pass all primary keys <url>/student_id/school_id");
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2)
        {
            Student itmStu = await _context.Students.Where(x => x.StudentId == KeyValue1).Where(x => x.SchoolId == KeyValue2).FirstOrDefaultAsync();
            return Ok(itmStu);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _stu = await _context.Students.Where(x => x.StudentId == _Item.StudentId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_stu != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Student already exists!");
                }

                _stu = new Student();
                _stu.StudentId = _Item.StudentId;
                _stu.SchoolId = _Item.SchoolId;
                _stu.Salutation = _Item.Salutation;
                _stu.FirstName = _Item.FirstName;
                _stu.LastName = _Item.LastName;
                _stu.StreetAddress = _Item.StreetAddress;
                _stu.Zip = _Item.Zip;
                _stu.Phone = _Item.Phone;
                _stu.Employer = _Item.Employer;
                _stu.RegistrationDate = _Item.RegistrationDate;
                _stu.CreatedBy = _Item.CreatedBy;
                _stu.ModifiedBy = _Item.ModifiedBy;
                _stu.CreatedDate = _Item.CreatedDate;
                _stu.ModifiedDate = _Item.ModifiedDate;
                _context.Students.Add(_stu);
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
        public async Task<IActionResult> Put([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _stu = await _context.Students.Where(x => x.StudentId == _Item.StudentId).Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (_stu != null)
                {
                    _stu.Salutation = _Item.Salutation;
                    _stu.FirstName = _Item.FirstName;
                    _stu.LastName = _Item.LastName;
                    _stu.StreetAddress = _Item.StreetAddress;
                    _stu.Zip = _Item.Zip;
                    _stu.Phone = _Item.Phone;
                    _stu.Employer = _Item.Employer;
                    _stu.RegistrationDate = _Item.RegistrationDate;
                    _stu.CreatedBy = _Item.CreatedBy;
                    _stu.ModifiedBy = _Item.ModifiedBy;
                    _stu.CreatedDate = _Item.CreatedDate;
                    _stu.ModifiedDate = _Item.ModifiedDate;
                    _context.Students.Update(_stu);
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
