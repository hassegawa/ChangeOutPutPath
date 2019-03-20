using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.BuildEngine;

namespace ChangeOutPutPath.Core
{
    public class Core
    {
        private string _currentBranch;
        private Comun comum;

        public Core()
        {
            comum = new Comun();
        }

        public string CurrentBranch
        {
            get
            {
                return _currentBranch;
            }
        }

        public void changeCsproft(string fullProjectPath, string destino)
        {
            var nameFileLog = string.Format("Log_{0}.txt", DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss"));
            var rollback = new Dictionary<string, string>();

            System.IO.StreamWriter fileTxt = new System.IO.StreamWriter(nameFileLog);

            _currentBranch = comum.GetCurrentBrach(fullProjectPath);

            try
            {
                foreach (string file in Directory.EnumerateFiles(
                    fullProjectPath, "*.csproj", SearchOption.AllDirectories))
                {
                    if (!file.ToUpper().Contains("TESTE") &&
                        !file.ToUpper().Contains("BENNER.SAUDE.BIT") &&
                        (file.ToUpper().Contains("BENNER.SAUDE.") || file.ToUpper().Contains("BENNER.SC.")))
                    {

                        Engine eng = new Engine();
                        Project proj = new Project(eng);
                        proj.Load(file);

                        var original = proj.GetEvaluatedProperty("OutputPath");

                        if (!string.IsNullOrWhiteSpace(original))
                        {

                            proj.SetProperty("OutputPath", destino, " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");
                            proj.Save(file);

                            rollback.Add(file, original);
                            fileTxt.Write(file);
                            fileTxt.Write(string.Format(" : {0} > ", original));
                            fileTxt.WriteLine(destino);
                        }

                    }
                }

                Console.WriteLine("Reverter a alteração [s/n] ?");
                var resposta = Console.ReadKey();

                if (resposta.KeyChar.Equals('s'))
                {
                    Reverter(rollback);
                }

            }
            catch (Exception ex)
            {
                fileTxt.WriteLine(ex.Message);
                Reverter(rollback);

            }
            finally
            {
                fileTxt.Close();
            }
        }

        private static void Reverter(Dictionary<string, string> rollback)
        {
            foreach (var arquivo in rollback)
            {
                Engine eng = new Engine();
                Project proj = new Project(eng);
                proj.Load(arquivo.Key);
                proj.SetProperty("OutputPath", arquivo.Value, " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");
                proj.Save(arquivo.Key);
            }
        }
    }
}
