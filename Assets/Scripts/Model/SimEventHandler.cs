using SimCapi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages event log. These are forwarded to the Capi controller.
/// </summary>
public class SimEventHandler : Singleton<SimEventHandler> {

    // Placeholder count. Events will be created and numbered from 0 to [Count-1].  
    // Needed for AELP platform interactions. 
    private const int PLACEHOLDER_COUNT = 10;
    private int nextEvent = 0;

    // Stores Capi event types and details.
    private List<SimCapiEnum<SimEvent>> typeList = new List<SimCapiEnum<SimEvent>>();
    private List<SimCapiStringArray> detailList = new List<SimCapiStringArray>();

    /// <summary>
    /// Inits CapiEventHandler.
    /// </summary>
    public void Init()
    {
        // Creates initial placeholders.
        for (int i = 0; i < PLACEHOLDER_COUNT; i++)
        {
            createEvent();
        }

    }

    /// <summary>
    /// Clears the current Log. Removes exposed events past the placeholder count.
    /// </summary>
    public void Clear()
    {
        nextEvent = 0;

        // Clears buffer values.
        for (int i = 0; i < PLACEHOLDER_COUNT; i++)
        {
            typeList[i].setValue(SimEvent.None);
            detailList[i].getList().Clear();
            detailList[i].updateValue();
        }

        // Removes any additional values outside the placeholders.
        while (typeList.Count >= PLACEHOLDER_COUNT)
        {
            typeList[PLACEHOLDER_COUNT].unexpose();
            typeList.RemoveAt(PLACEHOLDER_COUNT);
            detailList[PLACEHOLDER_COUNT].unexpose();
            detailList.RemoveAt(PLACEHOLDER_COUNT);
        }
    }

    // Logs the event.  Will create a new one if th

    public void Log(SimEvent type, params string[] details)
    {
        // Creates a new event if all the placeholders are used.
        while (nextEvent >= typeList.Count)
        {
            createEvent();
        }

        // Updates Type and details.
        typeList[nextEvent].setValue(type);
        detailList[nextEvent].getList().AddRange(details);
        detailList[nextEvent].updateValue();

        nextEvent++;
    }

    /// <summary>
    /// Creates and exposes an event. 
    /// </summary>
    /// <param name="i"></param>
    private void createEvent()
    {
        int id = typeList.Count;
        SimCapiEnum<SimEvent> type = new SimCapiEnum<SimEvent>(SimEvent.None);
        type.expose("Event." + id + ".Type", false, false);
        typeList.Add(type);

        SimCapiStringArray detail = new SimCapiStringArray();
        detail.expose("Event." + id + ".Details", false, false);
        detailList.Add(detail);
    }

}
