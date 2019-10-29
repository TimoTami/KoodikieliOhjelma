using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KoodikieliOhjelma
{

    class AakkonenNumeroPari
    {

        public string kirjain = "";
        public int? numero = null;

    }

    public class Program
    {

        static System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        static long elapsedMs = watch.ElapsedMilliseconds;

        static string[] aakkoset = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "å", "ä", "ö", "" };
        static string[] vokaalit = new string[] { "a", "e", "i", "o", "u", "y", "å", "ä", "ö" };
        static string[] konsonantit = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z" };
        static string[] vahanKaytetyt = new string[] { "b", "c", "f", "q", "w", "x", "z", "å" };
        //static int[] koodi = new int[] { 0, 8, 13, 4 };
        //static int[] koodi = new int[] { 0, 11, 11, 0, 10, 10, 0 };
        //static int[] koodi2 = new int[] { 0, 20, 10, 8 };

        static int[] koodi = new int[] { 0, 22, 22, 0 };
        //static int[] koodi = new int[] { 0, 22, 22, 0, 20, 20, 0 };
        static int[] koodi2 = new int[] { 0, 40, 20, 16 };
        static int[] koodi3 = new int[] { 8, 14, 20, 54 };

        static int[] koodiDistinct = koodi.Distinct().ToArray();
        static bool oikein = false;
        static bool suomea = true;
        static bool kelpaa;

        static string[][] oikeaVastausJagged = new string[][]
        {
            new string[]{"a","l","l","a","k","k","a" },
            new string[]{"a","u","k","i" },
            new string[]{"e","h","k","ä" },

        };

        static int[] sN = new int[aakkoset.Length];
        static int counter = 0;
        static int pyyntoja = 0;

        static int[][] koodiJagged = new int[][]
        {
            koodi,
            koodi2,
            koodi3
        };
        static int[][] koodiDistinctJagged = new int[koodiJagged.Length][];
        static string[][] vastausJagged = new string[koodiJagged.Length][];
        static string[][] vastausDistinctJagged = new string[vastausJagged.Length][];

        static List<AakkonenNumeroPari> aakkosetJaNumerot = new List<AakkonenNumeroPari>();


        static void Main()
        {
            for (int i = 0; i < koodiDistinctJagged.Length; i++)
            {
                koodiDistinctJagged[i] = koodiJagged[i].Distinct().ToArray();
            }

            for (int a = 1; a < koodiDistinctJagged.Length; a++)
            {
                for (int i = 0; i < koodiDistinctJagged.Length; i++)
                {
                    if (i != a)
                    {
                        koodiDistinctJagged[a].Except(koodiDistinctJagged[i]);
                    }

                }

            }

            for (int i = 0; i < vastausJagged.Length; i++)
            {
                vastausJagged[i] = new string[koodiJagged[i].Length];
            }

            for (int i = 0; i < vastausDistinctJagged.Length; i++)
            {
                vastausDistinctJagged[i] = new string[koodiDistinctJagged[i].Length];

            }

            for (int i = 0; i < aakkoset.Length - 1; i++)
            {
                aakkosetJaNumerot.Add(new AakkonenNumeroPari() { kirjain = aakkoset[i] });
            }

            for (int i = 0; i < aakkoset.Length; i++)
            {
                sN[i] = i + 1;
            }
            sN[aakkoset.Length - 1] = 0;

            for (int i = 0; i < koodiJagged.Length; i++)
            {
                ForSekoitus(0, i);
                foreach (var item in aakkosetJaNumerot.Where(n => n.numero != null))
                {
                    aakkoset = aakkoset.Where(a => a != item.kirjain).Select(k => k).ToArray();

                }
                sN = new int[aakkoset.Length];
                for (int h = 0; h < aakkoset.Length; h++)
                {

                    sN[h] = h + 1;
                }
                sN[aakkoset.Length - 1] = 0;
                oikein = false;
            }


            Console.ReadLine();
        }

        static void ForSekoitus(int vastauksenIndexi, int jaggedIndexi)
        {               //8                 0               8       8        alussa[8] joka on   0
            for (int sNIndexi = aakkoset.Length - 1; sN[sNIndexi] != aakkoset.Length - 1; sNIndexi = sN[sNIndexi])
            {
                while (oikein)
                {
                    return;
                }

                vastausDistinctJagged[jaggedIndexi][vastauksenIndexi] = aakkoset[sN[sNIndexi]];
                sN[sNIndexi] = sN[sN[sNIndexi]];

                if (vastauksenIndexi < vastausDistinctJagged[jaggedIndexi].Length - 1)
                {
                    ForSekoitus(vastauksenIndexi + 1, jaggedIndexi);
                }
                else
                {
                    for (int vIndexi = 0; vIndexi < vastausJagged[jaggedIndexi].Length; vIndexi++)
                    {

                        if (jaggedIndexi != 0 && aakkosetJaNumerot.Any(a => koodiJagged[jaggedIndexi][vIndexi].Equals(a.numero)))
                        {
                            var pp = aakkosetJaNumerot.Where(a => a.numero == koodiJagged[jaggedIndexi][vIndexi]);

                            foreach (var item in pp)
                            {
                                vastausJagged[jaggedIndexi][vIndexi] = item.kirjain;
                            }

                        }
                        else
                        {
                            vastausJagged[jaggedIndexi][vIndexi] = vastausDistinctJagged[jaggedIndexi][Array.IndexOf(koodiDistinctJagged[jaggedIndexi], koodiDistinctJagged[jaggedIndexi][Array.IndexOf(koodiDistinctJagged[jaggedIndexi], koodiJagged[jaggedIndexi][vIndexi])])];
                        }

                    }

                    counter += 1;

                    if (suomea)
                    {

                        var montakoXZW = vastausJagged[jaggedIndexi].Where(a => vahanKaytetyt.Contains(a));
                        if (montakoXZW.Count() <= vastausJagged[jaggedIndexi].Count() * 0.7)
                        {


                            if (!vastausJagged[jaggedIndexi].All(a => konsonantit.Contains(a)))
                            {
                                if (!vastausJagged[jaggedIndexi].All(a => vokaalit.Contains(a)))
                                {
                                    Console.WriteLine(string.Join("", vastausJagged[jaggedIndexi]));
                                    var task = MainAs(vastausJagged[jaggedIndexi]);
                                    task.Wait();
                                    kelpaa = task.Result;
                                }

                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine(string.Join("", vastausJagged[jaggedIndexi]));
                        var task = MainAs(vastausJagged[jaggedIndexi]);
                        task.Wait();
                        kelpaa = task.Result;
                    }



                    if (kelpaa)
                    {
                        var res = vastausDistinctJagged[jaggedIndexi].Select(k => k);
                        var result = aakkosetJaNumerot.Where(a => vastausDistinctJagged[jaggedIndexi].Any(v => a.kirjain == v));
                        int i = 0;
                        var uusOrderi = koodiDistinctJagged[jaggedIndexi].OrderBy(l => l).ToArray();

                        foreach (var kirjain in result)
                        {
                            kirjain.numero = uusOrderi[i];
                            i++;
                        }

                        oikein = true;
                        //Console.WriteLine(string.Join("", vastausJagged[jaggedIndexi]));
                        //Console.ReadLine();
                        return;
                    }


                    //if (string.Join("", vastausJagged[jaggedIndexi]) == string.Join("", oikeaVastausJagged[jaggedIndexi]))
                    //{
                    //    var res = vastausDistinctJagged[jaggedIndexi].Select(k => k);
                    //    var result = aakkosetJaNumerot.Where(a => vastausDistinctJagged[jaggedIndexi].Any(v => a.kirjain == v));
                    //    int i = 0;
                    //    var uusOrderi = koodiDistinctJagged[jaggedIndexi].OrderBy(l => l).ToArray();

                    //    foreach (var kirjain in result)
                    //    {
                    //        kirjain.numero = uusOrderi[i];
                    //        i++;
                    //    }

                    //    oikein = true;
                    //    Console.WriteLine(string.Join("", vastausJagged[jaggedIndexi]));
                    //    Console.ReadLine();
                    //    return;
                    //}

                }

                sN[sNIndexi] = Array.IndexOf(aakkoset, vastausDistinctJagged[jaggedIndexi][vastauksenIndexi]);
            }

        }



        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        public static async Task<bool> MainAs(string[] arvaus)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                pyyntoja++;

                string myParameters = String.Join("", arvaus);
                //string valmis = "https://autocomplete-0.sanakirja.org/?text=" + myParameters + "&sourceLanguage=3&targetLanguage=17&locale=fi";
                //string valmiimpi = "https://www.sanakirja.org/search.php?q=" + myParameters + "&l=3&l2=17";


                //var url = "https://sanat.oahpa.no/fin/olo/";
                //var parameters = new Dictionary<string, string> { { "lookup", myParameters } };
                //var encodedContent = new FormUrlEncodedContent(parameters);
                //var responssi2 = await client.PostAsync(url, encodedContent).ConfigureAwait(false);

                //// string responssi = await client.GetStringAsync(uri);

                //Console.WriteLine(responssi2);

                //if (responssi2.StatusCode == HttpStatusCode.OK)
                //{
                //    var responseContent = await responssi2.Content.ReadAsStringAsync ().ConfigureAwait (false);
                //    Console.WriteLine(responseContent);
                //    //Console.ReadLine();
                //}

                HttpResponseMessage response = await client.GetAsync("https://ilmainensanakirja.fi/suomi-englanti/" + myParameters);
                response.EnsureSuccessStatusCode();
                string responssi1 = await response.Content.ReadAsStringAsync();


                Console.WriteLine(responssi1);
                //Console.WriteLine(responseBody.ToString());
                //Console.WriteLine("<h1>Sanan <b> " + myParameters + " </b>käännös<b>suomi-englanti</b></h1>");
                //Console.ReadLine();
                if (responssi1.Contains("<h1>Sanan <b>" + myParameters + "</b> käännös <b>suomi-englanti</b></h1>"))
                {

                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine("Kesti: " + elapsedMs + "ms ja pyyntöjä tuli " + pyyntoja + ". Mutta tarkoititko tätä: " + myParameters + " k/e");
                    pyyntoja = 0;

                    if (Console.ReadKey().Key == ConsoleKey.K)
                    {
                        watch.Start();
                        return true;
                    }

                    else
                    {
                        watch.Start();
                        return false;
                    }

                }
                //Console.ReadLine();
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
                //Console.ReadLine();
                return false;

            }
        }
    }
}
