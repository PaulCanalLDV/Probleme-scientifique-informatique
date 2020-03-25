using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problème_scientifique_informatique
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage image = new MyImage("coco.bmp");
            //image.EffetMiroir(false);
            //image.From_Image_To_File("Sortie.bmp");
            //MyImage imageRot = image.Rotation(300);
            //imageRot.From_Image_To_File("Sortie - rotation.bmp");
            //MyImage imageGris = image.NuancesDeGris();
            //imageGris.From_Image_To_File("Sortie - gris.bmp");
            //MyImage imageNB = image.NoirEtBlanc();
            //imageNB.From_Image_To_File("Sortie - noir et blanc.bmp");
            //MyImage imageReduction = image.Taille(50, false);
            //imageReduction.From_Image_To_File("Sortie - reduction.bmp");
            //image.MatriceDeConvolution("repoussage");
            //image.From_Image_To_File("Sortie.bmp");
            MyImage imageFractale = image.Fractale();
            imageFractale.From_Image_To_File("Sortie - fractale.bmp");
            Console.ReadKey();
        }
    }
}
