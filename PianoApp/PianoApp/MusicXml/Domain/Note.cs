using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace MusicXml.Domain
{
	public class Note
	{
		internal Note()
		{
			Type = string.Empty;
			Duration = -1;
			Voice = -1;
			Staff = -1;
			IsChordTone = false;
		}

	    public bool Duplicate { get; set; } = false;

	    public int MeasureNumber { get; set; }

	    public float XPos { get; internal set; }

	    public string Type { get; internal set; }
		
		public int Voice { get; internal set; }

		public int Duration { get; internal set; }

		public Lyric Lyric { get; internal set; }
		
		public Pitch Pitch { get; internal set; }

		public int Staff { get; internal set; }

		public bool IsChordTone { get; internal set; }

		public bool IsRest { get; internal set; }
		
        public string Accidental { get; internal set; }

	    public bool Active { get; set; } = false;

	    public NoteState State { get; set; } = NoteState.Idle;

	    public Ellipse ell = new Ellipse();

        public Ellipse GetNote()
	    {
	        Color();
	        ell.Stroke = Brushes.Black;
	        ell.Width = 15;
	        ell.Height = ell.Width;
	        return ell;
	    }

        public void setIdle()
        {
            State = NoteState.Idle;
        }

        public void Color()
	    {
	        if (State.Equals(NoteState.Active))
	        {
	            ell.Fill = System.Windows.Media.Brushes.Blue;
	        }
	        else if(State.Equals(NoteState.Wrong))
	        {
	            ell.Fill = System.Windows.Media.Brushes.Red;
	        }
            else if (State.Equals(NoteState.Good))
            {
                ell.Fill = System.Windows.Media.Brushes.Green;
            }
	        else
	        {
	            ell.Fill = System.Windows.Media.Brushes.Black;
	        }
        }
    }

    public enum NoteState
    {
        Active,
        Idle,
        Wrong,
        Good
    }
}
