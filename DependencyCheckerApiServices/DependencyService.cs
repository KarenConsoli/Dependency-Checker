using AutoMapper;
using DependencyCheckerApiModels;
using System;
using System.Collections.Generic;

namespace DependencyCheckerApiServices
{
    public class DependencyService
    {
        public DependencyModel Read(DependencyModel modelo)
        {


            var bd = new BDConnection.BDConnection();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DependencyModel, DependencyModel>());
            IMapper iMapper = config.CreateMapper();
            var f = iMapper.Map<object, DependencyModel>(bd.ReadWhere<DependencyModel>(modelo));


            return f;


        }


        public List<DependencyModel> ReadLista(DependencyModel modelo)
        {

            var bd = new BDConnection.BDConnection();


            var lista = new List<DependencyModel>();


            var config = new MapperConfiguration(cfg => cfg.CreateMap<DependencyModel, DependencyModel>());
            IMapper iMapper = config.CreateMapper();

            try
            {
                foreach (var item in bd.ReadWhereList<DependencyModel>(modelo))
                {

                    lista.Add(iMapper.Map<object, DependencyModel>(item));
                }

            }
            catch (Exception e)
            {
                return null;
            }


            return lista;



        }
        public List<DependencyModel> ReadLike(DependencyModel modelo)
        {

            var bd = new BDConnection.BDConnection();


            var lista = new List<DependencyModel>();


            var config = new MapperConfiguration(cfg => cfg.CreateMap<DependencyModel, DependencyModel>());
            IMapper iMapper = config.CreateMapper();

            try
            {
                foreach (var item in bd.ReadLike<DependencyModel>(modelo))
                {

                    lista.Add(iMapper.Map<object, DependencyModel>(item));
                }

            }
            catch (Exception e)
            {
                return null;
            }


            return lista;



        }

        public List<DependencyModel> Read()
        {

            var bd = new BDConnection.BDConnection();


            var lista = new List<DependencyModel>();



            var config = new MapperConfiguration(cfg => cfg.CreateMap<DependencyModel, DependencyModel>());
            IMapper iMapper = config.CreateMapper();

            try
            {
                foreach (var item in bd.Read<DependencyModel>(new DependencyModel()))
                {

                    lista.Add(iMapper.Map<object, DependencyModel>(item));
                }

            }
            catch (Exception e)
            {

                return null;
            }



            return lista;

        }


        public bool Add(DependencyModel modelo)
        {


            var bd = new BDConnection.BDConnection();


            var res = bd.Add<DependencyModel>(modelo);


            return res;
        }


        public DependencyModel Read(long? id)
        {

            var bd = new BDConnection.BDConnection();


            var config = new MapperConfiguration(cfg => cfg.CreateMap<DependencyModel, DependencyModel>());
            IMapper iMapper = config.CreateMapper();


            if (id != null)
            {
                return iMapper.Map<object, DependencyModel>(bd.Read<DependencyModel>(new DependencyModel(), id.ToString()));

            }

            return null;


        }
        public bool Edit(DependencyModel modelo, long id)
        {
            var bd = new BDConnection.BDConnection();


            modelo.DependencyId = id;

            return bd.Edit<DependencyModel>(modelo, id.ToString());
        }





        public bool Delete(long id)
        {

            var bd = new BDConnection.BDConnection();


            var modelo = new DependencyModel();

            modelo.DependencyId = id;


            var g = bd.DeletePost<DependencyModel>(modelo, id);



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
