using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyCheckerContainerGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
          
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("                                                             ");
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.WriteLine("       Dependency Checker Container Generator  ");
            System.Console.BackgroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("                                                             \r\n\r\n");
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.ForegroundColor = ConsoleColor.White;


            var directory = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var user = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).Parent;


            Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Deleting Old Dependency Checker Instances");
            System.Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            var queryDelete = @"docker ps --filter ancestor=dependencycheckerapi:dev  --format ""{{.ID}}""";

            var validate = true;
            while (validate)
            {
                var delete = ExecuteProcess(queryDelete);
                if (delete != "")
                {
                    var query = @"docker stop " + delete;
                    ExecuteProcess(query);
                    query = @"docker rm " + delete;
                    ExecuteProcess(query);
                }
                else
                {
                    validate = false;
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Deleting Old Dependency Checker Instances Finish!");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine();
                }
            }




            Console.Clear();

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("                                                             ");
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.WriteLine("       Dependency Checker Container Generator  ");
            System.Console.BackgroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("                                                             \r\n\r\n");
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.ForegroundColor = ConsoleColor.White;


            Console.WriteLine();
            Console.WriteLine("How many Container Instancies from Dependency Check API do you want to create?");
            var response = Console.ReadLine();

            
            
           var listContainers = new List<ContainerModel>();

            int n;
            bool isNumeric = int.TryParse(response, out n);
            if (isNumeric)
            {

                for (int i = 1; i < n+1; i++)
                {
                    Console.WriteLine();
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Container Nº:" + i);
                    System.Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine();

                    var query = @"docker pull mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 & docker pull mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903";
                   
                    ExecuteProcess(query);

                    query = @"docker build -f """+ directory+@"\DependencyCheckerApi\Dockerfile"" --force-rm -t dependencycheckerapi:dev --target base  --label ""com.microsoft.created-by=visual-studio"" --label ""com.microsoft.visual-studio.project-name=DependencyCheckerApi"" """+directory+"";
                 
                    ExecuteProcess(query);

                    query = @"docker run -dt -v """+user+ @"\onecoremsvsmon\16.5.0102.0:C:\remote_debugger:ro"" -v """ + directory+@"\DependencyCheckerApi:C:\app"" -v """+directory+@":c:\src"" -v """+appdata+@"\Microsoft\UserSecrets:C:\Users\ContainerUser\AppData\Roaming\Microsoft\UserSecrets:ro"" -v """+appdata+@"\ASP.NET\Https:C:\Users\ContainerUser\AppData\Roaming\ASP.NET\Https:ro"" -v """+ user+@"\.nuget\packages\:c:\.nuget\fallbackpackages2"" -v ""C:\Microsoft\Xamarin\NuGet\:c:\.nuget\fallbackpackages"" -v ""c:\program files\dotnet\sdk\NuGetFallbackFolder:c:\.nuget\fallbackpackages3"" -e ""DOTNET_USE_POLLING_FILE_WATCHER=1"" -e ""ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=true"" -e ""ASPNETCORE_ENVIRONMENT=Development"" -e ""ASPNETCORE_URLS=https://+:443;http://+:80"" -e ""NUGET_PACKAGES=c:\.nuget\fallbackpackages2"" -e ""NUGET_FALLBACK_PACKAGES=c:\.nuget\fallbackpackages;c:\.nuget\fallbackpackages2;c:\.nuget\fallbackpackages3"" -P --name DependencyCheckerApi" + i + @" --entrypoint C:\remote_debugger\x64\msvsmon.exe dependencycheckerapi:dev /noauth /anyuser /silent /nostatus /noclrwarn /nosecuritywarn /nofirewallwarn /nowowwarn /fallbackloadremotemanagedpdbs /timeout:2147483646 /LogDebuggeeOutputToStdOut ";
              
                    var containerId = ExecuteProcess(query);
                    var containerObject = new ContainerModel();
                    containerObject.ContainerID = containerId;
                    containerObject.Image = ExecuteProcess(@"docker ps --filter id=" + containerId + @" --format ""{{.Image}}""");
                    containerObject.Created = ExecuteProcess(@"docker ps --filter id=" + containerId + @" --format ""{{.CreatedAt}}""");
                    containerObject.Command = ExecuteProcess(@"docker ps --filter id=" + containerId + @" --format ""{{.Command}}""");
                    containerObject.Names = ExecuteProcess(@"docker ps --filter id=" + containerId + @" --format ""{{.Names}}""");
                    containerObject.Ports = ExecuteProcess(@"docker ps --filter id=" + containerId + @" --format ""{{.Ports}}""");
                    containerObject.Status = ExecuteProcess(@"docker ps --filter id=" + containerId + @" --format ""{{.Status}}""");

                    //0.0.0.0:63794->80 / tcp, 0.0.0.0:63793->443 / tcp

                    containerObject.PortSSL = containerObject.Ports.Replace("/", "").Replace("0.0.0.0:", "").Replace("tcp", "").Replace(" ", "").Replace("->443", "").Replace(" ", "").Replace("->80", "").Split(',')[1];
                    containerObject.Port = containerObject.Ports.Replace("/", "").Replace("0.0.0.0:", "").Replace("tcp", "").Replace(" ", "").Replace("->443", "").Replace("->80", "").Replace(" ", "").Split(',')[0];

                    query = @"docker exec -i -e ASPNETCORE_HTTPS_PORT=""" + containerObject.PortSSL + @""" -w ""C:\app"" "+containerObject.ContainerID+@" ""C:\Program Files\dotnet\dotnet.exe"" --additionalProbingPath c:\\.nuget\\fallbackpackages2 --additionalProbingPath c:\\.nuget\\fallbackpackages --additionalProbingPath c:\\.nuget\\fallbackpackages3  ""c:\app\bin\Debug\netcoreapp3.1\DependencyCheckerApi.dll""";

                    ExecuteProcessAsync(query);

                    Thread.Sleep(5000);
                    listContainers.Add(containerObject);

                    Console.WriteLine();
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Container Nº:" + i +" Creation Finished");
                    System.Console.ForegroundColor = ConsoleColor.White;
                    

                }


                Console.Clear();


                System.Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Dependency Checker API Endpoints");
                System.Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

                var countContainer = 1;

                foreach (var container in listContainers)
                {
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Container Nº " + countContainer);
                    System.Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();

                    Console.Write("Swagger: ");
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("https://localhost:" + container.PortSSL + "/swagger/index.html");
                    System.Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();

                    Console.Write("GetDependencyFromFile: ");
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("https://localhost:" + container.PortSSL + "/FileDependency/GetDependencyFromFile?fileName=random.cpp");
                    System.Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine();
                    Console.Write("GetFileFromDependency: ");

                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("https://localhost:" + container.PortSSL + "/FileDependency/GetFileFromDependency?dependencyName=stddef.h");
                    System.Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine();
                    countContainer++;
                }

                Console.WriteLine();
                Console.WriteLine("Press Enter to Finish all Instances");
                Console.ReadLine();

                Console.Clear();
                Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Deleting Instances");
                System.Console.ForegroundColor = ConsoleColor.White;
                foreach (var container in listContainers)
                {
                    var query = @"docker stop " + container.ContainerID;
                    ExecuteProcess(query);
                    query = @"docker rm " + container.ContainerID;
                    ExecuteProcess(query);
                }

                Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("All Instances were deleted");
                System.Console.ForegroundColor = ConsoleColor.White;

                Console.ReadLine();

                Environment.Exit(0);

            }
            else
            {
                Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error! '"+ response + "' is not a number" );
                Console.ReadLine();

                Environment.Exit(0);
            }
        }

        static Task<int> ExecuteProcessAsync(string query)
        {
            var tcs = new TaskCompletionSource<int>();

            var process = new Process
            {
                StartInfo = { FileName = "cmd.exe", Arguments = @"/C " + query },
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
        static public string ExecuteProcess(string query)
        {
            Console.WriteLine();
            Console.WriteLine(query);
            Console.WriteLine();

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/C " + query;
            process.StartInfo = startInfo;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            string line= @"";
            
            while (!process.StandardOutput.EndOfStream)
            {
                line = process.StandardOutput.ReadLine();
                // do something with line
            }
            if (process.WaitForExit(5000))
            {
                process.Kill();
            }
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(line);
            System.Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();
            return line;
        }

    }
}
