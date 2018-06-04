using System;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;


namespace HttpServer
{
    internal static class Program
    {
        private static void Main()
        {
            var books = new List<Books>();
               
            void SaveToRemoteLib()
            {
                var bookArray = books.ToArray();
                var jsonFormatter = new DataContractJsonSerializer(typeof(Books[]));

                if (bookArray.Length == 0)
                    Console.WriteLine("Library is empty");
                else
                {
                    using (var fs = new FileStream("Library.json", FileMode.OpenOrCreate))
                    {
                    
                        jsonFormatter.WriteObject(fs, bookArray);
                    }
                }
            }
            var listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:8000/connection/");
            listener.Start();
            Console.WriteLine("Waiting for connections...");
          
            while (true)
            {

                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                if (request.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(request.InputStream))
                    {
                        try
                        {
                            var content = reader.ReadToEnd();
                            var jss = new JavaScriptSerializer();
                            var json = new StringReader(content).ReadToEnd();
                            var data = jss.Deserialize<List<Books>>(json);
                            var book = new Books();
                            foreach (var b in data)
                            {
                                book.Author = b.Author;
                                book.Title = b.Title;
                                book.Annotation = b.Annotation;
                                book.ISBN = b.ISBN;
                                book.PublicationDate = b.PublicationDate;
                                books.Add(book);
                                Console.WriteLine(b);
                            }

                            SaveToRemoteLib();
                            Console.WriteLine("Added books");
                            Console.WriteLine("URL: {0}", request.Url.OriginalString);
                            Console.WriteLine("Method: {0}", request.HttpMethod);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (request.HttpMethod == "GET")
                {
                    using (StreamReader r = new StreamReader("Library.json"))
                    {
                        string json = r.ReadToEnd();
                        Console.WriteLine(json);
                        r.Close();
                        string responseString = json;
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }


                    Console.WriteLine("Done reading from json");
                    Console.WriteLine("URL: {0}", request.Url.OriginalString);
                    Console.WriteLine("Method: {0}", request.HttpMethod);
                }
            }
        }

        }
    }
