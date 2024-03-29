﻿using Microsoft.AspNetCore.Mvc;
using SWARM.Shared.DTO;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Base
{
    public interface iBaseController<T>
    {
        Task<IActionResult> Delete(int KeyValue);
        Task<IActionResult> Get();
        Task<IActionResult> Get(int KeyValue);
        Task<IActionResult> Post([FromBody] T _Item);
        Task<IActionResult> Put([FromBody] T _Item);
    }
}