using System.ServiceProcess;

namespace autoCloseMyPc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
