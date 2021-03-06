using System.Collections.Generic;

namespace MusicXml.Domain
{
	public class Measure
	{
		internal Measure()
		{
			Width = -1;
            NewSystem = false;
			MeasureElements = new List<MeasureElement>();
		}

	    public int Number { get; set; }

		public decimal Width { get; internal set; }

        public bool NewSystem { get; set; }
		
		// This can be any musicXML element in the measure tag, i.e. note, backup, etc
		public List<MeasureElement> MeasureElements { get; internal set; }
		
		public MeasureAttributes Attributes { get; internal set; }
	}
}
