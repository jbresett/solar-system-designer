using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger {
    public delegate string[] ProcessDelegate(string[] arguments);
    public delegate void OutputDelegate(string[] lines);

    // Event Handles for Process and Output events.
    static private event ProcessDelegate ProcessEvent;
    static private event OutputDelegate OutputEvent;

    /// <summary>
    /// Sends a command line to the debug console.
    /// Command is parsed into arguments, sent to any processing delegates, 
    /// and then any results are sent to the output delegates to display/log/etc.
    /// </summary>
    /// <param name="commandLine"></param>
    static public void send(string commandLine)
    {
        // Split commandLine into arguments.
        string[] arguments = commandLine.Split(' ');

        // List is built from any Processors that return a 1+ length array.
        List<string> Results = new List<string>();
        
        // Runs each delegate in turn, sending the arguments and adding any
        // response to the Results.
        foreach (ProcessDelegate del in ProcessEvent.GetInvocationList())
        {
            foreach(string line in del(arguments))
            {
                Results.Add(line);
            }
        }

        // Send the Results to each of the output delegates.
        OutputEvent(Results.ToArray());
    }

    /// <summary>
    /// Adds a Command Processor to the debug. Every processor will be called
    /// when Debug.send(line) is sent, and the combined outputs of any
    /// results will be sent to the Output Delegates.
    /// </summary>
    /// <param name="pdelegate">string[] FunctionName(string[] agruments)</param>
    static public void AddProcessor(ProcessDelegate pdelegate)
    {
        ProcessEvent += new ProcessDelegate(pdelegate);
    }

    /// <summary>
    /// Adds an Output Delegate to the debug. After all processes are complete,
    /// the output will be sent to the output delegates.
    /// </summary>
    /// <param name="odelegate"></param>
    static public void AddOutput(OutputDelegate odelegate)
    {
        OutputEvent += new OutputDelegate(odelegate);
    }

    /// <summary>
    /// Logs a message in the debugger, sending the results to the system logger.
    /// </summary>
    /// <param name="msg"></param>
    static public void log(string msg)
    {
        // Log in System Default as well.
        Debug.Log(msg);
        if (OutputEvent != null) { 
            OutputEvent(new string[] { msg } );
        }
    }
   
}
