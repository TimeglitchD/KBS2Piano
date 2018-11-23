using System.Collections.Generic;

namespace MusicXml.Domain
{
	public class Score
	{
		internal Score()
		{
			Parts = new List<Part>();
			MovementTitle = string.Empty;
		}

		public string MovementTitle { get; internal set; }

		public Identification Identification { get; internal set; }

		public List<Part> Parts { get; internal set; }

        public int systems { get; internal set; }

        public double scale { get; internal set; }
	}
}
