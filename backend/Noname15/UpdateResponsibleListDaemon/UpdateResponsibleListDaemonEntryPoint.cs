using System;
using System.Threading;
using CleanCityCore;

namespace UpdateResponsibleListDaemon
{
    class UpdateResponsibleListDaemonEntryPoint
    {
        static void Main(string[] args)
        {
            var responsibleRepository = new ResponsibleRepository();
            var responsibleFounder = new ResponsibleFounder(responsibleRepository);
            Console.Out.WriteLine("UpdateResponsible daemon started!");
            while (true)
            {
                foreach (var responsible in responsibleFounder.GetAllResponsibles())
                {
                    responsibleRepository.AddResponsible(responsible);
                }

                Thread.Sleep(TimeSpan.FromMinutes(10));
            }
        }
    }
}