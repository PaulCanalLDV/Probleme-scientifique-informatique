using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problème_scientifique_informatique
{
    class Pixel
    {
		//Déclaration des champs
		#region
		protected int r = 0;
		protected int g = 0;
		protected int b = 0;
		#endregion

		//Déclaration des constructeurs
		#region
		public Pixel() { } //Constructeur par defaut
		public Pixel(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
		}
		public Pixel(Pixel pixel) //Constructeur par copie
		{
			r = pixel.r;
			g = pixel.g;
			b = pixel.b;
		}
		#endregion

		//Déclaration des propriétés
		#region
		public int R
		{
			get
			{
				return r;
			}
		}
		public int G
		{
			get
			{
				return g;
			}
		}
		public int B
		{
			get
			{
				return b;
			}
		}
		#endregion
	}
}
