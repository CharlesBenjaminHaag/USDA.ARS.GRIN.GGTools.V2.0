
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GRINGlobal.API.Controllers
{
    public class AccessionController : ApiController
    {

        public async Task<IHttpActionResult> Get(string piNumber = "")
        {
            try
            {
                AccessionViewModel viewModel = new AccessionViewModel();
                viewModel.Search();
                return Ok(viewModel.DataCollection.Where(x => x.ID < 10000));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}