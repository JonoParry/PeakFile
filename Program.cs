using System;
using System.IO;
using CommandLine;

namespace PeakFile
{
    class Options
    {
            [Option('f', "file", Required = true, HelpText = "Path of file to read.")]
            public string FilePath { get; set; }

            [Option('n', "lines", Required = false, HelpText = "Number of lines to read.")]
            public int NumberOfLines { get; set; }

            [Option('o', "output", Required = false, HelpText = "Path of file to write to.")]
            public string OutputFilePath { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //if (!ValidateArgs(args)) return;

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                        Process(o);
                   })
                   .WithNotParsed<Options>((errs) => {});
        }

        private static void Process(Options options)
        {
            int numberOfLines = 1;

            StreamWriter streamWriter = null;
            if (!string.IsNullOrEmpty(options.OutputFilePath))
            {
                streamWriter = new StreamWriter(options.OutputFilePath);
            }

            using (var fileReader = new FileStream(options.FilePath, FileMode.Open))
            using (var streamReader = new StreamReader(fileReader))
            {
                if (options.NumberOfLines > 1)
                {
                    numberOfLines = options.NumberOfLines;
                }

                for (int i = 0; i < numberOfLines; i++)
                {
                    var line = streamReader.ReadLine();
                    Console.WriteLine(line);
                    if (streamWriter != null)
                    {
                        streamWriter.WriteLine(line);
                    }

                    if (streamReader.EndOfStream)
                    {
                        break;
                    }
                }

                if (streamWriter != null)
                {
                    streamWriter.Dispose();
                }
            }
        }
    }
}
