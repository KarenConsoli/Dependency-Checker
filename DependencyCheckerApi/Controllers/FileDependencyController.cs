using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DependencyCheckerApi.ViewModel;
using FileCheckerApiServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DependencyCheckerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileDependencyController : ControllerBase
    {
    

        private readonly ILogger<FileDependencyController> _logger;

        public FileDependencyController(ILogger<FileDependencyController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<FileDependencyViewModel> GetDependencyFromFile(string )
        //{



        //    //var rng = new Random();
        //    //return Enumerable.Range(1, 5).Select(index => new FileDependency
        //    //{
        //    //    Date = DateTime.Now.AddDays(index),
        //    //    TemperatureC = rng.Next(-20, 55),
        //    //    Summary = Summaries[rng.Next(Summaries.Length)]
        //    //})
        //    //.ToArray();
        //}



        [HttpGet]
        public IEnumerable<FileDependencyViewModel> GetDependencyFromFile(string fileName)
        {

            var listDependencies = new List<FileDependencyViewModel>();
            var list = new FileService().Read();
          

            if (new FileService().Read().Any(x => x.FileName == fileName))
            {
                var fileId = new FileService().Read().Single(x => x.FileName == fileName);

            }
            else
            {
                listDependencies.Add(new FileDependencyViewModel
                {
                    Message="The File Does Not Exist"
                });

            }

            return listDependencies.ToArray();

        }
    }
}
