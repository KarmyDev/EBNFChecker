using System.Linq;
using System.IO;
using System;
using Eto.Parse.Grammars;

namespace EBNFChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Usage: ebnfcheck [GRAMMAR] [WORKSPACE]\n\nParsing...\n\n");
			try {
			// Find correct EbnfStyle!!! + Regroup grammar to go from top to down!!
			var grammar = new EbnfGrammar(EbnfStyle.Iso14977).Build(File.ReadAllText(args[0]), "startSequence");
			
			var grammarMatch = grammar.Match(File.ReadAllText(args[1]));
			if (string.IsNullOrEmpty(grammarMatch.ToString()))
			Console.WriteLine("\u001b[31m[X] Syntax error in ur source.by lol");
			else Console.WriteLine("\u001b[32;1m[O] Match is correct\n");
			
			Console.WriteLine("\nMatch Data:\n" + "\u001b[33;1m" + grammarMatch.ToString() + "\u001b[0m" + "\u001b[0m");
			
			Console.WriteLine("\nGetError: " + grammar.GetErrorMessage());
			Console.WriteLine("\n Success: " + grammarMatch.Success);
			Console.WriteLine("\n Child Error Index: " + grammarMatch.ChildErrorIndex);
			Console.WriteLine(" Child Error Line: " + grammarMatch.ChildErrorLine + "\n");
			
			int lineNr = 0;
			int indexNr = 0;
			foreach(string line in File.ReadAllLines(args[1]))
			{
				string printData = "\u001b[40m\u001b[37;1m";
				string[] secondData = new string[3];
				
				foreach (char c in line + " ")
				{
					if (indexNr == grammarMatch.ChildErrorIndex) printData += "\u001b[41m";
					else 
					{
						secondData[0] += c == '\t' ? "\t" : " ";
						secondData[1] += c == '\t' ? "\t" : " ";
						secondData[2] += c == '\t' ? "\t" : " ";
					}
					
					printData += c;
					
					if (indexNr == grammarMatch.ChildErrorIndex) 
					{
						printData += "\u001b[40m";
						secondData[0] += "\u001b[41m\u001b[37;1m↑\u001b[0m";
						secondData[1] += "\u001b[41m\u001b[37;1m|\u001b[0m";
						secondData[2] += "\u001b[41m\u001b[37;1m[HERE]\u001b[0m";
					}
					indexNr++;
				}
				if (lineNr == grammarMatch.ChildErrorLine - 1)
				{
					Console.WriteLine(printData + "\u001b[0m");
					Console.WriteLine($"{secondData[0]}\n{secondData[1]}\n{secondData[2]}");
					Console.WriteLine();
				}
					
				lineNr++;
			}
			
			Console.WriteLine("\n ErrorMsg: " + grammarMatch.ErrorMessage);
			
			}
			catch (Exception e) { Console.WriteLine("[[!]EXCEPTION]: " + e); }
		}
    }
}
