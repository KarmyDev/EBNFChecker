using System.Reflection.Metadata;
using System.Collections.Generic;
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
            Console.WriteLine("Usage: ebnfcheck [GRAMMAR] [WORKSPACE] [START SEQUENCE KEYWORD]\n\nParsing...\n\n");
			
			if (args.Length < 3) {
				Console.WriteLine("\u001b[31mError: Didn't provided any [START SEQUENCE KEYWORD]\u001b[0m\n");
				return;
			}
			
			try {
			// Find correct EbnfStyle!!! + Regroup grammar to go from top to down!!
			var grammar = new EbnfGrammar(EbnfStyle.Iso14977).Build(File.ReadAllText(args[0]), args[2]);
			
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
						secondData[0] += $"\u001b[41m\u001b[37;1m│\u001b[0m";
						secondData[1] += $"\u001b[41m\u001b[37;1m│\u001b[0m";
						secondData[2] += $"\u001b[41m\u001b[37;1m╰─[HERE]\u001b[0m";
					}
					indexNr++;
				}
				if (lineNr == grammarMatch.ChildErrorLine - 1)
				{
					Console.WriteLine($"\u001b[41m{grammarMatch.ChildErrorLine}\u001b[0m" + printData + "\u001b[0m");
					Console.WriteLine($"\u001b[40m{grammarMatch.ChildErrorLine+1}\u001b[0m{secondData[0]}\n\u001b[40m{grammarMatch.ChildErrorLine+2}\u001b[0m{secondData[1]}\n\u001b[40m{grammarMatch.ChildErrorLine+3}\u001b[0m{secondData[2]}");
					Console.WriteLine();
				}
					
				lineNr++;
			}
			
			if (grammarMatch.Success)
			{
				Console.WriteLine("\n--- BEGIN READING CHILDREN ---\n");
				// int intent = 0;
				/*
				void OutputChild(IEnumerable<Eto.Parse.Match> childs)
				{
					intent++;
					foreach(var child in childs)
					{
						string[] forbidden = new string[] {"does", "end", "class", "load", "function", "bind", "takes", "with", "set", "to", "of", "at", "from"};
						bool color = forbidden.Contains(child.StringValue);
						Console.WriteLine(new String('-', intent) + $" {(color ? "\u001b[32;1m" : "")}" + child.StringValue + $" {(color ? "\u001b[0m" : "")}");
						OutputChild(child.Matches);
					}
				}
				
				OutputChild(grammarMatch.Matches);
				*/
				
				int counter = 0;
				
				void PrintMatch(Eto.Parse.Match m, string p, int indent = 0) 
				{
    				bool isRedundent = m.StringValue == p;
	
					if (!isRedundent) {
						counter++;
        				string indent_s = new string(' ', indent * 2);
        				Console.WriteLine($"{indent_s} ╰── {(counter % 2 == 0 ? "\u001b[34;1m" : "")}{m.Name}{(counter % 2 == 0 ? "\u001b[0m" : "")} ── \u001b[32;1m{m.StringValue}\u001b[0m");
   					}
	
					int new_indent = indent + (isRedundent ? 0 : 1);
    				foreach (var child in m.Matches) PrintMatch(child, m.StringValue, new_indent);
				}
				
				foreach (var child in grammarMatch.Matches)
				{
					PrintMatch(child, string.Empty);
				}
				
				/*
				void OutputChild(Eto.Parse.Match match, int indent = 0) 
				{
       				string indent_s = new string('.', indent * 2);
        			Console.WriteLine($"{indent_s}| {match.Name} \u001b[32;1m: {match.StringValue}\u001b[0m");
        			foreach (var child in match.Matches) 
					// if (match.StringValue != child.StringValue) / OutputChild(child, indent+1);
    			}
				*/
				
				
				Console.WriteLine("\n--- END READING CHILDREN ---\n");
				}
				
				Console.WriteLine("\n ErrorMsg: " + grammarMatch.ErrorMessage);
				
				}
				catch (Exception e) { Console.WriteLine("[[!]EXCEPTION]: " + e); }
		}	
    }	
}
