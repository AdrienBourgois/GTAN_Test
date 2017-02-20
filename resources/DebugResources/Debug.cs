﻿using System.Linq;
using System.Reflection;
using GTANetworkServer;

public class Debug : Script
{
    public Debug()
    {
        API.onResourceStart += OnResourceStartHandler;
    }

    public void OnResourceStartHandler()
    {
        API.consoleOutput("[DebugResources] Debug Resources Ready !");
        ShowResourcesList();
    }

    public void ShowResourcesList()
    {
        API.consoleOutput("------------ RESOURCES ------------\n");

        string[] resourcesNames = API.getAllResources();

        foreach (string resource in resourcesNames)
        {
            if (API.isResourceRunning(resource))
            {
                string resourceName = API.getResourceName(resource);
                ResourceType resourceType = API.getResourceType(resource);
                string resourceAuthor = API.getResourceAuthor(resource);
                string resourceVersion = API.getResourceVersion(resource);
                string resourceDescription = API.getResourceDescription(resource);
                CommandInfo[] resourceCommand = API.getResourceCommandInfos(resource);
                string name = API.getResourceName(resource);

                string resourceString = "[" + resourceType + "] " + resourceName;
                if (resourceAuthor != "") resourceString += (" - By : " + resourceAuthor);
                if (resourceVersion != "") resourceString += (" (Version " + resourceVersion + ")");
                API.consoleOutput(resourceString);

                if (resourceDescription != "")
                    API.consoleOutput("\t- Description : " + resourceDescription);
                if (resourceCommand.Length > 0)
                {
                    API.consoleOutput("\t- Commands : " + resourceDescription);
                    foreach (CommandInfo command in resourceCommand)
                    {
                        string commandSyntax = "";
                        foreach (ParameterInfo parameter in command.Parameters)
                            commandSyntax += parameter.Name + ", ";
                        commandSyntax.Remove(commandSyntax.Length - 2);
                        API.consoleOutput("\t\t * " + command.Command + "(" + commandSyntax + ")");
                    }
                }

                API.consoleOutput("");
            }
        }

        API.consoleOutput("------------ RESOURCES ------------\n");
    }
};