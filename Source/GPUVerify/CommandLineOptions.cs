﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace GPUVerify
{

    class CommandLineOptions
    {

        public static List<string> inputFiles = new List<string>();

        public static string outputFile = null;

        public static bool OnlyDivergence = false;
        public static bool FullAbstraction;
        public static bool Inference;
        public static string invariantsFile = null;
        public static bool DividedArray = false;
        public static string ArrayToCheck = null;
        public static bool DividedAccesses = false;
        public static bool Eager = false;

        public static bool Symmetry = false;
        public static bool SetEncoding = false;

        public static int Parse(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                bool hasColonArgument = false;
                string beforeColon;
                string afterColon = null;
                int colonIndex = args[i].IndexOf(':');
                if (colonIndex >= 0 && (args[i].StartsWith("-") || args[i].StartsWith("/"))) {
                    hasColonArgument = true;
                    beforeColon = args[i].Substring(0, colonIndex);
                    afterColon = args[i].Substring(colonIndex + 1);
                } else {
                    beforeColon = args[i];
                }

                switch (beforeColon)
                {
                    case "-help":
                    case "/help":
                    case "-?":
                    case "/?":
                    return -1;
                    
                    case "-print":
                    case "/print":
                        if (!hasColonArgument)
                        {
                            Console.WriteLine("Error: filename expected after " + beforeColon + " argument");
                            Environment.Exit(1);
                        }
                        Debug.Assert(afterColon != null);
                        outputFile = afterColon;
                    break;

                    case "-onlyDivergence":
                    case "/onlyDivergence":
                    OnlyDivergence = true;

                    break;

                    case "-fullAbstraction":
                    case "/fullAbstraction":
                    FullAbstraction = true;

                    break;

                    case "-dividedArray":
                    case "/dividedArray":
                    if (hasColonArgument)
                    {
                        ArrayToCheck = afterColon;
                    }
                    DividedArray = true;

                    break;

                    case "-dividedAccesses":
                    case "/dividedAccesses":
                    DividedAccesses = true;

                    break;

                    case "-divided":
                    case "/divided":
                    DividedAccesses = true;
                    DividedArray = true;
                    break;

                    case "-symmetry":
                    case "/symmetry":
                    Symmetry = true;
                    break;

                    case "-setEncoding":
                    case "/setEncoding":
                    SetEncoding = true;
                    break;

                    case "-eager":
                    case "/eager":
                    Eager = true;
                    break;

                    case "-inference":
                    case "/inference":
                    Inference = true;
                    if (hasColonArgument)
                    {
                        Debug.Assert(afterColon != null);
                        invariantsFile = afterColon;
                    }

                    break;

                    default:
                        inputFiles.Add(args[i]);
                        break;
                }

                if (OnlyDivergence)
                {
                    DividedArray = false;
                    DividedAccesses = false;
                }
            }
            return 0;
        }

        private static bool printedHelp = false;

        public static void Usage()
        {
            // Ensure that we only print the help message once
            if (printedHelp)
            {
                return;
            }
            printedHelp = true;

            Console.WriteLine(@"GPUVerify: usage:  GPUVerify [ option ... ] [ filename ... ]
  where <option> is one of

  /help            : this message
  /fullAbstraction : apply full state abstraction
  /onlyDivergence  : only check for divergence-freedom, not race-freedom
  /symmetry        : apply symmetry breaking
  /eager           : check races eagerly, rather than waiting for barrier
  /inference:file  : use automatic invariant inference.  Optional file can include manually supplied candidates
  /raceCheckingContract : try to infer race-freedom contracts for procedures
  /setEncoding     : check races using set encoding
  /divided         : check individual pairs of possibly racing statements separately
  /dividedArray    : check races on arrays one at a time
  /dividedElement  : ???

");
        }


    }
}