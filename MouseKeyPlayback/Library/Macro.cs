using System;

namespace GlobalMacroRecorder
{

    /// <summary>
    /// All possible events that macro can record
    /// </summary>
    [Serializable]
    public enum MacroEventType
    {
        MouseMove,
        MouseDown,
        MouseUp,
        MouseWheel,
        KeyDown,
        KeyUp
    }

    /// <summary>
    /// Series of events that can be recorded any played back
    /// </summary>
    [Serializable]
    public class MacroEvent
    {
        
        public MacroEventType MacroEventType;
        public EventArgs EventArgs;
        public int TimeSinceLastEvent;
        /*public List<MacroEvent> events = new List<MacroEvent>();
        public List<MacroEvent> Events
        {
            get { return events; }
            set { events = value; }
        }*/

        public MacroEvent(MacroEventType macroEventType, EventArgs eventArgs, int timeSinceLastEvent)
        {
            MacroEventType = macroEventType;
            EventArgs = eventArgs;
            TimeSinceLastEvent = timeSinceLastEvent;
        }
    }}
