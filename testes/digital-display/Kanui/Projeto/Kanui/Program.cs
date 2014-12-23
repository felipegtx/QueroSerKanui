using Kanui.DI;
using Kanui.IO;
using Kanui.IO.Abstractions;
using Kanui.Parsers;
using System;
using System.Linq;

namespace Kanui
{
    class Program
    {
        private const string HELP_PARAMETER_NAME = "socorro";

        static void Main(string[] args)
        {
            try
            {
                /// We first setup our dependecies...
                var serializer = new Serializer();
                var fsController = new FSController();
                var logOutput = new LogOutput();
                InstanceResolverFor<ISerializer>.InstanceBuilder = () => serializer;
                InstanceResolverFor<ILogOutput>.InstanceBuilder = () => logOutput;
                InstanceResolverFor<IFSController>.InstanceBuilder = () => fsController;

#if Debug
            //args = new string[] { @"t>C:\Users\Felipe\Desktop\treinamento.txt" };
            //args = new string[] { @"i>C:\Users\Felipe\Desktop\data.txt" };
#endif

                if (args == null) { throw new Exception(string.Format("O programa não executou nenhum tarefa. Para ajuda, execute-o com o parâmetro '{0}'.", HELP_PARAMETER_NAME)); }
                if (args.Length != 1)
                {
                    throw new Exception(string.Format("Parâmetros de execução inválidos. Para ajuda, execute-o com o parâmetro '{0}'.", HELP_PARAMETER_NAME));
                }
                else
                {
                    /// Then we can rock on!
                    switch (args[0])
                    {
                        case HELP_PARAMETER_NAME:
                            ShowHelpText();
                            break;
                        default:
                            foreach (var result in DataParserResult.LoadUsing(CommandParser.Parse(args[0])).Result)
                            {
                                Console.WriteLine(result);
                            }
                            break;
                    }

                }
            }
            catch (Exception e) { Console.WriteLine(e); }

            Console.WriteLine();
            Console.WriteLine("Fim!");
            Console.ReadLine();
        }

        private static void ShowHelpText()
        {
            Console.WriteLine(
                string.Format("O programa aceita sempre apenas um parâmetro de entrada e possui dois modos de execução: Treinamento e identificação.{0}{0}" +
                                "MODO TREINAMENTO - prefixo t:{0}{0}" +
                                " -   O programa interpretará o parâmetro como sendo um arquivo de texto que será utilizado para cálculo do hash de identificação" +
                                "     dos dígitos. Como resultado o programa atualizará o arquivo de indices de caracteres que o acompanha (map.kanui) e passará a considerar" +
                                "     este novo formato em futuras execuções.{0}{0}" +
                                "" +
                                "     IMPORTANTE: Colisões e erros de cálculo serão identificados no output como '/!\\erro de formato/!\' e NÃO atualizarão o arquivo de indexação.{0}{0}" +
                                "" +
                                "     EXEMPLO: Kanui.exe t>C:\\MeuArquivo.txt" +
                                "{0}" +
                                "{0}" +
                                "{0}" +
                                "MODO IDENTIFICAÇAO - prefixo i:{0}{0}" +
                                "-   Com base no arquivo de indices, o programa tentará identificar os caracteres existentes no arquivo seguindo o conhecimento obtido em treinamentos" +
                                "    prévios. Erros de parse serão exibidos com a mensagem: '/!\\erro de formato/!\'{0}{0}" +
                                "" +
                                "    EXEMPLO: Kanui.exe i>C:\\MeuArquivo.txt" +
                 "              {0}",
                Environment.NewLine));
        }
    }
}
