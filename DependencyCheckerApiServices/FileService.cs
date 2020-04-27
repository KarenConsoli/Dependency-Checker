using AutoMapper;
using DependencyCheckerApiModels;
using System;
using System.Collections.Generic;

namespace FileCheckerApiServices
{
    public class FileService
    {
        public FileModel Read(FileModel modelo)
        {


            var bd = new BDConnection.BDConnection();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<FileModel, FileModel>());
            IMapper iMapper = config.CreateMapper();
            var f = iMapper.Map<object, FileModel>(bd.ReadWhere<FileModel>(modelo));


            return f;


        }


        public List<FileModel> ReadLista(FileModel modelo)
        {

            var bd = new BDConnection.BDConnection();


            var lista = new List<FileModel>();


            var config = new MapperConfiguration(cfg => cfg.CreateMap<FileModel, FileModel>());
            IMapper iMapper = config.CreateMapper();

            try
            {
                foreach (var item in bd.ReadWhereList<FileModel>(modelo))
                {

                    lista.Add(iMapper.Map<object, FileModel>(item));
                }

            }
            catch (Exception e)
            {
                return null;
            }


            return lista;



        }
        public List<FileModel> ReadLike(FileModel modelo)
        {

            var bd = new BDConnection.BDConnection();


            var lista = new List<FileModel>();


            var config = new MapperConfiguration(cfg => cfg.CreateMap<FileModel, FileModel>());
            IMapper iMapper = config.CreateMapper();

            try
            {
                foreach (var item in bd.ReadLike<FileModel>(modelo))
                {

                    lista.Add(iMapper.Map<object, FileModel>(item));
                }

            }
            catch (Exception e)
            {
                return null;
            }


            return lista;



        }

        public List<FileModel> Read()
        {

            var bd = new BDConnection.BDConnection();


            var lista = new List<FileModel>();



            var config = new MapperConfiguration(cfg => cfg.CreateMap<FileModel, FileModel>());
            IMapper iMapper = config.CreateMapper();

            try
            {
                foreach (var item in bd.Read<FileModel>(new FileModel()))
                {

                    lista.Add(iMapper.Map<object, FileModel>(item));
                }

            }
            catch (Exception e)
            {

                return null;
            }



            return lista;

        }


        public bool Add(FileModel modelo)
        {


            var bd = new BDConnection.BDConnection();


            var res = bd.Add<FileModel>(modelo);


            return res;
        }


        public FileModel Read(long? id)
        {

            var bd = new BDConnection.BDConnection();


            var config = new MapperConfiguration(cfg => cfg.CreateMap<FileModel, FileModel>());
            IMapper iMapper = config.CreateMapper();


            if (id != null)
            {
                return iMapper.Map<object, FileModel>(bd.Read<FileModel>(new FileModel(), id.ToString()));

            }

            return null;


        }
        public bool Edit(FileModel modelo, long id)
        {
            var bd = new BDConnection.BDConnection();


            modelo.FileId = id;

            return bd.Edit<FileModel>(modelo, id.ToString());
        }





        public bool Delete(long id)
        {

            var bd = new BDConnection.BDConnection();


            var modelo = new FileModel();

            modelo.FileId = id;


            var g = bd.DeletePost<FileModel>(modelo, id);



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
