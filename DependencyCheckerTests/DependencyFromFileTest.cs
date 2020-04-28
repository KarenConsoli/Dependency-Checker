using DependencyCheckerApi.Controllers;
using DependencyCheckerApi.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace DependencyCheckerTests
{
    [TestClass]
    public class DependencyFromFileTest
    {
        [TestMethod]
        public void GetDependencyFromFile_random()
        {
            var expected = @"[{""Result"":""Dependency Count: 5 - FileName: random.cpp"",""FileName"":null,""DependencyName"":null},{""Result"":""1"",""FileName"":""random.cpp"",""DependencyName"":""boost/random/random_device.hpp""},{""Result"":""2"",""FileName"":""random.cpp"",""DependencyName"":""cassert""},{""Result"":""3"",""FileName"":""random.cpp"",""DependencyName"":""cstdlib""},{""Result"":""4"",""FileName"":""random.cpp"",""DependencyName"":""limits""},{""Result"":""5"",""FileName"":""random.cpp"",""DependencyName"":""random""}]";


           var answer = new FileDependencyController().GetDependencyFromFile("random.cpp");

            Assert.AreEqual(expected, answer, "Random.Cpp Correct Dependencies");
        }

        [TestMethod]
        public void GetDependencyFromFile_randem()
        {
            var expected = @"[{""Result"":""The File Does Not Exist"",""FileName"":null,""DependencyName"":null}]";


            var answer = new FileDependencyController().GetDependencyFromFile("randem.cpp");

            Assert.AreEqual(expected, answer, "Randem.Cpp Does NOT Exist");
        }

        [TestMethod]
        public void GetDependencyFromFile_gui()
        {
            var expected = @"[{""Result"":""Dependency Count: 1 - FileName: gui/gui.cpp"",""FileName"":null,""DependencyName"":null},{""Result"":""1"",""FileName"":""gui/gui.cpp"",""DependencyName"":""The file has NO Dependencies""}]";


            var answer = new FileDependencyController().GetDependencyFromFile("gui/gui.cpp");

             Assert.AreEqual(expected, answer, "gui/gui.cpp has NO Dependencies");
        }



    }
}
