// vim:set ro: -*- buffer-read-only:t -*-
// Warning: This file was procedurally generated from Scripts/MinibufferVersion.cs.template.
/*
  Copyright (c) 2016 Seawisp Hunter, LLC

  Author: Shane Celis
*/
using System.Collections;
using System.Collections.Generic;
using SeawispHunter.MinibufferConsole.Extensions;

namespace SeawispHunter.MinibufferConsole {

/*
  I'd like to just stick this all into an AssemblyInfo.cs file, but that'd
  require me to place Minibuffer into its own shared library. I don't want to do
  that because I want the source code to remain easily accessible for the user.
  */
[Group("minibuffer", tag = "built-in")]
public static class MinibufferVersion {
  public const string product = "Minibuffer";
  public const string description = "A Developer Console for Unity";
  public const string configuration = "Release";
  public const string company = "Seawisp Hunter, LLC";
  public const string title = "SeawispHunter.MinibufferConsole";
  public const string copyright = "Copyright (c) 2016 Seawisp Hunter, LLC";
  public const string trademark = "Minibuffer(tm)";
  public const string culture = "";
  public const string guid = "6A3D0AB4-B838-43CF-84E0-22586EA0D6FA";

  // Version information for an assembly consists of the following four values:
  //
  //      Major Version
  //      Minor Version
  //      Build Number
  //      Revision

  public const string majorMinor = "0.4";
  public const string fileVersion = "0.4.643.3";
  public const string semanticVersion = "0.4.3";
  public const string informational = "v0.4.3-7-g42bba1f";
  public const string versionTimestamp = "2018-05-21T12:23:12EDT";
  public static Dictionary<string, string> metaData = new Dictionary<string, string>() {
    { "git.commit-hash", "42bba1f" }
    };


  // XXX This doesn't seem to work for some reason.
  // There's no telling when this class will be loaded and this will
  // be run.  Moving registration into Minibuffer class.
  // static MinibufferVersion() {
  //   Minibuffer.Register(typeof(MinibufferVersion));
  // }

  [Command("version"/*, subcommand = "minibuffer"*/)]
  public static string ShowVersion([UniversalArgument] bool verbose) {
    if (! verbose)
      return "Minibuffer v{0} {1}".Formatted(semanticVersion, copyright);
    else
      return "Minibuffer v{0} {1} of {2}".Formatted(fileVersion, copyright, versionTimestamp);
  }
}
}
