using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string pathLaundry = @"C:\Users\Loritzu\source\repos\ConsoleApp1\ConsoleApp1\Stream-uri si serializare\Streams.txt";
            int poz = 0;
            int lung = 0;
            using(StreamWriter logHeader = new StreamWriter(pathLaundry))
            {
                string textHeader = "|Tip Articol      " + "|" + "Material      " + "|" + "Culoare    " + "|" 
                    + "Bucăți   "+ "|" + "Operațiuni prestate     " + "|" + "Dată/Oră primire comandă   " + "|" + "Dată/Oră livrare comandă    ";
                logHeader.WriteLine(textHeader);
                logHeader.WriteLine();                
            }

            using (FileStream stream = new FileStream(pathLaundry, FileMode.Append, FileAccess.Write))
            {
                LaundryShop myLaundry = new LaundryShop("Shirt", "Cotton", "White", 4, "Laundry",
                                                        new DateTime(2022,2,21,9,45,0), new DateTime(2022, 2, 22, 9, 0, 0));
                string orderAsString = myLaundry.ToText();
               
                byte[] data = Encoding.UTF8.GetBytes(orderAsString);

                stream.Write(data, 0, data.Length);                

                lung = (int)data.Length; 

                stream.Close();

                Console.WriteLine("Am scris un obiect LaundryShop in fisierul:" + pathLaundry);
            }

            string pathExtract = @"C:\Users\Loritzu\source\repos\ConsoleApp1\ConsoleApp1\Stream-uri si serializare\Extract.txt";
            try
            {
                if(File.Exists(pathLaundry))
                {
                    using (StreamReader sr = new StreamReader(pathLaundry))
                    {
                        string line = sr.ReadLine();
                        Console.WriteLine("Am copiat capul de tabel:");
                        //de facut decodificarea pentru diacritice
                        Console.WriteLine(line);
                        Console.WriteLine();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
            }
            using (FileStream fs = new FileStream(pathLaundry, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];                
                int fslength = (int)fs.Length;                
                fs.Read(bytes, 0, fslength);
                fs.Close();
                string neededtext = Encoding.UTF8.GetString(bytes);

                poz = (int)(neededtext.IndexOf("~"));
                string textfinal = neededtext.Substring(poz+1);

                LaundryShop mylaundry = LaundryShop.FromFile(textfinal);
                string s = "Am reconstruit din fișier un obiect LaundryShop :";
                byte[] bs = Encoding.UTF8.GetBytes(s);
                string f = Encoding.UTF8.GetString(bs);
                Console.WriteLine(f);
                Console.WriteLine();
                Console.WriteLine("Tip Articol:"+mylaundry.ArticleType);
                Console.WriteLine("Material/Țesătură:"+mylaundry.Fabric);
                Console.WriteLine("Culoare:"+mylaundry.Color);
                Console.WriteLine("Nr. bucați:"+mylaundry.Pieces);
                Console.WriteLine("Serviciul prestat:"+mylaundry.Service);
                Console.WriteLine("Data recepției:"+mylaundry.Receive);
                Console.WriteLine("Data livrării:"+mylaundry.Deliver);
            }

                Console.ReadKey();
        }

        
    }

    public class LaundryShop
    {
        public string ArticleType;
        public string Fabric;
        public string Color;
        public int Pieces;
        public string Service;
        public DateTime Receive;
        public DateTime Deliver;

        public LaundryShop(string articleType, string fabric, string color, int pieces, string service, DateTime receive, DateTime deliver)
        {
            ArticleType = articleType;
            Fabric = fabric;
            Color = color;
            Pieces = pieces;
            Service = service;
            Receive = receive;
            Deliver = deliver;
        }
        public string ToText()
        {
            return "~"+ArticleType + "                |" + Fabric + "           |" + Color + "          |" + Pieces 
                   + "              |"+ Service + "                            |" + Receive + "                |" + Deliver;
        }


        // primeste continutul din fisier sub forma de string(stringul 'text') si returneaza un obiect de tip LaundryShop;
        public static LaundryShop FromFile (string text)
        {
            string[] parts = text.Split('|');
            LaundryShop LS = new LaundryShop(parts[0], parts[1], parts[2], int.Parse(parts[3]), parts[4],
                                            DateTime.Parse(parts[5]), DateTime.Parse(parts[6]));
            
            return LS;
        }

    }
    
}
