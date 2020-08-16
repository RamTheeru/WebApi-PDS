using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace pdstest.Models
{
    public class CustomResult : IActionResult
    {
        public readonly APIResult _result;

        public CustomResult(APIResult result)
        {
            _result = result;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objResult = new ObjectResult(_result)
            { 
                Value = JsonConvert.SerializeObject(_result)
            };

            await objResult.ExecuteResultAsync(context);
        }

    }
}
