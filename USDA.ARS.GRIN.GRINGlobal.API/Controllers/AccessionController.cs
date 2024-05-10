
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

        public async Task<IHttpActionResult> Get(string piNumber = "", string instCode = "", int offset = 0, int limit = 0, string mcpdFormat = "Y")
        {
            try
            {
                AccessionViewModel viewModel = new AccessionViewModel();
                viewModel.SearchEntity.AccessionNumber = piNumber;
                viewModel.SearchEntity.InstCode = instCode;
                viewModel.Search();
                return Ok(viewModel.DataCollection);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}