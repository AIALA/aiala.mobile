using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aiala.mobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FaLabel : Label
	{
		public FaLabel ()
		{
			InitializeComponent ();
		}
	}
}