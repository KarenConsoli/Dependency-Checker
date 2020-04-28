using DependencyCheckerApi.Controllers;
using DependencyCheckerApi.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace DependencyCheckerTests
{
    [TestClass]
    public class FileFromDependencyTest
    {
        [TestMethod]
        public void GetFileFromDependency_random()
        {
            var expected = @"[{""Result"":""File Count: 13 - DependencyName: stddef.h"",""FileName"":null,""DependencyName"":null},{""Result"":""1"",""FileName"":""lua/lauxlib.h"",""DependencyName"":""stddef.h""},{""Result"":""2"",""FileName"":""lua/ldebug.cpp"",""DependencyName"":""stddef.h""},{""Result"":""3"",""FileName"":""lua/ldump.cpp"",""DependencyName"":""stddef.h""},{""Result"":""4"",""FileName"":""lua/lfunc.cpp"",""DependencyName"":""stddef.h""},{""Result"":""5"",""FileName"":""lua/llimits.h"",""DependencyName"":""stddef.h""},{""Result"":""6"",""FileName"":""lua/lmem.cpp"",""DependencyName"":""stddef.h""},{""Result"":""7"",""FileName"":""lua/lmem.h"",""DependencyName"":""stddef.h""},{""Result"":""8"",""FileName"":""lua/lopcodes.cpp"",""DependencyName"":""stddef.h""},{""Result"":""9"",""FileName"":""lua/lstate.cpp"",""DependencyName"":""stddef.h""},{""Result"":""10"",""FileName"":""lua/lstrlib.cpp"",""DependencyName"":""stddef.h""},{""Result"":""11"",""FileName"":""lua/ltablib.cpp"",""DependencyName"":""stddef.h""},{""Result"":""12"",""FileName"":""lua/lua.h"",""DependencyName"":""stddef.h""},{""Result"":""13"",""FileName"":""lua/luaconf.h"",""DependencyName"":""stddef.h""}]";


           var answer = new FileDependencyController().GetFileFromDependency("stddef.h");

            Assert.AreEqual(expected, answer, "stddef.h Correct Dependencies");
        }

        [TestMethod]
        public void GetFileFromDependency_randem()
        {
            var expected = @"[{""Result"":""The Dependency Does Not Exist"",""FileName"":null,""DependencyName"":null}]";


            var answer = new FileDependencyController().GetFileFromDependency("asdfg");

            Assert.AreEqual(expected, answer, "asdfg Does NOT Exist");
        }

    


    }
}
