using AutoMapper;
using DependencyCheckerApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyCheckerApiServices
{
   public  class FileDependencService
    {
        public class FileDependencyService
        {
            public FileDependencyModel Read(FileDependencyModel modelo)
            {


                var bd = new BDConnection.BDConnection();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<FileDependencyModel, FileDependencyModel>());
                IMapper iMapper = config.CreateMapper();
                var f = iMapper.Map<object, FileDependencyModel>(bd.ReadWhere<FileDependencyModel>(modelo));


                return f;


            }


            public List<FileDependencyModel> ReadLista(FileDependencyModel modelo)
            {

                var bd = new BDConnection.BDConnection();


                var lista = new List<FileDependencyModel>();


                var config = new MapperConfiguration(cfg => cfg.CreateMap<FileDependencyModel, FileDependencyModel>());
                IMapper iMapper = config.CreateMapper();

                try
                {
                    foreach (var item in bd.ReadWhereList<FileDependencyModel>(modelo))
                    {

                        lista.Add(iMapper.Map<object, FileDependencyModel>(item));
                    }

                }
                catch (Exception e)
                {
                    return null;
                }


                return lista;



            }
            public List<FileDependencyModel> ReadLike(FileDependencyModel modelo)
            {

                var bd = new BDConnection.BDConnection();


                var lista = new List<FileDependencyModel>();


                var config = new MapperConfiguration(cfg => cfg.CreateMap<FileDependencyModel, FileDependencyModel>());
                IMapper iMapper = config.CreateMapper();

                try
                {
                    foreach (var item in bd.ReadLike<FileDependencyModel>(modelo))
                    {

                        lista.Add(iMapper.Map<object, FileDependencyModel>(item));
                    }

                }
                catch (Exception e)
                {
                    return null;
                }


                return lista;



            }

            public List<FileDependencyModel> Read()
            {

                var bd = new BDConnection.BDConnection();


                var lista = new List<FileDependencyModel>();



                var config = new MapperConfiguration(cfg => cfg.CreateMap<FileDependencyModel, FileDependencyModel>());
                IMapper iMapper = config.CreateMapper();

                try
                {
                    foreach (var item in bd.Read<FileDependencyModel>(new FileDependencyModel()))
                    {

                        lista.Add(iMapper.Map<object, FileDependencyModel>(item));
                    }

                }
                catch (Exception e)
                {

                    return null;
                }



                return lista;

            }


            public bool Add(FileDependencyModel modelo)
            {


                var bd = new BDConnection.BDConnection();


                var res = bd.Add<FileDependencyModel>(modelo);


                return res;
            }


            public FileDependencyModel Read(long? id)
            {

                var bd = new BDConnection.BDConnection();


                var config = new MapperConfiguration(cfg => cfg.CreateMap<FileDependencyModel, FileDependencyModel>());
                IMapper iMapper = config.CreateMapper();


                if (id != null)
                {
                    return iMapper.Map<object, FileDependencyModel>(bd.Read<FileDependencyModel>(new FileDependencyModel(), id.ToString()));

                }

                return null;


            }
            public bool Edit(FileDependencyModel modelo, long id)
            {
                var bd = new BDConnection.BDConnection();


                modelo.FileDependencyId = id;

                return bd.Edit<FileDependencyModel>(modelo, id.ToString());
            }





            public bool Delete(long id)
            {

                var bd = new BDConnection.BDConnection();


                var modelo = new FileDependencyModel();

                modelo.FileDependencyId = id;


                var g = bd.DeletePost<FileDependencyModel>(modelo, id);



                return g;
            }

            public long Execute(string texto)
            {

                var bd = new BDConnection.BDConnection();


                try
                {
                    var res = Convert.ToInt64(bd.ExecuteQuery(texto));

                    return res;
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;


                    Console.WriteLine("Error Deleting Old Rows.");

                    Console.ForegroundColor = ConsoleColor.White;

                }



                return 0;
            }


        }
    }
}
