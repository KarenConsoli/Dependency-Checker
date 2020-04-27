using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DependencyCheckerApi.ViewModel;
using DependencyCheckerApiServices;
using FileCheckerApiServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static DependencyCheckerApiServices.FileDependencService;

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

        


        [HttpGet]
        [Route("GetDependencyFromFile")]
        public IEnumerable<FileDependencyViewModel> GetDependencyFromFile(string fileName)
        {

            var listDependencies = new List<FileDependencyViewModel>();
            var dependencies = new DependencyService().Read();
           


            if (new FileService().Read().Any(x => x.FileName == fileName))
            {
                var fileId = new FileService().Read().Single(x => x.FileName == fileName);

                var fileDependencies = new FileDependencyService().Read().FindAll(x=>x.FileId== fileId.FileId);


                listDependencies.Add(new FileDependencyViewModel
                {
                    Result = "Dependency Count: "+ fileDependencies.Count.ToString() + " - FileName: "+ fileName,

                });

                var countId = 1;
                foreach (var dependency in fileDependencies.FindAll(x => x.DependencyId != 0))
                {
                    if (!listDependencies.Any(x => x.DependencyName == dependencies.Single(x => x.DependencyId == dependency.DependencyId).DependencyName))
                    {
                        listDependencies.Add(new FileDependencyViewModel
                        {
                            Result = countId.ToString(),
                            FileName = fileName,
                            DependencyName = dependencies.Single(x => x.DependencyId == dependency.DependencyId).DependencyName
                        });

                        countId++;
                    }
                }

                foreach (var dependency in fileDependencies.FindAll(x => x.DependencyId == 0))
                {
                
                        listDependencies.Add(new FileDependencyViewModel
                        {
                            Result = countId.ToString(),
                            FileName = fileName,
                            DependencyName = "The file has NO Dependencies"
                        });
                        countId++;
                    

                }


            }
            else
            {
                listDependencies.Add(new FileDependencyViewModel
                {
                    Result="The File Does Not Exist"
                });

            }

            return listDependencies.ToArray();

        }



        [HttpGet]
        [Route("GetFileFromDependency")]

        public IEnumerable<FileDependencyViewModel> GetFileFromDependency(string dependencyName)
        {

            
            var listDependencies = new List<FileDependencyViewModel>();
            
            
            var files = new FileService().Read();




            if (new DependencyService().Read().Any(x => x.DependencyName == dependencyName))
            {
                var dependencyId = new DependencyService().Read().Single(x => x.DependencyName == dependencyName);

                var fileDependencies = new FileDependencyService().Read().FindAll(x => x.DependencyId == dependencyId.DependencyId);


                listDependencies.Add(new FileDependencyViewModel
                {
                    Result = "File Count: " + fileDependencies.Count.ToString() + " - DependencyName: " + dependencyName,

                });

                var countId = 1;
                foreach (var file in fileDependencies)
                {
                    if (!listDependencies.Any(x=>x.FileName== files.Single(x => x.FileId == file.DependencyId).FileName))
                    {

                        listDependencies.Add(new FileDependencyViewModel
                        {
                            Result = countId.ToString(),

                            FileName = files.Single(x => x.FileId == file.FileId).FileName,


                            DependencyName = dependencyName,
                        });

                        countId++;
                    }
                   

                   
                }

           
            }
            else
            {
                listDependencies.Add(new FileDependencyViewModel
                {
                    Result = "The Dependency Does Not Exist"
                });

            }

            return listDependencies.ToArray();

        }
    }
}
