using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Problème_scientifique_informatique
{
    class MyImage
    {
		//Déclaration des champs et propriétés de classe
		#region
		public static int Convertir_Endian_To_Int(byte[] tab, int debut, int fin)
		{
			int res = 0;
			for (int i = debut; i <= fin; i++)
			{
				res += Convert.ToInt32(tab[i] * Math.Pow(256, i - debut));
			}
			return res;
		}
		public static byte[] Convertir_Int_To_Endian(int val, int tailleOctets)
		{
			byte[] res = new byte[tailleOctets];
			int val1 = val;
			int val2;
			for (int i = 0; i < tailleOctets; i++)
			{
				val2 = Convert.ToInt32(val1 % Math.Pow(256, 1));
				val1 /= Convert.ToInt32(Math.Pow(256, 1));
				res[i] = Convert.ToByte(val2);
			}
			return res;
		}
		public static void AfficherMatricePixel(Pixel[,] matricePixel)
		{
			for (int i = 0; i < matricePixel.GetLength(0); i++)
			{
				for (int j = 0; j < matricePixel.GetLength(1); j++)
				{
					Console.Write(matricePixel[i, j].R + " " + matricePixel[i, j].G + " " + matricePixel[i, j].B);
					Console.Write("  ");
				}
				Console.WriteLine();
			}
		}
		public static Pixel[,] CopieMatricePixel (Pixel[,] matriceCopiée)
		{
			Pixel[,] matriceCopie = new Pixel[matriceCopiée.GetLength(0), matriceCopiée.GetLength(1)];
			for (int i = 0; i < matriceCopiée.GetLength(0); i++)
			{
				for (int j = 0; j < matriceCopiée.GetLength(1); j++)
				{
					matriceCopie[i, j] = new Pixel(matriceCopiée[i, j]);
				}
			}
			return matriceCopie;
		}
		#endregion

		//Déclaration des champs
		#region
		protected string typeImage;
		protected int tailleFichier = 0;
		protected int tailleOffset = 0;
		protected int largeur = 0;
		protected int hauteur = 0;
		protected int nbBitsCouleur = 0;
		protected Pixel[,] matriceRGB;
		#endregion

		//Déclaration des constructeurs
		#region
		public MyImage() { } //Constructeur par defaut
		public MyImage(string myFile)
		{
			string[] fichier = myFile.Split('.');
			byte[] myfile = File.ReadAllBytes(myFile);
			UTF8Encoding utf8 = new UTF8Encoding();
			switch (fichier[fichier.Length - 1])
			{
				case "bmp":

					//Métadonnes du fichier
					typeImage = utf8.GetString(myfile, 0, 2);
					tailleFichier = Convertir_Endian_To_Int(myfile, 2, 3);
					tailleOffset = Convertir_Endian_To_Int(myfile, 10, 13);

					//Métadonnées de l'image
					largeur = Convertir_Endian_To_Int(myfile, 18, 21);
					hauteur = Convertir_Endian_To_Int(myfile, 22, 25);
					nbBitsCouleur = Convertir_Endian_To_Int(myfile, 28, 29);

					//L'image elle-même
					matriceRGB = new Pixel[hauteur, largeur];
					for (int i = tailleOffset; i < myfile.Length; i += 3)
					{
						int k = i - tailleOffset;
						k /= 3;
						int j = k / largeur;
						k %= largeur;
						matriceRGB[j, k] = new Pixel(myfile[i + 2], myfile[i + 1], myfile[i]);
					}
					//AfficherMatricePixel(matriceRGB);

					break;

				case "csv":

					break;

				default:
					Console.WriteLine("Format de fichier non reconnu");
					break;
			}
		}
		#endregion

		//Déclaration des propriétés
		#region
		public string TypeImage
		{
			get
			{
				return typeImage;
			}
			set
			{
				typeImage = value;
			}
		}
		public int TailleFichier
		{
			get
			{
				return tailleFichier;
			}
			set
			{
				tailleFichier = value;
			}
		}
		public int TailleOffset
		{
			get
			{
				return tailleOffset;
			}
			set
			{
				tailleOffset = value;
			}
		}
		public int Largeur
		{
			get
			{
				return largeur;
			}
			set
			{
				largeur = value;
			}
		}
		public int Hauteur
		{
			get
			{
				return hauteur;
			}
			set
			{
				hauteur = value;
			}
		}
		public int NbBitsCouleur
		{
			get
			{
				return nbBitsCouleur;
			}
			set
			{
				nbBitsCouleur = value;
			}
		}
		public Pixel[,] MatriceRGB
		{
			get
			{
				return matriceRGB;
			}
			set
			{
				matriceRGB = value;
			}
		}
		#endregion

		//Déclaration des méthodes
		#region
		public void From_Image_To_File(string file)
		{
			List<byte> fichierByte = new List<byte>();
			UTF8Encoding utf8 = new UTF8Encoding();
			for (int i = 0; i < 2; i++)
			{
				fichierByte.Add(utf8.GetBytes(typeImage)[i]);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(tailleFichier, 4)[i]);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(0b0000);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(tailleOffset, 4)[i]);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(tailleOffset - 14 , 4)[i]);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(largeur, 4)[i]);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(hauteur, 4)[i]);
			}
			for (int i = 0; i < 2; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(1, 2)[i]);
			}
			for (int i = 0; i < 2; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(nbBitsCouleur, 2)[i]);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(0b0000);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(Convertir_Int_To_Endian(tailleFichier - tailleOffset, 4)[i]);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(0b0000);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(0b0000);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(0b0000);
			}
			for (int i = 0; i < 4; i++)
			{
				fichierByte.Add(0b0000);
			}
			for (int i = 0; i < matriceRGB.GetLength(0); i++)
			{
				for (int j = 0; j < matriceRGB.GetLength(1); j++)
				{
					fichierByte.Add(Convert.ToByte(matriceRGB[i, j].B));
					fichierByte.Add(Convert.ToByte(matriceRGB[i, j].G));
					fichierByte.Add(Convert.ToByte(matriceRGB[i, j].R));
				}
				for (int k = 1; k <= matriceRGB.GetLength(1) % 4; k++)
				{
					fichierByte.Add(0b0000);
				}
			}
			byte[] tabByte = fichierByte.ToArray();
			File.WriteAllBytes(file, tabByte);
		}
		public void EffetMiroir(bool horizontal)
		{
			Pixel[,] matriceCopie = CopieMatricePixel(matriceRGB);
			switch (horizontal) //pour savoir selon quel axe l'effet doit se faire
			{
				case true:
					for (int i = 0; i < matriceRGB.GetLength(0); i++)
					{
						for (int j = 0; j < matriceRGB.GetLength(1); j++)
						{
							matriceRGB[matriceCopie.GetLength(0) - 1 - i, j] = new Pixel(matriceCopie[i, j]); //les pixels restent sur leurs colonnes respectives mais échangent de lignes
						}
					}
					break;

				case false:
					for (int i = 0; i < matriceRGB.GetLength(0); i++)
					{
						for (int j = 0; j < matriceRGB.GetLength(1); j++)
						{
							matriceRGB[i, matriceCopie.GetLength(1) - 1 - j] = new Pixel(matriceCopie[i, j]); //les pixels restent sur leurs lignes respectives mais échangent de colonnes
						}
					}
					break;
			}
		}
		public MyImage Rotation(double angle)
		{
			MyImage imageCopie = new MyImage();
			imageCopie.typeImage = typeImage;
			imageCopie.tailleFichier = tailleFichier;
			imageCopie.tailleOffset = tailleOffset;
			imageCopie.nbBitsCouleur = nbBitsCouleur;
			if (angle < 90) //calcul des dimensions
			{
				angle *= Math.PI;
				angle /= 180;
				imageCopie.largeur = Convert.ToInt32(largeur * Math.Cos(angle) + hauteur * Math.Sin(angle));
				imageCopie.hauteur = Convert.ToInt32(largeur * Math.Sin(angle) + hauteur * Math.Cos(angle));
			}
			else if (angle < 180)
			{
				angle *= Math.PI;
				angle /= 180;
				imageCopie.largeur = Convert.ToInt32(-largeur * Math.Cos(angle) + hauteur * Math.Sin(angle));
				imageCopie.hauteur = Convert.ToInt32(largeur * Math.Sin(angle) - hauteur * Math.Cos(angle));
			}
			else if (angle < 270)
			{
				angle *= Math.PI;
				angle /= 180;
				imageCopie.largeur = Convert.ToInt32(-largeur * Math.Cos(angle) - hauteur * Math.Sin(angle));
				imageCopie.hauteur = Convert.ToInt32(-largeur * Math.Sin(angle) - hauteur * Math.Cos(angle));
			}
			else
			{
				angle *= Math.PI;
				angle /= 180;
				imageCopie.largeur = Convert.ToInt32(largeur * Math.Cos(angle) - hauteur * Math.Sin(angle));
				imageCopie.hauteur = Convert.ToInt32(-largeur * Math.Sin(angle) + hauteur * Math.Cos(angle));
			}
			imageCopie.matriceRGB = new Pixel[imageCopie.hauteur, imageCopie.largeur];
			for (int i = 0; i < imageCopie.hauteur; i++)
			{
				for (int j = 0; j < imageCopie.largeur; j++)
				{
					int iXY = imageCopie.hauteur / 2 - i; //On calcule les coordonnées de l'image d'arrivée en coordonnées cartésiennes
					int jXY = j - imageCopie.largeur / 2;
					int iOrig = Convert.ToInt32(iXY * Math.Cos(angle) + jXY * Math.Sin(angle)); //On en déduit les coordonnées dans l'image de départ
					int jOrig = Convert.ToInt32(iXY * (-Math.Sin(angle)) + jXY * Math.Cos(angle));
					if (iOrig <= -hauteur/2 || iOrig >= hauteur/2) //Si les coordonnées sont en dehors des dimensions de l'image de départ
					{
						imageCopie.matriceRGB[i, j] = new Pixel(0, 0, 0); //on colorie le pixel en noir
					}
					else if (jOrig <= -largeur/2 || jOrig >= largeur/2)
					{
						imageCopie.matriceRGB[i, j] = new Pixel(0, 0, 0);
					}
					else
					{
						/*int iQuot = Convert.ToInt32(Math.Truncate(iOrig));
						double iRest = iOrig - iQuot;
						int jQuot = Convert.ToInt32(Math.Truncate(jOrig));
						double jRest = jOrig - jQuot;
						if (iQuot == hauteur/2 - 1)
						{

						}*/

						imageCopie.matriceRGB[i, j] = new Pixel(matriceRGB[hauteur/2 - 1 - iOrig, jOrig + largeur/2]); //sinon, on récupère la couleur du pixel de l'image de départ
					}
				}
			}
			return imageCopie;
		}
		public MyImage NuancesDeGris()
		{
			Pixel[,] pixelcopie = CopieMatricePixel(matriceRGB);
			MyImage imageCopie = new MyImage();
			imageCopie.typeImage = typeImage;
			imageCopie.tailleFichier = tailleFichier;
			imageCopie.tailleOffset = tailleOffset;
			imageCopie.nbBitsCouleur = nbBitsCouleur;
			imageCopie.largeur = largeur;
			imageCopie.hauteur = hauteur;
			imageCopie.matriceRGB = new Pixel[imageCopie.hauteur, imageCopie.largeur];
			for (int i = 0; i < matriceRGB.GetLength(0); i++)
			{
				for (int j = 0; j < matriceRGB.GetLength(1); j++)
				{
					int gris = (pixelcopie[i, j].R + pixelcopie[i, j].G + pixelcopie[i, j].B) / 3; //On fait la moyenne des trois
					imageCopie.matriceRGB[i, j] = new Pixel(gris, gris, gris);
				}
			}
			return imageCopie;
		}
		public MyImage NoirEtBlanc()
		{
			Pixel[,] pixelcopie = CopieMatricePixel(matriceRGB);
			MyImage imageCopie = new MyImage();
			imageCopie.typeImage = typeImage;
			imageCopie.tailleFichier = tailleFichier;
			imageCopie.tailleOffset = tailleOffset;
			imageCopie.nbBitsCouleur = nbBitsCouleur;
			imageCopie.largeur = largeur;
			imageCopie.hauteur = hauteur;
			imageCopie.matriceRGB = new Pixel[imageCopie.hauteur, imageCopie.largeur];
			for (int i = 0; i < matriceRGB.GetLength(0); i++)
			{
				for (int j = 0; j < matriceRGB.GetLength(1); j++)
				{
					int nombre = (pixelcopie[i, j].R + pixelcopie[i, j].G + pixelcopie[i, j].B) / 3; //On fait la moyenne des trois
					if (nombre >= 128)
					{
						imageCopie.matriceRGB[i, j] = new Pixel(255, 255, 255); //Si le pixel est 'clair' -> blanc
					}
					else
					{
						imageCopie.matriceRGB[i, j] = new Pixel(0, 0, 0); //Si le pixel est 'sombre' -> noir
					}
				}
			}
			return imageCopie;
		}
		public MyImage Taille(int pourcentage, bool reduction)
		{
			MyImage imageCopie = new MyImage();
			imageCopie.typeImage = typeImage;
			imageCopie.tailleFichier = tailleFichier;
			imageCopie.tailleOffset = tailleOffset;
			imageCopie.nbBitsCouleur = nbBitsCouleur;
			switch (reduction) //calcul des dimensions
			{
				case true:
					{
						imageCopie.largeur = Convert.ToInt32(largeur - (largeur * ((double) pourcentage / 100)));
						imageCopie.hauteur = Convert.ToInt32(hauteur - (hauteur * ((double) pourcentage / 100)));
						break;
					}
				case false:
					{
						imageCopie.largeur = Convert.ToInt32(largeur + (largeur * ((double) pourcentage / 100)));
						imageCopie.hauteur = Convert.ToInt32(hauteur + (hauteur * ((double) pourcentage / 100)));
						break;
					}
			}
			imageCopie.matriceRGB = new Pixel[imageCopie.hauteur, imageCopie.largeur];
			for (int i = 0; i < imageCopie.hauteur; i++)
			{
				for (int j = 0; j < imageCopie.largeur; j++)
				{
					int iOrig = i * hauteur / imageCopie.hauteur; //on calcule les coordonnées du pixel dans l'image de départ
					int jOrig = j * largeur / imageCopie.largeur;
					if (iOrig < 0)
					{
						iOrig = 0;
					}
					if (iOrig >= hauteur)
					{
						iOrig = hauteur - 1;
					}
					if (jOrig < 0)
					{
						jOrig = 0;
					}
					if (jOrig >= largeur)
					{
						jOrig = largeur - 1;
					}
					imageCopie.matriceRGB[i, j] = matriceRGB[iOrig, jOrig];
				}
			}
			return imageCopie;
		}
		public void MatriceDeConvolution(string filtre)
		{
			int[,] matriceConv = new int[0,0];
			switch (filtre)
			{
				case "détection":
					matriceConv = new int[3, 3] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
					break;

				case "renforcement":
					matriceConv = new int[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
					break;

				case "flou":
					matriceConv = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 }, { 0, 1, 1, 1, 0 }, { 0, 1, 1, 1, 0 }, { 0, 0, 0, 0, 0 } };
					break;

				case "repoussage":
					matriceConv = new int[3, 3] { { 2, 1, 0 }, { 1, 1, -1 }, { 0, -1, -2 } };
					break;
			}
			Pixel[,] matriceCopie = CopieMatricePixel(matriceRGB);
			for (int i = 0; i < hauteur; i++)
			{
				for (int j = 0; j < largeur; j++)
				{
					int valeurR = 0;
					int valeurG = 0;
					int valeurB = 0;
					for (int iN = i - matriceConv.GetLength(0) / 2; iN <= i + matriceConv.GetLength(0) / 2; iN++) //On parcourt les 2 lignes voisines ainsi que celle renseignée dans les paramètres
					{
						for (int jN = j - matriceConv.GetLength(1) / 2; jN <= j + matriceConv.GetLength(1) / 2; jN++) //Pareil mais avec les colonnes
						{
							if (iN < 0 || iN >= hauteur) //Si l'index dépasse la matrice vers le haut,
							{

							}
							else
							{
								if (jN < 0 || jN >= largeur) //Si l'index dépasse la matrice vers la gauche,
								{

								}
								else
								{
									valeurR += matriceCopie[iN, jN].R * matriceConv[iN - i + matriceConv.GetLength(0) / 2, jN - j + matriceConv.GetLength(1) / 2];
									valeurG += matriceCopie[iN, jN].G * matriceConv[iN - i + matriceConv.GetLength(0) / 2, jN - j + matriceConv.GetLength(1) / 2];
									valeurB += matriceCopie[iN, jN].B * matriceConv[iN - i + matriceConv.GetLength(0) / 2, jN - j + matriceConv.GetLength(1) / 2];
								}
							}

						}
					}
					if(valeurR > 255)
					{
						valeurR = 255;
					}
					if (valeurR < 0)
					{
						valeurR = 0;
					}
					if (valeurG > 255)
					{
						valeurG = 255;
					}
					if (valeurG < 0)
					{
						valeurG = 0;
					}
					if (valeurB > 255)
					{
						valeurB = 255;
					}
					if (valeurB < 0)
					{
						valeurB = 0;
					}
					matriceRGB[i, j] = new Pixel(valeurR, valeurG, valeurB);
				}
			}
		}
		public MyImage Fractale()
		{
			//Initialisation des variables _ Définition de la zone que l'on dessine

			float x1 = (float) - 2.1;
			float x2 = (float) 0.6;
			float y1 = (float) -1.2;
			float y2 = (float) 1.2;

			int zoom = 100; //100 pixel sur l’image

			int itération_max = 50;

			// Taille de l'image

			float image_x = zoom * (x2 - x1);

			float image_y = zoom * (y2 - y1);

			//Initialisation de l'image

			Pixel[,] pixelfractale = CopieMatricePixel(matriceRGB);

			MyImage imagefractale = new MyImage();

			imagefractale.typeImage = typeImage;

			imagefractale.tailleFichier = tailleFichier;

			imagefractale.tailleOffset = tailleOffset;

			imagefractale.nbBitsCouleur = nbBitsCouleur;

			imagefractale.largeur = Convert.ToInt32(image_x);

			imagefractale.hauteur = Convert.ToInt32(image_y);

			imagefractale.matriceRGB = new Pixel[imagefractale.hauteur, imagefractale.largeur];

			//Boucle conditionelle

			for (int l = 0; l < imagefractale.largeur; l++)

			{

				for (int j = 0; j < imagefractale.hauteur; j++)

				{

					//Initialisation des variables complexes

					float c_r = l / zoom + x1;

					float c_i = j / zoom + y1;

					float z_r = 0;

					float z_i = 0;

					float i = 0;

					while (z_r * z_r + z_i * z_i < 4 && i < itération_max)

					{

						float tmp = z_r;

						z_r = z_r * z_r - z_i * z_i + c_r;

						z_i = 2 * z_i * tmp + c_i;

						i = i + 1;

					}

					if (i == itération_max)

					{
						pixelfractale[l, j] = new Pixel(255, 255, 255);
					}

				}

			}
			return imagefractale;
		}
		#endregion
	}
}
