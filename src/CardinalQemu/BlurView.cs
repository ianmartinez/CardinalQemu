using System;
using System.Collections.Generic;
using System.Linq;
using Eto;
using Eto.Forms;

namespace CardinalQemu
{
	[Handler(typeof(IBlurView))]
	public class BlurView : Panel
	{
        public interface IBlurView : IHandler
        {
            new Control Content { get; set; }
        }
	}
}
