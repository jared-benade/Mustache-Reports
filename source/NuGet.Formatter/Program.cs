﻿using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NugetFile.Formatter
{
    class Program
    {
        // todo : edit existing package - replace the node_modules with updated ones
        // todo : then reference package so it persist to file system unpacked
        static void Main(string[] args)
        {
            var fileLocation = @"C:\Users\travis\.nuget\packages\mustache.reports.data\1.2.4\mustache.reports.data.nuspec";
            var fileoutput = @"D:\Systems\Mustache-Reports-Example\nuget-packages\.targets-2.txt";
            var data = File.ReadAllText(fileLocation);

            var itemGroupTemplate = "<ItemGroup> <Content Include=\"$(MSBuildThisFileDirectory)..\\..\\content\\{0}\"><CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory><Link>{0}</Link><Visible>false</Visible></Content></ItemGroup>";

            Regex regex = new Regex(@"(?<=\binclude="")[^""]*");
            Match match = regex.Match(data);
            do
            {
                if (match.Success)
                {
                    var value = match.Value;
                    if(value.Contains("/test/") 
                        || value.Contains("/spec/") 
                        || value.Contains("/jasmine")
                        || value.Contains(".travis.yml")
                        || value.ToUpper().Contains("README.MD")
                        || value.ToUpper().Contains("/README")
                        || value.ToUpper().Contains("HISTORY.MD")
                        )
                    {
                        // skip test stuff
                        match = match.NextMatch();
                        continue;
                    }

                    value = value.Replace("any/netcoreapp2.0/", "");
                    value = value.Replace("/", "\\");

                    var output = string.Format(itemGroupTemplate, value);

                    File.AppendAllText(fileoutput, output);
                    File.AppendAllText(fileoutput, Environment.NewLine);
                }
                match = match.NextMatch();
            } while (match.Length > 1);
        }
    }
}
